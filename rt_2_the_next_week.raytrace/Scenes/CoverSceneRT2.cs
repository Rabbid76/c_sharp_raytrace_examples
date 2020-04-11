using System;
using System.Collections.Generic;
using System.Reflection;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;
using rt_2_the_next_week.raytrace.Hitables.Instancing;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class CoverSceneRT2
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public CoverSceneRT2(double aspect)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            //string[] names = assembly.GetManifestResourceNames();
            var resource_stream = assembly.GetManifestResourceStream("rt_2_the_next_week.raytrace.Resource.worldmap1.png");
            var worldmap_texture = Texture.Create(resource_stream);
            var perlin_texture = NoiseTexture.Create(0.05, NoiseTexture.Type.SIN_Z);
            var hitables = new List<IHitable>();
            var sampler = new Random();
            var white = new Lambertian(ConstantTexture.Create(0.73, 0.73, 0.73));
            var ground = new Lambertian(ConstantTexture.Create(0.48, 0.83, 0.53));
            var light = new DiffuseLight(ConstantTexture.Create(7, 7, 7));
            var emat = new Lambertian(worldmap_texture);
            var boxList1 = new List<IHitable>();
            var nb = 20;
            for (int i = 0; i < nb; i ++)
            {
                for (int j = 0;  j < nb; j++)
                {
                    double w = 100;
                    var v_min = Vec3.Create(-1000 + i * w, 0, -1000 + j * w);
                    var v_max = Vec3.Create(v_min.X + w, 100 * (sampler.NextDouble() + 0.01), v_min.Z + w);
                    boxList1.Add(new Box(v_min, v_max, ground));
                }
            }
            var boxList2 = new List<IHitable>();
            int ns = 1000;
            for (int i = 0; i < ns; ++i)
            {
                boxList2.Add(new Sphere(Vec3.Create(165*sampler.NextDouble(), 165 * sampler.NextDouble(), 165 * sampler.NextDouble()), 10, white));
            }
            hitables.Add(new BVHNode(boxList1.ToArray(), 0, 1));
            hitables.Add(new XZRect(123, 423, 147, 412, 554, light));
            var center = Vec3.Create(400, 400, 200);
            hitables.Add(new MovingSphere(center, center + Vec3.Create(30, 0, 0), 0, 1, 50, 
                new Lambertian(new ConstantTexture(Vec3.Create(0.7, 0.3, 0.1)))));
            hitables.Add(new Sphere(Vec3.Create(260, 150, 45), 50, new Dielectric(1.5)));
            hitables.Add(new Sphere(Vec3.Create(0, 150, 145), 50, 
                new Metal(new ConstantTexture(Vec3.Create(0.8, 0.8, 0.9)), 10)));
            var boundary1 = new Sphere(Vec3.Create(360, 150, 145), 70, new Dielectric(1.5));
            hitables.Add(boundary1);
            hitables.Add(new ConstantMedium(boundary1, 0.2, new ConstantTexture(Vec3.Create(0.2, 0.4, 0.5))));
            var boundary2 = new Sphere(Vec3.Create(0), 5000, new Dielectric(1.5));
            hitables.Add(new ConstantMedium(boundary2, 0.0001, new ConstantTexture(Vec3.Create(1.0, 1.0, 1.0))));
            hitables.Add(new Sphere(Vec3.Create(400, 200, 400), 100, emat));
            hitables.Add(new Sphere(Vec3.Create(220, 280, 300), 80, new Lambertian(perlin_texture)));
            hitables.Add(new Translate(new RotateY(new BVHNode(boxList2.ToArray(), 0, 1), 15), Vec3.Create(-100, 270, 395)));
            World = new BVHNode(hitables.ToArray(), 0, 1);
            var lookFrom = Vec3.Create(478, 278, -600);
            var lookAt = Vec3.Create(278, 278, 0);
            double dist_to_focus = 10;
            double aderpture = 0;
            double vfov = 40;
            Camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), vfov, aspect, aderpture, dist_to_focus);
        }

        public Vec3 Sky(Ray r) => Vec3.Create(0);
    }
}
