using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_on_forms_beta1
{
    public class BlockedZones
    {
        public List<List<Vertex>> startPoints = new List<List<Vertex>>();

        public List<List<Vertex>> Points = new List<List<Vertex>>();

        public List<Vertex> startPlan = new List<Vertex>();

        public List<Vertex> Plan = new List<Vertex>();

        public Vertex startVert;

        public List<Vertex> endVert = new List<Vertex>();


        public void addPointList(List<List<Point>> point)
        {
            foreach (var t in point)
                Points.Add(t.Select(p => new Vertex(p)).ToList<Vertex>());
        }

        public void addPointList(List<Vertex> point)
        {
            startPoints.Add(point);
        }

        public void setPlan(List<Vertex> point)
        {
            startPlan = point;
        }

        public List<List<Vertex>> GetStartVertexLists()
        {
            return startPoints;
        }

        public List<List<Vertex>> GetVertexLists()
        {
            return Points;
        }
    }
}
