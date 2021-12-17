using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPlayerServer
{
    internal static class MusicContainer
    {
        private static string directory;

        private static string[] musicPaths;

        static MusicContainer()
        {
            directory = ConfigurationManager.AppSettings.Get("music");

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("папки не существует");

            string[] data = Directory.GetFiles(directory);

            var musicFiles = new List<string>();

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].EndsWith(".ogg"))
                {
                    musicFiles.Add(data[i]);
                }
            }

            musicPaths = musicFiles.ToArray();
        }

        public static BinaryReader GetMusicPath(int index)
        {
            if (musicPaths.Length == 0)
                throw new FileNotFoundException("Нет музыки!");

            return new BinaryReader(new StreamReader(musicPaths[index]).BaseStream);
        }
    }
}
