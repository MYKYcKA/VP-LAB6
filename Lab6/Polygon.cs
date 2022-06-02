using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace Lab6
{
    public class Polygon
    {
        public PointF[] Vertex = null;
        public int VerticeCount = 0;

        public Polygon()
        {
            VerticeCount = 4;
            Vertex = new PointF[VerticeCount];
        }

        public Polygon(params PointF[] points)
        {
            Vertex = points;
            VerticeCount = points.Length;
        }

        public void Draw(object sender, PaintEventArgs e)
        {
            var gr = e.Graphics;
            var p = new Pen(Color.BlueViolet, 5);
            gr.DrawPolygon(p, Vertex);
        }

        public void MoveBy(float x, float y)
        {
            var newVertex = new PointF[VerticeCount];
            for (int i = 0; i < Vertex.Length; i++)
            {
                var point = Vertex[i];
                newVertex[i] = new PointF(point.X + x, point.Y + y);
            }
            Vertex = newVertex;
        }

        public void Rotate(float angle, PointF origin)
        {
            double angleInRadians = angle * (Math.PI / 180);
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            var newVertex = new PointF[VerticeCount];
            for (int i = 0; i < Vertex.Length; i++)
            {
                newVertex[i] = new PointF
                {
                    X =
            (float)
            (cos * (Vertex[i].X - origin.X) -
            sin * (Vertex[i].Y - origin.Y) + origin.X),
                    Y =
            (float)
            (sin * (Vertex[i].X - origin.X) +
            cos * (Vertex[i].Y - origin.Y) + origin.Y)
                };
            }
            Vertex = newVertex;
        }

        static bool onSegment(PointF p, PointF q, PointF r)
        {
            if (q.X <= Math.Max(p.X, r.X) &&
                q.X >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) &&
                q.Y >= Math.Min(p.Y, r.Y))
            {
                return true;
            }
            return false;
        }

        static int orientation(PointF p, PointF q, PointF r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) -
                    (q.X - p.X) * (r.Y - q.Y);

            if (val == 0)
            {
                return 0;
            }
            return (val > 0) ? 1 : 2;
        }

        static bool doIntersect(PointF p1, PointF q1,
                                PointF p2, PointF q2)
        { 
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
            {
                return true;
            }

            if (o1 == 0 && onSegment(p1, p2, q1))
            {
                return true;
            }

            if (o2 == 0 && onSegment(p1, q2, q1))
            {
                return true;
            }

            if (o3 == 0 && onSegment(p2, p1, q2))
            {
                return true;
            }

            if (o4 == 0 && onSegment(p2, q1, q2))
            {
                return true;
            }

            return false;
        }


        public static bool isInside(PointF[] polygon, int n, PointF p)
        {
            if (n < 3)
            {
                return false;
            }

            PointF extreme = new PointF((float)10000, p.Y);

            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;

                if (doIntersect(polygon[i],
                                polygon[next], p, extreme))
                {
                    if (orientation(polygon[i], p, polygon[next]) == 0)
                    {
                        return onSegment(polygon[i], p,
                                        polygon[next]);
                    }
                    count++;
                }
                i = next;
            } while (i != 0);

            return (count % 2 == 1);
        }
    }
}
