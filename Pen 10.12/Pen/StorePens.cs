using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Pen
{
    class StorePens: Storage<Pen>
    {
        public List<Operation> operations;
      
        public void AddPen(Pen item)
            {
                _objs.Add(item);
            }
            public void RemovePen(Pen item)
        {
            _objs.Remove(item);

        }
        /*public Pen[] GetRandomPok(StorePens pens1)
        {
            Random rnd;
            rnd = new Random();
            int j = rnd.Next(1, pens1.GetCount());
            Pen[] pens2 = new Pen[j];
            for (int z = 1; z < j; z++)
            {
                pens2[z] = (pens1[rnd.Next(0, pens1.GetCount())]);

            }
            return pens2;


        }*/








    }
}
