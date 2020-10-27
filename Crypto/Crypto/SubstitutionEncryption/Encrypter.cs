using System;

namespace Crypto
{
    public class Encrypter
    {
        public static string Encrypt(string textToEncrypt, int shift)
        {
            // Initialize vars
            string output = "";
            
            // Loop over each character
            foreach (var character in textToEncrypt)
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
                    newCharVal = charVal + shift;
                    
                    // loop back around on small z
                    if (charVal > 96 & newCharVal > 122)
                    {
                        newCharVal = 96 + (newCharVal - 122);
                        
                    }
                    // loop back around on big Z 
                    else if (charVal < 91 & newCharVal > 90)
                    {
                        newCharVal = 64 + (newCharVal - 90);
                        
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