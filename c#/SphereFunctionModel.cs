using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artificial_Earth_Models_Satelite
{
    class SphereFunctionModel : MathModel
    {
        private const double mu = 398600.436e9; //3.98600544 * 100000000000000;//

        private double sigma(int k)
        {
            if (k == 0)
                return 0.5;
            else
                return 1;
        }

        protected int d_counterStop;
        protected double t0;
        protected double tk;
        protected Vector o_y = new Vector(6);
        protected Vector y0 = new Vector(6);
        
       
        public SphereFunctionModel(Vector o_V_Y, Vector o_V_Y0, double d_t0, double d_tk)
        {
            o_y.equal(o_V_Y);
            y0.equal(o_V_Y0);
            t0 = d_t0;
            tk = d_tk;
        }

        public override void Init(ref Vector I_y, ref Vector I_y0, ref double I_t0, ref double I_tk)
        {
            I_t0 = t0;
            I_tk = tk;
            I_y = new Vector(o_y.RowsCount);
            I_y0 = new Vector(y0.RowsCount);


            I_y.equal(o_y);
            I_y0.equal(y0);
            

        }
 
        private double NormalLegrdanPolinom(int n, int m,double fi)
        {
            if (n == m && n == 0)
                return 1;   
            if (n == m && n != 0)
                return (NormalLegrdanPolinom(n - 1, m - 1, fi) * Math.Cos(fi) * Math.Sqrt((2 * n + 1) / (2 * n * sigma(m - 1))));
            if (n > m)
            {
                return ((NormalLegrdanPolinom(n - 1, m, fi) * Math.Sin(fi) * Math.Sqrt((4 * n * n - 1) / (n * n - m * m))) -
                    (NormalLegrdanPolinom(n - 2, m, fi) * Math.Sqrt(((Math.Pow(n - 1, 2) - m * m) * (2 * n + 1)) / ((n * n - m * m) * (2 * n - 3)))));
            }
            else
            {
                return 0;
            }

        }

        private double DiffNormalLegrdan(int n,double fi)
        {
            return (Math.Sqrt(0.5 * n * (n + 1)) * NormalLegrdanPolinom(n, 1, fi));
        }

        double Cn0(int n, double[] J,int j)
        {
            
             return (-J[j] / Math.Sqrt(2 * n + 1));
           
        }

        protected void FigureOut(Vector y,Vector val)
        {
            Vector tmp = new Vector(3);
            double d_tempRo = 0;
            double d_tempFi = 0;
            double a =6378136;
            double[] J = new double[] { 1082.62575e-6, -2.37089e-6, 6.08e-9, -1.40e-11 };
          

            double lamda = Math.Atan2(y[2], y[0]); // y/x
            double fi = Math.Atan2(y[4], Math.Sqrt(y[0] * y[0] + y[2] * y[2])); //z/sqrt(x*x + y*y) 
            double ro = Math.Sqrt(y[0] * y[0] + y[2] * y[2] + y[4] * y[4]);

            Vector g = new Vector(3);
            int count =0;
            double val0 = 0;
            double val1 = 0;
            for(int n=2;n<=8;n++)
            {
                val0 = Cn0(n, J, count);
                val1 = DiffNormalLegrdan(n, fi) * Math.Pow(a / ro, n);
                d_tempRo += (n + 1) * Math.Pow(a / ro, n) * Cn0(n, J, count) * NormalLegrdanPolinom(n, 0, fi);
                d_tempFi += Math.Pow(a / ro, n) * Cn0(n, J, count) * DiffNormalLegrdan(n, fi);
                count++;
                n++;
            }

            g[0] = (-mu / Math.Pow(ro, 2)) * (1 + d_tempRo); //- mu * d_tempRo / Math.Pow(ro, 2);
            g[1] = (mu * d_tempFi / Math.Pow(ro, 2));

            tmp = TransitToRectangle(ro, y, g);

            val[1] = tmp[0];
            val[3] = tmp[1];
            val[5] = tmp[2];

           

           
            
        }

        private Vector TransitToRectangle(double ro, Vector y ,Vector g)
        {
            double r_0 = Math.Sqrt(y[0] * y[0] + y[2] * y[2]);
            Matrix transit = new Matrix(3, 3);

            transit[0, 0] = y[0] / ro;
            transit[1, 0] = -y[0] * y[4] / (ro * r_0);
            transit[2, 0] = -y[2] / r_0;

            transit[0, 1] = y[2] / ro;
            transit[1, 1] = -y[2] * y[4] / (ro * r_0);
            transit[2, 1] = y[0] / r_0;

            transit[0, 2] = y[4] / ro;
            transit[1, 2] = r_0 / ro;

            return transit * g;
        }

        public override void Func(Vector v_y, Vector dydx, double x)
        {
          
           

            double r = Math.Pow(Math.Pow(v_y[0], 2) + Math.Pow(v_y[2], 2) + Math.Pow(v_y[4], 2), 1.5);
            dydx[0] = v_y[1];//dx=a
            dydx[1] = -v_y[0] * mu / r;//da=-mu*(x/|r|^3)

            dydx[2] = v_y[3];//dy=b;
            dydx[3] = -v_y[2] * mu / r;//db=-mu*(y/|r|^3)

            dydx[4] = v_y[5];//dz=c
            dydx[5] = -v_y[4] * mu / r;//dc=-mu*(c/|r|^3)


           
            FigureOut(v_y, dydx);
            d_counterStop++;

          

           
          

        }
    }
}
