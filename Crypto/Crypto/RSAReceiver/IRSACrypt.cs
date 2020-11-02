using System.Security.Cryptography;

namespace RSAReceiver
{
    public interface IRSACrypt
    {
        void GenerateKeys();
        bool GenerateIfNotExists();
        byte[] Encrypt(byte[] plainText, bool oaep);
        byte[] Decrypt(byte[] encryptedData, bool oaep);
        string GetKeys(bool getPrivateKey);
    }
}