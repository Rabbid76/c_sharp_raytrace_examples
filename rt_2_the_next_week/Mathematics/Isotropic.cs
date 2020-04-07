using System;
using System.Collections.Generic;
using System.Text;

namespace rt_2_the_next_week.Mathematics
{
    public class Isotropic
        : Material
    {
        ITexture _albedo;

        public Isotropic(ITexture albedo)
        {
            _albedo = albedo;
        }

        public override bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            scattered = new Ray(rec.P, Sample.RandomInUnitSphere(), rec.T);
            attenuation = _albedo.Value(rec.U, rec.V, rec.P);
            return true;
        }
    }
}
