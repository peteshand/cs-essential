using UnityEngine;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using comms;

namespace comms
{
    public class JsConnection : MonoBehaviour, IConnection
    {
        [DllImport("__Internal")]
        public static extern void UnityToJs(string message);

        Dictionary<string, Action<string>> callbacks = new Dictionary<string, Action<string>>();
        
        public void Init(dynamic settings) {
            Debug.Log("JsConnection");
        }

        public void Send(string id, string message) {
            #if UNITY_WEBGL
            UnityToJs(message);
            #endif
        }

        public void On(string id, Action<string> callback) {
            callbacks.Add(id, callback);
        }

        public void Close()
        {
            
        }
        
        public void JsToUnity(string strValue)
        {
            CommsMessage message = JsonUtility.FromJson<CommsMessage>(strValue);
            foreach (var callback in callbacks) {
                if (callback.Key == message.id){
                    callback.Value(message.payload);
                }
            }
        }
    }
}