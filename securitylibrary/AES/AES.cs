using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
        // AES S-box
        private static readonly byte[] Sbox = new byte[]
        {
        0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76,
        0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0,
        0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15,
        0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75,
        0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84,
        0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF,
        0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8,
        0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2,
        0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73,
        0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB,
        0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79,
        0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08,
        0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A,
        0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E,
        0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF,
        0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16
        };

        // Rcon values for key schedule
        private static readonly byte[] Rcon = new byte[]
        {
        0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36
        };

        public override string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string plainText, string key)
        {
            byte[,] state = new byte[4,4];

            byte[] plainBytes = new byte[plainText.Length / 2 - 1];
            for (int i = 0; i < plainBytes.Length; i++)
            {
                plainBytes[i] = Convert.ToByte(plainText.Substring(2 * (i + 1), 2), 16);
            }
            byte[,] expandedKey = KeyExpansion(key);
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int  j= 0; j < 4; j++)
                {
                    state[i, j]=plainBytes[counter] ;
                    counter++;
                }
            }
            int round=0;
            byte[,] usedKeyPart = usedPartExpandedKey(expandedKey, round);
            state = AddRoundKey(state, usedKeyPart);
            round++;
            return state.ToString();
        }


        public static byte[,] KeyExpansion(string originalKey)
        {
            // Remove "0x" prefix and convert hex string to byte array
            byte[] keyBytes = new byte[originalKey.Length / 2 - 1];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Convert.ToByte(originalKey.Substring(2 * (i + 1), 2), 16);
            }

            int Nk = keyBytes.Length / 4; // Number of 32-bit words in the key
            int Nb = 4; // Number of columns in state (always 4 for AES)
            int Nr = Nk + 6; // Number of rounds
            int expandedKeySize = 4 * Nb * (Nr + 1); // Size of expanded key in bytes

            byte[,] expandedKey = new byte[Nb, Nb * (Nr + 1)]; // Expanded key

            // Copy original key into the first Nb columns of the expanded key
            for (int i = 0; i < Nk; i++)
            {
                expandedKey[i, 0] = keyBytes[i * 4];
                expandedKey[i, 1] = keyBytes[i * 4 + 1];
                expandedKey[i, 2] = keyBytes[i * 4 + 2];
                expandedKey[i, 3] = keyBytes[i * 4 + 3];
            }

            for (int col = Nk; col < (Nb * (Nr + 1)); col++)
            {
                byte[] temp = new byte[4];

                // Copy the previous column into temp
                for (int row = 0; row < 4; row++)
                {
                    temp[row] = expandedKey[row, col - 1];
                }

                // Perform key schedule core
                if (col % Nk == 0)
                {
                    // RotWord
                    byte tempByte = temp[0];
                    temp[0] = temp[1];
                    temp[1] = temp[2];
                    temp[2] = temp[3];
                    temp[3] = tempByte;

                    // SubWord
                    temp[0] = Sbox[temp[0]];
                    temp[1] = Sbox[temp[1]];
                    temp[2] = Sbox[temp[2]];
                    temp[3] = Sbox[temp[3]];

                    // XOR with Rcon
                    temp[0] ^= Rcon[col / Nk - 1];
                }
                else if (Nk > 6 && col % Nk == 4)
                {
                    // SubWord only for 256-bit keys
                    temp[0] = Sbox[temp[0]];
                    temp[1] = Sbox[temp[1]];
                    temp[2] = Sbox[temp[2]];
                    temp[3] = Sbox[temp[3]];
                }

                // XOR with the column Nk positions before
                for (int row = 0; row < 4; row++)
                {
                    expandedKey[row, col] = (byte)(expandedKey[row, col - Nk] ^ temp[row]);
                }
            }

            return expandedKey;
        }

        // Helper method to print the expanded key
        public static void PrintExpandedKey(byte[,] expandedKey)
        {
            int columns = expandedKey.GetLength(1);
            int rows = expandedKey.GetLength(0);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write($"{expandedKey[i, j]:X2} ");
                }
                Console.WriteLine();
            }
        }

        //determine which part of the expanded key will be used in the round
        public static byte[,] usedPartExpandedKey(byte[,] expandedKey,int index)
        {
            byte[,]newKey= new byte[4,4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    newKey[i, j] = expandedKey[i, j + (index*4)];
                }
            }
            return newKey;
        }
        public static byte[,] AddRoundKey(byte[,]plainText, byte[,] expandedKey)
        {
            byte[,] newState = new byte[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    newState[i,j] = (byte)(plainText[i, j] ^ expandedKey[i, j]);
                }
            }
            return newState;
        }

    }


}

