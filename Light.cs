using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class Light
    {
        public Vector3 position;
        public float distance;
        public Light(Vector3 position, float distance)
        {
            this.position = position;
            this.distance = distance;
        }
    }
}
