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

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Data.Materials;

namespace SeaBotCore.Cache
{
    public static class DefenitionCache
    {
        private const string _cachefolder = "cache";
        private const string _baseaddr = "https://static.seaportgame.com/build/definitions/";
        private const string _basedwnladdr = "https://static.seaportgame.com/build/";
        private static readonly object locker = new object();
        private static string _lastestdef = "1.763.0";

        private static BoatDefenitions.Root _boatdefenitions;

        private static ShipDefenitions.Root _shipdefenitions;

        private static MarketplaceDefenitions.Root _marketplacedefenitions;

        private static BarrelDefenitions.Root _barreldefenitions;

        private static BuildingDefentions.Root _buildingdefenitions;

        private static DealerDefenitions.Root _dealerdefenitions;

        private static MaterialsData.Root _materials;

        private static UpgradeableDefenition.Root _upgradeable;

        private static EventsDefenitions.Root _events;

        private static WreckDefinitions.Root _wreck;

        public static void Update(string currentversion)
        {
            var needupdate = false;
            if (File.Exists("cache/cacheversion.txt"))
            {
                var cachedversion = File.ReadAllText("cache/cacheversion.txt");

                var version1 =
                    new Version(cachedversion);
                var version2 = new Version(currentversion);

                var result = version1.CompareTo(version2);
                if (result != 0) needupdate = true;
            }
            else
            {
                needupdate = true;
                //its from 0.7 i think
            }

            if (needupdate)
            {
                if (Directory.Exists("cache")) Directory.Delete("cache", true);

                Directory.CreateDirectory("cache");
                _lastestdef = currentversion;
                DownloadCache();
                File.WriteAllText("cache/cacheversion.txt", currentversion);
            }
        }

        public static bool DownloadCache()
        {
            lock (locker)
            {
                try
                {
                    var xml = new WebClient().DownloadString(_baseaddr + _lastestdef + ".xml");
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
                                new WebClient().DownloadFile(_basedwnladdr + dl, "cache.zip");
                                using (var archive = ZipFile.OpenRead("cache.zip"))
                                {
                                    if (!Directory.Exists(_cachefolder))
                                    {
                                        Directory.CreateDirectory(_cachefolder);
                                    }

                                    foreach (var entry in archive.Entries)
                                    {
                                        entry.ExtractToFile(Path.Combine(_cachefolder, entry.Name), true);
                                    }
                                }

                                File.Delete("cache.zip");
                            }
                        }
                    }
                }
                catch
                    (Exception e)
                {
                    Logger.Logger.Fatal(e.ToString());
                    return false;
                }

                return true;
            }
        }

        public static BoatDefenitions.Root GetBoatLevelDefenitions()
        {
            if (_boatdefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\boat.json"))
                    if (!DownloadCache())
                    {
                    }

                _boatdefenitions = JsonConvert.DeserializeObject<BoatDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\boat.json"));
            }

            return _boatdefenitions;
        }

        public static ShipDefenitions.Root GetShipDefenitions()
        {
            if (_shipdefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\ship.json"))
                    if (!DownloadCache())
                    {
                    }

                _shipdefenitions = JsonConvert.DeserializeObject<ShipDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\ship.json"));
            }

            return _shipdefenitions;
        }

        public static MarketplaceDefenitions.Root GetMarketPlaceDefenitions()
        {
            if (_marketplacedefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\marketplace.json"))
                    if (!DownloadCache())
                    {
                    }

                _marketplacedefenitions = JsonConvert.DeserializeObject<MarketplaceDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\marketplace.json"));
            }

            return _marketplacedefenitions;
        }

        public static BarrelDefenitions.Root GetBarrelDefenitions()
        {
            if (_barreldefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\barrel.json"))
                    if (!DownloadCache())
                    {
                    }

                _barreldefenitions = JsonConvert.DeserializeObject<BarrelDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\barrel.json"));
            }

            return _barreldefenitions;
        }

        public static BuildingDefentions.Root GetBuildingDefenitions()
        {
            if (_buildingdefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\building.json"))
                    if (!DownloadCache())
                    {
                    }

                _buildingdefenitions = JsonConvert.DeserializeObject<BuildingDefentions.Root>(
                    File.ReadAllText(_cachefolder + "\\building.json"));
            }

            return _buildingdefenitions;
        }

        public static MaterialsData.Root GetMaterials()
        {
            if (_materials == null)
            {
                if (!File.Exists(_cachefolder + "\\material.json"))
                    if (!DownloadCache())
                    {
                    }

                _materials = JsonConvert.DeserializeObject<MaterialsData.Root>(
                    File.ReadAllText(_cachefolder + "\\material.json"));
            }

            return _materials;
        }

        public static UpgradeableDefenition.Root GetUpgradeablesDefenitions()
        {
            if (_upgradeable == null)
            {
                if (!File.Exists(_cachefolder + "\\upgradeable.json"))
                    if (!DownloadCache())
                    {
                    }

                _upgradeable = JsonConvert.DeserializeObject<UpgradeableDefenition.Root>(
                    File.ReadAllText(_cachefolder + "\\upgradeable.json"));
            }

            return _upgradeable;
        }

        public static EventsDefenitions.Root GetEventDefenitions()
        {
            if (_events == null)
            {
                if (!File.Exists(_cachefolder + "\\event.json"))
                    if (!DownloadCache())
                    {
                    }

                _events = JsonConvert.DeserializeObject<EventsDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\event.json"));
            }

            return _events;
        }

        public static DealerDefenitions.Root GetDealerDefenitions()
        {
            if (_dealerdefenitions == null)
            {
                if (!File.Exists(_cachefolder + "\\dealer.json"))
                    if (!DownloadCache())
                    {
                    }

                _dealerdefenitions = JsonConvert.DeserializeObject<DealerDefenitions.Root>(
                    File.ReadAllText(_cachefolder + "\\dealer.json"));
            }

            return _dealerdefenitions;
        }

        public static WreckDefinitions.Root GetWreckDefenitions()
        {
            if (_wreck == null)
            {
                if (!File.Exists(_cachefolder + "\\wreck.json"))
                    if (!DownloadCache())
                    {
                    }

                _wreck = JsonConvert.DeserializeObject<WreckDefinitions.Root>(
                    File.ReadAllText(_cachefolder + "\\wreck.json"));
            }

            return _wreck;
        }
    }
}