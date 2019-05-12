using System;
using System.Threading;

namespace signals
{
    public class Signal0 : BaseSignal
    {
        public void Dispatch()
        {
            fire();
        }

        public void Add(Action callback)
        {
            Handlers += callback.Invoke;
        }
    }
}
