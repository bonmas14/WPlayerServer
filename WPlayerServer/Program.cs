using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

//Copyright (c) 2020 BonMAS14
namespace WPlayerServer
{
    class Program
    {
        const string configFile = "config.txt";

        public static IPAddress address;
        public static string    musicDirectory;
        public static int       port;

        static string[]  music;

        static void Main(string[] args)
        {
            GetConfiguration();

        }

        private static void GetConfiguration()
        {
            address = IPAddress.Parse(ConfigurationManager.AppSettings.Get("address"));

            port = int.Parse(ConfigurationManager.AppSettings.Get("port"));

            MusicContainer.GetMusicPath(0);

            //HttpListen();
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
                    ClientExecutor clientObject = new ClientExecutor(listener.GetContext(), music);
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
    }
}
