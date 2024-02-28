using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string decryptedText = "";
            char[,] matrix5x5 = new char[5, 5];
            int txtCounter = 0;

            if (key.Contains('j'))
            {
                key.Replace('j', 'i');
            }
            //set the 5 * 5 matrix
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (txtCounter < key.Length && FoundChar(key[txtCounter], matrix5x5))
                    {
                        txtCounter++;
                    }
                    if (txtCounter < key.Length && !(FoundChar(key[txtCounter], matrix5x5)))
                    {
                        matrix5x5[i, j] = key[txtCounter];
                        txtCounter++;
                        continue;
                    }
                    txtCounter++;
                    if (txtCounter >= key.Length)
                    {
                        for (int k = 97; k <= 122; k++)
                        {
                            if (k == 106)
                            {
                                continue;
                            }
                            if (!FoundChar((char)k, matrix5x5))
                            {
                                matrix5x5[i, j] = (char)k;
                                break;
                            }
                        }
                    }
                }
            }
            //Get the position of every 2 chars in the matrix
            int FirstPosX = 0;
            int FirstPosY = 0;
            int SecPosX = 0;
            int SecPosY = 0;
            for (int i = 0; i < (cipherText.Length) / 2 ; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix5x5[j, k] == cipherText[i * 2])
                        {
                            FirstPosX = j;
                            FirstPosY = k;
                        }
                        if (matrix5x5[j, k] == cipherText[(i * 2) + 1])
                        {
                            SecPosX = j;
                            SecPosY = k;
                        }
                    }
                }
                //3 cases of solutions
                //First case same row 
                if (FirstPosX == SecPosX)
                {
                    decryptedText += matrix5x5[FirstPosX, (FirstPosY -1+5) % 5];
                    decryptedText += matrix5x5[SecPosX, (SecPosY - 1+5) % 5];
                }
                //Second case same column
                else if (FirstPosY == SecPosY)
                {
                    decryptedText += matrix5x5[(FirstPosX - 1 + 5) % 5, FirstPosY];
                    decryptedText += matrix5x5[(SecPosX - 1 + 5) % 5, SecPosY];
                }
                //Third case rectangle shape
                else
                {
                    decryptedText += matrix5x5[FirstPosX, SecPosY];
                    decryptedText += matrix5x5[SecPosX, FirstPosY];
                }

            }

            for (int i = 0; i < decryptedText.Length; i++)
            {
                if (i < decryptedText.Length - 3)
                {
                    if (decryptedText[i] == decryptedText[i + 2] && decryptedText[i + 1] == 'x')
                    {
                        decryptedText = decryptedText.Remove(i + 1, 1);
                    }
                }
                if (decryptedText[decryptedText.Length - 1] == 'x')
                {
                    decryptedText = decryptedText.Remove(decryptedText.Length - 1, 1);
                }
            }
            return decryptedText.ToLower();
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string encriptedText="";
            char[,] matrix5x5= new char[5,5];
            int txtCounter= 0;

            if (key.Contains('j'))
            {
                key.Replace('j', 'i');
            }
            //set the 5 * 5 matrix
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (txtCounter<key.Length && FoundChar(key[txtCounter],matrix5x5))
                    {
                        txtCounter++;
                    }
                    if (txtCounter<key.Length && !(FoundChar(key[txtCounter], matrix5x5)))
                    {
                        matrix5x5[i, j] = key[txtCounter];
                        txtCounter++;
                        continue;
                    }
                    txtCounter++;
                    if (txtCounter >= key.Length)
                    {
                        for (int k = 97; k <= 122 ; k++)
                        {
                            if (k==106)
                            {
                                continue;
                            }
                            if (!FoundChar((char)k, matrix5x5))
                            {
                                matrix5x5[i,j]=(char)k;
                                break;
                            }
                        }
                    }
                }
            }
            //Get the position of every 2 chars in the matrix
            int FirstPosX=0;
            int FirstPosY=0;
            int SecPosX = 0;
            int SecPosY = 0;
            for (int i = 0; i < ((plainText.Length)/2)+(plainText.Length %2); i++)
            {
                if (i ==(plainText.Length/2) && plainText.Length%2!=0)
                {
                    plainText += "x";
                }
                if (plainText[i * 2] == plainText[(i * 2) + 1])
                {
                    plainText = plainText.Insert((i * 2) + 1, "x");
                }
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matrix5x5[j, k] == plainText[i * 2])
                        {
                            FirstPosX = j;
                            FirstPosY = k;
                        }
                        if (matrix5x5[j, k] == plainText[(i * 2) + 1])
                        {
                            SecPosX = j;
                            SecPosY = k;
                        }
                    }
                }
                //3 cases of solutions
                //First case same row 
                if (FirstPosX == SecPosX)
                {
                    encriptedText+=matrix5x5[FirstPosX,(FirstPosY+1)%5];
                    encriptedText+=matrix5x5[SecPosX, (SecPosY + 1)%5];
                }
                //Second case same column
                else if (FirstPosY == SecPosY)
                {
                    encriptedText+=matrix5x5[(FirstPosX+1)%5,FirstPosY];
                    encriptedText+=matrix5x5[(SecPosX + 1) % 5, SecPosY];
                }
                //Third case rectangle shape
                else
                {
                    encriptedText+=matrix5x5[FirstPosX, SecPosY];
                    encriptedText+=matrix5x5[SecPosX,FirstPosY];
                }

            }
            return encriptedText.ToUpper();
        }
        public bool FoundChar(char Text, char[,] Ref)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if ( Text == Ref[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}