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
    public class CommsMessage
    {
        public string payload;
        public string id;
        
        public CommsMessage(string payload, string id)
        {
            this.payload = payload;
            this.id = id;
        }
    }
}

