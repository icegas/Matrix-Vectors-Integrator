using System;
using System.Collections.Generic;
using System.Text;


namespace sateliGNSS
{
    public class Vector
    {
        protected double[] Data;
        protected int Rows;

        public Vector(int rows)
        {
            Data = new double[rows];
            Rows = rows;
        }

        public Vector(double x, double y, double z)
        {
            Data = new double[3];
            Data[0] = x;
            Data[1] = y;
            Data[2] = z;
            Rows = 3;
        }
        public Vector CrossProduct(Vector vetc)
        {
            Vector val = new Vector(this.Count);
            val[0] = this[1] * vetc[2] - vetc[1] * this[2];
            val[1] = -this[0] * vetc[2] + vetc[0] * this[2];
            val[2] = this[0] * vetc[1] - vetc[0] * this[1];

            return val;
        }

        public int Count
        {
            get
            {
                return Rows;
            }
        }

        public void SetSize(int rows)
        {
            double[] sData = new double[rows];
            
            if(rows > this.Count)
            {
                for (int i = 0; i < this.Count; i++)
                    sData[i] = Data[i];
                Data = new double[rows];
                for (int i = 0; i < this.Count; i++)
                    Data[i] = sData[i];
            }  
            else
            {
                Data = new double[rows];
            }
            Rows = rows;
            
        }

        public double this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }      

        //vector transpose * vector horizontal on vertical
        public double sMult(Vector v1)
        {
            double val = 0;

            for (int i = 0; i < v1.Count; i++)
                val += v1[i] * this[i];
            return val;
        }


        //vector * vector.transpose !!!! v*v^T
        public static Matrix operator *(Vector v1, Vector v2)
        {
            Matrix result = new Matrix(v1.Count, v2.Count);
            for (int i = 0; i < result.ColumnsCount; i++)
                for (int j = 0; j < result.RowsCount; j++)
                {
                    result[j, i] += v1[j] * v2[i];
                }
            return result;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector v = new Vector(v1.Count);
            try
            {
                for (int j = 0; j < v.Count; j++)
                {
                    v[j] = v1[j] + v2[j];
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("rows mustbe equal");
            }
            return v;
        }

        public static Vector operator *(double value, Vector v)
        {
            Vector vret = new Vector(v.Count);
            for (int i = 0; i < v.Count; i++)
                vret[i] = v[i] * value;
            return vret;
        }

        public static Vector operator -(double value, Vector v)
        {
            Vector vret = new Vector(v.Count);
            for (int i = 0; i < v.Count; i++)
                vret[i] = value - v[i];
            return vret;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector v = new Vector(v1.Count);
            try
            {
                for (int j = 0; j < v.Count; j++)
                {
                    v[j] = v1[j] - v2[j];
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("rows mustbe equal");
            }
            return v;
        }

        /*public Vector RotateByRodrig(Vector e, double phi)
        {

            double teta = Math.Tan(phi / 2) * 2 * e;
            return this + (teta / (1 + Math.Pow(Math.Tan(phi / 2), 2))).CrossProduct(this + (teta / 2).CrossProduct(this));

        }*/


        public Vector RotateByQuaternion(Quaternion Q)
        {
            return ((Q * this) * Q.reverse()).vector;
        }

        public Vector norm()
        {
            Vector ret = new Vector(this.Count);
            for (int i = 0; i < Count; i++)
                ret[i] = this[i];
            double length = Math.Sqrt(this[0] * this[0] + this[1] * this[1] + this[2] * this[2]);
            return (length != 0) ? ret / length : ret;

        }

        public static Vector operator /(Vector v1, double num)
        {
            Vector retv = new Vector(v1.Count);
            for (int i = 0; i < v1.Count; i++)
                retv[i] = v1[i] / num;
            return retv;
        }


    }
}
