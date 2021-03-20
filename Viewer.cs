using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    class Viewer : IDisposable
    {
        // Float values for the view are in the native data units relative to it's origin
        // Int values for the control are in pixel relative to the control's location

        public PointF ViewSize
        {
            get { return ViewData.Size; }
        }

        // Event
        public delegate void SelectionDelegate(List<IViewerElements> Selection);
        public SelectionDelegate OnSelect;

        public PointF ViewMousePosition;
        public Point ControlMousePosition;
        public List<IViewerElements> Elements;

        private readonly Control _outputControl;

        private Point maxPan;
        private Point minPan;
        private PointF viewOrigin;

        public Point ControlPanPosition;

        private bool panning;
        private Point lastMousePosition;
        public float ZoomIncrements = 1.25f;
        private float zoomLevel;
        private float fitZoomLevel; //also happens to be minimum zoom level
        private float maxZoomLevel = 10.0f;
        public float MaxZoomLevel
        {
            get
            {
                return maxZoomLevel;
            }
            set
            {
                if (value > fitZoomLevel)
                {
                    maxZoomLevel = value;
                }
            }
        }
        public float ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            set
            {
                setZoomLevel(clamp(value, fitZoomLevel, maxZoomLevel));
            }
        }



        public struct viewData
        {
            public Graphics OutputGraphic;
            public PointF Size;        //size of control in "units"
            public float Scale;        //pixels per units           //scale from "units" to pixels
            public PointF PanPosition; //datum //in screen pixels
            public float ZoomLevel;    //slope //float based on 1.0f = 100%
        }

        private viewData ViewData;

        /// <summary>
        /// Create a vector-drawing-style viewer with basic mouse controls
        /// </summary>
        /// <param name="OutputControl">Set the control in which the mouse event and paint output will be overriden to display the view</param>
        /// <param name="Size">The maximum diplay area of the view in your unit</param>

        public Viewer(Control OutputControl)
        {
            _outputControl = OutputControl;
            ViewData.Size = new PointF(1.0f, 1.0f);
            ViewData.Scale = Math.Min(OutputControl.Width / ViewSize.X, OutputControl.Height / ViewSize.Y);

            init();
        }

        public Viewer(Control OutputControl, PointF Size)
        {
            _outputControl = OutputControl;
            ViewData.Size = Size;
            ViewData.Scale = Math.Min(OutputControl.Width / Size.X, OutputControl.Height / Size.Y);

            init();
        }

        public Viewer(Control OutputControl, PointF Size, PointF Origin)
        {
            _outputControl = OutputControl;
            ViewData.Size = Size;
            ViewData.Scale = Math.Min(OutputControl.Width / Size.X, OutputControl.Height / Size.Y);
            viewOrigin = Origin;

            init();
        }

        private void init()
        {
            _outputControl.Paint += OutputControlOnPaint;
            _outputControl.MouseMove += OutputControlOnMouseMove;
            _outputControl.MouseDown += OutputControlOnMouseDown;
            _outputControl.MouseUp += OutputControlOnMouseUp;
            _outputControl.DoubleClick += OutputControlOnDoubleClick;
            _outputControl.MouseWheel += OutputControlOnMouseWheel;
            _outputControl.Resize += OutputControlOnResize;

            FitImageToControl();
        }

        public void Dispose()
        {
            _outputControl.Paint -= OutputControlOnPaint;
            _outputControl.MouseMove -= OutputControlOnMouseMove;
            _outputControl.MouseDown -= OutputControlOnMouseDown;
            _outputControl.MouseUp -= OutputControlOnMouseUp;
            _outputControl.DoubleClick -= OutputControlOnDoubleClick;
            _outputControl.MouseWheel -= OutputControlOnMouseWheel;
            _outputControl.Resize -= OutputControlOnResize;
        }

        public PointF GetPointFromPix(int x, int y)
        {
            return new PointF((x + ControlPanPosition.X) / ViewData.Scale / zoomLevel, (y + ControlPanPosition.Y) / ViewData.Scale / zoomLevel);
        }

        private void OutputControlOnResize(object sender, EventArgs eventArgs)
        {
            var lastFitZoomLevel = fitZoomLevel;
            setFitZoomLevel();
            ZoomLevel = zoomLevel * fitZoomLevel / lastFitZoomLevel;
        }
        private void OutputControlOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Delta > 0)
            {
                ZoomLevel = Math.Min(maxZoomLevel, ZoomLevel * ZoomIncrements);
            }
            else if (mouseEventArgs.Delta < 0)
            {
                ZoomLevel = Math.Max(fitZoomLevel, ZoomLevel / ZoomIncrements);
            }
        }

        private void OutputControlOnDoubleClick(object sender, EventArgs eventArgs)
        {
            OnSelect?.Invoke(Elements.Where(viewerElements => viewerElements.TestSelection(ViewMousePosition)).ToList());
        }

        private void OutputControlOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            panning = false;
        }

        private void OutputControlOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            panning = true;
        }

        private void OutputControlOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            _outputControl.Select();
            ControlMousePosition = mouseEventArgs.Location;
            ViewMousePosition = GetPointFromPix(mouseEventArgs.X, mouseEventArgs.Y);
            if (panning)
            {
                ControlPanPosition.X = clamp(ControlPanPosition.X + lastMousePosition.X - mouseEventArgs.X, minPan.X, maxPan.X);
                ControlPanPosition.Y = clamp(ControlPanPosition.Y + lastMousePosition.Y - mouseEventArgs.Y, minPan.Y, maxPan.Y);

                _outputControl.Refresh();
            }
            lastMousePosition.X = mouseEventArgs.X;
            lastMousePosition.Y = mouseEventArgs.Y;
        }

        private void OutputControlOnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            ViewData.OutputGraphic = paintEventArgs.Graphics;
            ViewData.ZoomLevel = zoomLevel;
            ViewData.PanPosition = ControlPanPosition;

            if (Elements != null)
                foreach (var viewerElements in Elements)
                {
                  viewerElements.Draw(ViewData);
                }

        }
        private static int clamp(int val, int min, int max)
        {
            if (val > max) return max;
            if (val < min) return min;
            return val;
        }

        private static float clamp(float val, float min, float max)
        {
            if (val > max) return max;
            if (val < min) return min;
            return val;
        }
        private void setZoomLevel(float newLevel)
        {
            var vMaxX = (ViewData.Size.X - viewOrigin.X) * ViewData.Scale;
            var vMaxY = (ViewData.Size.Y - viewOrigin.Y) * ViewData.Scale;

            maxPan.X = (int)((newLevel * vMaxX) - _outputControl.Width);
            maxPan.Y = (int)((newLevel * vMaxY) - _outputControl.Height);
            minPan.X = (int) (newLevel * (-viewOrigin.X * ViewData.Scale));
            minPan.Y = (int) (newLevel * (-viewOrigin.Y * ViewData.Scale));

            if ((newLevel * ViewData.Size.X * ViewData.Scale) < _outputControl.Width) 
            {
                minPan.X = (int)(_outputControl.Width - vMaxX) / -2;
                maxPan.X = minPan.X;
            }
            if ((newLevel * ViewData.Size.Y * ViewData.Scale) < _outputControl.Height) 
            {
                minPan.Y = (int)(_outputControl.Height - vMaxY) / -2;
                maxPan.Y = minPan.Y;
            }
            //find new pan offset
            var tempX = ((ControlPanPosition.X + lastMousePosition.X) / zoomLevel * newLevel) - lastMousePosition.X;
            var tempY = ((ControlPanPosition.Y + lastMousePosition.Y) / zoomLevel * newLevel) - lastMousePosition.Y;

            ControlPanPosition.X = (int)clamp(tempX, minPan.X, maxPan.X);
            ControlPanPosition.Y = (int)clamp(tempY, minPan.Y, maxPan.Y);

            zoomLevel = newLevel;
            _outputControl.Refresh();
        }

        private void setFitZoomLevel()
        {
            if (Math.Abs((ViewData.Size.X * ViewData.Scale) - _outputControl.Width) > float.Epsilon)
            {
                fitZoomLevel = _outputControl.Width / (ViewData.Size.X * ViewData.Scale);
            }
            else
            {
                fitZoomLevel = 1.0f;
            }

            if (((ViewData.Size.Y * ViewData.Scale) * fitZoomLevel) > _outputControl.Height)
            {
                fitZoomLevel = _outputControl.Height / (ViewData.Size.Y * ViewData.Scale);
            }
        }

        public void FitImageToControl()
        {
            setFitZoomLevel();
            ControlPanPosition.X = (int)( ( (ViewData.Size.X * ViewData.Scale * fitZoomLevel) - _outputControl.Width ) * 0.5f);
            ControlPanPosition.Y = (int)( ( (ViewData.Size.Y * ViewData.Scale * fitZoomLevel) - _outputControl.Height) * 0.5f);
            zoomLevel = fitZoomLevel;//override panPosition compensation
            ZoomLevel = fitZoomLevel;
        }

        public void FitContentToControl()
        {
            var Xmax = float.MinValue;
            var Xmin = float.MaxValue;
            var Ymax = float.MinValue;
            var Ymin = float.MaxValue;

            if (Elements != null) foreach (var viewerElement in Elements)
                {
                  if (Math.Abs(viewerElement.size) > float.Epsilon)
                    {
                        Xmax = Math.Max(Xmax, viewerElement.position.X + viewerElement.size);
                        Xmin = Math.Min(Xmin, viewerElement.position.X - viewerElement.size);
                        Ymax = Math.Max(Ymax, viewerElement.position.Y + viewerElement.size);
                        Ymin = Math.Min(Ymin, viewerElement.position.Y - viewerElement.size);
                    }

                }

            var w = Xmax - Xmin;
            var h = Ymax - Ymin;

            if (w < 1.0f) w = 1.0f;
            if (h < 1.0f) h = 1.0f;

            w = ViewData.Scale * w;
            h = ViewData.Scale * h;

            var zw = _outputControl.Width / w;
            var zh = _outputControl.Height / h;

            var z = Math.Min(zh, zw);
            setZoomLevel(z);

          //  w = (Xmax + Xmin) / 2.0f;
           // h = (Ymax + Ymin) / 2.0f;

            ControlPanPosition.X = (int)Math.Round(Xmin * zoomLevel * ViewData.Scale);
            ControlPanPosition.Y = (int)Math.Round(Ymin * zoomLevel * ViewData.Scale);
        }
    }



    internal interface IViewerElements
    {
        int ID { get; }
        PointF position { get; }
        float size { get; }

        Color color { get; set; }
        bool isSelected { get; set; }
        void Draw(Viewer.viewData data);
        bool TestSelection(PointF SelectionLocation);
    }

    class CrossHair : IViewerElements
    {
        public int ID { get; set; }
        public Color color {
            get { return _color.Color; }
            set { _color = new Pen(value);}
        }
        private Pen _color;
        private float _x, _y;
        public PointF position { get { return new PointF(_x, _y); } }
        public float size { get { return 0.0f; } }
        public bool isSelected { get; set; }

        public CrossHair(float X, float Y, Color color)
        {
            _color = new Pen(color);
            _x = X;
            _y = Y;
            ID = -1;
        }
        public CrossHair(float X, float Y, Color color, int ID)
        {
            _color = new Pen(color);
            _x = X;
            _y = Y;
            this.ID = ID;
        }

        public void Draw(Viewer.viewData data)
        {
            var out_rectangle = ViewerHelper.ScaleRectangle(_x, _y, data.Size.X, data.Size.Y, data);
            data.OutputGraphic.DrawLine(_color, out_rectangle.X, 0, out_rectangle.X, out_rectangle.Height*2);
            data.OutputGraphic.DrawLine(_color, 0, out_rectangle.Y, out_rectangle.Width*2, out_rectangle.Y);
        }

        public void UpdatePosition(PointF newPosition)
        {
            _x = newPosition.X;
            _y = newPosition.Y;
        }

        public void UpdatePosition(float X, float Y)
        {
            _x = X;
            _y = Y;
        }

        public bool TestSelection(PointF SelectionLocation)
        {
            return (Math.Abs(SelectionLocation.X - _x) < 0.010f) || (Math.Abs(SelectionLocation.Y - _y) < 0.010f);
        }

    }

    class Node : IViewerElements
    {
        private float _radius, _diameter;
        public Color color
        {
            get { return _color.Color; }
            set { _color = new Pen(value, 2.0f); }
        }
        private Pen _color;
        private Pen _selectedPen = new Pen(DrillNode.nodeSelectedColor, 4.0f);
        private PointF _pos;
        public int ID { get; set; }

        public PointF position { get { return new PointF(_pos.X, _pos.Y); } }
        public float size { get { return _diameter; } }
        public bool isSelected { get; set; }

        public Node(PointF position, float diameter, Color color)
        {
            _diameter = diameter;
            _radius = diameter / 2.0f;
            _color = new Pen(color, 2.5f);
            _pos = position;
            ID = -1;
        }
        public Node(PointF position, float diameter, Color color, int ID)
        {
            _diameter = diameter;
            _radius = diameter / 2.0f;
            _color = new Pen(color, 2.5f);
            _pos = position;
            this.ID = ID;
        }

        public void Draw(Viewer.viewData data)
        {
            var out_pos = ViewerHelper.ScaleRectangle(_pos.X - _radius, _pos.Y - _radius, _diameter, _diameter, data);
            if (isSelected) data.OutputGraphic.DrawEllipse(_selectedPen, out_pos);
            data.OutputGraphic.DrawEllipse(_color, out_pos);
        }

        public bool TestSelection(PointF SelectionLocation)
        {
            return (Math.Abs(_pos.X - SelectionLocation.X) < _diameter) &&
                   (Math.Abs(_pos.Y - SelectionLocation.Y) < _diameter);
        }
    }

    class Line : IViewerElements
    {

        private Pen _color;
        private float _fx, _fy, _tx, _ty;
        public int ID { get; set; }
        public Color color
        {
            get { return _color.Color; }
            set { _color = new Pen(value); }
        }

        public PointF position { get { return new PointF(_fx, _fy); } }
        public float size { get { return 0.0f; } }
        public bool isSelected { get; set; }

        public Line(float fromX, float fromY, float toX, float toY, Color color)
        {
            _fx = fromX;
            _fy = fromY;
            _tx = toX;
            _ty = toY;
            _color = new Pen(color);
            ID = -1;
        }
        public Line(float fromX, float fromY, float toX, float toY, Color color, int ID)
        {
            _fx = fromX;
            _fy = fromY;
            _tx = toX;
            _ty = toY;
            _color = new Pen(color);
            this.ID = ID;
        }

        public void Draw(Viewer.viewData data)
        {
            var out_from = ViewerHelper.ScalePoint(_fx, _fy, data);
            var out_to = ViewerHelper.ScalePoint(_tx, _ty, data);
            data.OutputGraphic.DrawLine(_color, out_from, out_to);
        }

        public bool TestSelection(PointF SelectionLocation)
        {
            if (Math.Abs(_fx - _tx) < float.Epsilon) return (Math.Abs(SelectionLocation.X - _fx) < 0.010f);
            if (Math.Abs(_fy - _ty) < float.Epsilon) return (Math.Abs(SelectionLocation.Y - _fy) < 0.010f);

            //on X
            var slope = (_ty - _fy) / (_tx - _fx);
            var datum = _ty - (slope*_tx);

            var SelectedOnX = (Math.Abs(((slope * SelectionLocation.X) + datum) - SelectionLocation.Y) < 0.010f);

            //on Y
            slope = (_tx - _fx) / (_ty - _fy);
            datum = _tx - (slope * _ty);
            var SelectedOnY = (Math.Abs(((slope * SelectionLocation.Y) + datum) - SelectionLocation.X) < 0.010f);

            return SelectedOnX || SelectedOnY; 
        }
    }

    class Box : IViewerElements
    {
        private float _x, _y, _w, _h;
        private Pen _color;
        private Brush _fill;
        public Color color
        {
            get { return _color.Color; }
            set
            {
                _color = new Pen(value);
                _fill = new SolidBrush(value);
            }
        }
        public PointF position { get { return new PointF(_x + _w/2.0f, _y + _h/2.0f); } }
        //public float size { get { return Math.Max(_w, _h); } }
        public float size { get { return 0.0f; } }
        public bool isSelected { get; set; }

        public int ID { get; set; }

        private void box(float x, float y, float w, float h, Color FillColor, int id)
        {
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _color = new Pen(FillColor);
            _fill = new SolidBrush(FillColor);
            ID = id; 
        }

        public Box(PointF TopLeft, PointF Size, Color color)
        {
            box(TopLeft.X, TopLeft.Y, Size.X, Size.Y, color, -1);
        }

        public Box(float X, float Y, float Width, float Height, Color color)
        {
            box(X, Y, Width, Height, color, -1);
        }
        public Box(PointF TopLeft, PointF Size, Color color, int ID)
        {
            box(TopLeft.X, TopLeft.Y, Size.X, Size.Y, color, ID);
        }
        public Box(float X, float Y, float Width, float Height, Color color, int ID)
        {
            box(X, Y, Width, Height, color, ID);
        }

        public void Draw(Viewer.viewData data)
        {
            var out_rectangle = ViewerHelper.ScaleRectangle(_x, _y, _w, _h, data);
            data.OutputGraphic.DrawRectangle(_color, out_rectangle);
            data.OutputGraphic.FillRectangle(_fill, out_rectangle);
        }

        public bool TestSelection(PointF SelectionLocation)
        {
            return ((SelectionLocation.X > _x) && (SelectionLocation.Y > _y) && (SelectionLocation.X < (_x + _w)) &&
                    (SelectionLocation.Y < (_y + _h)));
        }

        public void UpdateSize(float w, float h)
        {
            _w = w;
            _h = h;
        }
    }

    internal class Cross : IViewerElements
    {
        public int ID { get; set; }

        public Color color
        {
            get { return _color.Color; }
            set { _color = new Pen(value); }
        }

        private Pen _color;
        private float _x, _y, _s;

        public PointF position { get { return new PointF(_x, _y); } }
        public float size { get { return 0.0f; } }
        public bool isSelected { get; set; }

        public Cross(float X, float Y, Color color)
        {
            _color = new Pen(color);
            _x = X;
            _y = Y;
            _s = 0.1f;

            ID = -1;
        }

        public Cross(float X, float Y, float Size, Color color, int ID)
        {
            _color = new Pen(color);
            _x = X;
            _y = Y;
            _s = Size/2.0f;
            this.ID = ID;
        }

        public void Draw(Viewer.viewData data)
        {
            var out_rectangle = ViewerHelper.ScaleRectangle(_x, _y, _s, _s, data);
            data.OutputGraphic.DrawLine(_color, out_rectangle.X - out_rectangle.Width, out_rectangle.Y, out_rectangle.Right, out_rectangle.Y);
            data.OutputGraphic.DrawLine(_color, out_rectangle.X, out_rectangle.Y-out_rectangle.Height, out_rectangle.X, out_rectangle.Bottom);
        }

        public void UpdatePosition(PointF newPosition)
        {
            _x = newPosition.X;
            _y = newPosition.Y;
        }

        public void UpdatePosition(float X, float Y)
        {
            _x = X;
            _y = Y;
        }

        public bool TestSelection(PointF SelectionLocation)
        {
            return false;
        }
    }

    static class ViewerHelper
    {
        public static Rectangle ScaleRectangle(float X, float Y, float W, float H, Viewer.viewData data)
        {
            var l = ((X*data.Scale)*data.ZoomLevel) - data.PanPosition.X;
            var t = ((Y*data.Scale)*data.ZoomLevel) - data.PanPosition.Y;

            var w = (W * data.Scale) * data.ZoomLevel;
            var h = (H * data.Scale) * data.ZoomLevel;
            return new Rectangle((int)l, (int)t, (int)w, (int)h);
        }

        public static Point ScalePoint(float X, float Y, Viewer.viewData data)
        {
            var l = ((X * data.Scale) * data.ZoomLevel) - data.PanPosition.X;
            var t = ((Y * data.Scale) * data.ZoomLevel) - data.PanPosition.Y;
            return new Point((int)l, (int)t);            
        }

        public static Point ScalePointF(PointF Pos, Viewer.viewData data)
        {
            var l = ((Pos.X * data.Scale) * data.ZoomLevel) - data.PanPosition.X;
            var t = ((Pos.Y * data.Scale) * data.ZoomLevel) - data.PanPosition.Y;
            return new Point((int)l, (int)t);
        }
    }

}
