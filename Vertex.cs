using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/// \file Vertex.cs
/// <summary>
/// Файл, содержащий класс работы с вершинами графиа
/// </summary>
namespace course_on_forms_beta1
{

    public class Vertex
    {

        public Vertex ParentVertex = null;

        public bool Visited = false;

        public double Cost;

        public Point Point;

        public Dictionary<Vertex, double> Connections;



        public Vertex(Point p)
        {
            Connections = new Dictionary<Vertex, double>();
            Point = p;
            Cost = double.MaxValue;
        }

        public void AddConnection(Vertex t)
        {
            if (Connections.Keys.Select(te => te.Point).Contains(t.Point) || t.Connections.Keys.Select(te => te.Point).Contains(Point))
                return;

            var distance = FindDistance(t.Point, this.Point);
            Connections.Add(t, distance);
            t.Connections.Add(this, distance);
        }

        public double FindDistance(Point p1, Point p2)
        {
            return Math.Round(Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y)), 2);
        }
    }
}
