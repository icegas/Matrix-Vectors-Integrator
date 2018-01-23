using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueMatrix_Integrator
{
    class MathModel
    {
      
        public List<Matrix>o_M_Result;
        protected Matrix y0  { get;  set; }
        protected double t0  { get;  set; }
        protected double tk  { get;  set; }
        protected double inc { get;  set; }
        protected double err {get; set;}
        protected MathModel(Matrix I_y0, double I_t0,  double I_tk,  double I_inc,  double fault)
        {
            y0 = new Matrix(I_y0.ColumnsCount, I_y0.RowsCount);
            y0.equal(I_y0);
            t0 = I_t0;
            tk = I_tk;
            inc = I_inc;
            err = fault;
        }
        public virtual void Func(Matrix v_y, ref Matrix dydx, double x)
        {

        }

        public void Init(ref Matrix I_y0,ref Matrix I_y, ref double I_t0, ref double I_tk,ref double I_inc,ref double fault)
        {
            I_t0 = t0;
            I_tk = tk;
            I_y = new Matrix(y0.ColumnsCount,y0.RowsCount);
            I_y0 = new Matrix(y0.ColumnsCount,y0.RowsCount);
            I_y0.equal(y0);
            I_inc = inc;
            fault = err;
        }

        public void GetMatrixResult(List<Matrix> matrix)
        {
            o_M_Result = new List<Matrix>();//(matrix.ColumnsCount, matrix.RowsCount);

            for (int k = 0; k < matrix.Count; k++)
            {
                o_M_Result.Add(matrix[k]);
                
            }
        }
       
    }
}
