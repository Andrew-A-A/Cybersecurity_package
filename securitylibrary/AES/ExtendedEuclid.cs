using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            //int Q, A, B, R = 0, T1 = 0, T2 = 1, T = 0, hold;

            //if(number > baseN)
            //{
            //    A = number;
            //    B = baseN;
            //}
            //else
            //{
            //    A = baseN;
            //    B = number;
            //}
            //hold = A;

            //Console.WriteLine("Q | A | B | R | T1 | T2 | T");

            //Q = A / B;
            //R = A % B;
            //T = T1 - T2 * Q;

            //Console.WriteLine(Q + " | " + A + " | " + B + " | "+ R + " | " + T1 + " | "+ T2 + " | " + T);

            //while (true)
            //{   
            //    if (B == 1)
            //    {
            //        if (T2 < 0)
            //           {
            //            int res = hold + T2;
            //            Console.WriteLine("1 : " + res);
            //            return hold + T2; }
            //        else
            //            {
            //            Console.WriteLine("2 : " + T2 % baseN);

            //            return T2 % baseN; }
            //    }

            //    if (R == 0)  // No inverse
            //        return -1;


            //    A = B;
            //    B = R;
            //    T1 = T2;
            //    T2 = T;

            //    Q = A / B;
            //    R = A % B;
            //    T = T1 - T2 * Q;

            //    Console.WriteLine(Q + " | " + A + " | " + B + " | " + R + " | " + T1 + " | " + T2 + " | " + T);




            //}

            List<int> Q, A1, A2, A3, B1, B2, B3;
            A1 = new List<int>();
            A2 = new List<int>();
            A3 = new List<int>();
            B1 = new List<int>();
            B2 = new List<int>();
            B3 = new List<int>();
            Q = new List<int>();

            Q.Add(123456789); // garbage value
            A1.Add(1);
            A2.Add(0);
            
            B1.Add(0); 
            B2.Add(1);

            if (number > baseN)
            {
                A3.Add(number);
                B3.Add(baseN);
            }
            else
            {
                A3.Add(baseN);
                B3.Add(number);
            }

            int i = 0;

            Console.WriteLine("Q | A1 | A2 | A3 | B1 | B2 | B3");
            Console.WriteLine(Q[i] + " | " + A1[i] + " | " + A2[i] + " | " + A3[i] + " | " + B1[i] + " | " + B2[i]+ " | "  + B3[i] );

            while (true)
            {

                Q.Add(A3[i] / B3[i]);

                A1.Add(B1[i]);
                A2.Add(B2[i]);
                A3.Add(B3[i]);

                B1.Add(A1[i] - Q[i+1] * B1[i]);
                B2.Add(A2[i] - Q[i+1] * B2[i]);
                B3.Add(A3[i] % B3[i]);


                if (B3[i] == 1)
                    if (B2[i] < 0)
                    {
                        if(baseN * B2[i] + number * B1[i] == 1)
                            return B1[i];
                        else
                            return B2[i] + baseN;
                    }
                    else
                        return B2[i];
                //else if (B3[i] == 0)
                //{
                //    Console.WriteLine(B2[i]);
                //    return B2[i];
                //}
                else if (A3[i] % B3[i] == 0)
                    return -1;

                i++;

                //Console.WriteLine(Q[i] + " | " + A1[i] + " | " + A2[i] + " | " + A3[i] + " | " + B1[i] + " | " + B2[i] + " | " + B3[i]);

            }
        }
    }
}
