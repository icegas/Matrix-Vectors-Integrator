using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artificial_Earth_Models_Satelite
{
    class MatrixTransition
    {
       

        public Vector FromOrbitToGeo(Vector o_v,double Omega,double Lamda,double i,double Anomaly){
            Matrix A = new Matrix(3, 3);
            double u= Omega + Anomaly;

            A[0,0] = Math.Cos(u) * Math.Cos(Lamda) - Math.Sin(u) * Math.Sin(Lamda) * Math.Cos(i);
            A[1,0] = -Math.Sin(u) * Math.Cos(Lamda) - Math.Cos(u) * Math.Sin(Lamda) * Math.Cos(i);
            A[2,0] = Math.Sin(i) * Math.Sin(Lamda);

            A[0,1] = Math.Cos(u) * Math.Sin(Lamda) + Math.Sin(u) * Math.Cos(Lamda) * Math.Cos(i);
            A[1,1] = -Math.Sin(u) * Math.Sin(Lamda) + Math.Cos(u) * Math.Cos(Lamda) * Math.Cos(i);
            A[2,1] = -Math.Sin(i) * Math.Cos(Lamda);

            A[0,2] = Math.Sin(u) * Math.Sin(i);
            A[1,2] = Math.Cos(u) * Math.Sin(i);
            A[2,2] = Math.Cos(i);

            return A * o_v;
        }
        
        
    }
}
