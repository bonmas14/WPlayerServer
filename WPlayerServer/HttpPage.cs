using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace WPlayerServer
{
    internal static class HttpPage
    {
        private static string patternsPos;
        private const  string mainpage  = @"\mainpage.html";

        static HttpPage()
        {
            patternsPos = ConfigurationManager.AppSettings.Get("httpPatterns");

            patternsPos = Directory.GetCurrentDirectory() + patternsPos;

            if (!Directory.Exists(patternsPos))
                Directory.CreateDirectory(patternsPos);
        }

        public static string GenerateMusicPage(IPAddress address, int index, int nextIndex)
        {
            var pageContainer = new StringBuilder();
            try
            {
                using (var reader = new StreamReader(patternsPos + mainpage))
                    pageContainer.AppendFormat(reader.ReadToEnd(), address.ToString(), index, nextIndex);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Не найден шаблон страницы");
            }

            return pageContainer.ToString();
        }
    }
}
