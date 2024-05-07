using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            // throw new NotImplementedException();

            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            string pt = "";
            int key = 1;
            while (key != plainText.Length)
            {
                pt = Decrypt(cipherText, key);
                if (pt.Length > plainText.Length)
                {
                    pt = pt.Remove(plainText.Length);
                }
                if (pt.ToString().Equals(plainText.ToString()))
                {
                    return key;
                }
                else
                {
                    key++;
                }
            }
            return 0;

        }

        public string Decrypt(string cipherText, int key)
        {
            // throw new NotImplementedException();

            int row = key;
            string pl = "";
            cipherText = cipherText.ToLower();
            int column = (int)Math.Ceiling((double)cipherText.Length / (double)key);

            char[,] arr = new char[row, column];
            int counter = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (counter < cipherText.Length)
                    {
                        arr[i, j] = cipherText[counter];
                        counter++;
                    }

                }
            }
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    pl += arr[j, i];

                }

            }
            return pl;
        }

        public string Encrypt(string plainText, int key)
        {
            // throw new NotImplementedException();

            int row = key;
            int column = (plainText.Length / key) + 1;
            char[,] arr = new char[row, column];
            char[,] res = new char[row, column];
            string CT = "";
            plainText = plainText.ToUpper();
            int counter = 0;
            for (int i = 0; i < column; i++)//3
            {
                for (int j = 0; j < row; j++)//
                {
                    if (counter < plainText.Length)
                    {
                        arr[j, i] = plainText[counter];
                        counter++;
                    }
                }
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    CT += arr[i, j];
                }
            }
            return CT;
        }
    }
}
