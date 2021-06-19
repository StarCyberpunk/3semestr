using System;
using System.Collections.Generic;
using System.Text;

namespace Pen
{//Перо Можно полонить чрнила и показать,Есть ли перья ? нужна ли чернильница? 
    class Pensil:Pen,IPopolnitCher,IClean
    {
        private bool p_HavePero;// ИМеется ли перья
        private bool p_NeedCherbox;// нужна ли чернильница?
        public bool HavePero {
            get { return p_HavePero; }
            set { p_HavePero = value; }
        }
        public bool NeedCherbox {
            get { return p_NeedCherbox; }
            set { p_NeedCherbox = value; }
        }
        public Pensil():
            base()
        { p_HavePero = false; p_NeedCherbox = false; }
        public Pensil(string p_Izgot, string p_color, double p_price, bool p_HavePero,bool p_NeedCherbox) 
            : base( p_Izgot, p_color,  p_price) {
            HavePero = p_HavePero;
            NeedCherbox = p_NeedCherbox;
        }
        
        public void Clean() { Console.WriteLine("Почистил"); }
        public void Popol() { Console.WriteLine("Макнул"); }
        public override string ToString()
        {
            return "Pensil\t"+base.ToString() + String.Format(" K={0} M={1} ", this.HavePero, this.NeedCherbox);
        }
    }
}
