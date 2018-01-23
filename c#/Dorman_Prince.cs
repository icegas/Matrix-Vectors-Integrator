using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using ZedGraph;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;



namespace Artificial_Earth_Models_Satelite
{
    
    class Dorman_Prince 
    {
       
        private const int precision = 7;
        private const double fault = 1e-5;
        private double inc = 100;
        private double hnew = 10e-6;
        private double h;
        private double teta;
        private double err;
        private double x;
        private double Xout;
        private double val0;

        private Vector y;
        private Vector y0;
        private Matrix matrix;
        private double t0, tk;

        public double d_Inc { get { return inc; } set { inc = value; } }

        private double[,] a = new double[,] { { 1.0 / 5.0, 0, 0, 0, 0, 0, 0 }, 
           { 3.0 / 40.0, 9.0 / 40.0, 0, 0, 0, 0, 0 }, { 44.0 / 45.0, -56.0 / 15.0, 32.0 / 9.0, 0, 0, 0, 0 } ,
           { 19372.0 / 6561.0 , -25360.0 / 2187.0 , 64448.0 / 6561.0 , -212.0 / 729.0 ,0,0,0},{ 9017.0 / 3168.0 , -355.0 / 33.0 , 46732.0 / 5247.0 , 49.0 / 176.0 , -5103.0 / 18656.0 ,0,0},
           { 35.0 / 384.0, 0 , 500.0 / 1113.0, 125.0 / 192.0 , -2187.0 / 6784.0 , 11.0 / 84.0 ,0}};

        private double[] c = new double[] { 1.0 / 5.0, 3.0 / 10.0, 4.0 / 5.0, 8.0 / 9.0, 1, 1 };

        private double[] b = new double[] { 35.0 / 384.0, 0, 500.0 / 1113.0, 125.0 / 192.0, -2187.0 / 6784.0, 11.0 / 84.0, 0 };

        private double[] b1 = new double[] { 5179.0 / 57600.0, 0, 7571.0 / 16695.0, 393.0 / 640.0, -92097.0 / 339200.0, 187.0 / 2100.0, 1.0 / 40.0 };

        private double[,] koff = new double[,]{ { 1.0 , 1.0 , -1337.0/480.0 , 1039.0/360.0 , -1163.0 / 1152.0 } , { 0,0,0,0,0 } , 
            { 100.0 , 1054.0/9275.0 , -4682.0 / 27825.0 , 379.0 / 5565.0 , 3.0 } , { -5.0 , 27.0 / 40.0 , -9.0 / 5.0 , 83.0 / 96.0 , 2.0 } ,
            { 18225.0 , -3.0 / 250.0 , 22.0 / 375.0 , -37.0 / 600.0 , 848.0 } , { -22.0 , -3.0 / 10.0 , 29.0 / 30.0 , -17.0 / 24.0 , 7.0 } };

        public double max(double val1, double val2, double val3, double val4)
        {
            double result=0;
            if (val1 >= val2 && val1 >= val3)
                result=val1;
            if (val2 >= val1 && val2 >= val3)
                result=val2;
            if (val3 >= val1 && val3 >= val2)
                result=val3;
            if (val4 >= val1 && val4 >= val2 && val4 >= val3)
                result = val4;
            return result;
        }

        public double max(double val1, double val2)
        {
            if (val1 > val2)
                return val1;
            else
                return val2;
        }

        public double min(double val1,double val2)
        {
            if (val1 < val2)
                return val1;
            else
                return val2;
        }

        private double MachineZero()
        {
            double v = 1, u = 0;
            while (1 + v > 1)
            {
                u = v;
                v = v / 2;
            }
            return u;
        }

        public void Evaluate(MathModel model)
        {
            
            model.Init(ref y, ref y0,ref t0,ref tk);
            matrix = new Matrix(y.RowsCount+1, 1);

            int RowNumber = 0;
            double u = MachineZero();
            double[,] k = new double[precision, y.RowsCount];
            double[] temp = new double[y.RowsCount];
            double[] dy = new double[y.RowsCount];
            double[] y5 = new double[y.RowsCount];
            double[] gap = new double[y.RowsCount];
            double[] Yout = new double[y.RowsCount];
            double[] tmp = new double[y.RowsCount];
            double[] d_y0 = new double[y.RowsCount];

          
            
            for (int i = 0; i < y.RowsCount; i++)
                d_y0[i] = y0[i];

            for (int i = 0; i < y.RowsCount; i++)
                temp[i] = y0[i];
          
            Xout = t0;
            val0 = t0;

            while (t0 <= tk)
            {
                h = hnew;
                err = 0;
                x = t0;

                for (int i = 0; i < y.RowsCount; i++)
                    temp[i] = y0[i];

               
                
                model.Func(y0,y, x);

               
                for (int j = 0; j < precision; j++)
                {
                    for (int i = 0; i < y.RowsCount; i++)
                    {

                        k[j, i] = y[i];
                        if (j != 6)
                        {

                            gap[i] = 0;
                            for (int count = 0; count < precision; count++)
                                gap[i] += (k[count, i] * a[j, count]);

                            y0[i] = temp[i] + h * gap[i];
                        }


                    }

                    if (j != 6)
                    {
                        x = t0 + h * c[j];
                        model.Func(y0,y,x);
                    }
                }

                for (int i = 0; i < y.RowsCount; i++)
                {
                    dy[i] = 0;

                    for (int count = 0; count < precision; count++)
                        dy[i] = dy[i] + (k[count, i] * b[count]);

                    y0[i] = temp[i] + h * dy[i];
                    dy[i] = 0;

                    for (int count = 0; count < precision; count++)
                        dy[i] = dy[i] + (k[count, i] * b1[count]);

                    y5[i] = temp[i] + dy[i] * h;
                    err = err + Math.Pow((h * (y0[i] - y5[i])) / (max(0.000001, Math.Abs(y0[i]), Math.Abs(d_y0[i]), 2.0 * u / fault)), 2.0);
                }
                err = Math.Sqrt((err / y.RowsCount));
                hnew = h / max(0.1, min(5, Math.Pow((err / fault), 0.2) / 0.9));
             
                if (err > fault)
                {
                    for (int i = 0; i < y.RowsCount; i++)
                        y0[i] = temp[i];

                    continue;
                }

                while (Xout < t0 + h && Xout < tk)
                {
                    teta = (Xout - t0) / h;
                    for (int i = 0; i < y.RowsCount; i++)
                    {
                        tmp[i] = 0;

                        tmp[i] += teta * (1 + teta * (koff[0, 2] + teta * (koff[0, 3] + teta * koff[0, 4]))) * k[0, i];


                        for (int j = 2; j < precision - 1; j++)
                            tmp[i] += (koff[j, 0] * Math.Pow(teta, 2) * (koff[j, 1] + teta * (koff[j, 2] + teta * (koff[j, 3]))) / koff[j, 4]) * k[j, i];

                        Yout[i] = temp[i] + h * tmp[i];

                        //matr.SetElement(RowNumber, i, Yout[i]);
                        //if (i == y.RowsCount - 1)
                        //    matr.SetElement(RowNumber, i + 1, model.t0);

                        matrix[i, RowNumber] = Yout[i];

                    }

                    matrix[Yout.Length, RowNumber] = Xout;

                    RowNumber++;
                    matrix.SetSize(y.RowsCount+1,RowNumber+1);
                    

                    
                    Xout += inc;
                   

                    
                }



               
                t0 += h;

            }

            model.GetMatrixResult(matrix);

        }
    
    }
}
