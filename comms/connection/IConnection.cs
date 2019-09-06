

using System;
using UnityEngine;

namespace comms
{
    public interface IConnection
    {
        void Init(dynamic settings = null); //int port, string multicastAddr
        void Send(string id, string message); // P2PMessage message
        void On(string id, Action<string> callback); // Action<P2PMessage> callback
        void Close();
    }
}