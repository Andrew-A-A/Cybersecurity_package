using System.Collections.Generic;
using System.Linq;

public class matrixHelperFunctions
{
    /* DO NOT IMPORT 
 * JUST COPY FUNCTIONS */
    public static int matDeterm(List<int> matrix)
    {

        if (matrix.Count() == 4)
        {

            // 2d Matrix :  a b      [a b c d]
            //              c d
            int ad = matrix[0] * matrix[3];
            int bc = matrix[1] * matrix[2];
            return ad - bc;
        }
        else
        {
            // 3d Matrix :  a b c   [a b c d e f g h i]
            //              d e f
            //              g h i
            int a = matrix[0];
            int b = matrix[1];
            int c = matrix[2];
            int ei = matrix[4] * matrix[8];
            int fh = matrix[5] * matrix[7];
            int di = matrix[3] * matrix[8];
            int fg = matrix[5] * matrix[6];
            int dh = matrix[3] * matrix[7];
            int eg = matrix[4] * matrix[6];
            return (a * (ei - fh) - b * (di - fg) + c * (dh - eg));
        }
    }


    public static List<int> matMult(List<int> matrix1, List<int> matrix2)
    {
        List<int> val = new List<int>();

        // 2x2 matrix
        if (matrix1.Count() == 4)
        {
            val.Add(matrix1[0] * matrix2[0] + matrix1[1] * matrix2[2]);
            val.Add((matrix1[0] * matrix2[1] + matrix1[1] * matrix2[3]));
            val.Add((matrix1[2] * matrix2[0] + matrix1[3] * matrix2[2]));
            val.Add((matrix1[2] * matrix2[1] + matrix1[3] * matrix2[3]));
        }
        // 3x3 matrix
        else
        {
            val.Add(matrix1[0] * matrix2[0] + matrix1[1] * matrix2[3] + matrix1[2] * matrix2[6]);
            val.Add(matrix1[0] * matrix2[1] + matrix1[1] * matrix2[4] + matrix1[2] * matrix2[7]);
            val.Add(matrix1[0] * matrix2[2] + matrix1[1] * matrix2[5] + matrix1[2] * matrix2[8]);

            val.Add(matrix1[3] * matrix2[0] + matrix1[4] * matrix2[3] + matrix1[5] * matrix2[6]);
            val.Add(matrix1[3] * matrix2[1] + matrix1[4] * matrix2[4] + matrix1[5] * matrix2[7]);
            val.Add(matrix1[3] * matrix2[2] + matrix1[4] * matrix2[5] + matrix1[5] * matrix2[8]);

            val.Add(matrix1[6] * matrix2[0] + matrix1[7] * matrix2[3] + matrix1[8] * matrix2[6]);
            val.Add(matrix1[6] * matrix2[1] + matrix1[7] * matrix2[4] + matrix1[8] * matrix2[7]);
            val.Add(matrix1[6] * matrix2[2] + matrix1[7] * matrix2[5] + matrix1[8] * matrix2[8]);

        }
        return val;
    }
}