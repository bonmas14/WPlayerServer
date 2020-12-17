using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//Copyright (c) 2020 BonMAS14
namespace WPlayerServer
{
    public class ClientObject
    {
        public HttpListenerContext context;
        public string[] music;

        public static string[] avalablePath =
        {
            "/audio/"
        };


        public ClientObject(HttpListenerContext context, string[] music)
        {
            this.context = context;
            this.music = music;
        }

        public void Process()
        {
            Console.WriteLine("Обработка");

            HttpListenerRequest request   = context.Request;
            HttpListenerResponse response = context.Response;

            string dataPath = request.Url.AbsolutePath;

            Console.WriteLine(dataPath);

            string serverRespToClient;

            byte[] sendBuffer;

            int index;

            if (dataPath.StartsWith(avalablePath[0]))
            {
                Console.WriteLine("отправка музыки...");

                index = int.Parse(dataPath.Substring(avalablePath[0].Length));
                
                Console.WriteLine("Индекс музыки: " + index);

                BinaryReader musicReader = new BinaryReader(new StreamReader(music[index]).BaseStream);

                Console.WriteLine(musicReader.BaseStream.Length);

                sendBuffer = musicReader.ReadBytes((int)musicReader.BaseStream.Length);

                musicReader.Close();

                response.ContentType = "audio/ogg";
            }
            else
            {
                index = int.Parse(dataPath.Substring(1));
                int nextIndex = 0;
                if (index < music.Length - 1)
                {
                    nextIndex = index + 1;
                }

                serverRespToClient = 
                    $"<html>" +
                        $"<head>" +
                            $"<meta charset=\'utf8\' />" +
                        $"</head>" +
                        $"<body>" +
                            $"<audio autoplay controls>" +
                                $"<source src=\"http://{Program.address}:{Program.port}/audio/{index}\" type=\"audio/ogg\">" +
                            $"</audio> " +
                            $"<a href=\"http://{Program.address}:{Program.port}/{nextIndex}\"> Следующая музыка </a>" +
                        $"</body>" +
                    $"</html>";

                sendBuffer = Encoding.UTF8.GetBytes(serverRespToClient);
            }

            Console.WriteLine("Длина отправляемого пакета: " + sendBuffer.Length);

            response.ContentLength64 = sendBuffer.Length;

            using (Stream stream = response.OutputStream)
            {
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }
        }
    }
}
