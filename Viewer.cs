using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CNC_Drill_Controller1
{
    class Viewer : IDisposable
    {

        //public Point inMouseLocationPix; superseeded by mousemove events

        public PointF Size
        {
            get { return ViewData.Size; }
        }

        public PointF MousePositionF;
        public Point MousePosition;
        //public Color BackColor; //superseeded by Rectangle element
        public List<IViewerElements> Elements;

        private Control _outputControl;


        private int maxPanX, maxPanY;
        private int minPanX, minPanY;
        private bool panning;
        private int lastMouseX, lastMouseY;
        public Point PanPosition;
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
            public PointF Size;                     //size of control in "units"
            public float Scale;                   //scale from "units" to pixels
            public PointF PanPosition; //datum //in screen pixels
            public float ZoomLevel;    //slope //float based on 1.0f = 100%
        }

        private viewData ViewData;

        /// <summary>
        /// Create a vector-drawing-style viewer with basic mouse controls
        /// </summary>
        /// <param name="OutputControl">Set the control in which the mouse event and paint output will be overriden to display the view</param>
        /// <param name="Size">The maximum diplay area of the view in you unit</param>

        public Viewer(Control OutputControl, PointF Size)
        {
            _outputControl = OutputControl;
            ViewData.Size = Size;
            ViewData.Scale = Math.Min(OutputControl.Width / Size.X, OutputControl.Height / Size.Y);

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
            return new PointF((x + PanPosition.X) / ViewData.Scale / zoomLevel, (y + PanPosition.Y) / ViewData.Scale / zoomLevel);
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
            //throw new NotImplementedException();
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
            MousePosition = mouseEventArgs.Location;
            MousePositionF = GetPointFromPix(mouseEventArgs.X, mouseEventArgs.Y);
            if (panning)
            {
                var tempX = clamp(PanPosition.X + lastMouseX - mouseEventArgs.X, minPanX, maxPanX);
                var tempY = clamp(PanPosition.Y + lastMouseY - mouseEventArgs.Y, minPanY, maxPanY);

                PanPosition = new Point(tempX, tempY);

                _outputControl.Refresh();
            }
            lastMouseX = mouseEventArgs.X;
            lastMouseY = mouseEventArgs.Y;
        }

        private void OutputControlOnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            ViewData.OutputGraphic = paintEventArgs.Graphics;
            ViewData.ZoomLevel = zoomLevel;
            ViewData.PanPosition = PanPosition;

            if (Elements != null)
                foreach (var viewerElements in Elements)
                {
                  viewerElements.Draw(ViewData);
                }

        }
        private static int clamp(int val, int min, int max)
        {
            if (val > max) val = max;
            if (val < min) val = min;
            return val;
        }

        private static float clamp(float val, float min, float max)
        {
            if (val > max) val = max;
            if (val < min) val = min;
            return val;
        }
        private void setZoomLevel(float newLevel)
        {
            var vdX = ViewData.Size.X * ViewData.Scale;
            var vdY = ViewData.Size.Y * ViewData.Scale;

            maxPanX = (int)((newLevel * vdX) - _outputControl.Width);
            maxPanY = (int)((newLevel * vdY) - _outputControl.Height);

            if ((newLevel * vdX) < _outputControl.Width)
            {
                minPanX = (int)(_outputControl.Width - (newLevel * vdX)) / -2;
            }
            else
            {
                minPanX = 0;
            }
            if ((newLevel * vdY) < _outputControl.Height)
            {
                minPanY = (int)(_outputControl.Height - (newLevel * vdY)) / -2;
            }
            else
            {
                minPanY = 0;
            }

            //find new pan offset
            var tempX = (((PanPosition.X + lastMouseX) / zoomLevel) * newLevel) - lastMouseX;
            var tempY = (((PanPosition.Y + lastMouseY) / zoomLevel) * newLevel) - lastMouseY;

            PanPosition = new Point((int)clamp(tempX, minPanX, maxPanX), (int)clamp(tempY, minPanY, maxPanY));

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
            PanPosition = new Point((int)(((ViewData.Size.X * ViewData.Scale * fitZoomLevel) - _outputControl.Width) * 0.5f), (int)(((ViewData.Size.Y * ViewData.Scale * fitZoomLevel) - _outputControl.Height) * 0.5f));
            zoomLevel = fitZoomLevel;//override panPosition compensation
            ZoomLevel = fitZoomLevel;
        }

        private Rectangle getCurrentRectangle()
        {
            var l = PanPosition.X / zoomLevel;
            var t = PanPosition.Y / zoomLevel;

            var w = _outputControl.Width / zoomLevel;
            var h = _outputControl.Height / zoomLevel;

            return new Rectangle((int)l, (int)t, (int)w, (int)h);
        }
    }



    internal interface IViewerElements
    {
        void Draw(Viewer.viewData data);
    }

    class CrossHair : IViewerElements
    {
        private Pen _color;
        private float _x, _y;

        public CrossHair(float X, float Y, Color color)
        {
            _color = new Pen(color);
            _x = X;
            _y = Y;
        }

        public void Draw(Viewer.viewData data)
        {
            var out_rectangle = ViewerHelper.ScaleUp(_x, _y,0, 0, data);
            data.OutputGraphic.DrawLine(_color, out_rectangle.X, 0, out_rectangle.X, data.Size.Y);
            data.OutputGraphic.DrawLine(_color, 0, out_rectangle.Y, data.Size.X, out_rectangle.Y);
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

    }

    class Node : IViewerElements
    {
        private float _radius, _diameter;
        private Pen _color;
        private PointF _pos;

        //nifty transparent property test:
        public Color color { get { return _color.Color; } set { _color = new Pen(value); } }

        public Node(PointF position, float diameter, Color color)
        {
            _diameter = diameter;
            _radius = diameter / 2.0f;
            _color = new Pen(color);
            _pos = position;
        }

        public void Draw(Viewer.viewData data)
        {
            data.OutputGraphic.DrawEllipse(_color, _pos.X - _radius, _pos.Y - _radius, _diameter, _diameter);
        }
    }

    class Line : IViewerElements
    {
        private Pen _color;
        private float _fx, _fy, _tx, _ty;
        public Line(float fromX, float fromY, float toX, float toY, Color color)
        {
            _fx = fromX;
            _fy = fromY;
            _tx = toX;
            _ty = toY;
            _color = new Pen(color);

        }
        public void Draw(Viewer.viewData data)
        {
            data.OutputGraphic.DrawLine(_color, _fx, _fy, _tx, _ty);
        }
    }

    class Box : IViewerElements
    {
        private float _x, _y, _w, _h;
        private Pen _color;
        private Brush _fill;

        public Box(PointF TopLeft, PointF Size, Color color)
        {
            _x = TopLeft.X;
            _y = TopLeft.Y;
            _w = Size.X;
            _h = Size.Y;
            _color = new Pen(color);
            _fill = new SolidBrush(color);
        }

        public Box(float X, float Y, float Width, float Height, Color color)
        {
            _x = X;
            _y = Y;
            _w = Width;
            _h = Height;
            _color = new Pen(color);
            _fill = new SolidBrush(color);
        }

        public void Draw(Viewer.viewData data)
        {
            var out_rectangle = ViewerHelper.ScaleUp(_x, _y, _w, _h, data);
            data.OutputGraphic.DrawRectangle(_color, out_rectangle);
            data.OutputGraphic.FillRectangle(_fill, out_rectangle);
        }
    }

    static class ViewerHelper
    {
        public static Rectangle ScaleUp(float X, float Y, float W, float H, Viewer.viewData data)
        {
            var l = ((X*data.Scale)*data.ZoomLevel) - data.PanPosition.X;
            var t = ((Y*data.Scale)*data.ZoomLevel) - data.PanPosition.Y;

            var w = (W * data.Scale) * data.ZoomLevel;
            var h = (H * data.Scale) * data.ZoomLevel;
            return new Rectangle((int)l, (int)t, (int)w, (int)h);
        }
    }

}
