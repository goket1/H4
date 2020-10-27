using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hashing
{
    class Program
    {
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        
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

        static void Main(string[] args)
        {
            // Welcome message
            Console.WriteLine(@"
 o                                               \ o /
/|\                                                |  
/ \                                               / \ 
Bob                                              Alice
");
            Hash hash = new Hash();

            Console.Write("Input key:        ");
            string key = Console.ReadLine();

            Console.Write("Input plain text: ");
            string plainText = Console.ReadLine();

            switch (Menu("Select one\n1. SHA1\n2. MD5\n3. SHA256\n4. SHA384\n5. SHA512", 5))
            {
                case 1:
                    hash.MACHandler("SHA1");
                    break;
                case 2:
                    hash.MACHandler("MD5");
                    break;
                case 3:
                    hash.MACHandler("SHA256");
                    break;
                case 4:
                    hash.MACHandler("SHA384");
                    break;
                case 5:
                    hash.MACHandler("SHA512");
                    break;
            }
            
            // Compute MAC
            Console.WriteLine("Compute MAC selected");

            byte[] hashed = hash.ComputeMAC(Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(key));


            switch (Menu("Select one\n1. Compute MAC\n2. Verify MAC", 2))
            {
                case 1:
                    Console.WriteLine(
                        $"HMAC\nAscii. {Encoding.ASCII.GetString(hashed)}\nHex. {BitConverter.ToString(hashed)}");

                    Console.WriteLine("\nTime to compute the different algorithms:");

                    // Create new stopwatch.
                    Stopwatch stopwatch = new Stopwatch();

// Begin timing.
                    stopwatch.Start();

                    hash.MACHandler("SHA1");

                    // Stop timing.
                    stopwatch.Stop();
// Write result.
                    Console.WriteLine("Time elapsed for SHA1: {0}", stopwatch.Elapsed);
// Begin timing.
                    stopwatch.Start();

                    hash.MACHandler("MD5");

// Stop timing.
                    stopwatch.Stop();
// Write result.
                    Console.WriteLine("Time elapsed for MD5: {0}", stopwatch.Elapsed);
// Begin timing.
                    stopwatch.Start();

                    hash.MACHandler("SHA256");

// Stop timing.
                    stopwatch.Stop();
// Write result.
                    Console.WriteLine("Time elapsed for SHA256: {0}", stopwatch.Elapsed);
// Begin timing.
                    stopwatch.Start();

                    hash.MACHandler("SHA384");

// Stop timing.
                    stopwatch.Stop();
// Write result.
                    Console.WriteLine("Time elapsed for SHA384: {0}", stopwatch.Elapsed);
// Begin timing.
                    stopwatch.Start();

                    hash.MACHandler("SHA512");

// Stop timing.
                    stopwatch.Stop();
// Write result.
                    Console.WriteLine("Time elapsed for SHA512: {0}", stopwatch.Elapsed);

                    break;
                case 2:
                    // Verify MAC
                    Console.WriteLine("Verify MAC selected");
                    
                    Console.Write("Input hash: ");
                    string hashToCheck = Console.ReadLine();

                    if (hash.CheckAuthenticity(Encoding.UTF8.GetBytes(plainText), StringToByteArray(hashToCheck),
                        Encoding.UTF8.GetBytes(key)))
                    {
                        Console.WriteLine("Hash match");
                    }
                    else
                    {
                        Console.WriteLine("Hash doesn't match");
                    }
                    
                    break;
            }
        }
    }
}