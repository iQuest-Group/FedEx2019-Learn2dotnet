using System.Net.Sockets;
using System.Text;

namespace Learn2DotNet.Devices.Domain
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket WorkSocket { get; set; }

        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] Buffer { get; set; } = new byte[BufferSize];

        // Received data string.  
        public StringBuilder StringBuilder { get; set; } = new StringBuilder();
    }
}