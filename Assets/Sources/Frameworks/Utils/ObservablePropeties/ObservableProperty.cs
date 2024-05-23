using System;

namespace Sources.Utils.ObservablePropeties
{
    public class ObservableProperty<T>
    {
        private T _value;

        public ObservableProperty(T value = default)
        {
            _value = value;
        }

        public event Action Changed;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke();
            }
        }

        public string StringValue => Value.ToString();
        public T GetValue => Value;
    }
}