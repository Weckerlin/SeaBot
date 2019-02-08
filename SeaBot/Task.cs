// // SeaBotCore
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace SeaBotCore
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using SeaBotCore.BotMethods;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Utils;

    #endregion

    public class Task
    {
        public interface IGameTask
        {
            string Action { get; }

            Dictionary<string, object> CustomObjects { get; }

            uint Time { get; }
        }

        public class ActivateEvent : IGameTask
        {
            public ActivateEvent(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_event";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ActivateItemsOfferTask : IGameTask
        {
            public ActivateItemsOfferTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_items_offer";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ActivateMerchantOfferTask : IGameTask
        {
            public ActivateMerchantOfferTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_merchant_offer";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ActivatePackTask : IGameTask
        {
            public ActivatePackTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "activate_pack";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ActivateShipTask : IGameTask
        {
            public ActivateShipTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "activate_ship";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class AssignCaptainTask : IGameTask
        {
            public AssignCaptainTask(int ship_inst_id, int captain_inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("ship_inst_id", ship_inst_id);
                this.CustomObjects.Add("captain_inst_id", captain_inst_id);
            }

            public string Action => "assign_ship_captain";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyBoatTask : IGameTask
        {
            public BuyBoatTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "boat");
                this.CustomObjects.Add("payment_type", "standard");
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyBuildAreaTask : IGameTask
        {
            public BuyBuildAreaTask(int def_id, int x, int y, bool gem = false)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "buildarea");
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("x", x);
                this.CustomObjects.Add("y", y);
                this.CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyBuildingTask : IGameTask
        {
            public BuyBuildingTask(int def_id, int x, int y, bool gem = false)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "building");

                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("x", x);
                this.CustomObjects.Add("y", y);
                this.CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyCaptainTask : IGameTask
        {
            public BuyCaptainTask(int def_id, int price_def_id, bool gem = false)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "captain");

                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("price_def_id", price_def_id);
                this.CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyItemsOfferTask : IGameTask
        {
            public BuyItemsOfferTask()
            {
                this.Time = (uint)TimeUtils.GetEpochTime();

                // todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyMoreSailorsTask : IGameTask
        {
            public BuyMoreSailorsTask(int amount)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "sailors");

                this.CustomObjects.Add("amount", amount);
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuySailorsTask : IGameTask
        {
            public BuySailorsTask(int amount)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "sailors");

                this.CustomObjects.Add("amount", amount);
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyShipsOfferShipTask : IGameTask
        {
            public BuyShipsOfferShipTask(int def_id, string type, int itemid, bool gem = false)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", type);
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("item_id", itemid);
                this.CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class BuyShipTask : IGameTask
        {
            public BuyShipTask(int def_id, bool gem = false)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("type", "ship");
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }

            public string Action => "purchase";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class CancelShipTask : IGameTask
        {
            public CancelShipTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "cancel_ship";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmAchievementTask : IGameTask
        {
            public ConfirmAchievementTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "confirm_achievement";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmBarrelTask : IGameTask
        {
            public ConfirmBarrelTask(int def_id, string type, int amount, int material_id, int player_lvl)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("material_id", material_id);
                this.CustomObjects.Add("amount", amount);
                this.CustomObjects.Add("player_level", player_lvl);
                this.CustomObjects.Add("type", type);
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "confirm_barrel";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmContractTask : IGameTask
        {
            public ConfirmContractTask(int def_id, int quest_id, ContractorDefinitions.Rewards rewards)
            {
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("quest_id", quest_id);
                this.Time = (uint)TimeUtils.GetEpochTime();
                if (rewards.Reward.Count > 0)
                {
                    var rew = new StringBuilder();
                    foreach (var reward in rewards.Reward)
                    {
                        rew.Append("\n<reward>");
                        rew.Append("\n<type>" + reward.Type + "</type>");
                        rew.Append("\n<id>" + reward.Id + "</id>");
                        rew.Append("\n<amount>" + reward.Amount + "</amount>");
                        rew.Append("\n</reward>");
                    }

                    this.CustomObjects.Add("rewards", rew.ToString());
                }
            }

            public string Action => "confirm_contractor_quest";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmGlobalContractorMilestoneTask : IGameTask
        {
            public ConfirmGlobalContractorMilestoneTask()
            {
                this.Time = (uint)TimeUtils.GetEpochTime();

                // todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "confirm_contractor_quest";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmGlobalContractorObjectiveTask : IGameTask
        {
            public ConfirmGlobalContractorObjectiveTask()
            {
                this.Time = (uint)TimeUtils.GetEpochTime();

                // todo reverse this
                throw new NotImplementedException();
            }

            public string Action => "confirm_global_contractor_quest";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmMuseumTask : IGameTask
        {
            public ConfirmMuseumTask(int turns)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("turns", turns);
            }

            public string Action => "confirm_museum";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmOutpostTask : IGameTask
        {
            public ConfirmOutpostTask(int def_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
            }

            public string Action => "confirm_outpost";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmSocialContractRewardsTask : IGameTask
        {
            public ConfirmSocialContractRewardsTask(int inst_ids)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_ids", inst_ids);
            }

            public string Action => "confirm_social_contract_rewards";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmSocialContractTask : IGameTask
        {
            public ConfirmSocialContractTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "confirm_social_contract";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmUpgradableTask : IGameTask
        {
            public ConfirmUpgradableTask(int def_id, int player_lvl)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("player_lvl", player_lvl);
            }

            public string Action => "confirm_upgradeable";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class ConfirmUpgradeableTask : IGameTask
        {
            public ConfirmUpgradeableTask(int def_id, int player_lvl)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("player_level", player_lvl);
            }

            public string Action => "confirm_upgradeable";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DeactivateShipTask : IGameTask
        {
            public DeactivateShipTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "deactivate_ship";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DeleteAllPlayerMaterialsTask : IGameTask
        {
            private DeleteAllPlayerMaterialsTask()
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
            }

            public string Action => "delete_all_player_materials";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DockShipSocialContractor : IGameTask
        {
            public DockShipSocialContractor(
                Ship ship,
                bool usecapt,
                int ship_capacity,
                int ship_capacity_used,
                int sailors,
                int sailors_used,
                int debug_loc_lvl,
                int uniqueid)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", ship.InstId);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
                this.CustomObjects.Add("debug_capacity", ship_capacity);
                this.CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                this.CustomObjects.Add("debug_sailors", sailors);
                this.CustomObjects.Add("debug_sailors_used", sailors_used);
                this.CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        this.CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        this.CustomObjects.Add(
                            "debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                this.CustomObjects.Add("debug_uniqueId", uniqueid);
            }

            public string Action => "dock_ship_social_contract";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DockShipTaskContractor : IGameTask
        {
            public DockShipTaskContractor(
                Ship ship,
                bool usecapt,
                int ship_capacity,
                int ship_capacity_used,
                int sailors,
                int sailors_used,
                int debug_loc_lvl,
                int contractid,
                int progress,
                int goalprogress,
                string objectivetypeid,
                int uniqueid)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", ship.InstId);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
                this.CustomObjects.Add("debug_capacity", ship_capacity);
                this.CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                this.CustomObjects.Add("debug_sailors", sailors);
                this.CustomObjects.Add("debug_sailors_used", sailors_used);
                this.CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        this.CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        this.CustomObjects.Add(
                            "debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                this.CustomObjects.Add("debug_uniqueId", contractid); // TODO: MESS EM AGAIN!!!
                this.CustomObjects.Add("debug_contractId", uniqueid);
                this.CustomObjects.Add("debug_progress", progress);
                this.CustomObjects.Add("debug_goalProgress", goalprogress);
                this.CustomObjects.Add("debug_objectiveTypeId", objectivetypeid);
            }

            public string Action => "dock_ship_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DockShipTaskGlobalContractor : IGameTask
        {
            public DockShipTaskGlobalContractor(
                Ship ship,
                bool usecapt,
                int ship_capacity,
                int ship_capacity_used,
                int sailors,
                int sailors_used,
                int debug_loc_lvl,
                int uniqueid)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", ship.InstId);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
                this.CustomObjects.Add("debug_capacity", ship_capacity);
                this.CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                this.CustomObjects.Add("debug_sailors", sailors);
                this.CustomObjects.Add("debug_sailors_used", sailors_used);
                this.CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        this.CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        this.CustomObjects.Add(
                            "debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                this.CustomObjects.Add("debug_uniqueId", uniqueid);
            }

            public string Action => "dock_ship_global_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class DockShipTaskOutPost : IGameTask
        {
            public DockShipTaskOutPost(
                Ship ship,
                bool usecapt,
                int ship_capacity,
                int ship_capacity_used,
                int sailors,
                int sailors_used,
                int debug_loc_lvl,
                int progress,
                int goalprogress,
                int uniqueid)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", ship.InstId);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
                this.CustomObjects.Add("debug_capacity", ship_capacity);
                this.CustomObjects.Add("debug_capacity_used", ship_capacity_used);
                this.CustomObjects.Add("debug_sailors", sailors);
                this.CustomObjects.Add("debug_sailors_used", sailors_used);
                this.CustomObjects.Add("debug_location_level", debug_loc_lvl);
                if (usecapt)
                {
                    if (ship.CaptainId != 0)
                    {
                        this.CustomObjects.Add("debug_captain_id", ship.CaptainId);
                        this.CustomObjects.Add(
                            "debug_captain_def_id",
                            Core.GlobalData.CaptainsNew.Where(n => n.InstId == ship.CaptainId).FirstOrDefault()
                                ?.InstId);
                    }
                }

                this.CustomObjects.Add("debug_uniqueId", uniqueid);

                this.CustomObjects.Add("debug_progress", progress);
                this.CustomObjects.Add("debug_goalProgress", goalprogress);
            }

            public string Action => "dock_ship_outpost";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class FinishBoatProducingTask : IGameTask
        {
            public FinishBoatProducingTask(int inst_id, int turns)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("turns", turns);
            }

            public string Action => "finish_boat_prod";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class FinishBuildingProducingTask : IGameTask
        {
            public FinishBuildingProducingTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_building_prod";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class FinishBuildingUpgradeTask : IGameTask
        {
            public FinishBuildingUpgradeTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_building_upg";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class FinishPersonalGuideTask : IGameTask
        {
            public FinishPersonalGuideTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "finish_personal_guide_task";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class GiftConfirmTask : IGameTask
        {
            public GiftConfirmTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "confirm_gift";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class HeartBeat : IGameTask
        {
            public HeartBeat()
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
            }

            public string Action => "heartbeat";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class LoadShipUpgradeableTask : IGameTask
        {
            public LoadShipUpgradeableTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "load_ship_upgradeable";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class LoadShipWreck : IGameTask
        {
            public LoadShipWreck(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "load_ship_wreck";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class OutpostSendShipTask : IGameTask
        {
            public OutpostSendShipTask(int inst_id, int outpost_id, int crew)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("outpost_id", outpost_id);
                this.CustomObjects.Add("crew", crew);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
            }

            public string Action => "send_ship_outpost";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class RemoveMaterialTask : IGameTask
        {
            public RemoveMaterialTask(int def_id, int amount)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("amount", amount);
            }

            public string Action => "remove_material";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class SendShipContractorTask : IGameTask
        {
            public SendShipContractorTask(int inst_id, int contractor_id, int material_id, int quest_id, int amount)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("contractor_id", contractor_id);
                this.CustomObjects.Add("material_id", material_id);
                this.CustomObjects.Add("amount", amount);
                this.CustomObjects.Add("quest_id", quest_id);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
            }

            public string Action => "send_ship_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class SendShipMarketplaceTask : IGameTask
        {
            public SendShipMarketplaceTask(int inst_id, int def_id, int dest_id, int amount)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("dest_id", dest_id);
                this.CustomObjects.Add("def_id", def_id);
                this.CustomObjects.Add("amount", amount);
            }

            public string Action => "send_ship_marketplace";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class SendShipUpgradeableTask : IGameTask
        {
            public SendShipUpgradeableTask(Ship ship, Upgradeable destination, int amount)
            {
                var destination_levels = Definitions.UpgrDef.Items.Item.First(n => n.DefId == destination.DefId).Levels
                    .Level.FirstOrDefault(n => n.Id == destination.Level);
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", ship.InstId);
                this.CustomObjects.Add("dest_id", destination.DefId);
                this.CustomObjects.Add("dest_amount", destination_levels.Amount);
                this.CustomObjects.Add(
                    "dest_material_koef",
                    destination_levels.MaterialKoef != 0 ? destination_levels.MaterialKoef : 1);
                this.CustomObjects.Add("dest_sailors", destination_levels.Sailors);
                this.CustomObjects.Add("amount", amount == 0 ? AutoShipUtils.GetCapacity(ship) : amount);
                this.CustomObjects.Add("player_level", Core.GlobalData.Level);
            }

            public string Action => "send_ship_upgradeable";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class SimpleDockShipContrator : IGameTask
        {
            public SimpleDockShipContrator(int id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("id", id);
            }

            public string Action => "dock_ship_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class StartBuildingProducingTask : IGameTask
        {
            public StartBuildingProducingTask(int inst_id, int prod_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("prod_id", prod_id);
            }

            public string Action => "start_building_prod";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class StartBuildingUpgradeTask : IGameTask
        {
            public StartBuildingUpgradeTask(
                int inst_id,
                int prod_id,
                int lvl,
                string debug_type,
                int debug_defId,
                int debug_tileX,
                int debug_tileY)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("payment_type", "standard");
                this.CustomObjects.Add("level", lvl);
                this.CustomObjects.Add("debug_type", debug_type);
                this.CustomObjects.Add("debug_defId", debug_defId);
                this.CustomObjects.Add("debug_tileX", debug_tileX);
                this.CustomObjects.Add("debug_tileY", debug_tileY);
            }

            public string Action => "start_building_upg";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class TakeFish : IGameTask
        {
            public TakeFish(Boat boat)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();

                // calculate turns :thinking:
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Definitions.BoatDef.Items.Item.First(n => n.DefId == 1).Levels.Level
                    .First(n => n.Id == Core.GlobalData.BoatLevel);
                var turns = Math.Round((TimeUtils.FixedUTCTime - started).TotalSeconds / b.TurnTime);
                this.CustomObjects.Add("inst_id", boat.InstId);
                if (turns > b.TurnCount)
                {
                    this.CustomObjects.Add("turns", b.TurnCount);
                }
                else
                {
                    this.CustomObjects.Add("turns", turns);
                }

                Core.GlobalData.Boats.First(n => n.InstId == boat.InstId).ProdStart = TimeUtils.GetEpochTime();
            }

            public string Action => "finish_boat_prod";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class UnloadShipContactorTask : IGameTask
        {
            public UnloadShipContactorTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class UnloadShipGlobalContractorTask : IGameTask
        {
            public UnloadShipGlobalContractorTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_global_contractor";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class UnloadShipOutpostTask : IGameTask
        {
            public UnloadShipOutpostTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_outpost";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class UnloadShipSocialContractTask : IGameTask
        {
            public UnloadShipSocialContractTask(int inst_id)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("inst_id", inst_id);
            }

            public string Action => "unload_ship_social_contract";

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }

        public class UnloadShipTask : IGameTask
        {
            public UnloadShipTask(
                int inst_id,
                int player_level,
                Enums.EObject eobj,
                int debug_capacity,
                int debug_capacity_used,
                int debug_sailors,
                int debug_sailors_used,
                int debug_locationlevel,
                Captain cpt = null,
                int debug_uniqueid = 1)
            {
                this.Time = (uint)TimeUtils.GetEpochTime();
                this.CustomObjects.Add("debug_capacity_used", debug_capacity_used);
                this.CustomObjects.Add("debug_sailors_used", debug_sailors_used);
                this.CustomObjects.Add("debug_capacity", debug_capacity);
                this.CustomObjects.Add("debug_uniqueid", debug_uniqueid);
                this.CustomObjects.Add("debug_sailors", debug_sailors);
                this.CustomObjects.Add("debug_location_level", debug_locationlevel);
                this.CustomObjects.Add("inst_id", inst_id);
                this.CustomObjects.Add("player_level", player_level);

                if (cpt != null)
                {
                    this.CustomObjects.Add("debug_captain_id", cpt.InstId);
                    this.CustomObjects.Add("debug_captain_def_id", cpt.DefId);
                }

                switch (eobj)
                {
                    case Enums.EObject.upgradeable:
                        this.Action = "unload_ship_upgradeable";
                        break;
                    case Enums.EObject.marketplace:
                        this.Action = "unload_ship_marketplace";
                        break;
                    case Enums.EObject.dealer:
                        this.Action = "unload_ship_dealer";
                        break;
                    case Enums.EObject.wreck:
                        this.Action = "unload_ship_wreck";
                        break;
                    case Enums.EObject.lost_treasure:
                        this.Action = "unload_ship_treasure";
                        break;
                }
            }

            public string Action { get; } = string.Empty;

            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();

            public uint Time { get; }
        }
    }
}