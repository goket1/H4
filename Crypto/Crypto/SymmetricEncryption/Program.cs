using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SymmetricEncryption
{
    class Program
    {
        static int Menu(string message, int optionCount)
        {
            bool invalid = true;
            int selected = 0;

            while (invalid)
            {
                Console.Write($"{message}\n: ");
                string input = Console.ReadLine();
                invalid = !(int.TryParse(input, out selected) & selected > 0 & selected <= optionCount);
            }

            return selected;
        }
        
        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("-", "");
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string FormatByteArray(byte[] byteArray)
        {
            return $"Ascii:{Encoding.ASCII.GetString(byteArray)}\nHex:{BitConverter.ToString(byteArray)}\nBase64:{ System.Convert.ToBase64String(byteArray)}";
        }
        
        static void Main(string[] args)
        {
            Crypt crypt = new Crypt();

            switch (Menu("Select an algorithm\n1. AES Managed\n2. DES\n3. Triple DES", 3))
            {
                case 1:
                    crypt.SelectAlgorithm("AESManaged");
                    break;
                case 2:
                    crypt.SelectAlgorithm("DES");
                    break;
                case 3:
                    crypt.SelectAlgorithm("TripleDES");
                    break;
            }
            
            Console.Write("Input key type g to generate:");
            string inputKey = Console.ReadLine();
            if (inputKey == "g")
            {
                crypt.key = crypt.GenerateKey();
            }
            else
            {
                crypt.key = StringToByteArray(inputKey);
            }
            Console.WriteLine($"Key:\n{FormatByteArray(crypt.key)}");

            Console.Write("Input iv type g to generate:");
            string inputIv = Console.ReadLine();
            if (inputIv == "g")
            {
                crypt.iv = crypt.GenerateIv();
            }
            else
            {
                crypt.iv = StringToByteArray(inputIv);
            }
            Console.WriteLine($"Iv:\n{FormatByteArray(crypt.iv)}");

            switch (Menu("Select one\n1. Encrypt\n2. Decrypt", 2))
            {
                case 1:
                    Console.Write("Plain text: ");
                    string plainText = Console.ReadLine();
                    
                    // Create new stopwatch.
                    Stopwatch stopwatch = new Stopwatch();
                    // Begin timing.
                    stopwatch.Start();

                    byte[] encrypted = crypt.Encrypt(plainText, crypt.key, crypt.iv);

                    // Stop timing.
                    stopwatch.Stop();
                    // Write result.
                    Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                    
                    Console.WriteLine($"Encrypted:\n{FormatByteArray(encrypted)}");
                    break;
                case 2:
                    Console.Write("Encrypted hex: ");
                    string encryptedText = Console.ReadLine();
                    
                    // Create new stopwatch.
                    Stopwatch stopwatch1 = new Stopwatch();
                    // Begin timing.
                    stopwatch1.Start();
                    
                    string decrypted = crypt.Decrypt(StringToByteArray(encryptedText), crypt.key, crypt.iv);
                    
                    // Stop timing.
                    stopwatch1.Stop();
                    // Write result.
                    Console.WriteLine("Time elapsed: {0}", stopwatch1.Elapsed);
                    
                    Console.WriteLine($"Decrypted:\n{decrypted}\nHex:\n{BitConverter.ToString(Encoding.Default.GetBytes(decrypted))}");
                    break;
            }
        }
    }
}