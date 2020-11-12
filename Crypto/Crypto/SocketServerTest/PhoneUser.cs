using System;

namespace SocketServerTest
{
    public class PhoneUser: IUser<String>
    {
        public string NickName { get; }
        public string Destionation { get; }
    }
}