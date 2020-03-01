namespace rt_2_the_next_week.Mathematics
{
    public interface INoise
    {
        double Noise(Vec3 p);
        double Turb(Vec3 p, int depth = 7);
    }
}
