using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        private void get_differences(char letter1, char letter2, string text, ref HashSet<int> difference)
        {
            //Console.WriteLine(letter1 + " " + letter2) ;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == letter1 || text[i] == letter2)
                {
                    //Console.WriteLine("IF");
                    for (int j = i + 1; j < text.Length; j++)
                        if ((text[j] == letter1 || text[j] == letter2) && (text[i] != text[j]))
                        {
                            difference.Add(Math.Abs(j - i));
                            //Console.WriteLine(Math.Abs(j - i));
                        }
                }
            }

        }
        private void fill_the_matrix(int rows, int columns, string plainText, ref char[,] matrix)
        {
            int plain_index = 0;
            //Console.WriteLine(plainText);
            //Console.WriteLine(plainText.Length);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (plain_index < plainText.Length)
                    {
                        //Console.WriteLine(plain_index);
                        matrix[i, j] = plainText[plain_index];
                        plain_index++;
                    }
                    else
                        matrix[i, j] = 'x';
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------");

        }
        private List<int> check_if_col_exist(int rows, int cols, char[,] matrix, string cipherText)
        {
            Console.WriteLine(cipherText);
            int cipher_index = 0;

            List<int> exclude = new List<int>();
            List<int> key = new List<int>();
            //  col  key
            SortedDictionary<int, int> permutation = new SortedDictionary<int, int>();
            int perm_order = 1;

            for (int i = 0; i < cols; i++)
            {
                Console.WriteLine("Matrix = " + matrix[0, i] + ", cipher = " + cipherText[cipher_index]);
                Console.WriteLine("EXCLUDE: " + i + " ? " + exclude.Contains(i));
                if (matrix[0, i] == cipherText[cipher_index] && !exclude.Contains(i))
                {
                    //Console.WriteLine(exclude.Contains(i));
                    int match_letters = 0;
                    int save_index = cipher_index;
                    for (int j = 0; j < rows; j++)
                    {
                        //Console.WriteLine(matrix[j, i] + " " + cipherText[cipher_index]);
                        Console.WriteLine(" i " + i + " j " + j + " cipher index " + cipher_index + "cipher len " + cipherText.Length);
                        if (cipher_index < cipherText.Length )
                        {
                            if (matrix[j, i] == cipherText[cipher_index])
                            {
                                //Console.WriteLine("IN");
                                match_letters++;
                                cipher_index++;
                            }
                            
                        }          
                        if (matrix[j, i] == 'x')
                            {
                                match_letters++;
                                //cipher_index--;
                                //Console.WriteLine("Current Letter = " + cipherText[cipher_index]);
                            }
                        //Console.WriteLine("loopppppppppppppppppppp  " + " j " + j + " i " + i);
                    }

                    Console.WriteLine("Match = " + match_letters + ", Rows = " + rows);
                    Console.WriteLine();
                    if (match_letters == rows)
                    {
                        //Console.WriteLine(i + 1);
                        //key.Add(i + 1);
                        permutation[i + 1] = perm_order;
                        exclude.Add(i);
                        //i = 0;
                        perm_order++;
                        i = -1;
                    }
                    else
                        cipher_index = save_index;

                    Console.WriteLine("////////////////////");

                }
                //Console.WriteLine("i in the loop = " + i);
                //foreach(int x in exclude) Console.WriteLine("Exclude : " + x);

                if (exclude.Count() == cols)
                    break;
            }
            Console.WriteLine("******************");

            foreach (var x in permutation)
                //Console.WriteLine("Column = " + x.Key + " ,Perm = " + x.Value);
                key.Add(x.Value);
            Console.WriteLine("Key count in check function = " + key.Count());
            if (key.Count() == cols)
                return key;

            return new List<int>();
        }
        public List<int> Analyse(string plainText, string cipherText)
        {
            HashSet<int> cols = new HashSet<int>();

            cipherText = cipherText.ToLower();

            get_differences(cipherText[0], cipherText[1], plainText, ref cols);
            get_differences(cipherText[cipherText.Length - 1], cipherText[cipherText.Length - 2], plainText, ref cols);

            //foreach (int col in cols)
            //    Console.WriteLine(col);

            Dictionary<int, int> cols_rows = new Dictionary<int, int>();
            int txt_len = cipherText.Length;

            //Console.WriteLine(cols.Count());

            foreach (int col in cols)
            {
                int row = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(txt_len / col)));
                if (row * col >= plainText.Length)
                    cols_rows.Add(col, row);
                else if ((row + 1) * col >= plainText.Length)
                    cols_rows.Add(col, row + 1);
                //Console.WriteLine(col.ToString() + "  " + Convert.ToInt32(Math.Ceiling(Convert.ToDouble(txt_len / col))));

            }

            List<int> key = new List<int>();
            foreach (var col_row in cols_rows)
            {
                int row = col_row.Value;
                int column = col_row.Key;
                char[,] matrix = new char[row, column];

                Console.WriteLine(row * column + " vs " + plainText.Length);
                fill_the_matrix(row, column, plainText, ref matrix);

                key = check_if_col_exist(row, column, matrix, cipherText);
                Console.WriteLine("Key Count = " + key.Count());
                if (key.Count() == column)
                    break;
            }

            foreach (int x in key)
                Console.WriteLine("Key = " + x);

            return key;

        }

        public string Decrypt(string cipherText, List<int> key)
        {
            int columns = key.Count();
            int rows = (cipherText.Length / columns) + 1;

            char[,] matrix = new char[rows, columns];

            cipherText = cipherText.ToLower();

            int cipher_index = 0;

            // fillng the free cells before the matrix so the algorithm can fill the cipher correct
            int ignore = rows * columns - cipherText.Length;
            for (int i = 0; i < ignore; i++)
                matrix[rows - 1, columns - i - 1] = 'x';


            for (int col = 0; col < columns; col++)
                for (int row = 0; row < rows; row++)
                {
                    int index = key.IndexOf(col + 1);
                    if (matrix[row, index] != 'x')
                        if (cipher_index >= cipherText.Length)
                            matrix[row, index] = 'x';
                        else
                        {
                            matrix[row, index] += cipherText[cipher_index];
                            cipher_index++;
                        }


                }

            string plainText = "";

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (matrix[i, j] != 'x')
                        plainText += matrix[i, j];


            return plainText;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            int columns = key.Count();
            int rows = (plainText.Length / columns) + 1;

            char[,] matrix = new char[rows, columns];

            plainText = plainText.ToLower();

            fill_the_matrix(rows, columns, plainText, ref matrix);

            string cipherText = "";

            for (int col = 0; col < columns; col++)
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
