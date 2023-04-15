using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static course_on_forms_beta1.Form1;

namespace course_on_forms_beta1
{
    static class GraphVisabillity
    {
        static public void ConnectPolygon(BlockedZones blockedZones)
        {
            foreach (var p in blockedZones.endVert)
            {
                if (!IntersectionFuncs.AnyIntersection(blockedZones.startVert, p,
                    blockedZones.GetVertexLists()) && !IntersectionFuncs.AnyIntersection(blockedZones.startVert, blockedZones.endVert[0],
                    new List<List<Vertex>>() { blockedZones.Plan }))
                {
                    blockedZones.startVert.AddConnection(p);
                }
            }

            //Соединение полигонов друг с другом
            for (int i = 0; i < blockedZones.Points.Count; i++)
            {
                List<Vertex> list1 = blockedZones.Points[i];
                for (int j = i + 1; j < blockedZones.Points.Count; j++)
                {
                    List<Vertex> list2 = blockedZones.Points[j];
                    foreach (var t1 in list1)
                        foreach (var t2 in list2)
                        {
                            if (!IntersectionFuncs.AnyIntersection(t1, t2,
                                blockedZones.Points) && !IntersectionFuncs.AnyIntersection(t1, t2, new List<List<Vertex>>() { blockedZones.Plan }))
                            {
                                t1.AddConnection(t2);
                            }
                        }
                }
            }
            //Соединение границы полигонов
            foreach (var item in blockedZones.Points)
                for (int i = 0; i < item.Count - 1; i++)
                {
                    item[i].AddConnection(item[i + 1]);
                }

            //Done: Сделать соединение границы плана, плана и полигонов и точек.
            //Соединение плана
            for (int i = 0; i < blockedZones.Plan.Count; i++)
            {
                for (int j = i + 1; j < blockedZones.Plan.Count; j++)
                {
                    if (!IntersectionFuncs.AnyIntersection(blockedZones.Plan[i], blockedZones.Plan[j], blockedZones.Points) &&
                        !IntersectionFuncs.AnyIntersection(blockedZones.Plan[i], blockedZones.Plan[j], new List<List<Vertex>>() { blockedZones.Plan }) &&
                        IntersectionFuncs.IsInside(IntersectionFuncs.GetCenterCoordinates(blockedZones.Plan[i], blockedZones.Plan[j]), blockedZones.Plan))
                    {
                        blockedZones.Plan[i].AddConnection(blockedZones.Plan[j]);
                    }
                }
            }

            for (int i = 0; i < blockedZones.Plan.Count - 1; i++)
            {
                blockedZones.Plan[i].AddConnection(blockedZones.Plan[i + 1]);
            }

            //Соединение плана и полигона
            for (int j = 0; j < blockedZones.Points.Count; j++)
            {
                List<Vertex> list2 = blockedZones.Points[j];
                foreach (var t1 in blockedZones.Plan)
                    foreach (var t2 in list2)
                    {
                        if (!IntersectionFuncs.AnyIntersection(t1, t2,
                            blockedZones.Points) &&
                            !IntersectionFuncs.AnyIntersection(t1, t2, new List<List<Vertex>>() { blockedZones.Plan })
                            &&
                            !IntersectionFuncs.AnyIntersection(t1, t2, new List<List<Vertex>>() { blockedZones.startPlan }))
                        {
                            t1.AddConnection(t2);
                            //Console.WriteLine(t1 + " " + t2);
                            //SolidBrush brush = new SolidBrush(Color.Gray);
                        }
                    }
            }
        }

        public static void ConnectWithPoints(Vertex current, BlockedZones blockedZones)
        {
            //Соединение точки и полигонов
            foreach (var item in blockedZones.Points)
            {
                foreach (var point in item)
                {
                    if (!IntersectionFuncs.AnyIntersection(current, point,
                    blockedZones.Points) &&
                    !IntersectionFuncs.AnyIntersection(current, point, new List<List<Vertex>>() { blockedZones.Plan }))
                    {
                        current.AddConnection(point);
                    }
                }
            }
            //Соединение точки и плана
            for (int i = 0; i < blockedZones.Plan.Count; i++)
            {
                if (!IntersectionFuncs.AnyIntersection(blockedZones.startVert, blockedZones.Plan[i],
                blockedZones.Points) &&
                !IntersectionFuncs.AnyIntersection(blockedZones.startVert, blockedZones.Plan[i], new List<List<Vertex>>() { blockedZones.Plan }))
                {
                    blockedZones.startVert.AddConnection(blockedZones.Plan[i]);
                }
            }

        }
    }
}
