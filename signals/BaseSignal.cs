using System;
namespace signals
{
    public class BaseSignal
    {
        public delegate void SignalEventHandler();
        public event SignalEventHandler Handlers;

        public BaseSignal()
        {
        }

        protected void fire()
        {
            //Console.WriteLine("dispatch");
            Handlers?.Invoke();
        }
    }
}
