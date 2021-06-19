using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace Pen
{
    public class PeopleWithEvents
    {
        public People Peop; // Люди
        public Pen pen;//Ручки
        int MaxInterval; // время операции
        int KolJob; // количество операций
        static object Locker = new object();  // блокировка на момент операции
        static Random rnd = new Random();
        public event DelOperation operationEv; // событие операции
        public List<Pen> penss;
        public PeopleWithEvents() // конструктор
        {
            this.Peop = null;
            this.MaxInterval = 1000; KolJob = 1;
        }
        // конструктор
        public PeopleWithEvents(People fa, int sleep, List<Pen> pen)
        {
            this.Peop = fa; this.KolJob = 1; this.MaxInterval = sleep; this.penss = pen;
        }
        // операция 
        public void Pokypka(People cp, Pen pen)
        {
            lock (Locker)
            {
                

                if (Peop.Active == true)
                {
                    if (operationEv != null) operationEv(this, new EventOperation(new Operation(TipOperation.Покупка, cp, pen), " Продал"));
                }
                
            }
        }

        // операция 
        public void Popolnenie(People cp, Pen pen)
        {
            lock (Locker)
            {
                if (Peop.Active == true)
                {

                    if (operationEv != null) operationEv(this, new EventOperation(new Operation(TipOperation.Пополнение, cp, pen), "Пополнил"));

                }
                
            }
        }
        // Моделирование действия работы 
        public void Go()
        {
            for (int k = 0; k < this.KolJob; k++)
            {
                int j = rnd.Next(1, penss.Count-1);//колличество
                
                    for (int i = 0; i < j; i++)
                    {
                        int m = rnd.Next(0, penss.Count -1);//рандомная ручка
                        Thread.Sleep(rnd.Next(this.MaxInterval));
                        Pokypka(Peop, penss[m]);
                        Popolnenie(Peop, penss[m]);
                    }

               
                Program.waitHandle.Set();
            }
        }
        
    }
}

