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

            string data = request.Url.AbsoluteUri;

            int i = data.IndexOf('/') + 1;

            data = data.Substring(i);
            i = data.IndexOf('/') + 1;

            data = data.Substring(i);
            i = data.IndexOf('/') + 1;

            data = data.Substring(i);

            Console.WriteLine(data);

            string resp;

            byte[] buffer;

            int index;

            if (data.StartsWith("audio/"))
            {
                Console.Write("отправка музыки ");
                data = data.Substring(6);
                Console.WriteLine(data);
                index = int.Parse(data);
                Console.WriteLine(index);
                BinaryReader reader = new BinaryReader(new StreamReader(music[index]).BaseStream);

                Console.WriteLine(reader.BaseStream.Length);

                buffer = reader.ReadBytes((int)reader.BaseStream.Length);
                reader.Close();
            }
            else
            {
                index = int.Parse(data);
                int nextIndex = 0;
                if (index < music.Length - 1)
                {
                    nextIndex = index + 1;
                }

                resp = $"<html><head><meta charset=\'utf8\' />" +
                    $"</head><body><audio autoplay controls>" +
                    $"<source src=\"http://{Program.address}:{Program.port}/audio/{index}\" type=\"audio/ogg\">" +
                    $"</audio> " +
                    $"<a href=\"http://{Program.address}:{Program.port}/{nextIndex}\"> Следующая музыка </a></body></html>";
                buffer = Encoding.UTF8.GetBytes(resp);
            }

            Console.WriteLine(index);

            Console.WriteLine("Длина отправляемого пакета: " + buffer.Length);

            response.ContentLength64 = buffer.Length;

            using (Stream stream = response.OutputStream)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
