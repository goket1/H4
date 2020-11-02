using System;
using System.Security.Cryptography;

namespace RSAReceiver
{
    public interface IRSACrypt
    {
        void GenerateKeys();
        bool GenerateIfNotExists();
        void CreateKeyFromInput(Byte[] PublicKey, Byte[] Exponent);
        byte[] Encrypt(byte[] plainText, bool oaep);
        byte[] Decrypt(byte[] encryptedData, bool oaep);
        RSAParameters GetKeys(bool getPrivateKey);
    }
}