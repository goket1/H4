using System;
using System.IO;
using System.Security.Cryptography;

namespace RSAReceiver
{
    public class RSACrypt : IRSACrypt
    {
        /*
         * I choose to store the keys in a file because .net core on Linux does not have an implementation to store keys
         * in a keystore. Even tho keystores do exist on Linux and many programs like Chromium use them... :(
         * https://stackoverflow.com/questions/48791488/using-asymmetric-key-on-net-core
         */

        private string publicKeyPath;
        private string privateKeyPath;
        
        public RSACrypt(string publicKeyPath, string privateKeyPath)
        {
            this.publicKeyPath = publicKeyPath;
            this.privateKeyPath = privateKeyPath;
        }

        public void GenerateKeys()
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;
                /*cspParameters = new CspParameters
                {
                    KeyContainerName = keyStoreName
                };
                using var rsa = new RSACryptoServiceProvider(cspParameters);
                */
                // TODO Find out how to use keystore on Linux
            
                var publicKeyfolder = Path.GetDirectoryName(publicKeyPath);
                var privateKeyfolder = Path.GetDirectoryName(privateKeyPath);
            
                if (!Directory.Exists(publicKeyfolder))
                {
                    Directory.CreateDirectory(publicKeyfolder);
                }

                if (!Directory.Exists(privateKeyfolder))
                {
                    Directory.CreateDirectory(privateKeyfolder);
                }
            
                File.WriteAllText(publicKeyPath, rsaCryptoServiceProvider.ToXmlString(false));
                File.WriteAllText(privateKeyPath, rsaCryptoServiceProvider.ToXmlString(true));
            }
            
        }

        public bool GenerateIfNotExists()
        {
            bool exists = File.Exists(privateKeyPath);
            if (!exists)
            {
                GenerateKeys();
            }

            return exists;
        }

        public byte[] Encrypt(byte[] plainText, bool oaep)
        {
            byte[] plain;

            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;                
                rsaCryptoServiceProvider.FromXmlString(File.ReadAllText(privateKeyPath));
                plain = rsaCryptoServiceProvider.Decrypt(plainText, oaep);
            }

            return plain;
        }

        public byte[] Decrypt(byte[] encryptedData, bool oaep)
        {
            byte[] plain;

            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;                
                rsaCryptoServiceProvider.FromXmlString(File.ReadAllText(privateKeyPath));
                plain = rsaCryptoServiceProvider.Decrypt(encryptedData, oaep);
            }

            return plain;
        }

        public string GetKeys(bool getPrivateKey)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(2048))
            {
                rsaCryptoServiceProvider.PersistKeyInCsp = false;
                rsaCryptoServiceProvider.FromXmlString(File.ReadAllText(privateKeyPath));
                return rsaCryptoServiceProvider.ToXmlString(getPrivateKey);
            }
        }
    }
}