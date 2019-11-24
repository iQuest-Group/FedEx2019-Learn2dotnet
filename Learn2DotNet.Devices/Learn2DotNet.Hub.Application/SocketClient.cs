using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Learn2DotNet.Hub.Domain;
using Learn2DotNet.Hub.Domain.Models;

namespace Learn2DotNet.Hub.Application
{
    public class SocketClient
    {
        private readonly IPAddress ipAddress;
        private readonly int port;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private string response = string.Empty;

        public event EventHandler DevicesChanged;

        //private void Start()
        //{
        //    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //    //IPAddress ipAddress = ipHostInfo.AddressList[0];
        //    IPAddress ipAddress = IPAddress.Loopback;

        //    //for (int port = 16000; port < 16100; port++)

        //    SendToDevice(ipAddress, 16000);

        //}

        public SocketClient(IPAddress ipAddress, int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
        }

        public void SendToDevice()
        {
            connectDone.Reset();
            sendDone.Reset();
            receiveDone.Reset();

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.ReceiveTimeout = 20000;
            client.SendTimeout = 20000;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
                client.BeginConnect(endPoint, ConnectCallback, client);
                connectDone.WaitOne();

                if (!client.Connected)
                    return;

                Send(client, "ping");
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();

                // Add the new device in repo
                if (!string.IsNullOrEmpty(response))
                {
                    DeviceRepository repository = new DeviceRepository();

                    Device device = new Device
                    {
                        Name = response
                    };

                    repository.Add(device);

                    OnDevicesChanged();
                }

                client.Shutdown(SocketShutdown.Both);
            }
            catch { }
            finally
            {
                client.Close();
                client.Dispose();
            }
        }

        private void Receive(Socket client)
        {
            StateObject state = new StateObject();
            state.WorkSocket = client;

            client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
        }

        private void ReceiveCallback(IAsyncResult ar)
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

        private static void Send(Socket client, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;

            try
            {
                client.EndConnect(ar);
            }
            catch { }

            connectDone.Set();
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesSent = client.EndSend(ar);
            sendDone.Set();
        }

        protected virtual void OnDevicesChanged()
        {
            DevicesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
