using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private Dictionary<char, double> frequencyInformation = new Dictionary<char, double>{
            {'e', 12.51},
            {'t', 9.25},
            {'a', 8.17},
            {'o', 7.60},
            {'i', 7.26},
            {'n', 7.09},
            {'s', 6.54},
            {'r', 6.12},
            {'h', 5.49},
            {'l', 4.14},
            {'d', 3.99},
            {'c', 3.06},
            {'u', 2.71},
            {'m', 2.53},
            {'f', 2.30},
            {'p', 2.00},
            {'g', 1.96},
            {'w', 1.92},
            {'y', 1.73},
            {'b', 1.54},
            {'v', 0.99},
            {'k', 0.67},
            {'x', 0.19},
            {'j', 0.16},
            {'q', 0.11},
            {'z', 0.09}
        };
        private string alphabets = "abcdefghijklmnopqrstuvwxyz";


        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            //List<char> keyChar = new List<char>{ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            //'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            //'s', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

            //plainText = plainText.ToLower();
            //cipherText = cipherText.ToLower();
            //for (int i = 0; i < cipherText.Length; i++)
            ////{
            ////    int index = ((int)plainText[i] - 97) % 26;
            ////    //Console.WriteLine("Index= " + index.ToString());
            ////    //Console.WriteLine("IndexPlain= "+ ((int)plainText[i]).ToString());
            ////    //Console.WriteLine("Plainchar= "+ plainText[i]);

            ////    keyChar[index] = cipherText[i];
            ////}

            //string key = "";
            ////foreach (char c in keyChar)
            ////     key += c;

            ////key = key.ToLower();
            ////Console.WriteLine(key);
            //string concat = alphabets;
            //string test = "";
            //for (int i = 0; i < alphabets.Length; i++)
            //{
            //    bool found = false;
            //    char letter = '_';
            //    for (int j = 0; j < plainText.Length; j++)
            //    {
            //        if (alphabets[i] == plainText[j])
            //        {
            //            letter = cipherText[j];
            //            found = true;
            //            test += alphabets[i];
            //            break;
            //        }
            //    }
            //    if (found)
            //        key += letter;
            //    else
            //        concat += alphabets[i];

            //}

            ////key += concat;
            //Console.WriteLine(alphabets);
            //Console.WriteLine(key);
            //Console.WriteLine(test);
            //Console.WriteLine(concat);
            //return key;


            /*                                  WORKING 
             plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            string alphaCopy = alphabets;

            char[] KeyArray = new char[26];

            //for (int i = 0; i < KeyArray.Length; i++)
            //    KeyArray[i] = '*';

            for (int i = 0; i < plainText.Length; i++)
                for (int j = 0; j < alphabets.Length; j++)
                    if (plainText[i] == alphabets[j])
                    {
                        KeyArray[j] = cipherText[i];
                        alphaCopy = alphaCopy.Replace(KeyArray[j].ToString(), string.Empty);
                        break;
                    }


            for (int i = 0; i < KeyArray.Length; i++)
                //if (KeyArray[i] == '*' && alphaCopy != "")
                if (KeyArray[i] == default(char) && alphaCopy != "")
                {
                    KeyArray[i] = alphaCopy[0];
                    alphaCopy = alphaCopy.Replace(alphaCopy[0].ToString(), string.Empty);
                }

            string key = new string(KeyArray);
            //Console.WriteLine(key);
            return key;
             */


            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            string alphaCopy = alphabets;

            string key = "";

            for (int i = 0; i < alphabets.Length; i++)
            {
                char letter = '*';
                for (int j = 0; j < plainText.Length; j++)
                    if (alphabets[i] == plainText[j])
                    {
                        letter = cipherText[j];
                        alphaCopy = alphaCopy.Replace(cipherText[j].ToString(), string.Empty);
                        break;
                    }
   
                key += letter;

            }

            string res = "";
            for (int i = 0; i < alphabets.Length; i++)
                if (key[i] == '*' && alphaCopy != "")
                {
                    res += alphaCopy[0];
                    alphaCopy = alphaCopy.Replace(alphaCopy[0].ToString(), string.Empty);
                }
                else
                    res += key[i];

     
            //Console.WriteLine(res);
            return res;
        }

        public string Decrypt(string cipherText, string key)
        {
            // throw new NotImplementedException();
            string alpha_letters = "abcdefghijklmnopqrstuvwxyz";
            string decryptedResult = "";
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (cipherText[i] == key[j])
                    {
                        decryptedResult += alpha_letters[j];
                    }
                }
            }
            return decryptedResult;

        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string alpha_letters = "abcdefghijklmnopqrstuvwxyz";
            string encryptedResult = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < alpha_letters.Length; j++)
                {
                    if (plainText[i] == alpha_letters[j])
                    {
                        encryptedResult += key[j];
                    }
                }
            }
            return encryptedResult;
        }







        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	=
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        /// 

        public string AnalyseUsingCharFrequency(string cipher)
        {

            //throw new NotImplementedException();

            cipher = cipher.ToLower();

            Dictionary<char, double> counter = new Dictionary<char, double>();

            for (int i = 0; i < cipher.Length; i++)
                if (counter.ContainsKey(cipher[i]))
                    counter[cipher[i]]++;
                else
                    counter[cipher[i]] = 1;

            Dictionary<char, double> frequencies = new Dictionary<char, double>();

            foreach (var item in counter)
                //{
                frequencies[item.Key] = Math.Round((item.Value / cipher.Length) * 100, 2);
            //    Console.WriteLine(item.Key + " = " + frequencies[item.Key].ToString());
            //}

            string key = "";
            for (int i = 0; i < alphabets.Length; i++)
            {
                char c = GetClosestLetter(frequencies[alphabets[i]], key);

                Console.WriteLine("Letter: " + alphabets[i] + " ,Enc: " + c);
                Console.WriteLine("Freq: " + frequencies[alphabets[i]].ToString() + " ,Freq: " + frequencyInformation[c].ToString());

                key += c;
            }

            Console.WriteLine(key);


            return key;

        }

        private char GetClosestLetter(double frequency, string text)
        {
            // Find the key with the closest value in the dictionary
            char closestLetter = frequencyInformation
                .OrderBy(kvp => Math.Abs(kvp.Value - frequency))
                .First().Key;

            char nextAvailableLetter = closestLetter;

            // Find the next available letter not present in the text
            while (text.Contains(nextAvailableLetter.ToString()))
            {
                int index = Array.IndexOf(frequencyInformation.Keys.ToArray(), nextAvailableLetter);

                // Check if the next available letter is not the last one in the dictionary
                if (index + 1 < frequencyInformation.Count)
                {
                    // Get the next letter in terms of frequency
                    nextAvailableLetter = frequencyInformation.Keys.ElementAt(index + 1);
                }
                else
                {
                    // Handle the case where we have checked all letters and none is available
                    // You can return a special character or throw an exception based on your requirements
                    // For simplicity, returning the closest letter in case of no match.
                    return closestLetter;
                }
            }

            return nextAvailableLetter;
        }
    }
}