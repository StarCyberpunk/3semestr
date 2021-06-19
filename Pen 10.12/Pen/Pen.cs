using System;
using System.Collections.Generic;
using System.Text;
namespace Pen
{
  public abstract class Pen:IPokaz
    {
       
        public string p_Izgot;// Изготовитель
        public string p_color;// Цвет
        public double p_price;// Цена
        private static int ID = 0;
        private int Id_pen;


        public string Izgot {
            get { return p_Izgot; }
            set { p_Izgot = value; }
        }
        public string Color
        {
            get { return p_color; }
            set { p_color = value; }
        }
        public double Price
        {
            get { return p_price ; }
            set { if (value > 0) p_price = value;
                else p_price = 0;
            }
        }
        public int IDPen { get { return Id_pen; } }
        public Pen() {
            p_Izgot = "No name";
            p_color = "No";
            p_price =0;
            ID++;
            Id_pen = ID;
        }
         public Pen( string p_Izgot,string p_color, double p_price)
        {
            Izgot = p_Izgot;
            Color = p_color;
            Price = p_price;
            ID++;
            Id_pen = ID;
        }
        public static double SaleForY(double p_price)// Цена для сотрудников
        {
            return p_price * 0.8;
        }
        public static string MorW(string p_color) {// Цвет ручки для мальчиков и девочек
            if (p_color== null) return "-1";
            if (p_color == "Blue") return "Men";
            else if (p_color == "Pink") return "Girl";
            else return "unisex";
        }
        public void Pokaz() { Console.WriteLine("Показал"); }// Абстр интерфейс показать ручку
        public override string ToString()
        {
            return String.Format("ID={0} IZgot={1} Price={2} Color={3} ",this.IDPen,this.Izgot,this.Price,this.Color);
        }
    }

    public interface ICheckCher { void CheckCher(); }// Проверить чернила
    public interface IRaspisat { void Pisat(); }// Расписать чернила
    public interface IPopolnitCher { void Popol(); }// Пополнить чернила
    public interface IPokaz { void Pokaz(); }// Показать ручку
    public interface IClean { void Clean(); }// Почистить ручку от чернил
}
