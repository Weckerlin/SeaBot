// SeaBotCore
// Copyright (C) 2018 Weespin
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
using System.Linq;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Utils;

namespace SeaBotCore
{
    public class Task
    {
        public interface IGameTask
        {
            string Action { get; }
            uint Time { get; }
            Dictionary<string, object> CustomObjects { get; }
        }

        public class TakeFish : IGameTask
        {
            public string Action => "finish_boat_prod";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public TakeFish(Boat boat)
            {
                _time = (uint) TimeUtils.GetEpochTime();
                //calculate turns :thinking:
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Defenitions.BoatDef.Items.Item.First(n => n.DefId==1).Levels.Level.First(n => n.Id == Core.GolobalData.BoatLevel);
                var turns = Math.Round((DateTime.UtcNow - started).TotalSeconds / b.TurnTime);
                CustomObjects.Add("inst_id", boat.InstId);
                if (turns > b.TurnCount)
                {
                    CustomObjects.Add("turns", b.TurnCount);
                }
                else
                {
                    CustomObjects.Add("turns", turns);
                }

                Core.GolobalData.Boats.First(n => n.InstId == boat.InstId).ProdStart =
                    TimeUtils.GetEpochTime();
            }
        }

        public class ActivateEvent : IGameTask
        {
            public string Action => "activate_event";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ActivateEvent(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class ActivateItemsOfferTask : IGameTask
        {
            public string Action => "activate_items_offer";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ActivateItemsOfferTask(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class ActivateMerchantOfferTask : IGameTask
        {
            public string Action => "activate_merchant_offer";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ActivateMerchantOfferTask(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class ActivatePackTask : IGameTask
        {
            public string Action => "activate_pack";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ActivatePackTask(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class ActivateShipTask : IGameTask
        {
            public string Action => "activate_ship";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ActivateShipTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class AssignCaptainTask : IGameTask
        {
            public string Action => "assign_ship_captain";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public AssignCaptainTask(string ship_inst_id, string captain_inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("ship_inst_id", ship_inst_id);
                CustomObjects.Add("captain_inst_id", captain_inst_id);
            }
        }

        public class BuyBoatTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyBoatTask(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "boat");
                CustomObjects.Add("payment_type", "standard");
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class BuyBuildareaTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyBuildareaTask(string def_id, string x, string y, bool gem = false)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "buildarea");
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("x", x);
                CustomObjects.Add("y", y);
                CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }
        }

        public class BuyBuildingTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyBuildingTask(string def_id, string x, string y, bool gem = false)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "building");

                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("x", x);
                CustomObjects.Add("y", y);
                CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }
        }

        public class BuyCaptainTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyCaptainTask(string def_id, string price_def_id, bool gem = false)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "captain");

                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("price_def_id", price_def_id);
                CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }
        }

        public class BuyItemsOfferTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyItemsOfferTask()
            {
                _time = (uint)TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }
        }

