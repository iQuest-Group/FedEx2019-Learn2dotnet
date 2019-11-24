using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Learn2DotNet.Devices.Domain.Model;

namespace Learn2DotNet.Devices.Domain
{
    public class SocketListener
    {
        public ManualResetEvent SendDone = new ManualResetEvent(false);

        private PairingStatus pairingStatus = PairingStatus.Off;

        private readonly int port;
        private readonly Device device;

        public event EventHandler PairingStatusChanged;

        public PairingStatus PairingStatus
        {
            get => pairingStatus;
            private set
            {
                pairingStatus = value;
                OnPairingStatusChanged();
            }
        }


        public SocketListener(int port, Device device)
        {
            if (port <= 0)
                throw new ArgumentOutOfRangeException(nameof(port));

            this.port = port;
            this.device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public Task StartListening()
        {
            PairingStatus = PairingStatus.On;

            return Task.Run(() =>
            {
                try
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    //IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPAddress ipAddress = IPAddress.Any;
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                    Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    listener.Bind(localEndPoint);
                    listener.Listen(100);
                    listener.ReceiveTimeout = 20000;
                    listener.SendTimeout = 20000;

                    SendDone.Reset();

                    try
                    {
                        Socket handler = listener.Accept();

                        try
                        {
                            byte[] bytes = new byte[1024];
                            string data = null;

                            int bytesRec = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                            if (data == "ping")
                            {
                                Send(handler, device.Name);
                                SendDone.WaitOne();
                            }

                            handler.Shutdown(SocketShutdown.Both);
                        }
                        finally
                        {
                            handler.Close();
                            handler.Dispose();
                        }
                    }
                    finally
                    {
                        listener.Close();
                        listener.Dispose();
                    }
                }
                finally
                {
                    PairingStatus = PairingStatus.Off;
                }
            });
        }

        //public void AcceptCallback(IAsyncResult ar)
        //{
        //    Socket listener = (Socket)ar.AsyncState;
        //    Socket handler = listener.EndAccept(ar);
        //    PairingStatus = PairingStatus.Off;

        //    StateObject state = new StateObject
        //    {
        //        WorkSocket = handler
        //    };

        //    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        //}

        //public static void ReadCallback(IAsyncResult ar)
        //{
        //    // Retrieve the state object and the handler socket  
        //    // from the asynchronous state object.  
        //    StateObject state = (StateObject)ar.AsyncState;
        //    Socket handler = state.WorkSocket;

        //    // Read data from the client socket.   
        //    int bytesRead = handler.EndReceive(ar);

        //    if (bytesRead > 0)
        //    {
        //        // There  might be more data, so store the data received so far.  
        //        state.StringBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));

        //        // Check for end-of-file tag. If it is not there, read   
        //        // more data.  
        //        string content = state.StringBuilder.ToString();
        //        if (content.IndexOf("<EOF>") > -1)
        //        {
        //            // All the data has been read from the   
        //            // client. Display it on the console.  
        //            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
        //            // Echo the data back to the client.  
        //            Send(handler, content);
        //        }
        //        else
        //        {
        //            // Not all data received. Get more.  
        //            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReadCallback, state);
        //        }
        //    }
        //}

        private void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                SendDone.Set();
            }
        }

        protected virtual void OnPairingStatusChanged()
        {
            PairingStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}