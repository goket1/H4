using System;

namespace SocketServerTest
{
    public interface IUser
    {
        public String NickName { get; }
    }
    public interface IUser<T> : IUser
    {
        public T Destionation { get; }
    }
}