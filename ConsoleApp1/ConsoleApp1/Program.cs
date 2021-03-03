using System;
using System.Linq;
using System.IO;
using System.Data;


namespace ConsoleApp1
{
    class Program
    {
        static DataSet1 ds1 = new DataSet1();
        static void Main(string[] args)
        {
            readFile("test.txt");
            

        }

        public static void readFile(string name)
        {
            StreamReader rFile = new StreamReader(@"C:\Users\gopplus\source\repos\ConsoleApp1\ConsoleApp1\test.txt");
            string line;
            string[] first;

            while ((line = rFile.ReadLine()) != null)
            {
                first=line.Split();
                switch (first[0].Trim())
                {
                    default:
                        break;
                    case "Driver":
                        register(first);
                        break;
                    case "Trip":
                        setService(first);
                        break;  

                }
            }
            printReport();
            rFile.Close();
            
        }

        static void register(string[] line)
        {
            
            ds1.Conductores.Rows.Add(line[1]);

        }

        static void printReport()
        {
            double miles;
            double mph;
            DataSet1.ViajesDataTable tab = new DataSet1.ViajesDataTable();
            foreach (var item in ds1.Viajes.GroupBy(g => g.ConductorID).OrderBy(j => j.Sum(k => k.DistanciaR)).Distinct())
            {
                miles = ds1.Viajes.Where(v => v.ConductorID == item.FirstOrDefault().ConductorID).Sum(g => g.DistanciaR);
                mph = ds1.Viajes.Where(v => v.ConductorID == item.FirstOrDefault().ConductorID).Sum(g => g.DistanciaR / (Convert.ToInt32(g.Fin - g.Inicio) / 60) / ds1.Viajes.Where(h => h.ConductorID == item.ID).Count());
                Console.WriteLine(ds1.Conductores.Where(s=>s.ID == item.FirstOrDefault().ConductorID).FirstOrDefault().Nombre + ": " + miles + " miles @" + mph + " mph");
            }
        }

        static void setService(string[] line)
        {
            int id = getDriver(line[1]);
            double speed = getSpeed(Convert.ToDateTime(line[2]), Convert.ToDateTime(line[3]), Convert.ToDouble(line[4]));
            if (speed > 5 && speed < 100)
            {
                DataRow dr = ds1.Viajes.NewRow();
                dr.SetField(1, id);
                dr.SetField(2, line[2]);
                dr.SetField(3, line[3]);
                dr.SetField(4, line[4]);
                ds1.Viajes.Rows.Add(dr);
            }
            
          
        }

        static private int getDriver(string name) => ds1.Conductores.Where(j => j.Nombre == name).FirstOrDefault().ID;

        static private double getSpeed(DateTime inicio, DateTime fin, double distancia) => distancia / (Convert.ToInt32(fin - inicio) / 60);


    }
}
