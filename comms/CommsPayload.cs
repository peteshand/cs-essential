using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using notifier;
using UnityEngine;
using static Trace;
using LitJson;

namespace comms
{
    [Serializable]
    public class CommsPayload<T>
    {
        public T value;
        
        public CommsPayload(T value)
        {
            this.value = value;
        }
    }
}

