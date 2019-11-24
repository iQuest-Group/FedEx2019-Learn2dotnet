using System.Net.Sockets;
using System.Text;

namespace Learn2DotNet.Hub.Application
{
    public class StateObject
    {
        public Socket WorkSocket = null;
        public const int BufferSize = 256;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder StringBuilder = new StringBuilder();
    }
}