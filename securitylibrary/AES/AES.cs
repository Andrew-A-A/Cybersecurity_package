using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class AES : CryptographicTechnique
    {
    
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

       
        private static readonly byte[] InverseSbox = new byte[]
        {
            0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
            0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
            0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
            0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
        };

        
        private static readonly byte[] Rcon = new byte[]
        {
        0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36
        };

        public override string Decrypt(string cipherText, string key)
        {
            byte[,] state = new byte[4, 4];

            byte[] cipherBytes = new byte[cipherText.Length / 2 - 1];
            for (int i = 0; i < cipherBytes.Length; i++)
            {
                cipherBytes[i] = Convert.ToByte(cipherText.Substring(2 * (i + 1), 2), 16);
            }
            byte[,] expandedKey = KeyExpansion(key);
            string[,] keyNW = new string[4, 44];
            keyNW = convertToHex(expandedKey);
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    state[j, i] = cipherBytes[counter];
                    counter++;
                }
            }
            string[,] hexState = new string[4, 4];
            int round = 10;
            byte[,] usedKeyPart = usedPartExpandedKey(expandedKey, round);
            hexState = convertToHex(state);
            hexState = convertToHex(usedKeyPart);
            //initial addRoundKey
            state = AddRoundKey(state, usedKeyPart);
            hexState = convertToHex(state);
            round--;
            //Repeating for 9 times
            for (; round > 0; round--)
            {
                state = ShiftRows(state,false);
                hexState = convertToHex(state);
                //state = shiftrows
                state = SubBytes(state, false);
                hexState = convertToHex(state);
                usedKeyPart = usedPartExpandedKey(expandedKey, round);
                state = AddRoundKey(state, usedKeyPart);
                hexState = convertToHex(state);
                //state = mixcol
                state = MixColumns(state, false);
                hexState = convertToHex(state);
            }
            //Last round
            //state = shiftrows
            state = ShiftRows(state,false);
            state = SubBytes(state, false);
            usedKeyPart = usedPartExpandedKey(expandedKey, round);
            state = AddRoundKey(state, usedKeyPart);
            hexState = convertToHex(state);
            string plain = "0x";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (hexState[j, i].ToString().Length == 1)
                    {
                        plain += "0";
                    }
                    plain += hexState[j, i].ToString();
                }
            }
            return plain;
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
            string[,] keyNW = new string[4, 44];
            keyNW=convertToHex(expandedKey);
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int  j= 0; j < 4; j++)
                {
                    state[j, i]=plainBytes[counter] ;
                    counter++;
                }
            }
            int round=0;
            byte[,] usedKeyPart = usedPartExpandedKey(expandedKey, round);
            string[,] hexState = new string[4, 4];
            hexState = convertToHex(state);
            hexState = convertToHex(usedKeyPart);
            //initial Add Round Key
            state = AddRoundKey(state, usedKeyPart);
            hexState = convertToHex(state);
            round++;
            //Repeat for 9 times
            for (; round < 10; round++)
            {
                state = SubBytes(state, true);
                hexState = convertToHex(state);
                //state = shiftrows
                state = ShiftRows(state,true);
                hexState = convertToHex(state);
                //state = mixcols
                state = MixColumns(state,true);
                hexState=convertToHex(state);
                //convert the mix column matrix to hex for testing
                
                
                usedKeyPart = usedPartExpandedKey(expandedKey, round);
                hexState=convertToHex(state) ;
                state = AddRoundKey(state, usedKeyPart);
                hexState = convertToHex(state);
            }
            //Last Round
            state = SubBytes(state, true);
            //state = shiftrows
            state = ShiftRows(state, true);
            usedKeyPart = usedPartExpandedKey(expandedKey, round);
            state = AddRoundKey(state,usedKeyPart);
            hexState = convertToHex(state);

            string cipher="0x";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (hexState[j,i].ToString().Length==1)
                    {
                        cipher += "0";
                    }
                    cipher += hexState[j, i].ToString();
                }
            }
            return cipher;
        }


        public static byte[,] KeyExpansion(string originalKey)
        {
           
            byte[] keyBytes = new byte[originalKey.Length / 2 - 1];
            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] = Convert.ToByte(originalKey.Substring(2 * (i + 1), 2), 16);
            }

            int Nk = keyBytes.Length / 4; 
            int Nr = Nk + 6; 
            int expandedKeySize = 4 * 4 * (Nr + 1);
            byte[,] expandedKey = new byte[4, 4 * (Nr + 1)];

            
            for (int i = 0; i < Nk; i++)
            {
                expandedKey[0, i] = keyBytes[i * 4];
                expandedKey[1, i] = keyBytes[i * 4 + 1];
                expandedKey[2, i] = keyBytes[i * 4 + 2];
                expandedKey[3, i] = keyBytes[i * 4 + 3];
            }

            for (int col = Nk; col < (4 * (Nr + 1)); col++)
            {
                byte[] temp = new byte[4];

               
                for (int row = 0; row < 4; row++)
                {
                    temp[row] = expandedKey[row, col - 1];
                }

             
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
        public static byte[,] SubBytes(byte[,] state, bool flag)
        {
            byte[,] updatedState = new byte[4, 4];
            //if flag = 1 then use forward S-box (for encrypt)
            //else then use inverse S-box (for decrypt)

            StringBuilder hex = new StringBuilder(state.Length * 2);
            foreach (byte b in state)
                hex.AppendFormat("{0:x2}", b);

            string hexString = hex.ToString();
            if (flag)
            {
                int cnt = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        string first = "0x" + hexString[cnt];
                        int intValue1 = Convert.ToInt32(first, 16);
                        string second = "0x" + hexString[cnt + 1];
                        int intValue2 = Convert.ToInt32(second, 16);

                        updatedState[i, j] = Sbox[(intValue1 * 16) + intValue2];
                        cnt += 2;
                    }
                }
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        string first = "0x" + hexString[cnt];
                        int intValue1 = Convert.ToInt32(first, 16);
                        string second = "0x" + hexString[cnt + 1];
                        int intValue2 = Convert.ToInt32(second, 16);

                        updatedState[i, j] = InverseSbox[(intValue1 * 16) + intValue2];
                        cnt += 2;
                    }
                }
            }
            return updatedState;
        }

        public static byte[,] ShiftRows(byte[,] state,bool flag)
        {
            byte[,] newState = new byte[4, 4];
            if (flag)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 0)
                        {
                            newState[i, j] = state[i, j];
                        }
                        else if (i == 1)
                        {
                            newState[i, j] = state[i, (j + 1) % 4];
                        }
                        else if (i == 2)
                        {
                            newState[i, j] = state[i, (j + 2) % 4];
                        }
                        else if (i == 3)
                        {
                            newState[i, j] = state[i, (j + 3) % 4];
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 3; j >= 0; j--)
                    {
                        if (i == 0)
                        {
                            newState[i, j] = state[i, j];
                        }
                        else if (i == 1)
                        {
                            newState[i, j] = state[i, (j + 3) % 4];
                        }
                        else if (i == 2)
                        {
                            newState[i, j] = state[i, (j + 2) % 4];
                        }
                        else if (i == 3)
                        {
                            newState[i, j] = state[i, (j + 1) % 4];
                        }
                    }
                }
            }
            return newState;
        }
        public static byte[,] MixColumns(byte[,] state,bool flag)
        {
            byte[,] newState = new byte[4, 4];
            byte[,] forwardMatrix = new byte[4, 4]
            {
                { 2, 3, 1, 1},
                { 1, 2, 3, 1},
                { 1, 1, 2, 3},
                { 3, 1, 1, 2}
            };

            byte[,] inverseMatrix = new byte[4, 4]
            {
                {14,11,13,9},
                {9,14,11,13},
                {13,9,14,11},
                {11,13,9,14}
            };

            if (flag)
            {


                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            int multRes = 0;
                            string binary;
                            if (forwardMatrix[i, k] == 3)
                            {
                                multRes = 2 * state[k, j];
                                binary = Convert.ToString(multRes, 2);
                                if (binary.Length > 8)
                                {
                                    binary = binary.Substring(1);
                                    multRes = Convert.ToByte(binary, 2);
                                    multRes ^= 27; // XOR with 1B
                                }
                                multRes ^= state[k, j];
                            }
                            else if (forwardMatrix[i, k] == 2)
                            {
                                multRes = forwardMatrix[i, k] * state[k, j];
                                binary = Convert.ToString(multRes, 2);
                                if (binary.Length > 8)
                                {
                                    binary = binary.Substring(1);
                                    multRes = Convert.ToByte(binary, 2);
                                    multRes ^= 27;
                                }
                            }
                            else if (forwardMatrix[i, k] == 1)
                            {
                                multRes = forwardMatrix[i, k] * state[k, j];
                            }
                            newState[i, j] ^= (byte)multRes;
                        }
                    }
                }
            }
            else 
            {

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            byte mulres = Multiply(state[k, j], inverseMatrix[i, k]);
                            newState[i, j] ^= mulres;
                        }
                    }
                }
            }

            return newState;
        }


        public static string[,] convertToHex(byte[,] state)
        {
            string[,] hexState = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hexState[i, j] = Convert.ToString(state[i, j], 16);
                }
            }
            return hexState;
        }
        static byte Multiply(byte a, byte b)
        {
            byte result = 0;
            byte lsb = 0x01;
            byte msb = 0x80;

            for (int i = 0; i < 8; i++)
            {
                if ((b & lsb) != 0)
                {
                    result ^= a;
                }
                bool high_set = (a & msb) != 0;
                a *= 2;
                if (high_set)
                {
                    a ^= 0x1B;
                } 
                b /= 2;
            }

            return result;
        }

    }


}

