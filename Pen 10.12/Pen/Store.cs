using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Pen
{
    public class Store
    {

        public string NameStore { get; set; }  // Наименование банка
        List<People> prodav; // Список клиентов
        List<PeopleWithEvents> pens; // Список семейных счетов
        List<Operation> operations; // журнал операц
        List<Pen> penss;
        // BD
        public SqlConnection con;
        public SqlDataAdapter daEv, daTP, daPE, daOP, daPO;
        public SqlCommandBuilder cmdEv, cmdTP, cmdPV, cmdOp, cmdPO;
        public DataSet DS, DS2;
        public DataTable dtEv, dtTp, dtPE, dtOp, dtPO;
        int idop = 0;


        public Store(string name) // Конструктор
        {
            this.NameStore = name;
            prodav = new List<People>();
            pens = new List<PeopleWithEvents>();
            penss = new List<Pen>();

            operations = new List<Operation>();
        }
        // Добавление ручки
        #region Методы для событий
        public void AddPen(Pen fa)
        {
            penss.Add(fa);
        }
        public void AddAccount(People fa)
        {
            prodav.Add(fa);
        }
        // Добавление клиента
        public void AddClient(PeopleWithEvents fcl)
        {
            pens.Add(fcl);
            // Подключение обработчика события для клиента
            fcl.operationEv += OnOperation;
        }
        // обработчик событий операций клиента
        public void OnOperation(object sender, EventOperation e)
        {
            if (e.Op.Client.Active == true)
            {
                operations.Add(e.Op);
            }
            //Rabota DB
            #region ручки
            Pen curac = e.Op.pens;
            int idac = curac.IDPen;
            DataRow[] selac = dtPE.Select(string.Format("idPen={0}", idac));
            if (selac.Length == 0)
            {
                DataRow dr = dtPE.NewRow();
                dr["idPen"] = idac;
                dr["Izgot"] = curac.Izgot;
                dr["idTypePen"] = GetIdPen(curac);
                dtPE.Rows.Add(dr);
            }
            #endregion
            #region Люди
            People curpeo = e.Op.Client;
            int idpeo = curpeo.IDPeo;
            DataRow[] selacpeo = dtPO.Select(string.Format("idPeople={0}", idpeo));
            if (selacpeo.Length == 0)
            {
                DataRow dr = dtPO.NewRow();
                dr["idPeople"] = idpeo;
                dr["Name"] = curpeo.Name;
                dr["Active"] = curpeo.Active.ToString();
                dtPO.Rows.Add(dr);
            }
            #endregion

            // Operations
            DataRow drop = dtOp.NewRow();
            idop++;
            drop["idOp"] = idop;
            drop["idPen"] = idac;
            drop["idPeople"] = idpeo;
            drop["idEvent"] = e.Op.tipOp;
            drop["TimeEvents"] = (DateTime)e.Op.TimeOperation;
            drop["Message"] = e.Message;
            dtOp.Rows.Add(drop);

            Console.WriteLine("{0} =>{1}", e.Op, e.Message);
        }

        // Просмотр всех операций 
        public void ViewOperations()
        {
            Console.WriteLine("Begin Operations");
            foreach (Operation op in operations)
            {
                Console.WriteLine(op);
            }
            Console.WriteLine("End Operations");
            //        Program.waitHandle.Set();
        }
        // просмотр всех операций продавца
        public void ViewOperationsByClient(People client)//!
        {
            Console.WriteLine("Begin Operations Продавца " + client.ToString());
            var operClient = from op in operations where op.Client == client select op;
            foreach (Operation op in operClient)
            {
                Console.WriteLine(op);
            }
            Console.WriteLine("End Operations " + client.ToString());
            //        Program.waitHandle.Set();
        }
        // Просмотр всех операций по семейному счету
        public void ViewOperationsByAccount(People acc)
        {
            Console.WriteLine("Begin Operations Продавца " + acc.ToString());
            var operClient = from op in operations where op.Client == acc select op;
            foreach (Operation op in operClient)
            {
                Console.WriteLine(op);
            }
            Console.WriteLine("End Operations " + acc.ToString());
            //        Program.waitHandle.Set();
        }
        // Просмотр операций за период
        public void ViewOperationsByTimeInterval(DateTime bt, DateTime et)
        {
            Console.WriteLine("Begin Operations ");
            var operClient = from op in operations where op.TimeOperation >= bt && op.TimeOperation <= et select op;
            foreach (Operation op in operClient)
            {
                Console.WriteLine(op);
            }
            Console.WriteLine("End Operations ");
            //        Program.waitHandle.Set();
        }
        // Просмотр продавцов
        public void ViewAccounts()
        {
            Console.WriteLine("Продавцы");
            foreach (People ac in prodav)
            {
                Console.WriteLine(ac);
            }
            Console.WriteLine(".");
            //        Program.waitHandle.Set();
        }
        // Просмотр ручек
        public void ViewClients()
        {
            Console.WriteLine("Ручки");
            foreach (Pen cl in penss)
            {
                Console.WriteLine(cl);
            }
            Console.WriteLine(".");
            //        Program.waitHandle.Set();
        }
        #endregion
        public void WriteToXml_Journal(string namefile)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = (" ");
            XmlWriter writer = XmlWriter.Create(namefile, settings);
            int id = 1;
            writer.WriteStartElement("Operations");
            foreach (var op in operations)
            {
                writer.WriteStartElement("Operation");
                writer.WriteAttributeString("ID", id.ToString());
                writer.WriteElementString("Type", op.tipOp.ToString());
                writer.WriteStartElement("DateTime");
                writer.WriteElementString("Date", op.TimeOperation.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Prodavec");
                writer.WriteElementString("People", op.Client.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("Pen");
                if (op.pens is PenSh)
                {
                    writer.WriteElementString("TypePen", "PenSh");
                }
                else if (op.pens is Pensil)
                {
                    writer.WriteElementString("TypePen", "Pensil");
                }
                else
                {
                    writer.WriteElementString("TypePen", "PenCher");
                }
                writer.WriteElementString("Name", op.pens.Izgot);
                writer.WriteElementString("Color", op.pens.Color);
                writer.WriteElementString("Price", op.pens.Price.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                id++;
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }
        public void ReadXml_Journal(string namefile)
        {
            XmlReader xmlreader = XmlReader.Create(namefile);
            while (xmlreader.Read())
            {
                if (xmlreader.IsStartElement())
                {
                    Console.WriteLine("<{0}> ", xmlreader.Name);
                    if (xmlreader.HasAttributes)
                    {
                        Console.WriteLine("Attributes of <" + xmlreader.Name + ">");
                        while (xmlreader.MoveToNextAttribute())
                        {
                            Console.WriteLine(" {0}={1}", xmlreader.Name, xmlreader.Value);
                        }
                        xmlreader.MoveToElement();
                    }
                    if (xmlreader.HasValue) Console.WriteLine(xmlreader.Value);
                }
                if (xmlreader.HasValue) Console.WriteLine(xmlreader.Value);
                if (xmlreader.NodeType == XmlNodeType.EndElement) Console.WriteLine("</{0}>", xmlreader.Name);
            }
            xmlreader.Close();
        }
        //тест
        public void TestDB()
        {
            Console.WriteLine("Start DB");
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Год\source\repos\Pen\Pen\PrimStorePen.mdf;Integrated Security=True;Connect Timeout=30");
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Events", con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("first {0} second {1} ", reader[0], reader[1]);
                    }
                }

                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Finish DB");
        }
        public int GetIdPen(Pen pe)
        {
            if (pe is PenCher) return 1;
            if (pe is Pensil) return 3;
            if (pe is PenSh) return 2;
            return 0;
        }
        public int GetIdPeople(People peo)
        {

            return peo.IDPeo;
        }
        public void InitDB()
        {
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Год\source\repos\Pen\Pen\PrimStorePen.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();
            CleanDBOp();
            DS = new DataSet("VDB");
            daEv = new SqlDataAdapter("select * from Events", con);
            daTP = new SqlDataAdapter("select * from TypePen", con);
            daPO = new SqlDataAdapter("select * from People", con);
            cmdEv = new SqlCommandBuilder(daEv);
            cmdTP = new SqlCommandBuilder(daTP);
            cmdPO = new SqlCommandBuilder(daPO);
            daPE = new SqlDataAdapter("select * from Pen", con);
            daOP = new SqlDataAdapter("select * from Operation", con);
            cmdOp = new SqlCommandBuilder(daOP);
            cmdPV = new SqlCommandBuilder(daPE);

            daEv.Fill(DS, "Events"); dtEv = DS.Tables[0];
            daTP.Fill(DS, "TypePen"); dtTp = DS.Tables[1];
            daPE.Fill(DS, "Pen"); dtPE = DS.Tables[2];
            daOP.Fill(DS, "Operation"); dtOp = DS.Tables[3];
            daPO.Fill(DS, "People"); dtPO = DS.Tables[4];
            //idop = dtOp.Rows.Count;
            idop = 0;




        }
        //очистка данных операциий
        public void CleanDBOp()
        {
            try
            {

                string sql = "Delete from Operation where IdOp < @id ";

                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = sql;

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 1000;

                // Выполнить Command (Используется для delete, insert, update).
                int rowCount = cmd.ExecuteNonQuery();
                // DS.AcceptChanges();

                Console.WriteLine("Очистил = " + rowCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }


        }

        public void QuitDB()
        {

            daPE.Update(DS, "Pen");
            daOP.Update(DS, "Operation");
            daTP.Update(DS, "TypePen");
            daEv.Update(DS, "Events");
            daPO.Update(DS, "People");
            Console.WriteLine("____________VIEW_DB______________");
            ViewDS(DS);
            con.Close();
        }
        #region Показ
        public void ViewDS(DataSet DS)
        {
            Console.WriteLine("DataSet is named: {0}", DS.DataSetName);
            // Вывести каждую таблицу.
            foreach (DataTable dt in DS.Tables)
            {
                ViewDataTable(dt);
            }
        }
        public void ViewDataTable(DataTable dt)
        {
            Console.WriteLine("\n----------------------------------");
            Console.WriteLine("Table =>  {0}", dt.TableName);
            // Вывести имена столбцов.
            for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
            {
                Console.Write(dt.Columns[curCol].ColumnName + "\t");
            }
            Console.WriteLine();
            // Вывести DataTable.
            for (int curRow = 0; curRow < dt.Rows.Count; curRow++)
            {
                for (int curCol = 0; curCol < dt.Columns.Count; curCol++)
                {
                    Console.Write(dt.Rows[curRow][curCol].ToString() + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n----------------------------------");

        }
        #endregion
        public void ReportFromDB()
        {
            Console.WriteLine("Репорт");
            var query = from tj in dtOp.AsEnumerable()
                        join ta in dtPE.AsEnumerable() on tj.Field<int>("idPen") equals ta.Field<int>("idPen")
                        join te in dtEv.AsEnumerable() on tj.Field<int>("idEvent") equals te.Field<int>("idEvent")
                        select new
                        {
                            Izgot = ta.Field<string>("Izgot"),
                            // typeac = tt.Field<string>("TypeAirCraft"),
                            dataevent = tj.Field<DateTime>("TimeEvents"),
                            nevents = te.Field<string>("NameEvent"),
                            idta = ta.Field<int>("IdTypePen"),
                            mes = tj.Field<string>("Message")
                        };
            foreach (var op in query)
            {
                DataRow[] tac = dtTp.Select(string.Format("idPe={0}", op.idta));
                Console.WriteLine("{0}  {1}  {2}  {3}  {4}", op.Izgot, tac[0]["NameType"], op.nevents, op.dataevent, op.mes);
            }
        }
        public void ReportFromDBSelect(People pe)
        {
           
            var query = from tj in dtOp.AsEnumerable()
                        join ta in dtPE.AsEnumerable() on tj.Field<int>("idPen") equals ta.Field<int>("idPen")
                        join te in dtEv.AsEnumerable() on tj.Field<int>("idEvent") equals te.Field<int>("idEvent")
                        join tp in dtPO.AsEnumerable() on tj.Field<int>("IdPeople") equals tp.Field<int>("IdPeople")
                        select new
                        {
                            Izgot = ta.Field<string>("Izgot"),
                            name=tp.Field<string>("Name"),
                            dataevent = tj.Field<DateTime>("TimeEvents"),
                            nevents = te.Field<string>("NameEvent"),
                            idta = ta.Field<int>("IdTypePen"),
                            mes = tj.Field<string>("Message"),
                            idpe=tj.Field<int>("IdPeople")
                        };
            Console.WriteLine("Операции продавца {0}",pe.Name);
            foreach (var op in query)
            {
                if (pe.IDPeo == op.idpe)
                {
                    DataRow[] tac = dtTp.Select(string.Format("idPe={0}", op.idta));
                    
                    Console.WriteLine("{0}  {1}  \n {2}  {3}  {4} {5} {6} ", op.name, op.idpe, op.Izgot, tac[0]["NameType"], op.nevents, op.dataevent, op.mes);
                }
            }
        }

    }
    }



