using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {

        public string Encrypt(string plainText, int key)
        {
            key = key % 25;
            string answer="";
            foreach (char character in plainText)
            {
                int shift = key;
                char c;
                if ((int)character + key > (int)'z')
                {
                    shift = key - ((int)'z' - (int)character)-1;
                    c= (char)((int)'a' + shift);
                }
                else
                {
                    c = (char)((int)character + shift);
                }
                answer += c;
            }
            return answer.ToUpper();
        }

        public string Decrypt(string cipherText, int key)
        {
            key = key % 25; 
            string answer = "";
            foreach (char character in cipherText.ToLower())
            {
                int shift = key;
                char c;
                if ((int)character - key < (int)'a')
                {
                    shift = key - ((int)character - (int)'a') - 1;
                    c = (char)((int)'z' - shift);
                }
                else
                {
                    c = (char)((int)character - shift);
                }
                answer += c;
            }
            return answer.ToLower();
        }

        public int Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            int key= ((int)cipherText[0]-(int)plainText[0] ) % 25;
            if (key < 0)
            {
                key+=26;
            }
            return key;
        }
    }
}