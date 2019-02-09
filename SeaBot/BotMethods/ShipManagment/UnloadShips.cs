using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBotCore.BotMethods.ShipManagment
{
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Utils;

    public static class UnloadShips
    {
        public static void UnloadAllShips()
        {
            var unloadedships = new List<int>();
            for (var index = 0; index < Core.GlobalData.Ships.Count; index++)
            {
                var ship = Core.GlobalData.Ships[index];
                if (ship.TargetId != 0 && ship.Activated != 0 && ship.IsVoyageCompleted())
                {

                    switch (ship.Type)
                    {
                        case "upgradeable":
                            {
                                if (ship.Loaded == 1)
                                {
                                    unloadedships.Add(ship.DefId);
                                    int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                    ShipManagment.UnloadShips.UnloadUpgradable(ship, uniqueid);
                                }

                                break;
                            }
                        case "marketplace":
                            {
                                unloadedships.Add(ship.DefId);
                                int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                ShipManagment.UnloadShips.UnloadMarketplace(ship,uniqueid);
                                break;
                            }
                        case "wreck":
                            {
                                unloadedships.Add(ship.DefId);
                                int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                ShipManagment.UnloadShips.UnloadWreck(ship,uniqueid);
                                break;
                            }
                        case "contractor":
                            {
                                unloadedships.Add(ship.DefId);
                                int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                ShipManagment.UnloadShips.UnloadContractor(ship,uniqueid);
                                break;
                            }
                        case "outpost":
                            {
                                unloadedships.Add(ship.DefId);
                                int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                ShipManagment.UnloadShips.UnloadOutpost(ship,uniqueid);
                                break;
                            }
                        case "social_contract":
                            {
                                unloadedships.Add(ship.DefId);
                                int uniqueid = unloadedships.Count(n => n == ship.DefId);
                                ShipManagment.UnloadShips.UnloadSocialcontract(ship,uniqueid);
                                break;
                            }
                    }
                }
            }
            unloadedships.Clear();
        }
        public static void UnloadDealer(Ship ship, int uniqueid)
        {
            // if (ship.Type == "dealer")
            // {
            // var predefined = Definitions.DealerDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
            // if (AutoShipUtils.isVoyageCompleted(ship))
            // {

            // _deship.Add(ship);
            // Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
            // Core.GlobalData.Level, Enums.EObject.dealer,
            // AutoShipUtils.GetCapacity(ship),
            // 0,
            // AutoShipUtils.GetSailors(ship), (int)predefined.Sailors,
            // ship.TargetLevel,
            // null, _deship.Count(n => n.DefId == ship.DefId)));
            // AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
            // }
            // }
        }
        public static void UnloadTreasure(Ship ship, int uniqueid)
        {
            // if (ship.Type == "treasure")
            // {
            // var predefined = Definitions.TreasureDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
            // if (AutoShipUtils.isVoyageCompleted(ship))
            // {
            // _deship.Add(ship);
            // Networking.AddTask(new Task.UnloadShipTask(ship.InstId,
            // Core.GlobalData.Level, Enums.EObject.treasure,
            // AutoShipUtils.GetCapacity(ship),
            // 0,
            // AutoShipUtils.GetSailors(ship), 0,
            // ship.TargetLevel,
            // null, _deship.Count(n => n.DefId == ship.DefId)));
            // AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
            // }
            // }
        }
        public static void UnloadSocialcontractor(Ship ship, int uniqueid)
        {
            // if (ship.Type == "global_contractor")
            // {
            // var predefined = Definitions.GConDef.Items.Item.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
            // if (AutoShipUtils.isVoyageCompleted(ship))
            // {
            // _deship.Add(ship);
            // Networking.AddTask(new Task.UnloadShipGlobalContractorTask(ship.InstId));
            // AutoShipUtils.NullShip(Core.GlobalData.Ships[index]);
            // }
            // }
        }

        public static void UnloadSocialcontract(Ship ship, int uniqueid)
        {
            var co = Core.GlobalData.SocialContracts.Where(n => n.InstId == ship.TargetId)
                .FirstOrDefault();
            if (co == null)
            {
                return;
            }

            ship.LogUnload();
            Networking.AddTask(
                new Task.DockShipSocialContractor(
                    ship,
                    false,
                    ship.Capacity(),
                    co.MaterialKoef / co.Amount,
                    ship.Sailors(),
                    co.Sailors,
                    ship.TargetLevel,
                    uniqueid));
            AutoShipUtils.NullShip(ship);
        }


        public static void UnloadOutpost(Ship ship, int uniqueid)
        {
          
            var loc = Core.GlobalData.Outposts.Where(n => n.DefId == ship.TargetId).First();
            ship.LogUnload();
            var outp = Core.GlobalData.Outposts.Where(n => n.DefId == ship.TargetId).FirstOrDefault();
            if (outp.CargoOnTheWay <= ship.Sailors())
            {
                outp.CargoOnTheWay -= ship.Sailors();
            }

            Networking.AddTask(
                new Task.DockShipTaskOutPost(
                    ship,
                    false,
                    AutoShipUtils.GetCapacity(ship),
                    ship.Cargo,
                    AutoShipUtils.GetSailors(ship),
                    ship.Crew,
                    ship.TargetLevel,
                    loc.CargoOnTheWay + loc.Crew,
                    loc.RequiredCrew,
                  uniqueid));
            AutoShipUtils.NullShip(ship);
        }
        public static void UnloadUpgradable(Ship ship,int uniqueid)
        {
            var defenition = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
            var lvl = defenition?.Levels.Level.FirstOrDefault(n => n.Id == ship.TargetLevel);
            if (lvl != null)
            {
                ship.LogUnload();
                var upg = Core.GlobalData.Upgradeables.FirstOrDefault(n => n.DefId == ship.TargetId);
                if (upg != null)
                {
                    upg.Progress += lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship);
                }

                Networking.AddTask(
                    new Task.UnloadShipTask(
                        ship.InstId,
                        Core.GlobalData.Level,
                        Enums.EObject.upgradeable,
                        AutoShipUtils.GetCapacity(ship),
                        lvl.MaterialKoef * AutoShipUtils.GetCapacity(ship),
                        AutoShipUtils.GetSailors(ship),
                        lvl.Sailors,
                        ship.TargetLevel,
                        null,uniqueid));
                AutoShipUtils.NullShip(ship);
            }
        }

        public static void UnloadWreck(Ship ship, int uniqueid)
        {
            var wrk = Core.GlobalData.Wrecks.Where(n => n.InstId == ship.TargetId).FirstOrDefault();
                    
            if (wrk != null)
            {
             
                Networking.AddTask(
                    new Task.UnloadShipTask(
                        ship.InstId,
                        Core.GlobalData.Level,
                        Enums.EObject.wreck,
                        AutoShipUtils.GetCapacity(ship),
                        0,
                        AutoShipUtils.GetSailors(ship),
                        wrk.Sailors,
                        ship.TargetLevel,
                        null,
                        uniqueid));
                AutoShipUtils.NullShip(ship);
            }
        }

        public static void UnloadContractor(Ship ship, int uniqueid)
        {
            var currentcontractor = Definitions.ConDef.Items.Item
                .FirstOrDefault(n => n.DefId == ship.TargetId);

            var quest = currentcontractor?.Quests.Quest.FirstOrDefault(n => n.Id == ship.TargetLevel);
            if (quest == null)
            {
                return;
            }

            var usedshit = quest.MaterialKoef * AutoShipUtils.GetCapacity(ship);
            var lcontract = Core.GlobalData.Contracts
                .FirstOrDefault(n => n.DefId == ship.TargetId);
            // TODO: increasing of progress or amount!
            ship.LogUnload();
            Networking.AddTask(
                new Task.DockShipTaskContractor(
                    ship,
                    false,
                    AutoShipUtils.GetCapacity(ship),
                    usedshit,
                    AutoShipUtils.GetSailors(ship),
                    currentcontractor.Sailors,
                    ship.TargetLevel,
                    currentcontractor.DefId,
                    lcontract.Progress,
                    (int)quest.InputAmount(),
                    quest.ObjectiveTypeId,
                    uniqueid));

            AutoShipUtils.NullShip(ship);
        }


        public static void UnloadMarketplace(Ship ship, int uniqueid)
        {
            var market = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
            var lvl = Definitions.MarketDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
            var mat = lvl?.Materials.Material.FirstOrDefault(n => n.Id == ship.MaterialId);
            if (mat != null)
            {
                ship.LogUnload();
                Networking.AddTask(
                    new Task.UnloadShipTask(
                        ship.InstId,
                        Core.GlobalData.Level,
                        Enums.EObject.marketplace,
                        AutoShipUtils.GetCapacity(ship),
                        mat.InputKoef * AutoShipUtils.GetCapacity(ship),
                        AutoShipUtils.GetSailors(ship),
                        market.Sailors,
                        ship.TargetLevel,
                        null,
                        uniqueid));
                AutoShipUtils.NullShip(ship);
            }
        }
    }
}
