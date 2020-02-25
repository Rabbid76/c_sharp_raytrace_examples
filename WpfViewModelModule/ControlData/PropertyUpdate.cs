using System;

namespace WpfViewModelModule.ControlData
{
    public class PropertyUpdate<T>
    {
        private T _property;
        private string _name;
        Action<string> _update;
        public PropertyUpdate(T property, string name, Action<string> update)
        {
            _property = property;
            _name = name;
            _update = update;
        }
        public static implicit operator T(PropertyUpdate<T> p) => p._property;
        public void Set(T property)
        {
            _property = property;
            _update(_name);
        }
    }
}
