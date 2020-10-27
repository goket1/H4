using System;

namespace Crypto
{
    public class Decrypter
    {
        public static string Decrypt(string textToDecrypt, int shift)
        {
            // Initialize vars
            string output = "";
            
            // Loop over each character
            foreach (var character in textToDecrypt)
            {
                // Initialize vars
                int charVal = ((int) character);
                int newCharVal = charVal;
                
                // Ignore space
                if (charVal == 32)
                {
                    // Space
                }
                else
                {
                    // Shift the character value by "shift" amounts
                    newCharVal = charVal - shift;

                    // loop back around on small a
                    if (charVal > 96 & newCharVal < 97)
                    {
                        newCharVal = 123 - (97 - newCharVal);

                    }
                    // loop back around on big A
                    else if (newCharVal < 65)
                    {
                        newCharVal = 90 - (64 - newCharVal);
                    }
                } 
               
                // Output debug info
                //Console.WriteLine($"Character: {character} code: {(int) character} newChar code: {newCharVal} newChar Character: {(char) (newCharVal)}");
                // Add char to output string
                output += (char) (newCharVal);
            }
            
            // Return the output
            return output;
        }
    }
}