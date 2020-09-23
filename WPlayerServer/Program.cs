using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WPlayerServer
{
    class Program
    {
        const string configFile = "config.txt";

        static IPAddress addres;
        static string    directory;
        static int       port;

        static void Main(string[] args)
        {

            // проверка наличия конфига
            if (!Directory.Exists(configFile))
            {
                CreateConfig();
                return;
            }
            // Чтение конфига
            StreamReader reader = new StreamReader(configFile);

            try
            {
                addres    = IPAddress.Parse(reader.ReadLine());
                port      = int.Parse(reader.ReadLine());
                directory = reader.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Неверная папка");
                return;
            }

            // Вытаскивание папки с музыкой
        }

        static void CreateConfig()
        {
            Console.WriteLine("Ненайден конфиг");

            StreamWriter writer = new StreamWriter(configFile);

            writer.WriteLine("127.0.0.1");
            writer.WriteLine("8888");
            writer.WriteLine("C:\\Users\\userName\\Music");

            writer.Close();
        }
    }
}
