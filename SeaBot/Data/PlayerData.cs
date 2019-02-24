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
namespace SeaBotCore.Data
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml;

    using SeaBotCore.BotMethods;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Utils;

    #endregion

    internal static class Parser
    {
        public static PlayerData ParseXmlToGlobalData(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var data = new PlayerData();
            if (doc.DocumentElement == null)
            {
                return null;
            }

            data.UserId = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("pid")?.InnerText);

            data.Inventory = new FullyObservableCollection<Item>();
            var inventory = doc.DocumentElement.SelectSingleNode("material");
            if (inventory != null)
            {
                foreach (XmlNode node in inventory.ChildNodes)
                {
                    data.Inventory.Add(
                        new Item
                            {
                                Id = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                Amount = Convert.ToInt32(node.SelectSingleNode("amount")?.InnerText)
                            });
                }
            }

            data.Level = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("level")?.InnerText);
            data.Xp = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("xp")?.InnerText);
            data.Sailors = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("sailors")?.InnerText);
            data.LighthouseLevel = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("lighthouse_level")?.InnerText);
            data.BoatLevel = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("boat_level")?.InnerText);
            data.BoatLevel = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("boat_level")?.InnerText);
            data.SyncInterval = Convert.ToByte(doc.DocumentElement.SelectSingleNode("sync_interval")?.InnerText);
            data.StorageCapacity =  Convert.ToInt32(doc.DocumentElement.SelectSingleNode("mat_slot_max")?.InnerText);
            data.Achievements = new List<Achievement>();
            var achievementsnode = doc.DocumentElement.SelectSingleNode("achievement");
            if (achievementsnode != null)
            {
                foreach (XmlNode node in achievementsnode.ChildNodes)
                {
                    data.Achievements.Add(
                        new Achievement
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                Progress = Convert.ToInt32(node.SelectSingleNode("progress")?.InnerText),
                                Done = Convert.ToInt32(node.SelectSingleNode("done")?.InnerText),
                                ConfirmedLevel = Convert.ToInt32(node.SelectSingleNode("confirmed_level")?.InnerText)
                            });
                }
            }

            

            data.Boats = new List<Boat>();
            var boatsnode = doc.DocumentElement.SelectSingleNode("boat");
            if (boatsnode != null)
            {
                foreach (XmlNode node in boatsnode.ChildNodes)
                {
                    data.Boats.Add(
                        new Boat
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                Turn = Convert.ToInt32(node.SelectSingleNode("turn")?.InnerText),
                                ProdStart = Convert.ToInt32(node.SelectSingleNode("prod_start")?.InnerText)
                            });
                }
            }

            

            #region Captains

            data.CaptainsNew = new List<Captain>();
            var captainnode = doc.DocumentElement.SelectSingleNode("captain_new");
            if (captainnode != null)
            {
                foreach (XmlNode node in captainnode.ChildNodes)
                {
                    data.CaptainsNew.Add(
                        new Captain
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                Charges = Convert.ToInt32(node.SelectSingleNode("charges")?.InnerText),
                                ShipId = Convert.ToInt32(node.SelectSingleNode("ship_id")?.InnerText),
                                Type = node.SelectSingleNode("inst_id")?.InnerText,
                                Created = Convert.ToInt32(node.SelectSingleNode("created")?.InnerText),
                                SourceType = node.SelectSingleNode("source_type")?.InnerText,
                                BonusAmount = Convert.ToInt32(node.SelectSingleNode("bonus_amount")?.InnerText)
                            });
                }
            }

            #endregion

            #region Contracts

            data.Contracts = new List<Contractor>();
            var contractornode = doc.DocumentElement.SelectSingleNode("contractor");
            if (contractornode != null)
            {
                foreach (XmlNode node in contractornode.ChildNodes)
                {
                    var a = new Contractor
                                {
                                    DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                    QuestId = Convert.ToInt32(node.SelectSingleNode("quest_id")?.InnerText),
                                    Progress = Convert.ToInt32(node.SelectSingleNode("progress")?.InnerText),
                                    Done = Convert.ToInt32(node.SelectSingleNode("done")?.InnerText),
                                    CargoOnTheWay =
                                        Convert.ToInt32(node.SelectSingleNode("cargo_on_the_way")?.InnerText),
                                    Amount = Convert.ToInt32(node.SelectSingleNode("amount")?.InnerText),
                                    PlayerLevel = Convert.ToInt32(node.SelectSingleNode("player_level")?.InnerText)
                                };
                    a.Rewards = new List<Reward>();
                    var rewardnode = node.SelectSingleNode("rewards");
                    foreach (XmlNode rnode in rewardnode)
                    {
                        a.Rewards.Add(
                            new Reward
                                {
                                    Id = Convert.ToInt32(rnode.SelectSingleNode("id")?.InnerText),
                                    Type = rnode.SelectSingleNode("type")?.InnerText,
                                    Amount = Convert.ToInt32(rnode.SelectSingleNode("amount")?.InnerText)
                                });
                    }

                    data.Contracts.Add(a);
                }
            }

            #endregion

            #region LostTreas

            data.LostTreasures = new List<LostTreasure>();
            var lostTreasurenode = doc.DocumentElement.SelectSingleNode("lost_treasure");
            if (lostTreasurenode != null)
            {
                foreach (XmlNode node in lostTreasurenode.ChildNodes)
                {
                    data.LostTreasures.Add(
                        new LostTreasure
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                Seed = Convert.ToInt32(node.SelectSingleNode("seed")?.InnerText),
                                ClaimedChests = Convert.ToInt32(node.SelectSingleNode("claimed_chests")?.InnerText),
                                UnlockStarted = Convert.ToInt32(node.SelectSingleNode("unlock_started")?.InnerText)
                            });
                }
            }

            #endregion

            #region Ships

            data.Ships = new List<Ship>();
            var shipsnode = doc.DocumentElement.SelectSingleNode("ship");
            if (shipsnode != null)
            {
                foreach (XmlNode node in shipsnode.ChildNodes)
                {
                    data.Ships.Add(
                        new Ship
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                Activated = Convert.ToInt32(node.SelectSingleNode("activated")?.InnerText),
                                Type = node.SelectSingleNode("type")?.InnerText,
                                TargetId = Convert.ToInt32(node.SelectSingleNode("target_id")?.InnerText),
                                TargetLevel = Convert.ToInt32(node.SelectSingleNode("target_level")?.InnerText),
                                Sent = Convert.ToInt32(node.SelectSingleNode("sent")?.InnerText),
                                Cargo = Convert.ToInt32(node.SelectSingleNode("cargo")?.InnerText),
                                MaterialId = Convert.ToInt32(node.SelectSingleNode("material_id")?.InnerText),
                                Loaded = Convert.ToInt32(node.SelectSingleNode("loaded")?.InnerText),
                                CaptainId = Convert.ToInt32(node.SelectSingleNode("captain_id")?.InnerText),
                                Crew = Convert.ToInt32(node.SelectSingleNode("crew")?.InnerText),
                                SourceType = node.SelectSingleNode("source_type")?.InnerText,
                                NextLevel = Convert.ToInt32(node.SelectSingleNode("next_level")?.InnerText),
                                SailorsLevel = Convert.ToInt32(node.SelectSingleNode("sailors_level")?.InnerText),
                                NextSailorsLevel =
                                    Convert.ToInt32(node.SelectSingleNode("next_sailors_level")?.InnerText),
                                NextCapacityLevel =
                                    Convert.ToInt32(node.SelectSingleNode("next_capacity_level")?.InnerText),
                                CapacityLevel = Convert.ToInt32(node.SelectSingleNode("capacity_level")?.InnerText)
                            });
                }
            }

            #endregion

            #region Upgradeables

            data.Upgradeables = new List<Upgradeable>();
            var upgradenode = doc.DocumentElement.SelectSingleNode("upgradeables");
            if (upgradenode != null)
            {
                foreach (XmlNode node in upgradenode.ChildNodes)
                {
                    data.Upgradeables.Add(
                        new Upgradeable
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                Progress = Convert.ToInt32(node.SelectSingleNode("progress")?.InnerText),
                                Done = Convert.ToInt32(node.SelectSingleNode("done")?.InnerText),
                                CargoOnTheWay = Convert.ToInt32(node.SelectSingleNode("cargo_on_the_way")?.InnerText),
                                ConfirmedTime = Convert.ToInt32(node.SelectSingleNode("confirmed_time")?.InnerText),
                                Amount = Convert.ToInt32(node.SelectSingleNode("amount")?.InnerText),
                                Sailors = Convert.ToInt32(node.SelectSingleNode("sailors")?.InnerText),
                                MaterialKoef = Convert.ToInt32(node.SelectSingleNode("material_koef")?.InnerText),
                                PlayerLevel = Convert.ToInt32(node.SelectSingleNode("player_level")?.InnerText)
                            });
                }
            }

            #endregion

            #region Buildings

            data.Buildings = new List<Building>();
            var buildnode = doc.DocumentElement.SelectSingleNode("building");
            if (buildnode != null)
            {
                foreach (XmlNode node in buildnode.ChildNodes)
                {
                    data.Buildings.Add(
                        new Building
                            {
                                DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                GridX = Convert.ToInt32(node.SelectSingleNode("grid_x")?.InnerText),
                                GridY = Convert.ToInt32(node.SelectSingleNode("grid_y")?.InnerText),
                                ProdStart = Convert.ToInt32(node.SelectSingleNode("prod_start")?.InnerText),
                                UpgStart = Convert.ToInt32(node.SelectSingleNode("upg_start")?.InnerText),
                                UpgType = Convert.ToInt32(node.SelectSingleNode("upg_type")?.InnerText),
                                NewBuildings = Convert.ToInt32(node.SelectSingleNode("new_buildings")?.InnerText),
                                ProdId = Convert.ToInt32(node.SelectSingleNode("prod_id")?.InnerText)
                            });
                }
            }

            #endregion

            #region Wreck

            data.Wrecks = new List<Wreck>();
            var wrecknode = doc.DocumentElement.SelectSingleNode("wreck");
            if (wrecknode != null)
            {
                foreach (XmlNode node in wrecknode.ChildNodes)
                {
                    var wr = new Wreck
                                 {
                                     DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                     Sailors = Convert.ToInt32(node.SelectSingleNode("sailors")?.InnerText),
                                     InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                     Spot = Convert.ToInt32(node.SelectSingleNode("spot")?.InnerText),
                                     Status = Convert.ToInt32(node.SelectSingleNode("status")?.InnerText)
                                 };
                    var rewardnode = node.SelectSingleNode("rewards");
                    wr.Rewards = new List<Reward>();
                    foreach (XmlNode rnode in rewardnode)
                    {
                        wr.Rewards.Add(
                            new Reward
                                {
                                    Id = Convert.ToInt32(rnode.SelectSingleNode("id")?.InnerText),
                                    Type = rnode.SelectSingleNode("type")?.InnerText,
                                    Amount = Convert.ToInt32(rnode.SelectSingleNode("amount")?.InnerText)
                                });
                    }

                    data.Wrecks.Add(wr);
                }
            }

            #endregion

            #region Slots

            data.Slots = new List<Slot>();
            var slotnode = doc.DocumentElement.SelectSingleNode("slot");
            if (slotnode != null)
            {
                foreach (XmlNode node in slotnode.ChildNodes)
                {
                    var wr = new Slot
                                 {
                                     Type = node.SelectSingleNode("type")?.InnerText,
                                     Level = Convert.ToInt32(node.SelectSingleNode("level")?.InnerText),
                                     SlotUsed = Convert.ToInt32(node.SelectSingleNode("slot_used")?.InnerText),
                                     SlotMax = Convert.ToInt32(node.SelectSingleNode("slot_max")?.InnerText),
                                     Ai = Convert.ToInt32(node.SelectSingleNode("ai")?.InnerText),
                                     LastUsed = Convert.ToInt32(node.SelectSingleNode("last_used")?.InnerText)
                                 };

                    data.Slots.Add(wr);
                }
            }

            #endregion

            #region Outpost

            data.Outposts = new List<Outpost>();
            var outpost = doc.DocumentElement.SelectSingleNode("outpost");
            if (slotnode != null)
            {
                foreach (XmlNode node in outpost.ChildNodes)
                {
                    var wr = new Outpost
                                 {
                                     DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                     Crew = Convert.ToInt32(node.SelectSingleNode("crew")?.InnerText),
                                     Done = Convert.ToByte(node.SelectSingleNode("done")?.InnerText) == 1,
                                     CargoOnTheWay =
                                         Convert.ToInt32(node.SelectSingleNode("cargo_on_the_way")?.InnerText),
                                     PlayerLevel = Convert.ToInt32(node.SelectSingleNode("player_level")?.InnerText),
                                     RequiredCrew = Convert.ToInt32(node.SelectSingleNode("required_crew")?.InnerText)
                                 };

                    data.Outposts.Add(wr);
                }
            }

            #endregion

            #region SocialContractors

            data.SocialContracts = new List<SocialContract>();
            var socialcontractNode = doc.DocumentElement.SelectSingleNode("social_contract");
            if (socialcontractNode != null)
            {
                foreach (XmlNode node in socialcontractNode.ChildNodes)
                {
                    var wr = new SocialContract
                                 {
                                     DefId = Convert.ToInt32(node.SelectSingleNode("def_id")?.InnerText),
                                     Done = Convert.ToByte(node.SelectSingleNode("done")?.InnerText) == 1,
                                     InstId = Convert.ToInt32(node.SelectSingleNode("inst_id")?.InnerText),
                                     Progress = Convert.ToInt32(node.SelectSingleNode("progress")?.InnerText),
                                     MaterialId = Convert.ToInt32(node.SelectSingleNode("material_id")?.InnerText),
                                     MaterialKoef = Convert.ToInt32(node.SelectSingleNode("material_koef")?.InnerText),
                                     Amount = Convert.ToInt32(node.SelectSingleNode("amount")?.InnerText),
                                     Sailors = Convert.ToInt32(node.SelectSingleNode("sailors")?.InnerText),
                                     RewardType = node.SelectSingleNode("material_id")?.InnerText,
                                     RewardId = Convert.ToInt32(node.SelectSingleNode("reward_id")?.InnerText),
                                     RewardAmount = Convert.ToInt32(node.SelectSingleNode("reward_amount")?.InnerText)
                                 };

                    data.SocialContracts.Add(wr);
                }
            }

            #endregion

            Barrels.BarrelController._lastBarrelSeed =
                Convert.ToDouble(doc.DocumentElement.SelectSingleNode("last_barrel_amount")?.InnerText);
            data.HeartbeatInterval =
                Convert.ToInt32(doc.DocumentElement.SelectSingleNode("config/heartbeat_interval")?.InnerText);
            return data;
        }
    }

    public class PlayerData
    {
        public int StorageCapacity { get; set; }

        public List<Achievement> Achievements { get; set; }

        public int BoatLevel { get; set; }

        public List<Boat> Boats { get; set; }

        public List<Building> Buildings { get; set; }

        public List<Captain> CaptainsNew { get; set; }

        public List<Contractor> Contracts { get; set; }

        public int HeartbeatInterval { get; set; }

        public FullyObservableCollection<Item> Inventory { get; set; }

        public int Level { get; set; }

        public int LighthouseLevel { get; set; }

        public List<LostTreasure> LostTreasures { get; set; }

        public List<Outpost> Outposts { get; set; }

        public int Sailors { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Slot> Slots { get; set; }

        public List<SocialContract> SocialContracts { get; set; }

        public byte SyncInterval { get; set; }

        public List<Upgradeable> Upgradeables { get; set; }

        public int UserId { get; set; }

        public List<Wreck> Wrecks { get; set; }

        public int Xp { get; set; }

        public int GetAmountItem(string name)
        {
            return this.GetAmountItem(MaterialDB.GetItem(name).DefId);
        }

        public int GetAmountItem(int id)
        {
            if (this.Inventory == null)
            {
                return 0;
            }

            if (this.Inventory.Any(n => n.Id == id))
            {
                return this.Inventory.First(n => n.Id == id).Amount;
            }

            return 0;
        }
    }

    public class Building
    {
       
        public int DefId;

        public int GridX;

        public int GridY;

        public int InstId;

        public int Level;

        public int NewBuildings;

        public int ProdId;

        public int ProdStart;

        public int UpgStart;

        public int UpgType;

        public BuildingDefentions.Building Definition
        {
            get
            {
                var def = LocalDefinitions.Buildings
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
        
        public BuildingDefentions.BuildingLevel DefinitionLevel
        {
            get
            {
                return this.Definition.BuildingLevels.Level.FirstOrDefault(n => n.Id == this.Level);
               
            }
        }
       
        public int ProductionTime
        {
            get
            {
                
                var prod = this.DefinitionLevel.ProdOutputs.ProdOutput
                    .FirstOrDefault(n => n.Id == this.ProdId);
                if (prod != null)
                {
                    return prod.Time;
                }
                
                
                return 0;
            }
        }

        public int UpgradeTime
        {
            get
            {
                var nextlvl = Definition.BuildingLevels.Level.FirstOrDefault(n => n.Id == this.Level + 1);
                if (nextlvl != null)
                {
                     return nextlvl.UpgradeTime;
                 }

                 return 0;
                
            }
        }
    }

    public class Outpost
    {
        public int CargoOnTheWay { get; set; }

        public int Crew { get; set; }

        public int DefId { get; set; }

        public bool Done { get; set; }

        public int PlayerLevel { get; set; }

        public int RequiredCrew { get; set; }

        public OutpostDefinitions.Outpost Definition
        {
            get
            {
                var def = LocalDefinitions.Outposts
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Upgradeable
    {
       
        public int Amount { get; set; }

        public int CargoOnTheWay { get; set; }

        public int ConfirmedTime { get; set; }

        public int DefId { get; set; }

        public int Done { get; set; }

        public int Level { get; set; }

        public int MaterialKoef { get; set; }

        public int PlayerLevel { get; set; }

        public int Progress { get; set; }

        public int Sailors { get; set; }

        public UpgradeableDefenition.Upgradable Definition
        {
            get
            {
                var def = LocalDefinitions.Upgradables
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Ship
    {
        public int Activated { get; set; }

        public int CapacityLevel { get; set; }

        public int CaptainId { get; set; }

        public int Cargo { get; set; }

        public int Crew { get; set; }

        public int DefId { get; set; }

        public int InstId { get; set; }

        public int Level { get; set; }

        public int Loaded { get; set; }

        public int MaterialId { get; set; }

        public int NextCapacityLevel { get; set; }

        public int NextLevel { get; set; }

        public int NextSailorsLevel { get; set; }

        public int SailorsLevel { get; set; }

        public int Sent { get; set; }

        public string SourceType { get; set; }

        public int TargetId { get; set; }

        public int TargetLevel { get; set; }

        public string Type { get; set; }

        public ShipDefenitions.Ship Definition
        {
            get
            {
                var def = LocalDefinitions.Ships
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class LostTreasure
    {
        public int ClaimedChests { get; set; }

        public int DefId { get; set; }

        public int Seed { get; set; }

        public int UnlockStarted { get; set; }

        public TreasureDefenitions.Treasure Definition
        {
            get
            {
                var def = LocalDefinitions.Treasures
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Wreck
    {
        public int DefId { get; set; }

        public int InstId { get; set; }

        public List<Reward> Rewards { get; set; }

        public int Sailors { get; set; }

        public int Spot { get; set; }

        public int Status { get; set; }
        
        public WreckDefinitions.Wreck Definition
        {
            get
            {
                var def = LocalDefinitions.Wrecks
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Slot
    {
        public int Ai { get; set; }

        public int LastUsed { get; set; }

        public int Level { get; set; }

        public int SlotMax { get; set; }

        public int SlotUsed { get; set; }

        public string Type { get; set; }
    }

    public class Contractor
    {
        public int Amount { get; set; }

        public int CargoOnTheWay { get; set; }

        public int DefId { get; set; }

        public int Done { get; set; }

        public int PlayerLevel { get; set; }

        public int Progress { get; set; }

        public int QuestId { get; set; }
        
        public ContractorDefinitions.Contractor Definition
        {
            get
            {
                var def = LocalDefinitions.Contractors
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }

        public List<Reward> Rewards { get; set; }
    }

    public class Reward
    {
        public int Amount { get; set; }

        public int Id { get; set; }

        public string Type { get; set; }
    }

    public class SocialContract
    {
        public int Amount { get; set; }

        public int DefId { get; set; }

        public bool Done { get; set; }

        public int FriendId { get; set; }

        public int InstId { get; set; }

        public int MaterialId { get; set; }

        public int MaterialKoef { get; set; }

        public int Progress { get; set; }

        public int RewardAmount { get; set; }

        public int RewardId { get; set; }

        public string RewardType { get; set; }

        public int Sailors { get; set; }
 
        public SocialContractDefenitions.SocialContract Definition
        {
            get
            {
                var def = LocalDefinitions.SocialContracts
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Captain
    {
        public int BonusAmount { get; set; }

        public int Charges { get; set; }

        public int Created { get; set; }

        public int DefId { get; set; }

        public int InstId { get; set; }

        public int ShipId { get; set; }

        public string SourceType { get; set; }

        public string Type { get; set; }

    }

    public class Boat
    {
        public int DefId { get; set; }

        public int InstId { get; set; }

        public int Level { get; set; }

        public int ProdStart { get; set; }

        public int Turn { get; set; }

        public BoatDefenitions.Boat Definition
        {
            get
            {
                var def = LocalDefinitions.Boats
                    .FirstOrDefault(n => n.DefId == this.DefId);
                return def;
            }
        }
    }

    public class Achievement
    {
        public int ConfirmedLevel { get; set; }

        public int DefId { get; set; }

        public int Done { get; set; }

        public int Level { get; set; }

        public int Progress { get; set; }
    }

    public class Item : INotifyPropertyChanged
    {
        private int _amount;

        private int _id;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Amount
        {
            get => this._amount;
            set
            {
                this._amount = value;
                this.OnPropertyChanged();
            }
        }

        public int Id
        {
            get => this._id;
            set
            {
                this._id = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}