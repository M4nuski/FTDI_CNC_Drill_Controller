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

        //public Color BackColor; //superseeded by Rectangle element
        public List<IViewerElements> Elements;

        private Control _outputControl;

        public float ZoomLevel;//todo add setter and getter
        private float minZoomLevel, maxZoomLevel;
        private int lastMouseX, lastMouseY;

        public struct viewData
        {
            public Graphics OutputGraphic;
            public PointF Size;
            public PointF PanPosition;
            public float ZoomLevel;
        }

        private viewData ViewData;

        public Viewer(Control OutputControl, PointF Size)
        {
            _outputControl = OutputControl;
            ViewData.Size = Size;

            _outputControl.Paint += OutputControlOnPaint;
            _outputControl.MouseMove += OutputControlOnMouseMove;
            _outputControl.MouseDown += OutputControlOnMouseDown;
            _outputControl.MouseUp += OutputControlOnMouseUp;
            _outputControl.DoubleClick += OutputControlOnDoubleClick;
            _outputControl.MouseWheel += OutputControlOnMouseWheel;
        }

        public void Dispose()
        {
            _outputControl.Paint -= OutputControlOnPaint;
            _outputControl.MouseMove -= OutputControlOnMouseMove;
            _outputControl.MouseDown -= OutputControlOnMouseDown;
            _outputControl.MouseUp -= OutputControlOnMouseUp;
            _outputControl.DoubleClick -= OutputControlOnDoubleClick;
            _outputControl.MouseWheel -= OutputControlOnMouseWheel;
        }

        public PointF GetPointFromPix(int x, int y)
        {
            return new PointF(0, 0);
        }

        private void OutputControlOnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OutputControlOnDoubleClick(object sender, EventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OutputControlOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OutputControlOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OutputControlOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            throw new NotImplementedException();
        }

        private void OutputControlOnPaint(object sender, PaintEventArgs paintEventArgs)
        {
            ViewData.OutputGraphic = paintEventArgs.Graphics;

            foreach (var viewerElements in Elements)
            {
                viewerElements.Draw(ViewData);
            }
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
            data.OutputGraphic.DrawLine(_color, _x, 0, _x, data.Size.Y);
            data.OutputGraphic.DrawLine(_color, 0, _y, data.Size.X, _y);
        }

        public void UpdatePosition(int x, int y) //set new crosshard position from control's coordinates
        {
            //todo implement after get pointF from XY is done;
        }

        public void UpdatePosition(PointF newPosition)
        {
            _x = newPosition.X;
            _y = newPosition.Y;
        }

    }

    class Node : IViewerElements
    {
        private float _radius, _diameter;
        private Pen _color;
        private PointF _pos;

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
            _ty = toX;
            _color = new Pen(color);

        }
        public void Draw(Viewer.viewData data)
        {
            data.OutputGraphic.DrawLine(_color, _fx, _fy, _tx, _ty);
        }
    }

    class Rectangle : IViewerElements
    {
        private float _x, _y, _w, _h;
        private Pen _color;

        public Rectangle(PointF TopLeft, PointF Size, Color color)
        {
            _x = TopLeft.X;
            _y = TopLeft.Y;
            _w = Size.X;
            _h = Size.Y;
            _color = new Pen(color);
        }

        public Rectangle(float X, float Y, float Width, float Height, Color color)
        {
            _x = X;
            _y = Y;
            _w = Width;
            _h = Height;
            _color = new Pen(color);
        }

        public void Draw(Viewer.viewData data)
        {
            data.OutputGraphic.DrawRectangle(_color, _x, _y, _w, _h);
        }
    }

}
