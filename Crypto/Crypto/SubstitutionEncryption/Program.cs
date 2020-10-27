using System;

namespace Crypto
{
    class Program
    {
        static void Main(string[] args)
        {
            int shiftAmount = 5;
            // Get user input and call encrypter
            string encrypted = Encrypter.Encrypt(Console.ReadLine(), shiftAmount);
            Console.WriteLine($"Encrypted:{encrypted}");

            string decrypted = Decrypter.Decrypt(encrypted, shiftAmount);
            Console.WriteLine($"Decrypted:{decrypted}");
        }
    }
}