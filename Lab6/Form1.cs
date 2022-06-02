using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {
        private Point MouseDownLocation;
        public Polygon poly = new Polygon(new PointF[] {
            new PointF(200, 300), 
            new PointF(850, 300),
            new PointF(400, 200), 
            new PointF(700, 200), 
            new PointF(400, 455), 
            new PointF(700, 455) });
        public Form1()
        {
            InitializeComponent();
        }

        public void Paint_Polygon(object sender, PaintEventArgs e)
        {
            poly.Draw(sender, e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseDownLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var vector2 = new Point(e.X - MouseDownLocation.X, e.Y - MouseDownLocation.Y);
            var vector1 = new Point(0, 1);
            double angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
            double lent = Math.Sqrt(vector2.X*vector2.X + vector2.Y + vector2.Y);
            if (e.Button == MouseButtons.Left && Polygon.isInside(poly.Vertex,poly.VerticeCount,MouseDownLocation))
            {
                poly.MoveBy((float)(e.X - MouseDownLocation.X), (float)(e.Y - MouseDownLocation.Y));
                MouseDownLocation = e.Location;
                this.Invalidate();
            }
            if (e.Button == MouseButtons.Right && lent > 10)
            {
                poly.Rotate((float)angleInRadians, new PointF(e.X, e.Y));
                MouseDownLocation = e.Location;
                this.Invalidate();
            }
        }
    }
}
