using System;
using System.Collections.Generic;
using System.Text;

namespace Pen
{
    public abstract class People
    {
        private static int ID = 0;
        private int Id_peo;
        public bool activ;
        public string nam;
        public string Name { 
            get { return nam; } 
            set { nam=value; } 
        }
        public bool Active {
            get {return activ; }
            set {activ=value; } 
        }
        public int IDPeo { get { return Id_peo; } }
        public People()
        {
            activ = false;
            nam = "Unknown";
            ID++;
            Id_peo = ID;
        }
        public People(bool activ,string nam)
        {
            Active=activ   ;
            Name=nam ;
            ID++;
            Id_peo = ID;
        }
        public override string ToString()
        {
            return String.Format("{0} {1} ",this.Name,this.Active);
        }
    }
}
