using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using SeaBotCore.Data.Defenitions;


namespace SeaBotCore
{
    public static class Cache
    {
        static object locker = new object();
        private const string _cachefolder = "cache";
        private const string _lastestdef = "definitions/1.763.0.xml";
        private const string _baseaddr = "http://r4a4v3g4.ssl.hwcdn.net/build/";

        public static bool DownloadCache()
        {
            lock (locker)
            {


                try
                {
                    var xml = new WebClient().DownloadString(_baseaddr + _lastestdef);
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    if (doc.DocumentElement != null)
                    {


                        var nodes = doc.SelectSingleNode("xml/files");
                        foreach (XmlNode node in nodes.ChildNodes)
                        {
                            if (node.InnerText.Contains("definitions_json.zip"))
                            {
                                var dl = new Regex(@"definitions_json\.zip,(.+)").Match(node.InnerText).Groups[1].Value;
                                new WebClient().DownloadFile(_baseaddr + dl, "cache.zip");
                                using (var archive = ZipFile.OpenRead("cache.zip"))
                                {
                                    if (!Directory.Exists(_cachefolder))
                                    {
                                        Directory.CreateDirectory(_cachefolder);
                                    }

                                    foreach (var entry in archive.Entries)
                                    {
                                        entry.ExtractToFile(Path.Combine(_cachefolder, entry.FullName), true);
                                    }

                                    
                                }
                                File.Delete("cache.zip");
                            }
                            


                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public static BoatDefenitions.Root GetBoatLevelDefenitions()
        {
           
            if (!File.Exists(_cachefolder + "\\boat.json"))
            {
                if (!DownloadCache())
                {
                  
                }
               
            }
            return JsonConvert.DeserializeObject<BoatDefenitions.Root>(File.ReadAllText(_cachefolder + "\\boat.json"));
        }
        public static BarrelDefenitions.Root GetBarrelDefenitions()
        {
        
            if (!File.Exists(_cachefolder + $"\\barrel.json"))
            {
                if (!DownloadCache())
                {
                    
                }

            }
            return JsonConvert.DeserializeObject<BarrelDefenitions.Root>(File.ReadAllText(_cachefolder + "\\barrel.json"));
        }
        public static BuildingDefentions.Root GetBuildingDefenitions()
        {

            if (!File.Exists(_cachefolder + $"\\building.json"))
            {
                if (!DownloadCache())
                {
                   
                }

            }
            return JsonConvert.DeserializeObject<BuildingDefentions.Root>(File.ReadAllText(_cachefolder + "\\building.json"));
        }

    }


}
