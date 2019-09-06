using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using time;
using System.Collections.Generic;
using comms;

namespace comms
{
    public class DatagramConnection : MonoBehaviour, IConnection
    {
        UdpClient udpclient;
        int port;
        IPAddress multicastIPaddress;
        IPAddress localIPaddress = IPAddress.Any;
        IPEndPoint localEndPoint;
        IPEndPoint remoteEndPoint;

        UTF8Encoding encoding = new UTF8Encoding ();
        Dictionary<string, Action<string>> callbacks = new Dictionary<string, Action<string>>();
        //Type P2PMessageType;

        public DatagramConnection()
        {
            //P2PMessageType = typeof(P2PMessage);
        }

        public void Init(dynamic settings = null) // IPAddress multicastIPaddress, int port, IPAddress localIPaddress = null
        {
            //if (settings == null){
                port = 33333;
                multicastIPaddress = IPAddress.Parse("233.255.255.255");
            if (settings != null){
                int _port = (int)settings.GetType().GetProperty("port").GetValue(settings, null);
                string _multicastAddr = (string)settings.GetType().GetProperty("multicastAddr").GetValue(settings, null);
                if (_multicastAddr != null){
                    multicastIPaddress = IPAddress.Parse(_multicastAddr);
                }
            }
 
            // Create endpoints
            //IPAddress address = IPAddress.Parse("233.255.255.255");
            //int port = 0;
            remoteEndPoint = new IPEndPoint(multicastIPaddress, port);
            localEndPoint = new IPEndPoint(localIPaddress, port);
 
            // Create and configure UdpClient
            udpclient = new UdpClient();
            // The following three lines allow multiple clients on the same PC
            udpclient.ExclusiveAddressUse = false;
            udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpclient.ExclusiveAddressUse = false;
            // Bind, Join
            udpclient.Client.Bind(localEndPoint);
            udpclient.JoinMulticastGroup(multicastIPaddress, localIPaddress);
 
            // Start listening for incoming data
            //udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);

            EnterFrame.Add(Tick);
        }

        void Tick()
        {
            //if (client.Available == 0) return;
            if (udpclient.Available == 0) return;

            // Get received data
            IPEndPoint sender = new IPEndPoint(0, 0);
            //Byte[] receivedBytes = udpclient.EndReceive(ar, ref sender);
            Byte[] receivedBytes = udpclient.Receive(ref sender); 
            string strValue = Encoding.ASCII.GetString(receivedBytes);
            //Debug.Log("strValue: " + strValue);

            CommsMessage message = JsonUtility.FromJson<CommsMessage>(strValue);
            foreach (var callback in callbacks) {
                if (callback.Key == message.id){
                    callback.Value(message.payload);
                }
            }
        }
 
        /// <summary>
        /// Send the buffer by UDP to multicast address
        /// </summary>
        /// <param name="bufferToSend"></param>
        public void SendMulticast(byte[] bufferToSend)
        {
            udpclient.Send(bufferToSend, bufferToSend.Length, remoteEndPoint);
        }

        public void Send(string id, string message) {
            //string Jsonstring = JsonUtility.ToJson(message);
            byte[] data = encoding.GetBytes (message);
            //Debug.Log("Send: " + message);
            //Debug.Log(Jsonstring);
            udpclient.Send (data, data.Length, remoteEndPoint);
        }
 
        /// <summary>
        /// Callback which is called when UDP packet is received
        /// </summary>
        /// <param name="ar"></param>
        /*
        private void ReceivedCallback(IAsyncResult ar)
        {
            // Get received data
            IPEndPoint sender = new IPEndPoint(0, 0);
            Byte[] receivedBytes = udpclient.EndReceive(ar, ref sender);
            string returnData = Encoding.ASCII.GetString(receivedBytes);
            Debug.Log("Receive: " + returnData);

            P2PMessage message = (P2PMessage)JsonUtility.FromJson(returnData, P2PMessageType);
            Debug.Log("id = " + message.id);

            /* foreach (var callback in callbacks) {
                if (callback.Key == message.id){
                    callback.Value(message);
                }
            }*/

            // fire event if defined
            /*
            if (UdpMessageReceived != null)
                UdpMessageReceived(this, new UdpMessageReceivedEventArgs() { message = message });
 
            // Restart listening for udp data packages
            udpclient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
        }
        */
 
        /// <summary>
        /// Event handler which will be invoked when UDP message is received
        /// </summary>
        //public event EventHandler<UdpMessageReceivedEventArgs> UdpMessageReceived;
 
        /// <summary>
        /// Arguments for UdpMessageReceived event handler
        /// </summary>
        //public class UdpMessageReceivedEventArgs: EventArgs
        //{
        //    public P2PMessage message {get;set;}
        //}

        public void On(string id, Action<string> callback) { //Action<P2PMessage> callback
            callbacks.Add(id, callback);
        }

        public void Close()
        {
            udpclient.Close();
        }
    }
}