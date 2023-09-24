using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class Ray
    {
        public Vector3 origin;
        public Vector3 direction;
        public Ray(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Intersection Intersect(Shape shape)
        {
            if (shape is Sphere)
            {
                Sphere sphere = (Sphere)shape;
                return IntersectSphere(sphere);
            }
            return null;
        }

        private Intersection IntersectSphere(Sphere sphere)
        {
            Vector3 oc = origin - sphere.center;
            float a = Vector3.Dot(direction, direction);
            float b = 2 * Vector3.Dot(oc, direction);
            float c = Vector3.Dot(oc, oc) - sphere.radius * sphere.radius;
            float d = b * b - 4 * a * c;
            if (d >= 0)
            {
                //MessageBox.Show("d >= 0");
                double t1 = (-1 * b + Math.Sqrt(d)) / (2 * a);
                double t2 = (-1 * b - Math.Sqrt(d)) / (2 * a);

                double t = Math.Max(t1, t2);

                if (t > 0)
                {
                    //MessageBox.Show("Intersection");
                    return new Intersection(this, sphere, origin + (float)t * direction);
                }
            }
            return null!;
        }
    }
}
