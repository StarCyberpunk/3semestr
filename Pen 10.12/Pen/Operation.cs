using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pen
{
  
        public enum TipOperation
        {
            Покупка=1,
            Пополнение=2
        }
        // Делегат события операции ( sender – объект связанный с собынием,
        // e – аргумент события операции 
        public delegate void DelOperation(object sender, EventOperation e);
        [Serializable]
        // класс операции
        public class Operation
        {
            public TipOperation tipOp { get; set; } // Тип операции
            public DateTime TimeOperation { get; set; } // время операции
            public People Client { get; set; } // Клиент
        public Pen pens { get; set; }
            
            public Operation(TipOperation to,  People clo,Pen clon)
            {
                this.tipOp = to; this.Client = clo;this.pens = clon;
                this.TimeOperation = DateTime.Now;
                
                
            }
            public Operation() // Конструктор
            {
                this.tipOp = TipOperation.Пополнение;  this.Client = null;
                this.TimeOperation = DateTime.Now; this.pens = null;
            }
            public override string ToString()
            {
                return "Operation (" + tipOp.ToString() + ", " + TimeOperation.ToString() + ", " + Client+"\n" +pens +")";
            }
        }
    
    //  класс аргумента события операции 
    // EventArgs – стандартный класс аргументов событий
    public class EventOperation : EventArgs
    {
        public Operation Op { get; set; } // Операция
        public string Message { get; set; } // Сообщение
        public EventOperation(Operation oper, string mes)
        {
            this.Op = oper; Message = mes;
        }
    }
}
