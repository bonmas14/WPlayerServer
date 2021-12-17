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
    public class ClientExecutor
    {
        public HttpListenerContext context;
        public string[] music;

        public static string[] avalablePath =
        {
            "/audio/"
        };


        public ClientExecutor(HttpListenerContext context, string[] music)
        {
            this.context = context;
            this.music = music;
        }

        public void Process()
        {
            HttpListenerRequest request   = context.Request;
            HttpListenerResponse response = context.Response;

            string dataPath = request.Url.AbsolutePath;

            Console.WriteLine(DateTime.UtcNow.TimeOfDay.ToString() + ": " + dataPath);

            string serverRespToClient;

            byte[] dataBuffer;

            int index;

            if (dataPath.StartsWith(avalablePath[0]))
            {
                index = int.Parse(dataPath.Substring(avalablePath[0].Length));
                
                BinaryReader musicReader = MusicContainer.GetMusicPath(index);

                dataBuffer = musicReader.ReadBytes((int)musicReader.BaseStream.Length);

                musicReader.Close();

                response.ContentType = "audio/ogg";
            }
            else
            {
                index = int.Parse(dataPath.Substring(1));
                
                serverRespToClient = HttpPage.GenerateMusicPage(Program.address, index, index % music.Length);

                dataBuffer = Encoding.UTF8.GetBytes(serverRespToClient);
            }

            response.ContentLength64 = dataBuffer.Length;

            using (Stream stream = response.OutputStream)
            {
                stream.Write(dataBuffer, 0, dataBuffer.Length);
            }
        }
    }
}
