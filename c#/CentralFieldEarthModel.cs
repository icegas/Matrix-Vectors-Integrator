using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artificial_Earth_Models_Satelite
{
    class CentralFieldEarthModel : MathModel
    {
        private double t0;
        private double tk;
        private Vector y = new Vector(6);
        private Vector y0 = new Vector(6);
        public CentralFieldEarthModel(Vector o_V_Y, Vector o_V_Y0, double d_t0, double d_tk)
        {
            y.equal(o_V_Y);
            y0.equal(o_V_Y0);
            t0 = d_t0;
            tk = d_tk;
        }

       

        private const double mu = 398600.436e9; //3.98600544 * 100000000000000;//

        public override void Init(ref Vector I_y, ref Vector I_y0, ref double I_t0, ref double I_tk)
        {
            I_t0 = t0;
            I_tk = tk;
            I_y = new Vector(y.RowsCount);
            I_y0 = new Vector(y0.RowsCount);

            for (int i = 0; i < y.RowsCount; i++)
            {
                I_y[i] = y[i];
                I_y0[i] = y0[i];
            }

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
        }
    }
}
