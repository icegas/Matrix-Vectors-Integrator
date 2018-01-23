using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Data;



namespace TrueMatrix_Integrator
{
    
    class Dorman_Prince 
    {
        private List<Matrix> matrix;
        private Matrix y;
        private Matrix y0;
        private double fault;
        private double inc;
        private double t0;
        private double tk;

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
            const int precision = 7;
           
            double hnew=0.001;
            double h;
            double teta;
            double err;
            double x;
            double Xout;
           
            model.Init(ref y0,ref y,ref t0,ref tk,ref inc,ref fault);
           
           
            double u = MachineZero();
            Matrix interim;
            List<Matrix>k = new List<Matrix>();
            Matrix temp = new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix dy = new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix y5 = new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix gap = new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix Yout = new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix tmp =new Matrix(y.ColumnsCount,y.RowsCount);
            Matrix d_y0 =new Matrix(y.ColumnsCount,y.RowsCount);
            matrix = new List<Matrix>();

            Matrix[] k0=new Matrix[precision];
            for (int i = 0; i < precision; i++)
            {
                k0[i] = new Matrix(y.ColumnsCount, y.RowsCount);
                k.Add(k0[i]);
            }

            d_y0.equal(y0); 

            temp.equal(y0);
          
            Xout = t0;
           
            while (t0 <= tk)
            {
                h = hnew;
                err = 0;
                x = t0;

             
                temp.equal(y0);

                model.Func(y0,ref y, x);

               
                for (int j = 0; j < precision; j++)
                {
                    for (int rab = 0; rab < y.ColumnsCount; rab++)
                    {
                        for (int i = 0; i < y.RowsCount; i++)
                        {

                            k[j][rab, i] = y[rab, i];
                            if (j != 6)
                            {

                                gap[rab, i] = 0;
                                for (int count = 0; count < precision; count++)
                                    gap[rab,i] += (k[count][rab, i] * a[j, count]);

                                y0[rab,i] = temp[rab,i] + h * gap[rab,i];
                            }


                        }
                    }

                    if (j != 6)
                    {
                        x = t0 + h * c[j];
                        model.Func(y0,ref y,x);
                    }
                }

                for (int rab = 0; rab < y.ColumnsCount;rab++ )
                    for (int i = 0; i < y.RowsCount; i++)
                    {
                        dy[rab, i] = 0;

                        for (int count = 0; count < precision; count++)
                            dy[rab, i] = dy[rab, i] + (k[count][rab, i] * b[count]);

                        y0[rab,i] = temp[rab, i] + h * dy[rab, i];
                        dy[rab,i] = 0;


                        for (int count = 0; count < precision; count++)
                            dy[rab, i] = dy[rab, i] + (k[count][rab, i] * b1[count]);

                        y5[rab, i] = temp[rab, i] + dy[rab,i] * h;
                        err = err + Math.Pow((h * (y0[rab, i] - y5[rab, i])) / (max(0.000001, Math.Abs(y0[rab, i]), Math.Abs(d_y0[rab,i]), 2.0 * u / fault)), 2.0);
                    }
                err = Math.Sqrt((err / y.RowsCount));
                hnew = h / max(0.1, min(5, Math.Pow((err / fault), 0.2) / 0.9));

                if (err > fault)
                {
                    y0.equal(temp);
                    continue;
                }

                while (Xout < t0 + h && Xout < tk)
                {
                    teta = (Xout - t0) / h;
                    for (int rab = 0; rab < y.ColumnsCount;rab++ )
                        for (int i = 0; i < y.RowsCount; i++)
                        {
                            tmp[rab,i] = 0;

                            tmp[rab,i] += teta * (1 + teta * (koff[0, 2] + teta * (koff[0, 3] + teta * koff[0, 4]))) * k[0][rab, i];


                            for (int j = 2; j < precision - 1; j++)
                                tmp[rab,i] += (koff[j, 0] * Math.Pow(teta, 2) * (koff[j, 1] + teta * (koff[j, 2] + teta * (koff[j, 3]))) / koff[j, 4]) * k[j][rab, i];

                            Yout[rab, i] = temp[rab, i] + h * tmp[rab,i];


                        }
                        interim = new Matrix(y.ColumnsCount + 1, y.RowsCount);

                         for (int i = 0; i < y.RowsCount; i++)
                             for (int j = 0; j < y.ColumnsCount; j++)
                                 interim[j, i] = Yout[j, i];

                         for (int i = 0; i < y.RowsCount; i++)
                             interim[y.ColumnsCount, i] = Xout;

                         matrix.Add(interim);
                           
                    Xout += inc;
                   

                    
                }

                t0 += h;

            }
            
            
            model.GetMatrixResult(matrix);

        }
    
    }
}
