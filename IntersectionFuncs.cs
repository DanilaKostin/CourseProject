using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

/// \file IntersectionFuncs.cs
/// <summary>
/// Файл, содержащий основыне функции проверки пересечений отрезков
/// </summary>
namespace course_on_forms_beta1
{
    /// <summary>
    /// Класс для работы с функциями пересечений отрезков
    /// </summary>
    public static class IntersectionFuncs
    {
        /// <summary>
        /// Функция проверки того, что отрезки пересекаются
        /// </summary>
        /// <param name="a">Начало 1 отрезка</param>
        /// <param name="b">Конец 1 отрезка</param>
        /// <param name="c">Начало 2 отрезка</param>
        /// <param name="d">Конец 2 отрезка</param>
        /// <returns>Булеана того, пересекаются ли отрезки</returns>
        /// \note О классе <see cref="Vertex"/>
        public static bool isLinePartsIntersected(Vertex a, Vertex b, Vertex c, Vertex d)
        {

            //if (a.Point.Y == c.Point.Y && c.Point.X > a.Point.X || b.Point.Y == d.Point.Y && d.Point.X > b.Point.X)
/*            if (a.Point.Y == c.Point.Y && b.Point.Y == c.Point.Y)
            {
                return true;
            }*/
            double common = (b.Point.X - a.Point.X) * (d.Point.Y - c.Point.Y) - (b.Point.Y - a.Point.Y) * (d.Point.X - c.Point.X);

            if (common == 0) return false;

            double rH = (a.Point.Y - c.Point.Y) * (d.Point.X - c.Point.X) - (a.Point.X - c.Point.X) * (d.Point.Y - c.Point.Y);
            double sH = (a.Point.Y - c.Point.Y) * (b.Point.X - a.Point.X) - (a.Point.X - c.Point.X) * (b.Point.Y - a.Point.Y);

            double r = rH / common;
            double s = sH / common;

            if (r >= 0 && r <= 1 && s >= 0 && s <= 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Функция проверки наличия какого-либо пересечения отрезка и списка точек
        /// </summary>
        /// <param name="start"><paramref name="Vertex"/> вершина</param>
        /// <param name="end"><paramref name="Vertex"/> вершина</param>
        /// <param name="list1">Список списков <paramref name="Vertex"/></param>
        /// <returns>Булеана, что произошло хотя бы одно пересечение</returns>
        /// \note О классе <see cref="Vertex"/>
        public static bool AnyIntersection(Vertex start, Vertex end, List<List<Vertex>> list1)
        {
            foreach (var list in list1)
                for (int i = 0; i < list.Count - 1; i++)
                {
                    Vertex pt1 = list[i];
                    Vertex pt2 = list[i + 1];

                    if (pt1.Point.Equals(start.Point) || pt2.Point.Equals(start.Point)
                        || pt1.Point.Equals(end.Point) || pt2.Point.Equals(end.Point))
                    {
                        continue;
                    }
                    if (isLinePartsIntersected(pt1, pt2, start, end))
                        return true;
                }
            return false;
        }

        public static bool AnyIntersection(Vertex start, Vertex end, List<Vertex> list)
        {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    Vertex pt1 = list[i];
                    Vertex pt2 = list[i + 1];

                    if (pt1.Point.Equals(start.Point) || pt2.Point.Equals(start.Point)
                        || pt1.Point.Equals(end.Point) || pt2.Point.Equals(end.Point))
                    {
                        continue;
                    }
                    if (isLinePartsIntersected(pt1, pt2, start, end))
                        return true;
                }
            return false;
        }

        /// <summary>
        /// Функция проверки того, что точка лежит внутри полигона
        /// </summary>
        /// <param name="point"><paramref name="Vertex"/> вершина</param>
        /// <param name="list">Список <paramref name="Vertex"/></param>
        /// <returns>Булеана того, что точка лежит внутри <paramref name="list"/> вершина</returns>
        /// \note О классе <see cref="Vertex"/>
        public static bool IsInside(Vertex point, List<Vertex> list)
        {
            Vertex start1 = new Vertex(new Point(0, point.Point.Y));
            start1 = new Vertex(new Point(1000, 1000));
            Vertex end1 = new Vertex(new Point(point.Point.X, point.Point.Y));

            Vertex start2 = new Vertex(new Point(point.Point.X, point.Point.Y));
            Vertex end2 = new Vertex(new Point(int.MaxValue, point.Point.Y ));

            int count1 = 0;
            int count2 = 0;

            for (int i = 0; i < list.Count - 1; i++)
            {
                Vertex pt1 = list[i];
                Vertex pt2 = list[i + 1];

                if (isLinePartsIntersected(pt1, pt2, start1, end1))
                    count1++;
                if (isLinePartsIntersected(pt1, pt2, start2, end2))
                    count2++;
            }
            if (count1 % 2 == 1)
                return true;
            return false;
        }

        public static bool IsInsideAny(Vertex point, List<List<Vertex>> list1)
        {
            foreach (var list in list1)
            {
                if (IsInside(point, list))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Функция получения центра отрезка
        /// </summary>
        /// <param name="p1"><paramref name="Vertex"/> вершина</param>
        /// <param name="p2"><paramref name="Vertex"/> вершина</param>
        /// <returns></returns>
        public static Vertex GetCenterCoordinates(Vertex p1, Vertex p2)
        {
            return new Vertex(new Point((p1.Point.X + p2.Point.X) / 2, (p1.Point.Y + p2.Point.Y) / 2));
        }
    }
}
