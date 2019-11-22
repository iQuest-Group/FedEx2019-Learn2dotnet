using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Learn2DotNet.Hub.Domain;
using Learn2DotNet.Hub.Domain.Models;

namespace Learn2DotNet.Hub.Application
{
    public class SocketClient
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static string response = string.Empty;

        public event EventHandler DevicesChanged;

        public void Start()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            //for (int port = 16000; port < 16100; port++)

            Task task = Task.Run(() => SendToDevice(ipAddress, 16000));

        }

        private void SendToDevice(IPAddress ipAddress, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.BeginConnect(endPoint, ConnectCallback, client);
            connectDone.WaitOne();

            Send(client, "Ping!<EOF>");
            sendDone.WaitOne();

            Receive(client);
            receiveDone.WaitOne();

            // Add the new device in repo
            DeviceRepository repository = new DeviceRepository();
            repository.Add(new Device
            {
                Name = "new device"
            });

            OnDevicesChanged();

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

        protected virtual void OnDevicesChanged()
        {
            DevicesChanged?.Invoke(this, EventArgs.Empty);
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
