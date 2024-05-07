using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SecurityLibrary.DiffieHellman
{


    public class DiffieHellman
    {

      public  int power(int f, int s, int sf)
        {
            //throw new NotImplementedException();
            int result = 1;
            f = f % sf;

            while (s > 0)
            {
                if ((s & 1) > 0)
                    result = (result * f) % sf;

                s = s >> 1;
                f = (f * f) % sf;
            }
            return result;
        }

       public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {

            //throw new NotImplementedException();
            int A = power(alpha, xa, q);
            int B = power(alpha, xb, q);

            int keyA = power(B, xa, q);
            int keyB = power(A, xb, q);

            return new List<int>() { keyA, keyB };
        }
    }
}