using SeaBotCore.Cache;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Localizaion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBotCore.BotMethods
{
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
                if(nexlv==null||currquest==null)
                {
                    continue;
                }
                if (upg.Amount!=0&&upg.Progress >= upg.Amount&&upg.QuestId<def.QuestCount)
                {
                    //upgrade ofc
               
                  
                    Logger.Logger.Info("BETA CONTRACTOR "+Localization.UPGRADABLE_UPGRADED +
                                       LocalizationCache.GetNameFromLoc(def.NameLoc, def.Name));
                    //todo: add new local
                    Networking.AddTask(new Task.ConfirmContractTask(upg.DefId,upg.QuestId,currquest.Rewards));
                    upg.QuestId++;
                    upg.Progress = 0;
                    upg.Amount = (int)nexlv.Amount;
                }
            }
        }
    }
}
