﻿using System;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Materials
{
    public class Dielectric
        : Material
    {
        private double _ref_idx;
        private Random _sampler = new Random();

        public Dielectric(double ref_idx)
        {
            _ref_idx = ref_idx;
        }

        public override bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 reflected = Vec3.Reflect(r_in.Direction, rec.Normal);
            attenuation = Vec3.Create(1.0);

            Vec3 outward_normal;
            double ni_over_nt;
            double consine;
            if (r_in.Direction.Dot(rec.Normal) > 0)
            {
                outward_normal = - rec.Normal;
                ni_over_nt = _ref_idx;
                consine = _ref_idx * r_in.Direction.Dot(rec.Normal) / r_in.Direction.Length;
            }
            else
            {
                outward_normal = rec.Normal;
                ni_over_nt = 1 / _ref_idx;
                consine = -r_in.Direction.Dot(rec.Normal) / r_in.Direction.Length;
            }

            Vec3 refracted;
            double reflect_probe;
            if (Vec3.Refract(r_in.Direction, outward_normal, ni_over_nt, out refracted))
            {
                reflect_probe = Function.Schlick(consine, _ref_idx);
            }
            else
            {
                reflect_probe = 1.0;
            }

            if (_sampler.NextDouble() < reflect_probe)
            {
                scattered = new Ray(rec.P, reflected, r_in.Time);
            }
            else
            {
                scattered = new Ray(rec.P, refracted, r_in.Time);
            }
            return true;
        }
    }
}