using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            throw new NotImplementedException();

        }

        public string Encrypt(string plainText, List<int> key)
        {
            int columns = key.Count();
            int rows = (plainText.Length / columns) + 1;

            char[,] matrix = new char[rows, columns];

            plainText = plainText.ToLower();

            int plain_index = 0;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (plain_index >= plainText.Length)
                        matrix[i, j] = 'x';
                    else
                    {
                        matrix[i, j] = plainText[plain_index];
                        plain_index++;
                    }
                }

            string cipherText = "";

            for (int col = 0; col < key.Count(); col++)
            {
                int index = key.IndexOf(col + 1);
                for (int row = 0; row < rows; row++)
                    if (matrix[row, index] != 'x')
                        cipherText += matrix[row, index];
            }

            return cipherText;
        }
    }
}
