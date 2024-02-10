using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace course_on_forms_beta1
{
    /// <summary>
    /// Класс, содержащий функции работы с вектором
    /// </summary>
    static class Geometry
    {
        /// <summary>
        /// Метод номализации вектора
        /// </summary>
        /// <param name="p">Вектор-точка</param>
        /// <param name="epsilon">Множитель нормализации</param>
        /// \note Под <paramref name="epsilon"/> понимается число, на которое будут домножены координаты вектора.
        /// <returns>Точка-вектор</returns>
        public static Point NormalizeVector(Point p, int epsilon)
        {
            PointF floatP = new PointF((float)(p.X / Math.Sqrt(p.X * p.X + p.Y * p.Y)), (float)(p.Y / Math.Sqrt(p.X * p.X + p.Y * p.Y)));
            if (p.Equals(new Point(0, 0)))
                return p;
            return new Point((int)Math.Round(floatP.X * epsilon), (int)Math.Round(floatP.Y * epsilon));
        }

        /// <summary>
        /// Функция получения длины вектора
        /// </summary>
        /// <param name="p">Точка вектор</param>
        /// <returns>Длина double вектора</returns>
        public static double GetVectorLength(Point p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        /// <summary>
        /// Функция получения перпендикулярного вектора к переданному в функцию
        /// </summary>
        /// <param name="p">Вектор-точка</param>
        /// <param name="length">Длина желаемого перпендикулярного вектора</param>
        /// <returns>Перпендикулярный точка-вектор</returns>
        public static Point GetPerpendicularVector(Point p, int length)
        {
            return NormalizeVector(new Point(p.Y, -p.X), length);
        }
    }
}
