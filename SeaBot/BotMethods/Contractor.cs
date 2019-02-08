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
namespace SeaBotCore.BotMethods
{
    #region

    using System.Linq;

    using SeaBotCore.Cache;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;

    #endregion

    public static class Contractor
    {
        public static void UpgradeContractor()
        {
            return;

            ///BETA!!!
            for (var index = 0; index < Core.GlobalData.Contracts.Count; index++)
            {
                var upg = Core.GlobalData.Contracts[index];
                var def = Definitions.ConDef.Items.Item.FirstOrDefault(n => n.DefId == upg.DefId);
                var nexlv = def?.Quests.Quest.FirstOrDefault(n => n.Id == upg.QuestId + 1);
                var currquest = def?.Quests.Quest.FirstOrDefault(n => n.Id == upg.QuestId);
                if (nexlv == null || currquest == null)
                {
                    continue;
                }

                if (upg.Amount != 0 && upg.Progress >= upg.Amount && upg.QuestId < def.QuestCount)
                {
                    // upgrade ofc
                    Logger.Info(
                        "BETA CONTRACTOR " + Localization.UPGRADABLE_UPGRADED
                                           + LocalizationCache.GetNameFromLoc(def.NameLoc, def.Name));

                    // todo: add new local
                    Networking.AddTask(new Task.ConfirmContractTask(upg.DefId, upg.QuestId, currquest.Rewards));
                    upg.QuestId++;
                    upg.Progress = 0;
                    upg.Amount = nexlv.Amount;
                }
            }
        }
    }
}