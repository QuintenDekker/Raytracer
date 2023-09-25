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
        float maxT = 100;

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
            } else if (shape is Plane)
            {
                Plane plane = (Plane)shape;
                return IntersectPlane(plane);
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

                double tfar = Math.Max(t1, t2);
                double tnear = Math.Min(t1, t2);

                if (tnear > 0)
                {
                    return new Intersection(this, sphere, origin + (float)tnear * direction, (float)tnear);
                } else if (tfar > 0)
                {
                    return new Intersection(this, sphere, origin + (float)tfar * direction, (float)tfar);
                }
            }
            return null!;
        }

        private Intersection IntersectPlane(Plane plane)
        {
            float t = (Vector3.Dot(plane.center - origin, plane.normal)) / Vector3.Dot(direction, plane.normal);

            if (t >= 0 && t < maxT)
            {
                Vector3 intersectionPoint = origin + t * direction;
                return new Intersection(this, plane, intersectionPoint, t);
            }

            return null!;
        }
    }
}
