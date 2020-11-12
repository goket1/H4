using System.Net;

namespace SocketServerTest
{
    public class SocketUser : IUser<IPEndPoint>
    {
        public string NickName { get; }
        public IPEndPoint Destionation { get; }
        
        
    }
}