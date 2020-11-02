using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
        
        public static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("-", "");
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public static string FormatKey(RSAParameters rsaParameters)
        {
            return
                $"Public Data:\n\nExponent:{BitConverter.ToString(rsaParameters.Exponent)}\nModulus:{BitConverter.ToString(rsaParameters.Modulus)}\n\n\nPrivate data:\n\nD:{BitConverter.ToString(rsaParameters.D)}DP:{BitConverter.ToString(rsaParameters.DP)}DQ:{BitConverter.ToString(rsaParameters.DQ)}Inverse Q:{BitConverter.ToString(rsaParameters.InverseQ)}P:{BitConverter.ToString(rsaParameters.P)}Q:{BitConverter.ToString(rsaParameters.Q)}\n\n\n";
        }
        
        static void Main(string[] args)
        {
            switch (Menu("Select:\n1. Receiver\n2. Sender", 2))
            {
                case 1:
                    RSACrypt rsaCryptReceiver = new RSACrypt("/tmp/receiver_public.xml", "/tmp/receiver_private.xml");
                    // TODO Use Path to make it platform agnostic
                    if (rsaCryptReceiver.GenerateIfNotExists())
                    {
                        Console.WriteLine("Using previous key");
                    }
                    else
                    {
                        RSAParameters rsaParametersReceiver = rsaCryptReceiver.GetKeys(true);
                        Console.WriteLine($"Generated new keys: \n{FormatKey(rsaParametersReceiver)}");
                    }

                    Console.Write("Text to decrypt: ");
                    string input = Console.ReadLine();
                    byte[] input_bytearray = StringToByteArray(input);
                    byte[] decrypted_bytearray = rsaCryptReceiver.Decrypt(input_bytearray, true);
                    string decrypted = Encoding.UTF8.GetString(decrypted_bytearray);
                    Console.WriteLine($"Decrypted: {decrypted}");
                    break;
                case 2:
                    RSACrypt rsaCryptSender = new RSACrypt("/tmp/sender_public.xml", "/tmp/sender_private.xml");
                    Console.Write("Exponent:");
                    byte[] exponent = StringToByteArray(Console.ReadLine());
                    Console.Write("Modulus:");
                    byte[] modulus = StringToByteArray(Console.ReadLine());
                    rsaCryptSender.CreateKeyFromInput(modulus, exponent);
                    Console.WriteLine("Text to encrypt");
                    Console.WriteLine($"Encrypted:{BitConverter.ToString(rsaCryptSender.Encrypt(Encoding.UTF8.GetBytes(Console.ReadLine()), true))}");
                    break;
            }
        }
    }
}