using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artificial_Earth_Models_Satelite
{
    class MathModel
    {
        public Matrix o_M_Result;

        public virtual void Func(Vector y, Vector dydx, double x)
        {

        }

        public virtual void Init(ref Vector I_y, ref Vector I_y0, ref double I_t0, ref double I_tk)
        {
            
        }

        public void GetMatrixResult(Matrix matrix)
        {
            o_M_Result = new Matrix(matrix.ColumnsCount, matrix.RowsCount);
            for (int i = 0; i < matrix.RowsCount; i++)
                for (int j = 0; j < matrix.ColumnsCount; j++)
                    o_M_Result[j, i] = matrix[j, i];
        }
    }
}
