using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Pen
{
    class Program
    {
        public static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            Random rnd;
            rnd = new Random();
            #region Ручки   
            Pen[] pens = new Pen[6];
            pens[0] = new PenSh("a", "Red", 510.0, true, "PPtwP");
            pens[1] = new PenCher("Ip", "Pink", 2010.0, true, "Treqae");
            pens[2] = new Pensil("Nl", "White", 112.0, true, true);
            pens[3] = new PenSh("BBB", "Red", 510.0, true, "PewPP");//Конструктор
            pens[4] = new PenCher("Io", "Pink", 200.0, true, "Tree");
            pens[5] = new Pensil("Nw", "White", 12.0, true, true);
            #endregion
            #region People
            People[] people = new People[7];
            people[0] = new Prodavec(true,"Jony",2) ;
            people[1] = new Prodavec(false,"Raychel",3);
            people[2] = new Prodavec(true, "Jade", 3);
            people[3] = new Prodavec(true, "Udny", 9);
            people[4] = new Prodavec(false, "FaDchel", 1);
            people[5] = new Prodavec(true, "Jadine", 5);


            #endregion
            StorePens sp = new StorePens();
            StorePens chois = new StorePens();
            foreach (Pen d in pens)

            {
                
                sp.AddPen(d);
            }
            
            
            sp.AddPen(new Pensil("Abba", "Black", 7.0, true, false));
            Console.WriteLine("View Store Pens");
            
            foreach (Pen d in sp)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine("__________________________________________________");

            #region
            Store mb = new Store("My Store"); // Создание банка
            mb.InitDB();
            Console.ReadKey();
            Prodavec acc1 = new Prodavec(true, "Jony", 2) ; // Создание продавца
            Prodavec acc2 = new Prodavec(false, "FaDchel", 1);// Создание продавца
            Prodavec acc3 = new Prodavec(true, "Udny", 9);

            List<Pen> penss = new List<Pen>();
            foreach (Pen d in pens)

            {

                penss.Add(d);
                mb.AddPen(d);
            }
            /* PenSh pen1= new PenSh("a", "Red", 510.0, true, "PPtwP");
             PenCher pen2= new PenCher("Ip", "Pink", 2010.0, true, "Treqae");
             Pensil pen3=new Pensil("Nl", "White", 112.0, true, true);*/
            // Создание модели продавец и товар 
            PeopleWithEvents cl11 = new PeopleWithEvents(acc1,100,penss);
            PeopleWithEvents cl12 = new PeopleWithEvents(acc2, 100, penss);
            PeopleWithEvents cl13 = new PeopleWithEvents(acc3, 100, penss);
           
            // Добавление продавцов и ручек
            mb.AddAccount(acc1);
            mb.AddAccount(acc2);
            mb.AddAccount(acc3);
            mb.AddClient(cl11);
            mb.AddClient(cl12);
            mb.AddClient(cl13);

            mb.ViewAccounts();
            Console.WriteLine("__________________________________________________");
            mb.ViewClients();

           
            // Запуск действий продавцов в отдельных потоках
            Thread Ths11 = new Thread(cl11.Go);
            Ths11.Start();
            Thread Ths12 = new Thread(cl12.Go);
            Ths12.Start();
            Thread Ths13 = new Thread(cl13.Go);
            Ths13.Start();
            
           
            // ожидание окончания потоков
            Ths11.Join();
            Ths12.Join();
            Ths13.Join();
           
            waitHandle.WaitOne();
            Console.WriteLine("FINISH");
            // Просмотры ручек,продавцов
            mb.ViewOperations();
            Console.WriteLine("__________________________________________________");
           
            //mb.ViewOperationsByAccount(acc2);
            DateTime et = DateTime.Now;
            DateTime bt = et.AddMinutes(-1);
            mb.ViewOperationsByTimeInterval(bt, et);
            Console.ReadKey();
            #endregion События
            #region XmlReader
            Console.WriteLine();
            Console.WriteLine("-----------------------------");
            Console.WriteLine();
            Console.WriteLine("XmlJournal");
            mb.WriteToXml_Journal("out2.xml");
            mb.ReadXml_Journal("out2.xml");
            #endregion
            //mb.TestDB();
            mb.ReportFromDB();
            mb.ReportFromDBSelect(acc1);
            mb.QuitDB();
        }
    }



}
 
           
