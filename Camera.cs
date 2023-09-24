using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class Camera
    {
        public Vector3 position;
        public Vector3 facing;
        public Vector3 up;
        public Camera(Vector3 loc, Vector3 facing, Vector3 up)
        {
            this.position = loc;
            this.facing = facing;
            this.up = up;
        }
        public void Move(Vector3 dir)
        {

        }
        public void Look(Vector3 dir)
        {

        }
    }
}
