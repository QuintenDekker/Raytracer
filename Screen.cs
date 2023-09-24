using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class Screen
    {
        public Vector3 centerPos;
        public Vector3 up;
        public Vector3 right;
        public Screen(Vector3 centerPos, Vector3 up, Vector3 right)
        {
            this.centerPos = centerPos;
            this.up = up;
            this.right = right;
        }
    }
}
