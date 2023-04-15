using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;




/**
 * @file Form1.cs
 * 
 * @date 25.04.2022
 * 
 * @brief Основной файл формы проекта.
 * 
 * @htmlonly 
 * <span style="font-weight: bold">History</span> 
 * @endhtmlonly 
 * Версия|Автор|Дата|Описание
 * ------|----|------|-------- 
 * 1.3|Костин Данила|25.04.2022|Создание документации   
*/

namespace course_on_forms_beta1
{

    public partial class Form1 : Form
    {
        public Graphics boder;

        public Form1()
        {
            InitializeComponent();
            boder = pictureBox1.CreateGraphics();
            SetBlocks();
            pictureBox1.MouseClick += OnPictureBoxClicked;
        }

        public int koefError = 0;

        public bool pointSetStart = true;

        private BlockedZones blockedZones = new BlockedZones();

        public int currentPoint = 0;

        

        private void button1_Click(object sender, EventArgs e)
        {
            clearPicture();
            pictureBox1.Update();
            DrawPolygons();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawPolygons();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void clearPicture()
        {
            pictureBox1.Image = null;
        }

        public void DrawPolygons()
        {
            Pen blackPen = new Pen(Color.Black, 3);
            SolidBrush brush = new SolidBrush(Color.Gray);
            foreach (var p in blockedZones.GetStartVertexLists())
            {
                boder.FillPolygon(brush, p.Select(t => t.Point).ToArray());
                boder.DrawPolygon(blackPen, p.Select(t => t.Point).ToArray());
            }
            boder.DrawLines(blackPen, blockedZones.startPlan.Select(t => t.Point).ToArray());

            var penR = new SolidBrush(Color.Red);
            var penB = new SolidBrush(Color.Blue);

            if (blockedZones.startVert != null)
            boder.FillEllipse(penR, blockedZones.startVert.Point.X-5, blockedZones.startVert.Point.Y-5, 10, 10);
            if (blockedZones.endVert.Count != 0)
                foreach (var endPoint in blockedZones.endVert)
                    boder.FillEllipse(penB, endPoint.Point.X - 5, endPoint.Point.Y - 5, 10, 10);
        }

        public void OnPictureBoxClicked(object sender, MouseEventArgs args)
        {
            Point p = args.Location;
            Console.WriteLine(p);
            Vertex current = new Vertex(p);

            foreach (var item in blockedZones.Points)
            {
                if (IntersectionFuncs.IsInside(current, item))
                    return;
            }

            if (pointSetStart)
            {
                blockedZones.startVert = new Vertex(p);
                current = blockedZones.startVert;
            }

            else
            {
                blockedZones.endVert.Add(new Vertex(p));
                current = blockedZones.endVert.Last();
            }

            p.X -= 5;
            p.Y -= 5;

            SolidBrush pen;
            if (pointSetStart)
                pen = new SolidBrush(Color.Red);
            else pen = new SolidBrush(Color.Blue);

            boder.FillEllipse(pen, p.X, p.Y, 10, 10);

            p.X += 5;
            p.Y += 5;
            GraphVisabillity.ConnectWithPoints(current, blockedZones);
            //connectPoints(current, blockedZones.Points, blockedZones.Plan);

        }

        private void SetBlocks()
        {
            List<Vertex> polygon;
/*            polygon = new List<Vertex> { new Vertex(new Point(100, 100)), new Vertex(new Point(250, 100)), new Vertex(new Point(150, 300)), new Vertex(new Point(120, 325)), new Vertex(new Point(100, 100)) };
            blockedZones.addPointList(polygon);
            polygon = new List<Vertex> { new Vertex(new Point(500, 300)), new Vertex(new Point(550, 300)), new Vertex(new Point(550, 350)), new Vertex(new Point(500, 350)), new Vertex(new Point(500, 300)) };
            blockedZones.addPointList(polygon);
            polygon = new List<Vertex> { new Vertex(new Point(400, 100)), new Vertex(new Point(420, 120)), new Vertex(new Point(410, 167)), new Vertex(new Point(380, 108)), new Vertex(new Point(400, 100)) };
            blockedZones.addPointList(polygon);
            polygon = new List<Vertex> { new Vertex(new Point(275, 169)), new Vertex(new Point(350, 197)), new Vertex(new Point(320, 271)), new Vertex(new Point(290, 228)),
                new Vertex(new Point(237, 256)), new Vertex(new Point(275, 169)) };
            blockedZones.addPointList(polygon);*/
            foreach (var i in PointsReader.ReadBlockedZones("C:\\Users\\Данила\\Desktop\\course_on_forms_preRealese1_6\\Holes.txt"))
            {
                blockedZones.addPointList(i);
            }
            //new Vertex(new Point())
/*            polygon = new List<Vertex> { new Vertex(new Point(44, 40)), new Vertex(new Point(273, 38)), new Vertex(new Point(308, 108)) ,
            new Vertex(new Point(354,32)),new Vertex(new Point(475,32)),new Vertex(new Point(477,185)),new Vertex(new Point(396,225)),new Vertex(new Point(397,276)),
            new Vertex(new Point(568,261)),new Vertex(new Point(588,377)),new Vertex(new Point(379,390)),new Vertex(new Point(312,324)),
            new Vertex(new Point(229,381)),new Vertex(new Point(89,384)),new Vertex(new Point(71,254)),new Vertex(new Point(24,254)),new Vertex(new Point(18,73)),
            new Vertex(new Point(44, 40))};*/
            //blockedZones.setPlan(polygon);
            blockedZones.setPlan(PointsReader.ReadPlan("C:\\Users\\Данила\\Desktop\\course_on_forms_preRealese1_6\\Plan.txt"));
            blockedZones.addPointList(IncreasePolygons(blockedZones.GetStartVertexLists(), -1));
            List<List<Vertex>> vertices = new List<List<Vertex>> { blockedZones.startPlan };
            blockedZones.Plan = IncreasePolygons(vertices, 1)[0].Select(k => new Vertex(k)).ToList();
        }

        public List<Point> AddErrorIntoWay(List<Point> way)
        {
            Point add_vector = new Point();
            Random r = new Random();
            double errorPersent;
            for (int i = 1; i <= way.Count-1; i++)
            {
                add_vector.X = way[i].X - way[i-1].X;
                add_vector.Y = way[i].Y - way[i-1].Y;
                int sign = (int)Math.Pow(-1, r.Next(0, 2));

                errorPersent = (double)koefError / 100.0;
                add_vector.X = (int)(add_vector.X * errorPersent);
                add_vector.Y = (int)(add_vector.Y * errorPersent);

                for (int j = i; j <= way.Count-1; j++)
                {
                    var p = way[j];
                    p.X += add_vector.X * sign;
                    p.Y += add_vector.Y * sign;
                    
                    way[j] = p;
                }
                double angle = r.Next(0, koefError);
                angle = koefError * Math.PI / 180;
                angle *= sign;
                angle %= Math.PI /  2;
                for (int j = i; j <= way.Count - 1; j++)
                {
                    var p = new Point(0, 0);
                    //p.X = (int)((way[j].X - way[i].X) * Math.Cos(angle) - (way[j].Y - way[i].Y) * Math.Sin(angle) + way[i].X);
                    //p.Y = (int)((way[j].X - way[i].X) * Math.Sin(angle) + (way[j].Y - way[i].Y) * Math.Cos(angle) + way[i].Y);
                    way[j] = RotateOnAngle(way[j], angle, way[i]);
                }
            }
            return way;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //Вызываем соединение плана и полигонов
            GraphVisabillity.ConnectPolygon(blockedZones);
            Dijkstra path = new Dijkstra();
            blockedZones.startVert.Cost = 0;
            blockedZones.startVert.ParentVertex = null;
            for (int j = -1; j < blockedZones.endVert.Count - 1; j++)
            {
                List<Point> way;

                if (j == -1) {
                    way = path.GetMinDistance(blockedZones.startVert, blockedZones.endVert[0]);
                }
                else
                {
                    way = path.GetMinDistance(blockedZones.endVert[j], blockedZones.endVert[j + 1]);

                }
                Pen pen = new Pen(Color.Blue, 3);
                Pen pen2 = new Pen(Color.Orange, 3);
                SolidBrush brush = new SolidBrush(Color.Red);


                way = WayToSteps(way);
                way = Imitation(way);

                for (int i = 0; i < way.Count - 1; i++)
                {
                    if (i % 2 == 0)
                        boder.DrawLines(pen, new[] { way[i], way[i + 1] });
                    else boder.DrawLines(pen, new[] { way[i], way[i + 1] });
                }

                way = AddErrorIntoWay(way);

                for (int i = 0; i < way.Count - 1; i++)
                {
                    if (i % 2 == 0)
                        boder.DrawLines(pen2, new[] { way[i], way[i + 1] });
                    else boder.DrawLines(pen2, new[] { way[i], way[i + 1] });
                }
            }
            ParticleFilter particleFilter = new ParticleFilter();
            particleFilter.GenerateParticles(1000, 800, 800, blockedZones.startPoints, blockedZones.startPlan);
            foreach (var p in particleFilter.particle)
            {
                SolidBrush br_brush = new SolidBrush(Color.Red);
                boder.FillEllipse(br_brush, p.Point.X, p.Point.Y, 3, 3);
            }
            blockedZones.endVert.Last().Cost = double.MaxValue;
            blockedZones.endVert.Last().ParentVertex = null;
        }

        public List<Point> WayToSteps(List<Point> way)
        {
            double l = 10;
            List<Point> steps = new List<Point>();
            steps.Add(way[0]);
            var random = new Random();

            for(int i = 0; i < way.Count-1; i++)
            {
                var p1 = way[i];
                var p2 = way[i + 1];
                while (true)
                {
                    double deviation = random.Next(5, 30);
                    double shift = deviation / 100 * l;
                    double step = shift + l;
                    var t = step / Math.Sqrt( Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
                    var stepPointX = p1.X + (p2.X - p1.X) * t;
                    var stepPointY = p1.Y + (p2.Y - p1.Y) * t;
                    var stepPoint = new Point((int)Math.Round(stepPointX), (int)Math.Round(stepPointY));

                    if (t > 1 || t < 0)
                    {
                        break;
                    }
                    steps.Add(stepPoint);
                    p1 = stepPoint;
                }
            }
            steps.Add(way.Last());
            return steps;
        }

        public static List<Point> PointsToSteps(List<Point> points)
        {
            List<Point> path = new List<Point>();

            var random = new Random();
            int sign;
            path.Add(points[0]);
            for (int i = 0; i < points.Count - 2; i++)
            {
                sign = random.Next(1, 10) % 2 == 0 ? 1 : -1;
                var vector = new Point(points[i + 1].X - points[i].X, points[i + 1].Y - points[i].Y);
                var perpendicular = Geometry.GetPerpendicularVector(vector, 4);
                path.Add(new Point(points[i + 1].X + sign * perpendicular.X, points[i + 1].Y + sign * perpendicular.Y));
            }
            path.Add(points.Last());
            return path;
        }

        public List<Point> Imitation(List<Point> p)
        {
            int lastPoints = 8;
            List<Point> path = new List<Point>();

            double angle;
            Random rand = new Random();
            double maxAngle = 1.5;
            double currentAngle = 0;
            int sign = -1;

            path.Add(p[0]);
            for (int i = 0; i < p.Count - lastPoints-1; i++)
            {

                angle = sign * (rand.NextDouble() % 0.2 + 0.1);
                currentAngle += angle;
                if (Math.Abs(currentAngle) > maxAngle)
                {
                    sign *= -1;
                }
                Point vector = new Point(p[i + 1].X - p[i].X, p[i + 1].Y - p[i].Y);

                Point rotatedVector = RotateOnAngle(vector, angle);

                double skalarAngle = (vector.X * rotatedVector.X + vector.Y * rotatedVector.Y) / (Geometry.GetVectorLength(vector)* Geometry.GetVectorLength(rotatedVector));
                int length = (int)Math.Round(Geometry.GetVectorLength(vector) / skalarAngle);

                rotatedVector = Geometry.NormalizeVector(rotatedVector, length);
                rotatedVector = new Point(path[i].X + rotatedVector.X, path[i].Y + rotatedVector.Y);
                if (IntersectionFuncs.IsInsideAny(new Vertex(rotatedVector), blockedZones.startPoints) 
                    || IntersectionFuncs.AnyIntersection(new Vertex(rotatedVector), new Vertex(path.Last()), blockedZones.startPoints) ||
                    IntersectionFuncs.AnyIntersection(new Vertex(rotatedVector), new Vertex(path.Last()), blockedZones.startPlan))
                {
                    i = -1;
                    path.Clear();
                    path.Add(p[0]);
                    continue;
                }
                path.Add(rotatedVector);
            }

            sign = -(int)(currentAngle / Math.Abs(currentAngle));
            angle = sign * Math.Abs(currentAngle / lastPoints);
            for (int i = p.Count - lastPoints - 1; i < p.Count - 1; i++)
            {
                Point vector = new Point(p.Last().X - path[i].X, p.Last().Y - path[i].Y);
                vector = Geometry.NormalizeVector(vector, 8);
                vector = RotateOnAngle(vector, angle);
                vector = new Point(path[i].X + vector.X, path[i].Y + vector.Y);
                path.Add(vector);
            }
            path.Add(p.Last());
            return path;
        }

        public static Point RotateOnAngle(Point p, double angle, Point center = new Point())
        {
            if (center.IsEmpty)
            {
                return new Point((int)(p.X * Math.Cos(angle) - p.Y * Math.Sin(angle)), (int)(p.X * Math.Sin(angle) + p.Y * Math.Cos(angle)));
            }
            //p.X = (int)((way[j].X - way[i].X) * Math.Cos(angle) - (way[j].Y - way[i].Y) * Math.Sin(angle) + way[i].X);
            //p.Y = (int)((way[j].X - way[i].X) * Math.Sin(angle) + (way[j].Y - way[i].Y) * Math.Cos(angle) + way[i].Y);
            //return new Point((int)(p.X * Math.Cos(angle) - p.Y * Math.Sin(angle)), (int)(p.X * Math.Sin(angle) + p.Y * Math.Cos(angle)));
            return new Point((int)((p.X - center.X) * Math.Cos(angle) - (p.Y - center.Y) * Math.Sin(angle) + center.X),
                             (int)((p.X - center.X) * Math.Sin(angle) + (p.Y - center.Y) * Math.Cos(angle) + center.Y));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pointSetStart = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pointSetStart = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            foreach (var t in blockedZones.Points)
            boder.DrawLines(pen, t.Select(p => p.Point).ToArray());
            boder.DrawLines(pen, blockedZones.Plan.Select(p => p.Point).ToArray());
        }

        public List<List<Point>> IncreasePolygons(List<List<Vertex>> List, int sign)
        {
            //Получаем от каждого угла векторы, нормируем * 10, 
            int epsilon = 15;
            Pen pen = new Pen(Color.Black, 2);
            List<List<Point>> polygon = new List<List<Point>>();
            for (int k = 0; k < List.Count; k++)
            {
                var pol = List[k];
                List<Point> biss = new List<Point>();
                Point vector1 = pol[pol.Count - 2].Point;
                Point vector2 = pol[0].Point;
                Point vector3 = pol[1].Point;
                vector1 = new Point(vector1.X - vector2.X, vector1.Y - vector2.Y);
                vector3 = new Point(vector3.X - vector2.X, vector3.Y - vector2.Y);
                vector1 = Geometry.NormalizeVector(vector1, epsilon);
                vector3 = Geometry.NormalizeVector(vector3, epsilon);

                vector2 = Geometry.NormalizeVector(new Point(vector1.X + vector3.X, vector1.Y + vector3.Y), epsilon);
                var p = new Point(pol[0].Point.X + sign * vector2.X, pol[0].Point.Y + sign * vector2.Y);
                if (!IntersectionFuncs.IsInside(new Vertex(p), pol) && sign > 0 || IntersectionFuncs.IsInside(new Vertex(p), pol) && sign < 0)
                    p = new Point(pol[0].Point.X + (-1) * sign * vector2.X, pol[0].Point.Y + (-1) * sign * vector2.Y);
                biss.Add(p);

                for (int i = 1; i < pol.Count - 1; i++)
                {
                    vector1 = pol[i - 1].Point;
                    vector2 = pol[i].Point;
                    vector3 = pol[i + 1].Point;
                    vector1 = new Point(vector1.X - vector2.X, vector1.Y - vector2.Y);
                    vector3 = new Point(vector3.X - vector2.X, vector3.Y - vector2.Y);
                    vector1 = Geometry.NormalizeVector(vector1, epsilon);
                    vector3 = Geometry.NormalizeVector(vector3, epsilon);
                    vector2 = Geometry.NormalizeVector(new Point(vector1.X + vector3.X, vector1.Y + vector3.Y), epsilon);
                    p = new Point(pol[i].Point.X + sign * vector2.X, pol[i].Point.Y + sign * vector2.Y);
                    if (!IntersectionFuncs.IsInside(new Vertex(p), pol) && sign > 0 || IntersectionFuncs.IsInside(new Vertex(p), pol) && sign < 0)
                        p = new Point(pol[i].Point.X + (-1)*sign * vector2.X, pol[i].Point.Y + (-1) * sign * vector2.Y);
                    biss.Add(p);
                }
                biss.Add(biss[0]);
                polygon.Add(biss);
            }
            return polygon;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            blockedZones.endVert.Clear();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            koefError = (int)numericUpDown1.Value;
            Console.WriteLine(koefError);
        }
    }
}

/*        public void setBackDistances(Vertex a)
        {
            a.Cost = double.MaxValue;
            a.ParentVertex = null;
        }

        public void setBackDistances(List<List<Vertex>> c)
        {
            //Обнуление цен у вершин полигонов, плана и старт/конеч точек
            foreach (var list in c)
                foreach (var item in list)
                {
                    item.Cost = double.MaxValue;
                    item.ParentVertex = null;
                }
        }

        public void setBackDistances(List<Vertex> d)
        {
            //Обнуление цен у вершин полигонов, плана и старт/конеч точек
            foreach (var item in d)
            {
                item.Cost = double.MaxValue;
                item.ParentVertex = null;
            }
        }*/

/*                    setBackDistances(blockedZones.startVert);
            foreach (var p in blockedZones.endVert)
                setBackDistances(p);
            setBackDistances(blockedZones.Points);
            setBackDistances(blockedZones.Plan);
            blockedZones.endVert[j+1].Cost = 0;
            blockedZones.endVert[j + 1].ParentVertex = null;*/

/*                    foreach (var p in blockedZones.endVert)
            setBackDistances(p);
        setBackDistances(blockedZones.startVert);
        setBackDistances(blockedZones.Points);
        setBackDistances(blockedZones.Plan);
        blockedZones.endVert[0].Cost = 0;
        blockedZones.endVert[0].ParentVertex = null;*/