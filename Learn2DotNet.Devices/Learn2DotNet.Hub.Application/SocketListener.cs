using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Learn2DotNet.Hub.Domain;

namespace Learn2DotNet.Hub.Application
{
    public class SocketListener
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);

        public void StartListening()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 16100);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                AllDone.Reset();
                listener.BeginAccept(AcceptCallback, listener);
                AllDone.WaitOne();
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            AllDone.Set();

            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            StateObject state = new StateObject
            {
                WorkSocket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

                content = state.StringBuilder.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    Send(handler, content);
                }
                else
                {
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
                }
            }
        }

        private static void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            int bytesSent = handler.EndSend(ar);
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
