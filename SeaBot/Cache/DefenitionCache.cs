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
using System.Collections.Generic;
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
    internal static class DefenitionCache
    {
        private const string _cachefolder = "cache";
        private const string _baseaddr = "https://static.seaportgame.com/build/definitions/";
        private const string _basedwnladdr = "https://static.seaportgame.com/build/";
        private static readonly object locker = new object();
        private static string _lastestdef = "1.763.0";
        private static Dictionary<EDefinitionType, IDefinition> _cache = new Dictionary<EDefinitionType, IDefinition>();

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
                if (result != 0)
                {
                    needupdate = true;
                }
            }
            else
            {
                needupdate = true;
                //its from 0.7 i think
            }

            if (needupdate)
            {
                if (Directory.Exists("cache"))
                {
                    Directory.Delete("cache", true);
                }

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


        public static IDefinition GetDefinition(EDefinitionType type)
        {
            if (_cache.ContainsKey(type))
            {
                return _cache[type];
            }

            IDefinition ret = null;
            var filename = "";
            switch (type)
            {
                case EDefinitionType.Barrels:
                    filename = "barrel";
                    break;
                case EDefinitionType.Events:
                    filename = "event";
                    break;
                case EDefinitionType.Buildings:
                    filename = "building";
                    break;
                case EDefinitionType.Boat:
                    filename = "boat";
                    break;
                case EDefinitionType.Dealer:
                    filename = "dealer";
                    break;
                case EDefinitionType.Marketplace:
                    filename = "marketplace";
                    break;
                case EDefinitionType.Ship:
                    filename = "ship";
                    break;
                case EDefinitionType.Upgradable:
                    filename = "upgradeable";
                    break;
                case EDefinitionType.Wreck:
                    filename = "wreck";
                    break;
                case EDefinitionType.Material:
                    filename = "material";
                    break;
                case EDefinitionType.GlobalContractor:
                    filename = "global_contractor";
                    break;
                case EDefinitionType.Contractor:
                    filename = "contractor";
                    break;
                case EDefinitionType.SocialContract:
                    filename = "social_contract";
                    break;
                case EDefinitionType.Outpost:
                    filename = "outpost";
                    break;
                case EDefinitionType.Treasure:
                    filename = "treasure";
                    break;
                case EDefinitionType.MuseumLevels:
                    filename = "museum_level";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }


            if (!File.Exists(_cachefolder + "\\" + filename + ".json"))
            {
                DownloadCache();
            }

            var content = File.ReadAllText(_cachefolder + "\\" + filename + ".json");
            switch (type)
            {
                case EDefinitionType.Barrels:
                    ret = JsonConvert.DeserializeObject<BarrelDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Boat:
                    ret = JsonConvert.DeserializeObject<BoatDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Buildings:
                    ret = JsonConvert.DeserializeObject<BuildingDefentions.Root>(
                        content);
                    break;
                case EDefinitionType.Dealer:
                    ret = JsonConvert.DeserializeObject<DealerDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Wreck:
                    ret = JsonConvert.DeserializeObject<WreckDefinitions.Root>(
                        content);
                    break;
                case EDefinitionType.Ship:
                    ret = JsonConvert.DeserializeObject<ShipDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Marketplace:
                    ret = JsonConvert.DeserializeObject<MarketplaceDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Upgradable:
                    ret = JsonConvert.DeserializeObject<UpgradeableDefenition.Root>(
                        content);
                    break;
                case EDefinitionType.Events:
                    ret = JsonConvert.DeserializeObject<EventsDefenitions.Root>(
                        content);
                    break;
                case EDefinitionType.Material:
                    ret = JsonConvert.DeserializeObject<MaterialsData.Root>(content);
                    break;
                case EDefinitionType.Contractor:
                    ret = JsonConvert.DeserializeObject<ContractorDefinitions.Root>(content);
                    break;
                case EDefinitionType.GlobalContractor:
                    ret = JsonConvert.DeserializeObject<GlobalContractorDefinitions.Root>(content);
                    break;
                case EDefinitionType.SocialContract:
                    ret = JsonConvert.DeserializeObject<SocialContractDefenitions.Root>(content);
                    break;
                case EDefinitionType.Outpost:
                    ret = JsonConvert.DeserializeObject<OutpostDefinitions.Root>(content);
                    break;
                case EDefinitionType.Treasure:
                    ret = JsonConvert.DeserializeObject<TreasureDefenitions.Root>(content);
                    break;
                case EDefinitionType.MuseumLevels:
                    ret = JsonConvert.DeserializeObject<MuseumLevelDefenitions.Root>(content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            _cache.Add(type, ret);
            return ret;
        }
    }
}