using System;
using System.Collections.Generic;
using System.Text;

namespace Pen
{
    class Prodavec:People
    {
        public int Rank_P;
        public int Rank { get { return Rank_P; } set{Rank_P=value; } }
        public Prodavec():base()
        { Rank_P = 1;}
        public Prodavec(bool activ, string nam,int Rank_P) : base(activ,nam)
        {
            Rank = Rank_P;
        }
        public override string ToString()
        {
            return " "+base.ToString()+String.Format("Rank {0} ",this.Rank);
        }
    }
}
