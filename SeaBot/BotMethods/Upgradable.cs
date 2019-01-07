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
                if (upg.Progress == currentlvl.Amount)
                {
                    //upgrade ofc
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Level++;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Progress = 0;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().Amount =(int) nextlvl.Amount;
                    Core.GlobalData.Upgradeables.Where(n => n.DefId == upg.DefId).First().MaterialKoef =
                        (int) nextlvl.MaterialKoef;
                    Logger.Logger.Info("Upgraded "+def.Name);
                    Networking.AddTask(new Task.ConfirmUpgradeableTask(upg.DefId.ToString(),Core.GlobalData.Level));
                }
            }
        }
    }
}
