﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {
        #region Initialize constants 
        private static readonly byte[] IP = {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };
        private static int[] FP = { 40, 8, 48, 16, 56, 24, 64,
            32, 39, 7, 47, 15, 55,
            23, 63, 31, 38, 6, 46,
            14, 54, 22, 62, 30, 37,
            5, 45, 13, 53, 21, 61,
            29, 36, 4, 44, 12, 52,
            20, 60, 28, 35, 3, 43,
            11, 51, 19, 59, 27, 34,
            2, 42, 10, 50, 18, 58,
            26, 33, 1, 41, 9, 49,
            17, 57, 25 };
        private static readonly byte[] NShiftBits = {
            1, 1, 2, 2, 2, 2, 2, 2,
            1, 2, 2, 2, 2, 2, 2, 1
        };
        private static int[] EP = { 32, 1, 2, 3, 4, 5, 4,
            5, 6, 7, 8, 9, 8, 9, 10,
            11, 12, 13, 12, 13, 14, 15,
            16, 17, 16, 17, 18, 19, 20,
            21, 20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29, 28,
            29, 30, 31, 32, 1 };
        private static readonly byte[] SBoxPermutation = {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25
        };
        private static readonly byte[] K1P = {
            57, 49, 41, 33, 25,
            17, 9, 1, 58, 50, 42, 34, 26,
            18, 10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36, 63,
            55, 47, 39, 31, 23, 15, 7, 62,
            54, 46, 38, 30, 22, 14, 6, 61,
            53, 45, 37, 29, 21, 13, 5, 28,
            20, 12, 4
        };
        private static readonly byte[] K2P = {
            14, 17, 11, 24, 1, 5, 3,
            28, 15, 6, 21, 10, 23, 19, 12,
            4, 26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32
        };
        private static readonly byte[,,] SBox =
        {
            {
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },
            {
                {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },
            {
                {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },
            {
                {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },
            {
                {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },
            {
                {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },
            {
                {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },
            {
                {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };
        #endregion
        public override string Encrypt(string plainText, string key)
        {
          return  EncryptDecrypt(plainText, key,true);
        }
        public override string Decrypt(string cipherText, string key)
        {
            return EncryptDecrypt(cipherText, key);
        }
        #region Helper Functions
        private  string EncryptDecrypt(string text, string key,bool isEncrypt=false)
        {
            // Convert plain text and key to binary format
            var binaryPlain = BinaryValueOf(text);
            var permutatedBinary = Permutation(binaryPlain, IP);
            var leftPlain = permutatedBinary.Substring(0, 32);
            var rightPlain = permutatedBinary.Substring(32, 32);
            var binaryKey = BinaryValueOf(key);
            var newBinaryKey = Permutation(binaryKey, K1P);

            // Generate subkeys
            var keys = GetKeys(newBinaryKey);


            if (isEncrypt)
            {
                // Encryption rounds
                for (int nRound = 0; nRound < 16; nRound++)
                {
                    // Expansion permutation
                    var expandedRight = Permutation(rightPlain, EP);

                    // XOR operation
                    var temp = XOR(keys[nRound], expandedRight);

                    // S-Box substitution
                    var sboxTemp = Sbox(temp);

                    // Permutation
                    var permutationTemp = Permutation(sboxTemp, SBoxPermutation);

                    // XOR operation
                    leftPlain = XOR(leftPlain, permutationTemp);

                    // Swap left and right
                    Swap(ref leftPlain, ref rightPlain);
                }
            }
            else
            {
                // Decryption rounds
                for (int nRound = 15; nRound >= 0; nRound--)
                {
                    // Expansion permutation
                    var expandedRight = Permutation(rightPlain, EP);

                    // XOR operation
                    var temp = XOR(keys[nRound], expandedRight);

                    // S-Box substitution
                    var sboxTemp = Sbox(temp);

                    // Permutation
                    var permutationTemp = Permutation(sboxTemp, SBoxPermutation);

                    // XOR operation
                    leftPlain = XOR(leftPlain, permutationTemp);

                    // Swap left and right
                    Swap(ref leftPlain, ref rightPlain);
                }
            }

            // Last swap
            Swap(ref leftPlain, ref rightPlain);

            // Combine left and right and perform final permutation
            string result = Permutation(leftPlain + rightPlain, FP);

            // Convert result to hexadecimal format
            return HexaValueOf(result);
        }
        private static string Sbox(string input)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                string block = input.Substring(i * 6, 6);
                int row = Convert.ToInt32(new string(new[] { block[0], block[5] }), 2);
                int column = Convert.ToInt32(block.Substring(1, 4), 2);
                result.Append(Convert.ToString(SBox[i, row, column], 2).PadLeft(4, '0'));
            }
            return result.ToString();
        }
        private static List<string> GetKeys(string initialKey)
        {
            var result = new List<string>();
            string leftKey = initialKey.Substring(0, 28);
            string rightKey = initialKey.Substring(28, 28);
            for (int nRound = 0; nRound < 16; nRound++)
            {
                leftKey = ShiftLeft(leftKey, NShiftBits[nRound]);
                rightKey = ShiftLeft(rightKey, NShiftBits[nRound]);

                string fullKey = leftKey + rightKey;
                string keySecondPerm = Permutation(fullKey, K2P);

                result.Add(keySecondPerm);
            }
            return result;
        }
        private static string Permutation(string orig, IEnumerable<int> arr)
        {
            var res = "";
            foreach (int index in arr)
                res += orig[index - 1];
            return res;
        }
        // Overload specifically for byte arrays
        private static string Permutation(string orig, byte[] arr)
        {
            return Permutation(orig, arr.Select(b => (int)b));
        }
        private void Swap(ref string s1, ref string s2)
        {
            string temp=s1;
            s1 = s2;
            s2 = temp;
        }
        private static string ShiftLeft(string s, int n = 1)
        {
            var result = s;
            for (var i = 0; i < n; i++)
                result = result.Substring(1, s.Length - 1) + result.Substring(0, 1);
            return result;

        }
        private static string XOR(string x, string y)
        {
            string ans = "";
            for (int i = 0; i < x.Length; i++)
            {
                // If the Character matches
                if (x[i] == y[i])
                    ans += "0";
                else
                    ans += "1";
            }
            return ans;
        }
        private string BinaryValueOf(string hexaValue)
        {

            string binarystring = String.Join(String.Empty,
                hexaValue.Replace("0x", "").Select(
                    c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
                )
            );
            return binarystring;
        }
        private string HexaValueOf(string binaryValue)
        {
            var hex = string.Join("", Enumerable.Range(0, binaryValue.Length / 8)
                                                .Select(i => Convert.ToByte(binaryValue.Substring(i * 8, 8), 2).ToString("X2")));
            return "0x" + hex;
        }
        #endregion
    }
}
