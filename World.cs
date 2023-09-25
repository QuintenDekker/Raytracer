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
        List<Light> lights;
        Camera camera;
        Screen screen;

        float screenStepSizeWide;
        float screenStepSizeHigh;

        // when intersecting shadow ray with shapes, might find shape itself as blocking
        // so need some distance from intersection point before allowing blocking
        float epsilon = 0.0001f;
        public World(Bitmap bm, Form form)
        {
            this.bm = bm;
            this.form = form;

            shapes = new List<Shape>()
            {
                new Sphere(new Vector3(0,3,10), Color.Blue, 3, Material.Diffuse),
                new Plane(new Vector3(0,0,0),new Vector3(0,1,0), true, Material.None)
            };

            lights = new List<Light>()
            {
                new Light(new Vector3(0,3,0), 50)
            };

            camera = new Camera(new Vector3(0,3,0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
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
                    // height - y because y = 0 is at the top, but must be less than height so - 1
                    int bmY = bm.Height - (int)((j + halfF) * bm.Height) - 1;

                    Vector3 screenPixel = screen.centerPos + i * screen.right + j * screen.up;

                    Ray ray = new Ray(camera.position, Vector3.Normalize(screenPixel - camera.position));

                    Intersection closestIntersect = IntersectEach(ray);

                    if (closestIntersect != null)
                    {
                        float shadowModifier = 0;

                        foreach (Light light in lights)
                        {
                            Ray shadowRay = new Ray(closestIntersect.position, Vector3.Normalize(light.position - closestIntersect.position));
                            shadowModifier += IntersectEachLight(light, shadowRay);
                        }

                        Color c;
                        if (closestIntersect.shape is Plane)
                        {
                            Plane plane = (Plane)closestIntersect.shape;
                            if (plane.checkboard)
                            {
                                // decide for both x and y whether it's an odd or even coordinate
                                // summing them results in 0, 1 or 2
                                // even sum means black, uneven sum means white
                                double sum = Math.Floor(closestIntersect.position.X) % 2 + Math.Floor(closestIntersect.position.Z) % 2;
                                if (sum % 2 == 0)
                                {
                                    c = Color.Black;
                                } else
                                {
                                    c = Color.White;
                                }
                            } else
                            {
                                c = plane.color;
                            }
                        } else
                            c = closestIntersect.shape.color;

                        c = Color.FromArgb(shadowify(c.R, shadowModifier), shadowify(c.G, shadowModifier), shadowify(c.B, shadowModifier));


                        bm.SetPixel(bmX, bmY, c);
                    }
                }
            }

            form.Invalidate();
        }

        // intersects the given ray with each shape in the world
        // finds closest intersection
        private Intersection IntersectEach(Ray ray)
        {
            Intersection closestIntersection = null!;
            foreach (Shape shape in shapes)
            {
                Intersection intersection = ray.Intersect(shape);
                if (intersection != null && (closestIntersection == null || intersection.t < closestIntersection.t))
                {
                    closestIntersection = intersection;
                }
            }
            return closestIntersection!;
        }

        // intersects the given ray with each shape in the world
        // if it intersects any, return true
        private float IntersectEachLight(Light light, Ray ray)
        {
            foreach (Shape shape in shapes)
            {
                Intersection intersection = ray.Intersect(shape);
                if (intersection != null && intersection.t > epsilon)
                {
                    return Math.Max(1 - (Vector3.Distance(ray.origin, light.position) / light.distance), 0);
                }
            }
            return Math.Max(1 - (Vector3.Distance(ray.origin, light.position) / light.distance), 0);
        }

        private int shadowify(byte color, float shadowModifier)
        {
            return (int)Math.Max(0, Math.Min(255, color * shadowModifier));
        }
    }
}
