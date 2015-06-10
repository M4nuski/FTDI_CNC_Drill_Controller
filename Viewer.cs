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

        public float ZoomLevel;


        //public Color BackColor;
        public List<IViewerElements> Elements;

        private Control _outputControl;

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
        public void Draw(Viewer.viewData data)
        {
           //
        }

        public void UpdatePosition(int x, int y)
        {
            //
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
            _radius = diameter/2.0f;
            _color = new Pen(color);
            _pos = position;
        }

        public void Draw(Viewer.viewData data)
        {
            data.OutputGraphic.DrawEllipse(_color, _pos.X - _radius, _pos.Y - _radius, _diameter, _diameter);
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
