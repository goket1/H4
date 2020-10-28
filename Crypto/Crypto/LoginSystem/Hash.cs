using System;
using System.Security.Cryptography;

namespace LoginSystem
{
    public class Hash
    {
        private SHA512 sha512 = new System.Security.Cryptography.SHA512CryptoServiceProvider();
        
        public bool CheckAuthenticity(byte[] mes, byte[] mac)
        {
            return (CompareByteArrays(mes, mac, mes.Length));
        }
        public bool CompareByteArrays(byte[] a, byte[] b, int len)
        {
            for (int i = 0; i < len; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }
        
        /*public byte[] ComputePasswordHash(byte[] pass, byte[] salt)
        {
            var combined = Combine(pass, salt);
            var hashed = sha512.ComputeHash(combined);
            // Console.WriteLine($"ComputePasswordHash called with: \nPassword: len: {pass.Length} |{BitConverter.ToString(pass)}|\nSalt: len: {salt.Length} |{BitConverter.ToString(salt)}|\nCombined: len {combined.Length} |{BitConverter.ToString(combined)}|\nHashed: len {hashed.Length} |{BitConverter.ToString(hashed)}|");
            return hashed;
        }*/
        
        public byte[] ComputePasswordHash(string pass, byte[] salt, int iterations = 5000, int hashByteSize = 64)
        {
            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(pass, salt))
            {
                hashGenerator.IterationCount = iterations;
                return hashGenerator.GetBytes(hashByteSize);
            }
        }
        
        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }
        
        public byte[] GenerateSalt(int saltLength = 16)
        {
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] random = new byte[saltLength];
                cryptoServiceProvider.GetBytes(random);
                return random;
            }
        }
    }
}