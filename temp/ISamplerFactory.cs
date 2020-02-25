namespace rt_1_in_one_week.Process
{
    public interface ISamplerFactory
    {
        ISamapler Create(int depth, int x, int y);
    }
}
