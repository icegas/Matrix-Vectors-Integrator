using System;
using System.Collections.Generic;
using System.Text;


namespace statProjsWork
{
    public class Matrix
    {
        public Matrix(int rows, int column)
        {
            Data = new double[rows, column];
            Colums = column;
            Rows = rows;
        }

        public int ColumnsCount
        {
            get
            {
                return Colums;
            }
            //set
            //{
            //}
        }

        public int RowsCount
        {
            get
            {
                return Rows;
            }
            //set
            //{
            //}
        }
        public static Matrix operator +(Matrix matrA,Matrix matrB)
        {
            Matrix m = new Matrix(matrA.RowsCount, matrA.ColumnsCount);

            try
            {
                for (int i = 0; i < matrA.ColumnsCount; i++)
                {
                    for (int j = 0; j < matrA.RowsCount; j++)
                    {
                        m[j, i] = matrA[j, i] + matrB[j, i];

                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("columns and rows must be equal");
            }

            return m;
        }
       
        public Matrix Flip()
        {
            Matrix val = new Matrix(Colums, Rows);
            for (int i = 0; i < Colums; i++)
                for (int j = 0; j < Rows; j++)
                    val[i, j] = Data[j, i];
            return val;
        }

        public Matrix Inverse()
        {
            Matrix m = new Matrix(Rows, Colums);
            Matrix E = new Matrix(Rows, Colums);
            Matrix Zero = new Matrix(Rows, Colums);
            int k;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    m[j, i] = Data[j, i];

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    if(i==j)
                        E[i,j] = 1;

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Colums; j++)
                    Zero[j, i] = 0;


                    //начало
                    for (int j = 0; j < Colums; j++)
                    {
                        k = Rows - j;
                        //проверка значения на 0 если 0 то меняем местами строки
                        while (m[j, j] == 0)
                        {
                            if (k != 1)
                            {
                                m.SwapRow(ref m, j, Rows - k + 1);
                                E.SwapRow(ref E, j, Rows - k + 1);
                                k--;
                            }
                            else
                            {
                                Console.WriteLine("Matrix nonsingular.Please Change Matrix Value and continue.");
                                return Zero;
                            }


                        }

                        E.MultRow(ref E, j, 1 / m[j, j]);
                        m.MultRow(ref m, j, 1 / m[j, j]);

                        for (int i = j + 1; i < Rows; i++)
                        {
                            E.MultDecRow(ref E, j, i, m[i, j]);
                            m.MultDecRow(ref m, j, i, m[i, j]);

                        }

                    }

            //обратный ход
            for (int i = Rows - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    E.MultDecRow(ref E, i, j, m[j, i]);
                    m.MultDecRow(ref m, i, j, m[j, i]);
                }
            }

           
            return E;
                
        }

        public static Matrix operator *(Matrix matrA, Matrix matrB)
        {
            Matrix m = new Matrix(matrA.RowsCount, matrB.ColumnsCount);
            double temp = 0;
            try
            {
                for (int i = 0; i < matrA.RowsCount; i++)
                    for (int j = 0; j < matrB.ColumnsCount; j++)
                    {
                        for (int k = 0; k < matrB.RowsCount; k++)
                        {
                            temp += (matrA[i, k] * matrB[k, j]);

                        }
                        m[i, j] = temp;
                        temp = 0;
                    }

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("columns and rows must be equal");
            }


            return m;
        }

        public double this[int rows, int column]
        {
            get { return Data[rows, column]; }
            set { Data[rows, column] = value; }
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            Vector ret = new Vector(m.RowsCount);
            double temp = 0;
            try
            {
                for (int i = 0; i < m.RowsCount; i++)
                {
                    for (int j = 0; j < v.Count; j++)
                        temp += m[i, j] * v[j];
                    ret[i] = temp;
                    temp = 0;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("rows mustbe equal");
            }
            return ret;
        }
        public void SetSize(int rows, int column)
        {
            
                double[,] data = new double[rows, column];
          
                //for (int i = 0; i < Colums; i++)
                //    for (int j = 0; j < Rows; j++)
                //        data[i, j] = Data[i, j];
                Data = data;
                Colums = column;
                Rows = rows;
            
        }

        public Matrix MultRow(ref Matrix m, int row, double val)
        {

            for (int i = 0; i < m.ColumnsCount; i++)
                m[row, i] = m[row, i]*val;

            return m;
        }

        public Matrix MultDecRow(ref Matrix m, int drow, int row, double val)
        {

            for (int i = 0; i < m.ColumnsCount; i++)
                m[row, i] = m[row, i] - (val*m[drow, i]);
            return m;
        }

        public Matrix AddRow(ref Matrix m, int Arow, int row)
        {
            for (int i = 0; i < m.ColumnsCount; i++)
                m[row, i] = m[row, i] + m[Arow, i];
            return m;
        }



        public Matrix SwapRow(ref Matrix m, int Row, int SwapRow)
        {

            double temp;

            for (int i = 0; i < m.ColumnsCount; i++)
            {
                temp = m[Row, i];
                m[Row, i] = m[SwapRow, i];
                m[SwapRow, i] = temp;
            }
            return m;
        }
        
            


        protected double[,] Data;
        protected int Colums;
        protected int Rows;
    }
}
