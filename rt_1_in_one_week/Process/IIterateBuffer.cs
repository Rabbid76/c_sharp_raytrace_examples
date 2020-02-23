namespace rt_1_in_one_week.Process
{
    interface IIterateBuffer
    {
        bool Next(out int x, out int y);
        void Reset();
    }
}
