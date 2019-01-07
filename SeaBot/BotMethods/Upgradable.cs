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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Data.Defenitions;

namespace SeaBotCore.BotMethods
{
    public static class Upgradable
    {
        public static void UpgradeUpgradable()
        {
            foreach (var upg in Core.GlobalData.Upgradeables)
            {
                var def = Defenitions.UpgrDef.Items.Item.Where(n => n.DefId == upg.DefId).First();
                var currentlvl = def.Levels.Level.Where(n => n.Id == upg.Level).First();
                if (upg.Level >= def.MaxLevel)
                {
                    continue;
                }

                var nextlvl = def.Levels.Level.Where(n => n.Id == upg.Level + 1).First();
                if (upg.Progress >= currentlvl.Amount)
                {
                    //upgrade ofc
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Level++;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Progress = 0;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Amount = (int) nextlvl.Amount;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().MaterialKoef =
                        (int) nextlvl.MaterialKoef;
                    Logger.Logger.Info("Upgraded " + def.Name);
                    Networking.AddTask(new Task.ConfirmUpgradeableTask(upg.DefId.ToString(), Core.GlobalData.Level));
                }
            }
        }
    }
}