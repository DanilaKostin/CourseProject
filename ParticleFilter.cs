using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_on_forms_beta1
{
    public class Particle
    {
        public Point Point;
        public double Orientation;
        public double Weight;
        public Particle(Point point, double orientation, double weight) 
        {  
            Point = point; 
            Orientation = orientation;
            Weight = weight;    
        }
    }
    public class ParticleFilter
    {
        public List<Particle> particle;
/*        public ParticleFilter(int count, int max_x, int max_y) 
        {
            GenerateParticles(count, max_x, max_y);
        }*/

        public void GenerateParticles(int count, int max_x, int max_y, List<List<Vertex>> holes, List<Vertex> plan)
        {
            var r = new Random();
            double weigth = 1 / count;
            
            particle = new List<Particle>();
            int current_count = 0;
            while(current_count < count)
            {
                int r_x = r.Next(max_x);
                int r_y = r.Next(max_y);
                double orientation = 2 * Math.PI / r.Next(360);
                Point p = new Point(r_x, r_y);
                if (!IntersectionFuncs.IsInsideAny(new Vertex(p), holes) && IntersectionFuncs.IsInside(new Vertex(p), plan))
                {
                    particle.Add(new Particle(new Point(r_x, r_y), orientation, weigth));
                    current_count++;
                }
            }
        }
    }
}
