using System;
using System.Collections.Generic;
using signals;

namespace notifier
{
    public class Notifier<T> : Signal1<T>
    {
        public bool requireChange = true;

        private T _value;
        private string id;

        public T Value
        {
            get
            {
                return this._value;
            }
            set
            {
                //Console.WriteLine("SET");
                //this._value = value;

                if (!changeRequired(value)) return;
                this._value = value;
                this.Dispatch(value);
            }
        }

        private bool changeRequired(T value)
        {
            return !EqualityComparer<T>.Default.Equals(_value, value) || !requireChange;
        }

        public Notifier()
        {

        }

        public Notifier(T defaultValue, string id = "", bool fireOnAdd = false) 
        {
            this._value = defaultValue;
            this.id = id;
        }
    }
}
