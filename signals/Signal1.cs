using System;

namespace signals
{
    public class Signal1<T> : BaseSignal
    {
        private T value;

        public void Dispatch(T value)
        {
            this.value = value;
            fire();
        }

        public void Add(Action<T> callback)
        {
            Handlers += () =>
            {
                callback(value);
            };
        }

    }
}
