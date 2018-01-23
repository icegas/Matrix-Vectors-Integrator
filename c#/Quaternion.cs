using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sateliGNSS
{
    public class Quaternion
    {

        protected Vector E;
    

        public double PHI { get; set; }
        public Quaternion(double l0, double l1, double l2, double l3)
        {

            E = new Vector(3);   
            this.Scalar = l0;
            E[0] = l1;
            E[1] = l2;
            E[2] = l3;
            E = E.norm();
        }

        public Quaternion() : this(0, 0, 0, 0) { }

        public Quaternion(double phi,Vector e)
        {
            E = new Vector(e.Count);
            e = e.norm();
            PHI = phi;

            E = Math.Sin(phi/2) * e.norm();
           
            this.Scalar = Math.Cos(phi/2);

        }

        public Vector vector
        {
            get { return E; }
        }

        public void setVector(Vector v)
        {
            for (int i = 0; i < v.Count; i++)
                this.E[i] = v[i];
        }

        public double Scalar { get; set; }
        
        public static Quaternion operator +(Quaternion Q1, Quaternion Q2)
        {
            Quaternion result = new Quaternion();

            result.Scalar = Q1.Scalar + Q2.Scalar;
            result.setVector(Q1.vector + Q2.vector);
            return result;
        }  

        public static Quaternion operator *(Quaternion Q1, Quaternion Q2)
        {
            Quaternion result =new Quaternion(0,new Vector(3));
            Vector a = Q1.vector;
            Vector b = Q2.vector;
            Vector v = new Vector(3);
            //result.Scalar = Q1.Scalar * Q2.Scalar - Q2.vector.sMult(Q1.vector);
            //result.setVector(Q1.Scalar * Q2.vector + Q2.Scalar * Q1.vector + Q1.vector.CrossProduct(Q2.vector));
            result.Scalar = Q1.Scalar * Q2.Scalar - a[0] * b[0] - a[1] * b[1] - a[2] * b[2];
            v[0] = Q1.Scalar * b[0] + a[0] * Q2.Scalar + a[1] * b[2] - a[2] * b[1];
            v[1] = Q1.Scalar * b[1] - a[0] * b[2] + a[1] * Q2.Scalar + a[2] * b[0];
            v[2] = Q1.Scalar * b[2] + a[0] * b[1] - a[1] * b[0] + a[2] * Q2.Scalar;
            result.setVector(v);
            return result;

        }

     
        public static Quaternion operator *(Quaternion Q,Vector b){
            Quaternion val=new Quaternion();
            /*  val.Scalar=-(Q.vector.sMult(v));
              val.setVector(Q.Scalar * v + Q.vector.CrossProduct(v));//v.CrossProduct(Q.vector);*/
            Vector v = new Vector(3);
            Vector a = Q.vector;
            val.Scalar = -a[0] * b[0] - a[1] * b[1] - a[2] * b[2];
            v[0] = Q.Scalar * b[0] + a[1] * b[2] - a[2] * b[1];
            v[1] = Q.Scalar * b[1] - a[0] * b[2] + a[2] * b[0];
            v[2] = Q.Scalar * b[2] + a[0] * b[1] - a[1] * b[0];
            val.setVector(v);
            return val;
        }

        public double norm()
        {
            double val = 0;
            for (int i = 0; i < E.Count; i++)
                val += Math.Pow(this.vector[i], 2);
            val += Math.Pow(this.Scalar, 2);
            return val;
        }

        public double length()
        {
            return Math.Sqrt(this.norm());
        }


        public static Quaternion operator /(Quaternion Q, double num)
        {
            Quaternion val = new Quaternion(Q.Scalar, Q.vector);
            val.Scalar = Q.Scalar / num;
            val.setVector(Q.vector / num );
            return val;
        }

        public Quaternion reverse()
        {
            return this.conj() / this.length();
        }

        public Quaternion conj()
        {
            Quaternion val=new Quaternion();
            val.Scalar = this.Scalar;
            val.setVector(-1 * this.vector);
            return val;
        }    

    }
}
