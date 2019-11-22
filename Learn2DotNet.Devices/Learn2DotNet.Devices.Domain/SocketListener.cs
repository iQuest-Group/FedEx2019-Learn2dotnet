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
        public static ManualResetEvent AllDone = new ManualResetEvent(false);
        private static PairingStatus pairingStatus = PairingStatus.Off;
        private int port { get; }
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


        public SocketListener(int port)
        {
            this.port = port;
        }

        public void StartListening()
        {
            PairingStatus = PairingStatus.On;

            Task.Run(() =>
            {

                // Establish the local endpoint for the socket.  
                // The DNS name of the computer  
                // running the listener is "host.contoso.com".  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPAddress ipAddress = IPAddress.Any;
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.  
                Socket listener = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.  
                try
                {
                    listener.Bind(localEndPoint);
                    listener.Listen(100);
                    listener.ReceiveTimeout = 30000;

                    while (true)
                    {
                        // Set the event to nonsignaled state.  
                        AllDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        listener.BeginAccept(AcceptCallback, listener);

                        // Wait until a connection is made before continuing.  
                        AllDone.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    
                }

            });
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            PairingStatus = PairingStatus.Off;

            // Create the state object.  
            StateObject state = new StateObject
            {
                WorkSocket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.StringBuilder.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.StringBuilder.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        protected virtual void OnPairingStatusChanged()
        {
            PairingStatusChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}