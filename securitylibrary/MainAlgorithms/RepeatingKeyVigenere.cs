using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
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
            //throw new NotImplementedException();
            List<List<char>> matrix = GetMatrix();
            string key = "";
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (matrix[j][(int)plainText[i] - 'a'] == cipherText[i])
                    {
                        key += (char)(j + 'a');
                        break;
                    }
                }
            }

            for (int i = 1; i <= key.Length / 2; i++)
            {
                string pattern = key.Substring(0, i);
                if (key.Substring(i, i) == pattern)
                {
                    return pattern;
                }
            }

            return key;

        }


        public string Decrypt(string cipherText, string key)
        {
                List<List<char>> matrix = GetMatrix();
                string plainText = "";
                cipherText = cipherText.ToLower();
                while (key.Length < cipherText.Length)
                {
                    key += key;
                }

                key = key.Substring(0, cipherText.Length);

                for (int i = 0; i < cipherText.Length; i++)
                {
                    List<char> row = matrix[(int)key[i] - 'a'];
                    int colindex = row.IndexOf(cipherText[i]) + 'a';
                    plainText += (char)colindex;
                }

                return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            // throw new NotImplementedException();
            List<List<char>> matrix = GetMatrix();

            while (key.Length < plainText.Length)
            {
                key += key;
            }

            key = key.Substring(0, plainText.Length);

            string cypher = "";

            for (int i = 0; i < plainText.Length; i++)
            {
                cypher += matrix[(int)plainText[i] - 'a'][(int)key[i] - 'a'];
            }

            return cypher;
        }
    }
}

