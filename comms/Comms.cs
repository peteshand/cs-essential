using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using LitJson;
using notifier;
using UnityEngine;
using static Trace;

namespace comms
{
	public class Comms
	{
        internal static Dictionary<string, dynamic> Broadcasters { get => broadcasters; set => broadcasters = value; }
        internal static Dictionary<string, dynamic> Subscribers { get => subscribers; set => subscribers = value; }

        private static Dictionary<string, dynamic> broadcasters = new Dictionary<string, dynamic>();
        private static Dictionary<string, dynamic> subscribers = new Dictionary<string, dynamic>();
        static public IConnection communication;

        public static Transform contextViewTransform;

        public static void initialize(Transform contextViewTransform)
        {
            Comms.contextViewTransform = contextViewTransform;
        }

        public static void install(IConnection communication) {
            Comms.communication = communication;
            Send("initialize", true);
        }

        public static void addBroadcast<T>(Notifier<T> notifier, string id) {
            if (Comms.communication == null) {
                trace("Comms.install(...) needs to be called before you can add broadcasters");
                return;
            }
            broadcasters.Add(id, new Broadcaster<T>(notifier, id));
        }

        public static void addSubscriber<T>(Notifier<T> notifier, string id) {
            if (Comms.communication == null) {
                trace("Comms.install(...) needs to be called before you can add subscribers");
                return;
            }
            subscribers.Add(id, new Subscriber<T>(notifier, id));
        }

        public static void Send<T>(string id, T payload)
        {
            if (Comms.communication == null) {
                trace("Comms.install(...) needs to be called before you can add broadcasters");
                return;
            }
            new Broadcaster<T>(payload, id);
        }

        public static void On<T>(string id, Action<T> callback) {
            if (Comms.communication == null) {
                trace("Comms.install(...) needs to be called before you can add subscribers");
                return;
            }
            subscribers.Add(id, new Subscriber<T>(callback, id));
        }

        public static void Close()
        {
            Comms.communication.Close();
        }

        private static Broadcaster<T> getBroadcaster<T>(string id)
        {
            dynamic broadcaster = null;
            broadcasters.TryGetValue(id, out broadcaster);
            if (broadcaster != null){
                Broadcaster<T> broadcaster2 = (Broadcaster<T>)broadcaster;
                return broadcaster2;
            }
            return null;
        }
	}

    class Broadcaster<T> {
        Notifier<T> notifier;
        string id;

        public Broadcaster(T payload, string id) {
            this.id = id;
            Send(payload);
        }

        public Broadcaster(Notifier<T> notifier, string id) {
            this.id = id;
            this.notifier = notifier;
            notifier.Add(Send);
        }

        public void Send(T value) {
            string payloadStr = JsonUtility.ToJson(new CommsPayload<T>(value));
            CommsMessage message = new CommsMessage(payloadStr, id);
            string Jsonstring = JsonUtility.ToJson(message);
            Comms.communication.Send(id, Jsonstring);
        }
    }

    class Subscriber<T> {
        Notifier<T> notifier;
        Action<T> callback;
        
        public Subscriber(Action<T> callback, string id) {
            this.callback = callback;
            Comms.communication.On(id, onMessage);
        }

        public Subscriber(Notifier<T> notifier, string id) {
            this.notifier = notifier;
            Comms.communication.On(id, onMessage);
        }

        void onMessage(string strPayload) {
            
            try {
                CommsPayload<T> payload = JsonUtility.FromJson<CommsPayload<T>>(strPayload);
                T v = payload.value;
                if (notifier != null){
                    notifier.Value = v;
                }
                if (callback != null){
                    callback(v);
                }
                
            } catch (Exception ex){
                Debug.Log("Exception: " + ex);
            }
        }

        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }
    }
}

