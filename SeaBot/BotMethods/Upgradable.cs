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

    public static class Upgradable
    {
        public static void UpgradeUpgradable()
        {
            for (var index = 0; index < Core.GlobalData.Upgradeables.Count; index++)
            {
                var upg = Core.GlobalData.Upgradeables[index];
                var def = Definitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == upg.DefId);
                var currentlvl = def?.Levels.Level.FirstOrDefault(n => n.Id == upg.Level);
                if (def == null || currentlvl == null)
                {
                    continue;
                }

                if (upg.Level >= def.MaxLevel)
                {
                    continue;
                }

                var nextlvl = def.Levels.Level.FirstOrDefault(n => n.Id == upg.Level + 1);
                if (nextlvl == null)
                {
                    continue;
                }

                if (upg.Progress >= currentlvl.Amount)
                {
                    // upgrade ofc
                    Core.GlobalData.Upgradeables[index].Level++;
                    Core.GlobalData.Upgradeables[index].Progress = 0;
                    Core.GlobalData.Upgradeables[index].Amount = nextlvl.Amount;
                    Core.GlobalData.Upgradeables[index].MaterialKoef = nextlvl.MaterialKoef;
                    Logger.Info(
                        Localization.UPGRADABLE_UPGRADED + LocalizationCache.GetNameFromLoc(def.NameLoc, def.Name));
                    Networking.AddTask(new Task.ConfirmUpgradeableTask(upg.DefId, Core.GlobalData.Level));
                }
            }
        }
    }
}