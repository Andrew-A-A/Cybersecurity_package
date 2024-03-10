using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private List<List<char>> GetMatrix() 
        {
            List<List<char>> matrix = new List<List<char>>();

            for (int i = 0; i < 26; i++)
            {
                List<char> singleRow = new List<char>();
                for (int j = 0; j < 26; j++)
                {
                    int val = (i + j) % 26 + 'a';
                    singleRow.Add((char)val);
                }
                matrix.Add(singleRow);
            }

            return matrix;
        } 
        public string Analyse(string plainText, string cipherText)
        {
            List<List<char>> matrix = GetMatrix();
            string key = "";
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            bool flag=false;
            int idx = -1;
            for (int i = 0; i < cipherText.Length; i++)
            {
                List<char> row = matrix[(int)plainText[i] - 'a'];
                int colindex = row.IndexOf(cipherText[i]) + 'a';
                key += (char)colindex;
                if (key[i] == plainText[0]) 
                {
                    flag= true;
                    continue;
                }
                if (flag) 
                {
                    if (key[i] == plainText[1]) 
                    {
                        idx= i-1;
                    }
                    flag = false;
                }
            }
            if (idx != -1) 
            {
                string newkey = key.Substring(0, idx);
                return newkey;
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            List<List<char>> matrix = GetMatrix();
            string plainText = "";
            cipherText=cipherText.ToLower();
            for (int i = 0; i < key.Length; i++)
            {
                List<char> row = matrix[(int)key[i] - 'a'];
                int colindex = row.IndexOf(cipherText[i]) + 'a';
                plainText += (char)colindex;
                //plainText += (char)(matrix[((int)key[i] - 'a')].IndexOf(cipherText[i])) + 'a';
            }
            if (plainText.Length < cipherText.Length)
            {
                int diff = cipherText.Length - key.Length;
                for (int i = 0; i < diff; i++)
                {
                    List<char> row = matrix[(int)plainText[i] - 'a'];
                    int colindex = row.IndexOf(cipherText[i + key.Length]) + 'a';
                    plainText += (char)colindex;
                    //plainText += (char)(matrix[((int)plainText[i] - 'a')].IndexOf(cipherText[i + key.Length-1])) + 'a';
                }
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            List<List<char>> matrix= GetMatrix();
            string newKey = "";

            if (key.Length < plainText.Length) 
            {
                int diff = plainText.Length - key.Length;
                newKey = key+plainText.Substring(0, diff);
            }

            key = newKey;
            string cypher = "";

            for (int i = 0;i < plainText.Length;i++) 
            {
                cypher += matrix[((int)plainText[i] - 'a')][((int)key[i] - 'a')];
            }
           return cypher;
    }
    }
}
