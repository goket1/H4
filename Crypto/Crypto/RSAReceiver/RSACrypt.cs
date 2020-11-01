using System;
using System.Security.Cryptography;

namespace RSAReceiver
{
    public class RSACrypt
    {
        RSACryptoServiceProvider RSA = null;

        public void InitializeKey()
        {
            this.RSA = new RSACryptoServiceProvider();
            RSAParameters rsaParameters = new RSAParameters();
            RSA.ImportParameters(rsaParameters);
          
            Console.WriteLine("End");
        }
    }
}