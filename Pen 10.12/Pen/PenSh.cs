using System;
using System.Collections.Generic;
using System.Text;

namespace Pen
{//Шариковая: Механизм открытия,колпачеок. Можно рассписать и показать 
     class PenSh:Pen,IRaspisat,ICheckCher
    {
        protected string p_Mech;//механизм открытия 
        protected bool p_kolpachok;// Имеется ли колпачок
        public string Mech
        {
            get { return p_Mech; }
            set { p_Mech = value; }
        }
        public bool kolpachok {
            get { return p_kolpachok; }
            set { p_kolpachok = value; }
        }
        public PenSh() :
           base()
        { p_kolpachok = false; p_Mech = "none"; }
        public PenSh(string p_Izgot, string p_color, double p_price,bool p_kolpachok,string p_Mech) 
            : base(p_Izgot, p_color, p_price) {
            
            kolpachok =p_kolpachok; 
            Mech = p_Mech;
        }
        
        public void Pisat() { Console.WriteLine("Написал"); }
        public void CheckCher() { Console.WriteLine("Чернила есть"); }
        public override string ToString()
        {
            return "PenSh "+base.ToString()+String.Format(" K={0} M={1} ",this.kolpachok,this.Mech);
        }
    }
}
