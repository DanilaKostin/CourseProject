using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace course_on_forms_beta1
{
    internal class PointsReader
    {
        static public List<List<Vertex>> ReadBlockedZones(string FilePath)
        {
            Regex regex = new Regex(@"\d+,\s.\d+");

            var polygons = new List<List<Vertex>>();

            string[] lines = File.ReadAllLines(FilePath);

            for (int i = 0; i < lines.Count(); i++) 
            {
                var matches = regex.Matches(lines[i]);
                var points = new List<Vertex>();
                foreach (Match match in matches)
                {
                    var x = int.Parse(match.Value.Replace(",", "").Split(' ')[0]);;
                    var y = int.Parse(match.Value.Replace(",", "").Split(' ')[1]);
                    points.Add(new Vertex(new Point(x, y)));
                    Console.WriteLine(x + " " + y);
                }
                polygons.Add(points);
            }

            return polygons;
        }

        static public List<Vertex> ReadPlan(string FilePath)
        {
            Regex regex = new Regex(@"\d+,\s.\d+");
            var polygon = new List<Vertex>();

            string lines = File.ReadAllLines(FilePath).First();

            var matches = regex.Matches(lines);
            foreach (Match match in matches)
            {
                var x = int.Parse(match.Value.Replace(",", "").Split(' ')[0]);
                var y = int.Parse(match.Value.Replace(",", "").Split(' ')[1]);
                polygon.Add(new Vertex(new Point(x, y)));
                Console.WriteLine(x + " " + y);
            }
            return polygon;
        }
    }
}
