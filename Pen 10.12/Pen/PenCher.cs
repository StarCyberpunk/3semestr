using System;
using System.Collections.Generic;
using System.Text;

namespace Pen
{//Ручка чернильная не перо, пополнить черницы из чернильницы,показать, материал ,разбирается или нет
    class PenCher : Pen, IPopolnitCher, ICheckCher
    {
        private bool p_razbor;// разбирается ли?
        private string p_material;// Какой материал у ручку?
        public bool Razbor
        {
            get { return p_razbor; }
            set { p_razbor = value; }
        }
        public string Material
        {
            get { return p_material; }
            set { p_material = value; }
        }

        public PenCher() :
            base()
        { p_razbor = false; p_material = "none"; }
        public PenCher(string p_Izgot, string p_color, double p_price, bool p_razbor, string p_material)
            : base(p_Izgot, p_color, p_price)
        {
            Razbor = p_razbor;
            Material = p_material;
        }

        public void CheckCher() { Console.WriteLine("Половина"); }
        public void Popol() { Console.WriteLine("Пополнить"); }
        public override string ToString()
        {
            return "Pencher " + base.ToString() + String.Format("K={0} M={1} ", this.Razbor, this.Material);

        }
    }
}
