using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
   
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            List<int> newPlain = new List<int>();
            List<int> newCipher = new List<int>();
            int determinant = 0;
            bool found = FindDeterminantAndInvert(plainText, cipherText, ref newPlain, ref newCipher, ref determinant);

            if (!found)
                throw new InvalidAnlysisException(); // Custom exception if analysis fails

            List<int> invertedPlain = InvertMatrix(newPlain, determinant);
            return MultiplyMatrices(newCipher, invertedPlain, 2); // Multiplying matrices to get the key
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        
        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> Decrypted = new List<int>();
            List<int> keyInverse = new List<int>();
            int determ;

            // Calculating determinant based on key size
            if (key.Count % 2 == 0)
            {
                determ = MatrixDeterm2d(key) % 26;
                while (determ < 0)
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
                if (determ == 0 || b == 0)
                {
                    throw new InvalidAnlysisException();
                }
                keyInverse = MatrixInverse2d(key, b);
            }
            else
            {
                determ = MatrixDeterm3d(key) % 26;
                while (determ < 0)
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
                if (determ == 0 || b == 0)
                {
                    throw new InvalidAnlysisException();
                }
                keyInverse = MatrixInverse3d(key, b);
            }
            // Making sure key values are positive
            for (int i = 0; i < keyInverse.Count; i++)
            {
                while (keyInverse[i] < 0)
                {
                    keyInverse[i] += 26;
                }
            }
            Decrypted = Encrypt(cipherText, keyInverse); // Decrypting using the inverted key
            return Decrypted;
        }

        
        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        
        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> Encrypted = new List<int>();
            if (key.Count % 2 == 0)
            {
                for (int i = 0; i < (plainText.Count); i += 2)
                {
                    for (int j = 0; j <= 2; j += 2)
                    {
                        Encrypted.Add(((key[j] * plainText[i]) + (key[j + 1] * plainText[i + 1])) % 26);
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
            List<int> key = new List<int>();
            int plainDeterm = MatrixDeterm3d(plain3) % 26;
            if (plainDeterm == 0)
            {
                throw new InvalidAnlysisException();
            }
            if (plainDeterm < 0)
            {
                plainDeterm += 26;
            }

            int b = 0;
            for (int i = 0; i < 27; i++)
            {
                if ((i * plainDeterm) % 26 == 1)
                {
                    b = i;
                    break;
                }
            }
            List<int> invPlain = MatrixInverse3d(plain3, b);
            for (int i = 0; i < invPlain.Count(); i++)
            {
                invPlain[i] = invPlain[i] % 26;
                while (invPlain[i] < 0)
                {
                    invPlain[i] += 26;
                }
            }
            key = MatrixMultiplication(invPlain, cipher3);

            for (int i = 0; i < key.Count(); i++)
            {
                key[i] = key[i] % 26;

                if (key[i] < 0)
                {
                    key[i] += 26;
                }
            }
            key = MatrixTranspose3d(key);
            return key;
        }

     
        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }

        // Method to calculate determinant of a 2x2 matrix
        public static int MatrixDeterm2d(List<int> matrix)
        {
            int ad = matrix[0] * matrix[3];
            int bc = matrix[1] * matrix[2];
            return ad - bc;
        }

        // Method to find inverse of a 2x2 matrix
        public static List<int> MatrixInverse2d(List<int> matrix, int determ)
        {
            List<int> inverse = new List<int>();
            int a = matrix[0];
            int b = matrix[1];
            int c = matrix[2];
            int d = matrix[3];
            inverse.Add((determ * d) % 26);
            inverse.Add((determ * -b) % 26);
            inverse.Add((determ * -c) % 26);
            inverse.Add((determ * a) % 26);
            return inverse;
        }

        // Method to calculate determinant of a 3x3 matrix
        public static int MatrixDeterm3d(List<int> matrix)
        {
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

        // Method to find inverse of a 3x3 matrix
        public static List<int> MatrixInverse3d(List<int> matrix, int determInverse)
        {
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
            transpose.Add((determInverse * inverse[0]) % 26);
            transpose.Add((determInverse * inverse[3]) % 26);
            transpose.Add((determInverse * inverse[6]) % 26);
            transpose.Add((determInverse * inverse[1]) % 26);
            transpose.Add((determInverse * inverse[4]) % 26);
            transpose.Add((determInverse * inverse[7]) % 26);
            transpose.Add((determInverse * inverse[2]) % 26);
            transpose.Add((determInverse * inverse[5]) % 26);
            transpose.Add((determInverse * inverse[8]) % 26);
            return transpose;
        }

        // Method to transpose a 3x3 matrix
        public static List<int> MatrixTranspose3d(List<int> matrix)
        {
            List<int> transpose = new List<int>();
            transpose.Add((matrix[0]));
            transpose.Add((matrix[3]));
            transpose.Add((matrix[6]));
            transpose.Add((matrix[1]));
            transpose.Add((matrix[4]));
            transpose.Add((matrix[7]));
            transpose.Add((matrix[2]));
            transpose.Add((matrix[5]));
            transpose.Add((matrix[8]));
            return transpose;
        }

        // Method to multiply two matrices
        public static List<int> MatrixMultiplication(List<int> matrix1, List<int> matrix2)
        {
            List<int> val = new List<int>();

            // 2x2 matrix multiplication
            if (matrix1.Count() == 4)
            {
                val.Add(matrix1[0] * matrix2[0] + matrix1[1] * matrix2[2]);
                val.Add((matrix1[0] * matrix2[1] + matrix1[1] * matrix2[3]));
                val.Add((matrix1[2] * matrix2[0] + matrix1[3] * matrix2[2]));
                val.Add((matrix1[2] * matrix2[1] + matrix1[3] * matrix2[3]));
            }
            // 3x3 matrix multiplication
            else
            {
                val.Add(matrix1[0] * matrix2[0] + matrix1[1] * matrix2[3] + matrix1[2] * matrix2[6]);
                val.Add(matrix1[0] * matrix2[1] + matrix1[1] * matrix2[4] + matrix1[2] * matrix2[7]);
                val.Add(matrix1[0] * matrix2[2] + matrix1[1] * matrix2[5] + matrix1[2] * matrix2[8]);

                val.Add(matrix1[3] * matrix2[0] + matrix1[4] * matrix2[3] + matrix1[5] * matrix2[6]);
                val.Add(matrix1[3] * matrix2[1] + matrix1[4] * matrix2[4] + matrix1[5] * matrix2[7]);
                val.Add(matrix1[3] * matrix2[2] + matrix1[4] * matrix2[5] + matrix1[5] * matrix2[8]);

                val.Add(matrix1[6] * matrix2[0] + matrix1[7] * matrix2[3] + matrix1[8] * matrix2[6]);
                val.Add(matrix1[6] * matrix2[1] + matrix1[7] * matrix2[4] + matrix1[8] * matrix2[7]);
                val.Add(matrix1[6] * matrix2[2] + matrix1[7] * matrix2[5] + matrix1[8] * matrix2[8]);
            }
            return val;
        }

        // Method to find the Greatest Common Divisor
        private int GCD(int a, int b)
        {
            if (b == 0)
                return a;

            if (a == 0)
                return b;

            if (a == b)
                return a;

            if (a > b)
                return GCD(a - b, b);

            return GCD(a, b - a);
        }

        // Method to find determinant and invert the matrix
        private bool FindDeterminantAndInvert(List<int> plainText, List<int> cipherText, ref List<int> newPlain, ref List<int> newCipher, ref int determinant)
        {
            for (int i = 0; i < plainText.Count; i += 2)
            {
                for (int j = 0; j < plainText.Count; j += 2)
                {
                    if (j == i)
                        continue;

                    determinant = (plainText[i] * plainText[j + 1] - plainText[i + 1] * plainText[j]) % 26;
                    determinant = (determinant < 0) ? determinant + 26 : determinant;

                    if (GCD(determinant, 26) == 1 && determinant != 0)
                    {
                        newPlain.AddRange(new[] { plainText[i], plainText[j], plainText[i + 1], plainText[j + 1] });
                        newCipher.AddRange(new[] { cipherText[i], cipherText[j], cipherText[i + 1], cipherText[j + 1] });
                        return true;
                    }
                }
            }
            return false;
        }

        // Method to invert a matrix
        private List<int> InvertMatrix(List<int> matrix, int determinant)
        {
            List<int> invertedMatrix = new List<int>();
            int b = ModInverse(determinant, 26);

            invertedMatrix.Add(matrix[3]);
            invertedMatrix.Add(-1 * matrix[1]);
            invertedMatrix.Add(-1 * matrix[2]);
            invertedMatrix.Add(matrix[0]);

            for (int i = 0; i < invertedMatrix.Count; i++)
            {
                while (invertedMatrix[i] < 0)
                    invertedMatrix[i] = invertedMatrix[i] + 26;
                invertedMatrix[i] = (invertedMatrix[i] * b) % 26;
            }
            return invertedMatrix;
        }

        // Method to multiply matrices
        private List<int> MultiplyMatrices(List<int> a, List<int> b, int M)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < M; i++)
            {
                int index1 = i * M;
                for (int j = 0; j < M; j++)
                {
                    int index2 = j;
                    int res = 0;
                    for (int k = 0; k < M; k++)
                    {
                        res += a[index1 + k] * b[index2];
                        index2 += M;
                    }
                    result.Add(res % 26);
                }
            }
            return result;
        }

        // Method to find modular inverse
        private int ModInverse(int a, int Mod)
        {
            int M = Mod, K = 0, d = 1;
            while (a > 0)
            {
                int t = M / a, x = a;
                a = M % x;
                M = x;
                x = d;
                d = K - t * x;
                K = x;
            }
            K %= Mod;
            if (K < 0)
                K = (K + Mod) % Mod;
            return K;
        }

    }

  
}
