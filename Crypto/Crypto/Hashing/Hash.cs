using System.Diagnostics;
using System.Security.Cryptography;

namespace Hashing
{
    public class Hash
    {
        private HMAC myMAC;

        public void MACHandler(string name)
        {
            switch (name)
            {
                case "SHA1":
                    myMAC = new HMACSHA1();
                    break;
                case "MD5":
                    myMAC = new HMACMD5();
                    break;
                case "SHA256":
                    myMAC = new HMACSHA256();
                    break;
                case "SHA384":
                    myMAC = new HMACSHA384();
                    break;
                case "SHA512":
                    myMAC = new HMACSHA512();
                    break;
            }
        }
        
        public byte[] ComputeMAC(byte[] mes, byte[] key)
        {
            myMAC.Key = key;
            return myMAC.ComputeHash(mes);
        }

        public bool CheckAuthenticity(byte[] mes, byte[] mac, byte[] key)
        {
            myMAC.Key = key;
            return (CompareByteArrays(myMAC.ComputeHash(mes), mac, myMAC.HashSize / 8));
        }

        public int MACByteLength()
        {
            return myMAC.HashSize / 8;
        }

        public bool CompareByteArrays(byte[] a, byte[] b, int len)
        {
            for (int i = 0; i < len; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }
    }
}