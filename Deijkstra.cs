using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/// <summary>
/// Файл, реализующий логику работы алгоритма Дейкстры
/// </summary>
/// \file Deijkstra.cs
/// \date 25.04.2022 19:46


namespace course_on_forms_beta1
{

    public class Dijkstra
    {

        public Dijkstra()
        {

        }

        public List<Point> GetMinDistance(Vertex start, Vertex end)
        {
            List<Point> way = new List<Point>();
            Queue PriorityQueue = new Queue();

            PriorityQueue.Insert(new Tuple<Vertex, double>(start, 0));

            Dictionary<Vertex, Vertex> track = new Dictionary<Vertex, Vertex>();
            Dictionary<Vertex, double> costs = new Dictionary<Vertex, double>();

            Vertex currentVertex;
            track.Add(start, null);
            costs.Add(start, 0);
            while ((currentVertex = PriorityQueue.DeleteMin()) != null)
            {
                foreach (var nearVertex in currentVertex.Connections.Keys)
                {
                    if (!track.ContainsKey(nearVertex) || costs[nearVertex] > costs[currentVertex] + currentVertex.Connections[nearVertex])
                    {
                        track[nearVertex] = currentVertex;
                        costs[nearVertex] = costs[currentVertex] + currentVertex.Connections[nearVertex];
                        nearVertex.ParentVertex = currentVertex;
                        PriorityQueue.Insert(new Tuple<Vertex, double>(nearVertex, costs[nearVertex]));
                    }
                }
            }
            currentVertex = end;
            way.Add(currentVertex.Point);
            while (track[currentVertex] != null)
            {
                way.Add(track[currentVertex].Point);
                currentVertex = track[currentVertex];
            }
            way.Add(start.Point);
            way.Reverse();
            return way;
        }
    }
}

