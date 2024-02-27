using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            string answer="";
            foreach (char character in plainText)
            {
                char c =(char) ((int)character + key);
                answer += c;
            }
            return answer;
        }

        public string Decrypt(string cipherText, int key)
        {
            throw new NotImplementedException();
        }

        public int Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
    }
}