        public class BuyMoreSailorsTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyMoreSailorsTask(int amount)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "sailors");

                CustomObjects.Add("amount", amount);
            }
        }

        public class BuySailorsTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuySailorsTask(int amount)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "sailors");

                CustomObjects.Add("amount", amount);
            }
        }

        public class BuyShipTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyShipTask(string def_id, bool gem = false)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", "ship");
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }
        }

        public class BuyShipsOfferShipTask : IGameTask
        {
            public string Action => "purchase";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public BuyShipsOfferShipTask(string def_id, string type, string itemid, bool gem = false)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("type", type);
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("item_id", itemid);
                CustomObjects.Add("payment_type", gem ? "gem" : "standard");
            }
        }

        public class CancelShipTask : IGameTask
        {
            public string Action => "cancel_ship";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public CancelShipTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class ConfirmAchievementTask : IGameTask
        {
            public string Action => "confirm_achievement";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmAchievementTask(string def_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
            }
        }

        public class ConfirmBarrelTask : IGameTask
        {
            public string Action => "confirm_barrel";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmBarrelTask(string def_id, string type, string amount, string material_id, string player_lvl)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("material_id", material_id);
                CustomObjects.Add("amount", amount);
                CustomObjects.Add("player_level", player_lvl);
                CustomObjects.Add("type", type);
                CustomObjects.Add("def_id", def_id);
                
               
             

            }
        }

        public class ConfirmContractTask : IGameTask
        {
            public string Action => "confirm_contractor_quest";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmContractTask()
            {
                _time = (uint)TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }
        }

        public class ConfirmGlobalContractorMilestoneTask : IGameTask
        {
            public string Action => "confirm_contractor_quest";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmGlobalContractorMilestoneTask()
            {
                _time = (uint)TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }
        }

        public class ConfirmGlobalContractorObjectiveTask : IGameTask
        {
            public string Action => "confirm_global_contractor_quest";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmGlobalContractorObjectiveTask()
            {
                _time = (uint)TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }
        }

        public class ConfirmMuseumTask : IGameTask
        {
            public string Action => "confirm_museum";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmMuseumTask(int turns)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("turns", turns);
            }
        }

        public class ConfirmSocialContractRewardsTask : IGameTask
        {
            public string Action => "confirm_social_contract_rewards";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmSocialContractRewardsTask(string inst_ids)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_ids", inst_ids);
            }
        }

        public class ConfirmSocialContractTask : IGameTask
        {
            public string Action => "confirm_social_contract";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmSocialContractTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class ConfirmUpgradeableTask : IGameTask
        {
            public string Action => "confirm_upgradeable";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public ConfirmUpgradeableTask(string def_id, int player_lvl)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("def_id", def_id);
                CustomObjects.Add("player_level", player_lvl);
            }
        }

        public class DeactivateShipTask : IGameTask
        {
            public string Action => "deactivate_ship";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public DeactivateShipTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class DeleteAllPlayerMaterialsTask : IGameTask
        {
            public string Action => "delete_all_player_materials";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;

            DeleteAllPlayerMaterialsTask()
            {
                _time = (uint)TimeUtils.GetEpochTime();
            }
        }

        public class DockShipTask : IGameTask
        {
            public string Action => "confirm_global_contractor_quest";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public DockShipTask()
            {
                _time = (uint) TimeUtils.GetEpochTime();
                //todo reverse this
                throw new NotImplementedException();
            }
        }

        public class FinishBoatProductionTask : IGameTask
        {
            public string Action => "finish_boat_prod";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public FinishBoatProductionTask(string inst_id, int turns)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("turns", turns);
            }
        }

        public class FinishBuildingProductionTask : IGameTask
        {
            public string Action => "finish_building_prod";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public FinishBuildingProductionTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class FinishBuildingUpgradeTask : IGameTask
        {
            public string Action => "finish_building_upg";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public FinishBuildingUpgradeTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class FinishPersonalGuideTask : IGameTask
        {
            public string Action => "finish_personal_guide_task";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public FinishPersonalGuideTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class GiftConfirmTask : IGameTask
        {
            public string Action => "confirm_gift";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public GiftConfirmTask(string inst_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
            }
        }

        public class StartBuildingProductionTask : IGameTask
        {
            public string Action => "start_building_prod";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public StartBuildingProductionTask(string inst_id, string prod_id)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("prod_id", prod_id);
            }
        }
        public class StartBuildingUpgradeTask : IGameTask
        {
            public string Action => "start_building_upg";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;
            public StartBuildingUpgradeTask(string inst_id, string prod_id,int lvl,string debug_type,string debug_defId,string debug_tileX,string debug_tileY)
            {
                _time = (uint)TimeUtils.GetEpochTime();
                CustomObjects.Add("inst_id", inst_id);
                CustomObjects.Add("payment_type", "standard");
                CustomObjects.Add("level",lvl);
                CustomObjects.Add("debug_type",debug_type);
                CustomObjects.Add("debug_defId",debug_defId);
                CustomObjects.Add("debug_tileX",debug_tileX);
                CustomObjects.Add("debug_tileY",debug_tileY);
            }
        }

        public class HeartBeat : IGameTask
        {
            public string Action => "heartbeat";
            public uint Time => _time;
            public Dictionary<string, object> CustomObjects { get; } = new Dictionary<string, object>();
            private uint _time;

            public HeartBeat()
            {
                _time = (uint)TimeUtils.GetEpochTime();
            }
        }
    }
}