using System;

namespace signals
{
    public class Signal2<T1, T2> : BaseSignal
    {
        private T1 value1;
        private T2 value2;

        public void Dispatch(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
            fire();
        }

        public void Add(Action<T1, T2> callback)
        {
            Handlers += () =>
            {
                callback(value1, value2);
            };
        }

    }
}
