using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q , 
                C = Calc(M, e, n); 

            return C;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int n = p * q,
             totient_n = (p - 1) * (q - 1),
             d = new ExtendedEuclid().GetMultiplicativeInverse(e, (int)totient_n),
             M = Calc(C, d, n);
            return M;
        }

        private int Calc(int base_, int power, int mod)
        {
            int num = 1;

            for (int i = 0; i < power; i++)
                num = (num * base_) % mod;

            return num;
        }
    }
}
