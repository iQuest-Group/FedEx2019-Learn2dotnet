﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Learn2DotNet.Hub.Application
{
    public class PairingUseCase
    {
        private const string localHost = "127.0.0.1";
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static string response = string.Empty;

        public void Start()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            ipAddress = IPAddress.Parse(localHost);

            for (int port = 16000; port < 16100; port++)
            {
                Task task = Task.Run(() => SendToDevice(ipAddress, port));
            }
        }

        private void SendToDevice(IPAddress ipAddress, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.BeginConnect(endPoint, ConnectCallback, client);
            connectDone.WaitOne();

            Send(client, "Hello from hub!");
            sendDone.WaitOne();

            Receive(client);
            receiveDone.WaitOne();
            
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static void Receive(Socket client)
        {
            StateObject state = new StateObject();
            state.WorkSocket = client;

            client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.WorkSocket;
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
            }
            else
            {
                if (state.StringBuilder.Length > 1)
                {
                    response = state.StringBuilder.ToString();
                }
                receiveDone.Set();
            }
        }

        private static void Send(Socket client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            connectDone.Set();
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesSent = client.EndSend(ar);
            sendDone.Set();
        }
    }

    public class StateObject
    {
        public Socket WorkSocket = null;
        public const int BufferSize = 256;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder StringBuilder = new StringBuilder();
    }
}