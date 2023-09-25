using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    public class Shape
    {
        public Color color;
    }

    public class Sphere : Shape
    {
        public Vector3 center;
        public float radius;
        public Sphere(Vector3 center, Color color, float radius)
        {
            this.center = center;
            this.color = color;
            this.radius = radius;
        }

    }

    public class Plane : Shape
    {
        public Vector3 center;
        public Vector3 normal;
        public bool checkboard;

        public Plane(Vector3 center, Vector3 normal, bool checkboard)
        {
            this.center = center;
            this.normal = normal;
            this.checkboard = checkboard;
        }
    }
}
