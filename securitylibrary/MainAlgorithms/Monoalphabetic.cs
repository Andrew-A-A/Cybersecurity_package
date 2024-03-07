using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private const string alphabets = "abcdefghijklmnopqrstuvwxyz";

        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();

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

            return res;
        }

        public string Decrypt(string cipherText, string key)
        {
            string decryptedResult = "";
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (cipherText[i] == key[j])
                    {
                        decryptedResult += alphabets[j];
                    }
                }
            }
            return decryptedResult;
        }

        public string Encrypt(string plainText, string key)
        {
            string encryptedResult = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < alphabets.Length; j++)
                {
                    if (plainText[i] == alphabets[j])
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
            List<char> frequencyInformation = new List<char>() { 'e', 't', 'a', 'o', 'i', 'n', 's', 'r', 'h', 'l', 'd', 'c', 'u', 'm', 'f', 'p', 'g', 'w', 'y', 'b', 'v', 'k', 'x', 'j', 'q', 'z' };

            cipher = cipher.ToLower();
            string key = "";

            Dictionary<char, double> counter = new Dictionary<char, double>();

            for (int i = 0; i < cipher.Length; i++)
                if (counter.ContainsKey(cipher[i]))
                    counter[cipher[i]]++;
                else
                    counter[cipher[i]] = 1;

            Dictionary<char, double> frequencies = counter.OrderByDescending(kv => kv.Value).ToDictionary(kv => kv.Key, kv => kv.Value);

            Dictionary<char, char> KeyMap = new Dictionary<char, char>();

            for (int i = 0; i < frequencyInformation.Count; i++)
                if (i < frequencies.Count)
                {
                    char c = frequencies.ElementAt(i).Key;
                    KeyMap[frequencyInformation[i]] = c;
                }

            for (int i = 0; i < alphabets.Length; i++)
                key += KeyMap[alphabets[i]];

            return Decrypt(cipher, key);
        }

    }
}