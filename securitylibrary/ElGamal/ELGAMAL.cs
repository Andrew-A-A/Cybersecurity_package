using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 

        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            List<long> Cipher = new List<long>();

            long C1 = Calc(alpha, k, q);
            Cipher.Add(C1);

            long K = Calc(y, k, q);
            long C2 = (K * m) % q;
            Cipher.Add(C2);

            return Cipher;
        }
     
        public int Decrypt(int c1, int c2, int x, int q)
        {
            int y = (int)Calc(c1, x, q);

            AES.ExtendedEuclid EE = new AES.ExtendedEuclid();
            int MulInv = EE.GetMultiplicativeInverse(y, q);

            int M = (c2 * MulInv) % q;

            return M;
        }

        private long Calc(int base_, int power, int mod)
        {
            long num = 1;

            for (int i = 0; i < power; i++)
                    num = (num * base_) % mod;

            return num;
        }
    }
}
