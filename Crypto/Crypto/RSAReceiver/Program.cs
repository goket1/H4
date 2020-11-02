using System;
using System.Linq;

namespace RSAReceiver
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
        
        public static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        
        static void Main(string[] args)
        {
            RSACrypt rsaCrypt = new RSACrypt("/tmp/public.xml", "/tmp/private.xml");
            // TODO Use Path to make it platform agnostic
            if (rsaCrypt.GenerateIfNotExists())
            {
                Console.WriteLine("Using previous key");
            }
            else
            {
                Console.WriteLine($"Generated new keys: {rsaCrypt.GetKeys(true)}");
            }
        }
    }
}