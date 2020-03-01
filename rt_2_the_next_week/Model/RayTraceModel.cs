using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using rt_2_the_next_week.ViewModel;
using rt_2_the_next_week.Mathematics;
using rt_2_the_next_week.Color;
using rt_2_the_next_week.Process;

namespace rt_2_the_next_week.Model
{
    public class RayTraceModel
    {
        private RayTraceViewModel _vm;
        private Thread _rt_thread;
        private bool _render = false;

        public RayTraceModel()
        { }

        public RayTraceViewModel ViewModel 
        {
            set { _vm = value; }
        }

        public void StartRayTrace()
        {
            _render = true;
            _rt_thread = new Thread(RayTraceThread);
            _rt_thread.Start(this);
        }

        public void TerminateRayTrace()
        {
            _render = false;
            _rt_thread?.Join();
        }

        private static void RayTraceThread(Object obj)
        {
            var model = obj as RayTraceModel;
            var viewModel = model._vm;

            // get current settings
            var cx = viewModel.BitmapWidth;
            var cy = viewModel.BitmapHeight;
            var no_samples = viewModel.RenderSamples;
            var update_rate = viewModel.RenderUpdateRate;
            int no_samples_outer = Math.Max(1, (int)(no_samples * update_rate + 0.5));
            int no_samples_inner = (no_samples + no_samples_outer - 1) / no_samples_outer;
            var scene = Int32.Parse(viewModel.CurrentScene.Number);

            var aspect = (double)cx / (double)cy;
            var sampler = new Random();
            var camera = Camera.CreateByVerticalFiled(90, aspect);
            IHitable world;

            switch (scene)
            {
                default:

                // cover scene 1
                case 0:
                    {
                        List<IHitable> hitables = new List<IHitable>();
                        hitables.Add(new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(ConstantTexture.Create(0.5, 0.5, 0.5))));
                        for (int a = -11; a < 11; ++a)
                        {
                            for (int b = -11; b < 11; ++b)
                            {
                                double choos_mat = sampler.NextDouble();
                                Vec3 center = Vec3.Create(a + 0.9 * sampler.NextDouble(), 0.2, b + 0.9 * sampler.NextDouble());
                                if ((center - Vec3.Create(4, 0.2, 0)).Length > 0.9)
                                {
                                    if (choos_mat < 0.8) // diffuse
                                        hitables.Add(new Sphere(center, 0.2,
                                            new Lambertian(ConstantTexture.Create(sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble()))));
                                    else if (choos_mat < 0.9) // metal
                                        hitables.Add(new Sphere(center, 0.2,
                                            new Metal(ConstantTexture.Create(0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble())), 0.5 * sampler.NextDouble())));
                                    else // glass
                                        hitables.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                                }
                            }
                        }
                        hitables.Add(new Sphere(Vec3.Create(0, 1, 0), 1, new Dielectric(1.5)));
                        hitables.Add(new Sphere(Vec3.Create(-4, 1, 0), 1, new Lambertian(ConstantTexture.Create(0.4, 0.2, 0.1))));
                        hitables.Add(new Sphere(Vec3.Create(4, 1, 0), 1, new Metal(ConstantTexture.Create(0.7, 0.6, 0.5), 0)));
                        world = new BVHNode(hitables.ToArray(), 0, 0);
                        var lookFrom = Vec3.Create(12, 2, 3);
                        var lookAt = Vec3.Create(0, 0, 0);
                        double dist_to_focus = 10;
                        double aderpture = 0.1;
                        camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
                    }
                    break;

