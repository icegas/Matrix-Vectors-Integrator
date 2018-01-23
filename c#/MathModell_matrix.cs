using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueMatrix_Integrator
{
    class MathModell : MathModel
    {
        public MathModell(Matrix I_y0, double I_t0, double I_tk, double I_inc, double fault)
            : base(I_y0, I_t0, I_tk, I_inc, fault)
        {
            Set(K0);
            

        }
        public double f(double dq)
        {
            if (dq > 2)
                return 2;
            if (dq < -2)
                return -2;
            return dq;

        }
        private int count = 0;
        private double Eps;
        private bool flag = false;
        
        private const double D = 0.09273021797, M = 0, s = 2;
        private const double K3 = 21, T3 = 5, K4 = 1, T4 = 0.01, Xi4 = 1;
        private const double K6 = 1, T6 = 0.2, Tf = 0.002222, Kf = 0.006625, Xif = 1;
        public Matrix A = new Matrix(6, 6);
        public Matrix B = new Matrix(1,6);
        public Matrix K0 = new Matrix(6, 6);
       

        public void Set(Matrix K0)
        {
            A.SetElement(1, 0, 1);

            A.SetElement(0, 1, -1 / (Tf * Tf));
            A.SetElement(1, 1, -2 * Xif / Tf);

            A.SetElement(5, 2, -K3 / T3);
            A.SetElement(1, 2, K3 / T3);
            A.SetElement(2, 2, -1 / T3);
            

            A.SetElement(4, 3, 1);

            if (K0.GetElement(4, 4)!=0)
            A.SetElement(2, 4, K4 / (T4 * T4));
            else
            A.SetElement(2, 4,  K4 / (T4 * T4));
            A.SetElement(3, 4, -1 / (T4 * T4));
            A.SetElement(4, 4, -2 * Xi4 / T4);

            A.SetElement(4, 5, K6 / T6);
            A.SetElement(5, 5, -1 / T6);

            

            B.SetElement(0, 1, Kf / (Tf * Tf));

            

        }

        public void SetK(ref Matrix K)
        {
            K.SetElement(0, 0, 1);
            K.SetElement(1, 1, 1);
            K.SetElement(2, 2, 1);
            K.SetElement(3, 3, 1);
            K.SetElement(4, 4, 1);
            K.SetElement(5, 5, 1);
           
        }

        public override void Func(Matrix K,ref Matrix dK, double x)
        {
         
           dK = A * K + K * A.Flip() + B * B.Flip();

        }
    }
}
