using System;

namespace RSAReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            RSACrypt rsaCrypt = new RSACrypt();
            
            rsaCrypt.InitializeKey();
        }
    }
}