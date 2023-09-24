using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracer
{
    internal class World
    {
        const float fps = 10;
        const float timerInterval = 1000 / fps;
        const float halfF = (float)1 / (float)2;

        Vector3 zeroVector = new Vector3(0, 0, 0);
        const float camToScreenDist = halfF; // changed by fov

        Bitmap bm;
        Form form;

        List<Shape> shapes;
        Camera camera;
        Screen screen;

        float screenStepSizeWide;
        float screenStepSizeHigh;
        public World(Bitmap bm, Form form)
        {
            this.bm = bm;
            this.form = form;

            shapes = new List<Shape>()
            {
                new Sphere(new Vector3(0,0,10), Color.Blue, 3)
            };

            camera = new Camera(zeroVector, new Vector3(0, 0, 1), new Vector3(0, 1, 0));
            screen = new Screen(camera.position + Vector3.Multiply(camera.facing, camToScreenDist), new Vector3(0, 1, 0), new Vector3(1, 0, 0));

            screenStepSizeWide = (float)1 / bm.Width;
            screenStepSizeHigh = (float)1 / bm.Height;

            //StartTicks();

            Tick();
        }

        // for when things actually move
        private async void StartTicks()
        {
            PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(timerInterval));
            while (await timer.WaitForNextTickAsync())
            {
                //MessageBox.Show("Tick");
                Tick();
            }
        }

        private void Tick()
        {
            // draw rays from camera to each pixel in screen
            // intersect with all shapes
            // if intersect, set color to shape's color
            for (float i = -1 * halfF; i <= halfF; i+=screenStepSizeWide)
            {
                int bmX = (int)((i + halfF) * bm.Width);
                for (float j = -1 * halfF; j <= halfF; j+= screenStepSizeHigh)
                {
                    //MessageBox.Show("yes " + (j + halfF));
                    int bmY = (int)((j + halfF) * bm.Height);
                    //MessageBox.Show("" + bmY);
                    Vector3 screenPixel = screen.centerPos + i * screen.right + j * screen.up;

                    Ray ray = new Ray(camera.position, Vector3.Normalize(screenPixel - camera.position));

                    //MessageBox.Show("Trying " + i + ", " + j);

                    Intersection closestIntersect = IntersectEach(ray);

                    if (closestIntersect != null)
                    {
                        //MessageBox.Show("drawing " + bmX + ", " + bmY);
                        bm.SetPixel(bmX, bmY, closestIntersect.shape.color);
                    }
                }
            }

            form.Invalidate();
        }

        private Intersection IntersectEach(Ray ray)
        {
            Intersection closestIntersection = null!;
            double closestDistance = 0;
            foreach (Shape shape in shapes)
            {
                Intersection intersection = ray.Intersect(shape);
                if (intersection != null)
                {
                    double dist = Vector3.Distance(camera.position, intersection.position);
                    if (closestIntersection == null || dist < closestDistance)
                    {
                        //MessageBox.Show("new closer");
                        closestIntersection = intersection;
                        closestDistance = dist;
                    }
                }
            }
            return closestIntersection!;
        }
    }
}
