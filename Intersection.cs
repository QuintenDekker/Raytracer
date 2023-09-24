using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class Intersection
    {
        public Ray ray;
        public Shape shape;
        public Vector3 position;
        public Intersection(Ray ray, Shape shape, Vector3 position)
        {
            this.ray = ray;
            this.shape = shape;
            this.position = position;
        }
    }
}
