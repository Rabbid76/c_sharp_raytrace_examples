using System;

// [Where can I find the “clamp” function in .NET?](https://stackoverflow.com/questions/2683442/where-can-i-find-the-clamp-function-in-net)
// [How to force a number to be in a range in C#?](https://stackoverflow.com/questions/3176602/how-to-force-a-number-to-be-in-a-range-in-c)

namespace WpfViewModelModule
{
    public class RangeLimit<T> where T : IComparable<T>
    {
        public T Min { get; }
        public T Max { get; }
        public RangeLimit(T min, T max)
        {
            if (min.CompareTo(max) > 0)
                throw new InvalidOperationException("invalid range");
            Min = min;
            Max = max;
        }

        public void Validate(T param)
        {
            if (param.CompareTo(Min) < 0 || param.CompareTo(Max) > 0)
                throw new InvalidOperationException("invalid argument");
        }

        public T Clamp(T param) => param.CompareTo(Min) < 0 ? Min : param.CompareTo(Max) > 0 ? Max : param;
    }
}
