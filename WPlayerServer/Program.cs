using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace WPlayerServer
{
    class Program
    {
        const string configFile = "config.txt";

        public static IPAddress address;
        public static string    directory;
        public static int       port;

        static string[]  music;

        static void Main(string[] args)
        {
            Execute();
            Console.ReadKey();
        }

        private static void Execute()
        {
            StreamReader reader;
            try
            {
                // Чтение конфига
                reader = new StreamReader(configFile);
            }
            catch (FileNotFoundException)
            {
                CreateConfig();
            }
            finally
            {
                reader = new StreamReader(configFile);
            }


            try
            {
                address = IPAddress.Parse(reader.ReadLine());
                port = int.Parse(reader.ReadLine());
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

            string[] data = Directory.GetFiles(directory);

            List<string> musicFiles = new List<string>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].EndsWith(".ogg"))
                {
                    musicFiles.Add(data[i]);
                }
            }

            music = musicFiles.ToArray();

            if (music.Length == 0)
            {
                Console.WriteLine("Нет музыки!");
                return;
            }

            
                HttpListen();
        }

        private static void HttpListen()
        {
            Console.WriteLine("Открытие соединения");

            HttpListener listener = new HttpListener();

            for (int i = 0; i < music.Length; i++)
            {
                listener.Prefixes.Add($"http://{address}:{port}/{i}/");
                listener.Prefixes.Add($"http://{address}:{port}/audio/{i}/");
            }

            listener.Start();
            
            Console.WriteLine("Приём запросов на адресе: " + address.ToString());
            try
            {
                while (true)
                {
                    ClientObject clientObject = new ClientObject(listener.GetContext(), music);
                    Task task = new Task(clientObject.Process);

                    task.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                listener.Stop();
            }
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