                // cover scene 1 motion
                case 4:
                    {
                        List<IHitable> hitables = new List<IHitable>();
                        hitables.Add(new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(CheckerTexture.Create(0.2, 0.3, 0.1, 0.9, 0.9, 0.9))));
                        for (int a = -11; a < 11; ++a)
                        {
                            for (int b = -11; b < 11; ++b)
                            {
                                double choos_mat = sampler.NextDouble();
                                Vec3 center = Vec3.Create(a + 0.9 * sampler.NextDouble(), 0.2, b + 0.9 * sampler.NextDouble());
                                if ((center - Vec3.Create(4, 0.2, 0)).Length > 0.9)
                                {
                                    if (choos_mat < 0.8) // diffuse
                                    {
                                        hitables.Add(new MovingSphere(center, center + Vec3.Create(0, 0.5, 0) * sampler.NextDouble(), 0, 1, 0.2,
                                            new Lambertian(ConstantTexture.Create(sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble()))));
                                    }
                                    else if (choos_mat < 0.9) // metal
                                        hitables.Add(new Sphere(center, 0.2,
                                            new Metal(ConstantTexture.Create(0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble())), 0.5 * sampler.NextDouble())));
                                    else // glass
                                        hitables.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                                }
                            }
                        }
                        hitables.Add(new Sphere(Vec3.Create(0, 1, 0), 1, new Dielectric(1.5)));
                        hitables.Add(new Sphere(Vec3.Create(-4, 1, 0), 1, new Lambertian(ConstantTexture.Create(0.4, 0.2, 0.1))));
                        hitables.Add(new Sphere(Vec3.Create(4, 1, 0), 1, new Metal(ConstantTexture.Create(0.7, 0.6, 0.5), 0)));
                        double time0 = 0;
                        double time1 = 1;
                        world = new BVHNode(hitables.ToArray(), time0, time1);
                        var lookFrom = Vec3.Create(12, 2, 3);
                        var lookAt = Vec3.Create(0, 0, 0);
                        double dist_to_focus = 10;
                        double aderpture = 0.1;
                        camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus, time0, time1);
                    }
                    break;

                // materials
                case 1:
                    {
                        IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -100.5, 0), 100, new Lambertian(ConstantTexture.Create(0.8, 0.8, 0.0))),
                            new Sphere(Vec3.Create(0, 0, 0), 0.5, new Lambertian(ConstantTexture.Create(0.1, 0.2, 0.5))),
                            new Sphere(Vec3.Create(1, 0, 0), 0.5, new Metal(ConstantTexture.Create(0.8, 0.6, 0.2), 0.3)),
                            new Sphere(Vec3.Create(-1, 0, 0), 0.5, new Dielectric(1.5)),
                            new Sphere(Vec3.Create(-1, 0, 0), -0.45, new Dielectric(1.5))
                        };
                        world = new BVHNode(hitables, 0, 1);
                        camera = Camera.CreateLookAt(Vec3.Create(0.25, 0.5, 2.2), Vec3.Create(0.1, 0.0, 0), Vec3.Create(0, 1, 0), 45, aspect, 0, 1);
                    }
                    break;

                // defocus blur
                case 2:
                    {
                        IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -100.5, -1), 100, new Lambertian(ConstantTexture.Create(0.8, 0.8, 0.0))),
                            new Sphere(Vec3.Create(0, 0, -1), 0.5, new Lambertian(ConstantTexture.Create(0.1, 0.2, 0.5))),
                            new Sphere(Vec3.Create(1, 0, -1), 0.5, new Metal(ConstantTexture.Create(0.8, 0.6, 0.2), 0.3)),
                            new Sphere(Vec3.Create(-1, 0, -1), 0.5, new Dielectric(1.5)),
                            new Sphere(Vec3.Create(-1, 0, -1), -0.45, new Dielectric(1.5))
                        };
                        world = new BVHNode(hitables, 0, 1);
                        var lookFrom = Vec3.Create(3, 3, 2);
                        var lookAt = Vec3.Create(0, 0, -1);
                        var dist_to_focus = (lookFrom - lookAt).Length;
                        double aderpture = 2;
                        camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 25, aspect, aderpture, dist_to_focus);
                    }
                    break;

                // test 1
                case 3:
                    {
                        double R = Math.Cos(Math.PI / 4);
                        IHitable[] hitables = {
                            new Sphere(Vec3.Create(-R, 0, -1), R, new Lambertian(ConstantTexture.Create(1, 0, 0))),
                            new Sphere(Vec3.Create(R, 0, -1), R, new Lambertian(ConstantTexture.Create(0, 0, 1)))
                        };
                        world = new BVHNode(hitables, 0, 1);
                        camera = Camera.CreateByVerticalFiled(90, aspect);
                    }
                    break;

                // checker texture
                case 5:
                    {
                        var checker_texture = CheckerTexture.Create(0.2, 0.3, 0.2, 0.9, 0.9, 0.9);
                        IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -10, 0), 10, new Lambertian(checker_texture)),
                            new Sphere(Vec3.Create(0, 10, 0), 10, new Lambertian(checker_texture))
                        };
                        world = new BVHNode(hitables, 0, 1);
                        var lookFrom = Vec3.Create(13, 2, 3);
                        var lookAt = Vec3.Create(0, 0, 0);
                        double dist_to_focus = 10;
                        double aderpture = 0;
                        camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
                    }
                    break;

                // noise texture
                case 6:
                    {
                        var perlin_texture = NoiseTexture.Create(2, NoiseTexture.Type.SIN_Z);
                        IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(perlin_texture)),
                            new Sphere(Vec3.Create(0, 2, 0), 2, new Lambertian(perlin_texture))
                        };
                        world = new BVHNode(hitables, 0, 1);
                        var lookFrom = Vec3.Create(13, 2, 3);
                        var lookAt = Vec3.Create(0, 0, 0);
                        double dist_to_focus = 10;
                        double aderpture = 0;
                        camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
                    }
                    break;
            }

            var colorBuffer = new RGBdBuffer(cx, cy);
            int processed = 0;
            for (int outer_i = 0; model._render && (outer_i * no_samples_inner) < no_samples; ++outer_i)
            {
                foreach ((int x, int y) in new IterateBufferExp2(cx, cy))
                {
                    if (model._render == false)
                        break;

                    int no_start = no_samples_inner * outer_i;
                    int no_end = Math.Min(no_samples, no_start + no_samples_inner);
                    for (int sample_i = no_start; model._render && sample_i < no_end; ++sample_i)
                    {
                        (double dx, double dy) = ((double)x + sampler.NextDouble(), (double)y + sampler.NextDouble());
                        (double u, double v) = (dx / cx, dy / cy);
                        var sampleColor = RaytraceColor(camera.Get(u, v), world, 0);
                        colorBuffer.Set(x, y, sampleColor, sample_i);
                    }

                    var col = colorBuffer.Get(x, y);
                    viewModel.SetBitmapPixel(x, cy - y - 1, ColorFactory.CreateSquare(col));
                    viewModel.Progress = (double)(++processed) / (double)(cx * cy * no_samples_outer);

                    //Thread.Yield();
                    //Thread.SpinWait(1);
                    //Thread.Sleep(1);
                }
            }
        }

        private static Vec3 CreateSky(Ray r)
        {
            var unit_direction = Vec3.Normalize(r.Direction);
            var t = unit_direction.Y * 0.5 + 0.5;
            var v = new Vec3(1.0) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
            return v;
        }

        private static Vec3 RaytraceColor(Ray r, IHitable hitable, int depth)
        {
            HitRecord rec;
            if (hitable.Hit(r, 0.001, Double.MaxValue, out rec))
            {
                Ray scattered;
                Vec3 attenuation;
                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return attenuation * RaytraceColor(scattered, hitable, depth + 1);
                }
                return Vec3.Create(0.0);
            }
            return CreateSky(r);
        }
    }
}
