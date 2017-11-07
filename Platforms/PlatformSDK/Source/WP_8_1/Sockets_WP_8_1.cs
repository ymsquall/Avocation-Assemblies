#if UNITY_WP_8_1
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
using Windows.Networking.Sockets;
#else
//using System.Net.Sockets;
#endif
namespace Sockets_WP_8_1
{
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
    using SocketError = SocketErrorStatus;
#else
    using SocketError = System.Net.Sockets.SocketError;
#endif
    public enum AddressFamily
    {
        Unknown = -1,
        Unspecified = 0,
        Unix = 1,
        InterNetwork = 2,
        ImpLink = 3,
        Pup = 4,
        Chaos = 5,
        Ipx = 6,
        NS = 6,
        Osi = 7,
        Iso = 7,
        Ecma = 8,
        DataKit = 9,
        Ccitt = 10,
        Sna = 11,
        DecNet = 12,
        DataLink = 13,
        Lat = 14,
        HyperChannel = 15,
        AppleTalk = 16,
        NetBios = 17,
        VoiceView = 18,
        FireFox = 19,
        Banyan = 21,
        Atm = 22,
        InterNetworkV6 = 23,
        Cluster = 24,
        Ieee12844 = 25,
        Irda = 26,
        NetworkDesigners = 28,
        Max = 29,
    }
    public enum SocketType
    {
        Unknown = -1,
        Stream = 1,
        Dgram = 2,
        Raw = 3,
        Rdm = 4,
        Seqpacket = 5,
    }
    public enum ProtocolType
    {
        Unknown = -1,
        IPv6HopByHopOptions = 0,
        Unspecified = 0,
        IP = 0,
        Icmp = 1,
        Igmp = 2,
        Ggp = 3,
        IPv4 = 4,
        Tcp = 6,
        Pup = 12,
        Udp = 17,
        Idp = 22,
        IPv6 = 41,
        IPv6RoutingHeader = 43,
        IPv6FragmentHeader = 44,
        IPSecEncapsulatingSecurityPayload = 50,
        IPSecAuthenticationHeader = 51,
        IcmpV6 = 58,
        IPv6NoNextHeader = 59,
        IPv6DestinationOptions = 60,
        ND = 77,
        Raw = 255,
        Ipx = 1000,
        Spx = 1256,
        SpxII = 1257,
    }
    public enum SocketAsyncOperation
    {
        None = 0,
        Accept = 1,
        Connect = 2,
        Disconnect = 3,
        Receive = 4,
        ReceiveFrom = 5,
        ReceiveMessageFrom = 6,
        Send = 7,
        SendPackets = 8,
        SendTo = 9,
    }

    public class IPAddress
    {
        public static IPAddress Parse(string ipString)
        {
            return null;
        }
        public bool IsIPv6LinkLocal { get { return false; } }
    }
    public abstract class EndPoint
    {
        protected EndPoint() { }

        //public virtual AddressFamily AddressFamily { get; }

        //public virtual EndPoint Create(SocketAddress address);
        //public virtual SocketAddress Serialize();
    }
    public class IPEndPoint : EndPoint
    {
        public IPEndPoint(IPAddress address, int port)
        {
        }
    }
    public class IPHostEntry
    {
        public IPAddress[] AddressList { get; set; }
    }
    public class SocketAsyncEventArgs : System.EventArgs//, System.IDisposable
    {
        public event System.EventHandler<SocketAsyncEventArgs> Completed;
        public EndPoint RemoteEndPoint { get; set; }
        public SocketAsyncOperation LastOperation { get { return SocketAsyncOperation.Connect; } }
        public SocketError SocketError { get; set; }
        public bool SocketError_Success
        {
            get
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                return false;
#else
                return SocketError == SocketError.Success;
#endif
            }
        }
        public bool SocketError_NoSuccess
        {
            get
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                return false;
#else
                return SocketError != SocketError.Success;
#endif
            }
        }
        public int BytesTransferred { get { return 0; } }
        public byte[] Buffer { get { return null; } }
        public void SetBuffer(byte[] buffer, int offset, int count) { }
    }
    public static class Dns
    {
        public static IPHostEntry GetHostEntry(string hostNameOrAddress) { return null; }
        public static string GetHostName() { return ""; }

    }
    public class Socket
    {
        public Socket(AddressFamily family, SocketType type, ProtocolType proto)
        {
        }
        public bool NoDelay { get; set; }
        public bool ConnectAsync(SocketAsyncEventArgs e) { return false; }
        public bool ReceiveAsync(SocketAsyncEventArgs e) { return false; }
        public bool SendAsync(SocketAsyncEventArgs e) { return false; }
        public void Close() { }
    }
}
#endif