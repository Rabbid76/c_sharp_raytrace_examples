using rt_1_in_one_week.Mathematics;

namespace rt_1_in_one_week.Process
{
    public interface ISamapler
    {
        bool Next(out Vec3 direction);
    }
}
