using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CryptoRandomVSRandom
{
    class Program
    {
        static void CryptoRandom(int bytes, int count)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                Byte[] data = new Byte[bytes];
                for (int i = 0; i < count; i++)
                {
                    rng.GetBytes(data);

                    int value = BitConverter.ToInt32(data, 0);
                    // Console.WriteLine(value);
                }
            }
        }

        static void RegularRandom(int bytes, int count)
        {
            Random random = new Random();
            Byte[] data = new Byte[bytes];
            for (int i = 0; i < count; i++)
            {
                int value = random.Next();
                // Console.WriteLine(value);
            }
        }
        
        static void Main(string[] args)
        {
            int timesToRun = 10000000;
            
            // Create new stopwatch.
            Stopwatch stopwatch1 = new Stopwatch();

            // Begin timing.
            stopwatch1.Start();
            
            Console.WriteLine("Crypto: ");
            CryptoRandom(4, timesToRun);

            // Stop timing.
            stopwatch1.Stop();

            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch1.Elapsed);

            
            // Create new stopwatch.
            Stopwatch stopwatch2 = new Stopwatch();

            // Begin timing.
            stopwatch2.Start();

            Console.WriteLine("Regular: ");
            RegularRandom(4, timesToRun);
            
            // Stop timing.
            stopwatch1.Stop();

            // Write result.
            Console.WriteLine("Time elapsed: {0}", stopwatch2.Elapsed);
            
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}