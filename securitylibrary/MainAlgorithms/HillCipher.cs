using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {

        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {

            throw new NotImplementedException();
        }
        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }
        
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> Decrypted = new List<int>();
            List<int> keyInverse = new List<int>();
            //1- calc determ
            int determ ;
            if (key.Count %2==0)
            {
                determ = getMatrixDeterm2d(key)%26;
                while (determ<0)
                {
                    determ += 26;
                }
                int b = 0;
                for (int i = 0; i < 27; i++)
                {
                    if ((i * determ) % 26 == 1)
                    {
                        b = i;
                        break;
                    }
                }
                //3- find K inverse
                if (determ == 0 || b==0)
                {
                    throw new InvalidAnlysisException();
                }
                keyInverse = getMatrixInverse2d(key, b);   
            }
            else
            {
                determ = getMatrixDeterm3d(key)%26;
                while (determ < 0)
                {
                    determ += 26;
                }
                //2- find b
                int b = 0;
                for (int i = 0; i < 27; i++)
                {
                    if ((i * determ) % 26 == 1)
                    {
                        b = i;
                        break;
                    }
                }
                if (determ == 0 || b == 0)
                {
                    throw new InvalidAnlysisException();
                }
                //3- find K inverse
                keyInverse = getMatrixInverse3d(key, b);
            }
            for (int i = 0; i < keyInverse.Count; i++)
            {
                while (keyInverse[i]<0)
                {
                    keyInverse[i] += 26;
                }
            }
            Decrypted = Encrypt(cipherText, keyInverse);
            return Decrypted;
        }
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> Encrypted = new List<int>();
            if (key.Count%2==0)
            {
                for (int i = 0; i < (plainText.Count) ; i+=2)
                {
                    for (int j = 0; j <= 2; j+=2)
                    {
                        Encrypted.Add(((key[j] * plainText[i])+(key[j+1]*plainText[i+1]))%26);
                    }
                }
            }
            else
            {
                for (int i = 0; i < (plainText.Count); i += 3)
                {
                    for (int j = 0; j <= 6; j += 3)
                    {
                        Encrypted.Add(((key[j] * plainText[i]) + (key[j + 1] * plainText[i + 1]) + (key[j + 2] * plainText[i + 2])) % 26);
                    }
                }
            }
            return Encrypted;
        }
        public string Encrypt(string plainText, string key)
        {
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {

            throw new NotImplementedException();
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }

        public static int getMatrixDeterm2d(List<int>matrix)
        {
            // 2d Matrix :  a b  
            //              c d
            int ad = matrix[0] * matrix[3];
            int bc = matrix[1] * matrix[2];
            return ad - bc;
        }
        public static List<int> getMatrixInverse2d(List<int> matrix,int determ)
        {
            List<int> inverse = new List<int>();
            int a = matrix[0];
            int b = matrix[1];
            int c = matrix[2];
            int d = matrix[3];
            inverse.Add((determ*d)%26);
            inverse.Add((determ * -b)%26);
            inverse.Add((determ * -c) % 26);
            inverse.Add((determ * a) % 26);
            return inverse;
        }
        public static int getMatrixDeterm3d(List<int> matrix)
        {
            // 3d Matrix :  a b c
            //              d e f
            //              g h i
            int a = matrix[0];
            int b = matrix[1];
            int c = matrix[2];
            int ei = matrix[4] * matrix[8];
            int fh = matrix[5] * matrix[7];
            int di = matrix[3] * matrix[8];
            int fg = matrix[5] * matrix[6];
            int dh = matrix[3] * matrix[7];
            int eg = matrix[4] * matrix[6];
            return (a * (ei - fh) - b * (di - fg) + c * (dh - eg));
        }
        public static List<int> getMatrixInverse3d(List<int> matrix, int determInverse)
        {
            // 3d Matrix :  0 1 2                     0 3 6
            //              3 4 5     Transpose--->   1 4 7
            //              6 7 8                     2 5 8
            List<int> inverse = new List<int>();
            List<int> transpose = new List<int>();
            inverse.Add((matrix[4] * matrix[8]) - (matrix[7] * matrix[5]));
            inverse.Add(-((matrix[3] * matrix[8]) - (matrix[6] * matrix[5])));
            inverse.Add((matrix[3] * matrix[7]) - (matrix[6] * matrix[4]));
            inverse.Add(-((matrix[1] * matrix[8]) - (matrix[7] * matrix[2])));
            inverse.Add((matrix[0] * matrix[8]) - (matrix[6] * matrix[2]));
            inverse.Add(-((matrix[0] * matrix[7]) - (matrix[6] * matrix[1])));
            inverse.Add((matrix[1] * matrix[5]) - (matrix[4] * matrix[2]));
            inverse.Add(-((matrix[0] * matrix[5]) - (matrix[3] * matrix[2])));
            inverse.Add((matrix[0] * matrix[4]) - (matrix[3] * matrix[1]));
            transpose.Add((determInverse * inverse[0])%26);
            transpose.Add((determInverse * inverse[3])%26);
            transpose.Add((determInverse * inverse[6]) % 26);
            transpose.Add((determInverse * inverse[1]) % 26);
            transpose.Add((determInverse * inverse[4]) % 26);
            transpose.Add((determInverse * inverse[7]) % 26);
            transpose.Add((determInverse * inverse[2]) % 26);
            transpose.Add((determInverse * inverse[5]) % 26);
            transpose.Add((determInverse * inverse[8]) % 26);
            return transpose;
        }

    }
}

