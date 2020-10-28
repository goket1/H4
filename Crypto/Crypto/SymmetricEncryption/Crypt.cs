using System.IO;
using System.Security.Cryptography;

namespace SymmetricEncryption
{
    public class Crypt
    {
        public SymmetricAlgorithm SymmetricAlgorithm = new AesManaged();
        public byte[] key;
        public byte[] iv;

        public void SelectAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "AESManaged":
                    this.SymmetricAlgorithm = new AesManaged();
                    break;
                case "DES":
                    this.SymmetricAlgorithm = new DESCryptoServiceProvider();
                    break;
                case "TripleDES":
                    this.SymmetricAlgorithm = new TripleDESCryptoServiceProvider();
                    break;
            }
        }

        public byte[] GenerateIv()
        {
            this.SymmetricAlgorithm.GenerateIV();
            return this.SymmetricAlgorithm.IV;
        }
        
        public byte[] GenerateKey()
        {
            this.SymmetricAlgorithm.GenerateKey();
            return this.SymmetricAlgorithm.Key;
        }

        public byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            using (this.SymmetricAlgorithm)
            {
                this.SymmetricAlgorithm.Key = this.key;
                this.SymmetricAlgorithm.IV = this.iv;

                ICryptoTransform encryptor =
                    this.SymmetricAlgorithm.CreateEncryptor(this.SymmetricAlgorithm.Key, this.SymmetricAlgorithm.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public string Decrypt(byte[] cipher, byte[] key, byte[] iv)
        {
            string plainText = null;
            using (this.SymmetricAlgorithm)
            {
                this.SymmetricAlgorithm.Key = this.key;
                this.SymmetricAlgorithm.IV = this.iv;

                ICryptoTransform decryptor =
                    this.SymmetricAlgorithm.CreateDecryptor(this.SymmetricAlgorithm.Key, this.SymmetricAlgorithm.IV);
                using (MemoryStream memoryStream = new MemoryStream(cipher))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }
    }
}