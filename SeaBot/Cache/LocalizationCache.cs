// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace SeaBotCore.Cache
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml;

    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;

    #endregion

    public static class LocalizationCache
    {
        private const string _baseaddr = "https://static.seaportgame.com/localization/";

        private const string _baseaddr2 = "https://static.seaportgame.com/build/";

        private const string _cachefolder = "loccache";

        private static readonly object locker = new object();

        private static string _lastestdef = "1.427.0";

        private static Dictionary<string, string> _local;

        public static bool DownloadCache()
        {
            lock (locker)
            {
                try
                {
                    var neededlangs = Enum.GetNames(typeof(LocalizationController.ELanguages));
                    Logger.Info(Localization.CORE_LOCAL_DOWNLOAD_STARTED);
                    var xml = new WebClient().DownloadString(_baseaddr + _lastestdef + ".xml");
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    if (doc.DocumentElement != null)
                    {
                        var nodes = doc.SelectSingleNode("xml/files");
                        foreach (XmlNode node in nodes.ChildNodes)
                        {
                            if (node.InnerText.Contains("localization.csv")
                                && neededlangs.Where(n => node.InnerText.Contains(n)).Any())
                            {
                                var dl = new Regex(@"\/(.+)\/localization\.csv,(.+)").Match(node.InnerText);
                                if (!Directory.Exists(_cachefolder))
                                {
                                    Directory.CreateDirectory(_cachefolder);
                                }

                                new WebClient().DownloadFile(
                                    _baseaddr2 + dl.Groups[2].Value,
                                    $"{_cachefolder}/{dl.Groups[1].Value}.lang");
                                Logger.Info(string.Format(Localization.CORE_LOCAL_DOWNLOAD_STEP, dl.Groups[1].Value));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Fatal(e.ToString());
                    return false;
                }

                return true;
            }
        }

        public static string GetNameFromLoc(string item, string defname)
        {
            if (_local == null || _local.Count == 0)
            {
                if (Directory.Exists(_cachefolder))
                {
                    if (File.Exists($"{_cachefolder}/{Core.Config.language}.lang"))
                    {
                        _local = LoadDictionary(File.ReadAllLines($"{_cachefolder}/{Core.Config.language}.lang"));
                    }
                    else
                    {
                        DownloadCache();
                        _local = LoadDictionary(File.ReadAllLines($"{_cachefolder}/{Core.Config.language}.lang"));
                    }
                }
                else
                {
                    DownloadCache();
                    _local = LoadDictionary(File.ReadAllLines($"{_cachefolder}/{Core.Config.language}.lang"));
                }
            }

            var str = _local.FirstOrDefault(n => string.Compare(n.Key, item, true) == 0);
            if (str.Value == null)
            {
                return defname;
            }

            return str.Value;
        }

        // https://static.seaportgame.com/localization/1.427.0.xml
        public static void Update(string currentversion)
        {
            var needupdate = false;
            if (File.Exists(_cachefolder + "/cacheversion.txt"))
            {
                var cachedversion = File.ReadAllText(_cachefolder + "/cacheversion.txt");

                var version1 = new Version(cachedversion);
                var version2 = new Version(currentversion);

                var result = version1.CompareTo(version2);
                if (result != 0)
                {
                    _lastestdef = currentversion;
                    needupdate = true;
                }
            }
            else
            {
                needupdate = true;

                // its from 0.7 i think
            }

            if (needupdate)
            {
                if (Directory.Exists(_cachefolder))
                {
                    Directory.Delete(_cachefolder, true);
                }

                Directory.CreateDirectory(_cachefolder);
                _lastestdef = currentversion;
                DownloadCache();
                File.WriteAllText(_cachefolder + "/cacheversion.txt", currentversion);
            }

            _lastestdef = currentversion;
        }

        private static Dictionary<string, string> LoadDictionary(string[] arr)
        {
            var ret = new Dictionary<string, string>();
            foreach (var item in arr)
            {
                string key;
                string value;
                var str = item.Replace("*#*", "#");
                var charLocation = str.IndexOf('#');

                if (charLocation > 0)
                {
                    key = str.Substring(0, charLocation);
                }
                else
                {
                    continue;
                }

                value = str.Substring(charLocation + 1);
                ret.Add(key, value);
            }

            return ret;
        }
    }
}