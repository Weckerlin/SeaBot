using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Data.Definitions;

namespace SeaBotCore.Utils
{
  static  class ContractorHelper
    {
        public static double GetQuestDifficulty(this ContractorDefinitions.Quest quest)
        {
            
            var median = 0;
            if (quest.ObjectiveTypeId != "sailor")
            {
                 median = (int )Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First()
                    .MedianCrew;
            }
            else
            {
                median = (int)Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First()
                    .MedianCapacity;
            }
            return Math.Ceiling(median / 100D * quest.Difficulty);
        }

        public static double Amount(this ContractorDefinitions.Reward reward, double Difficulty)
        {
            double ret;
            if (reward.Type != "material")
            {
                ret = Math.Floor(Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First()
                          .Xp / 1000D * reward.XpPct);
            }
            else
            {
                if (reward.Amount > 0)
                {
                    ret =  reward.Amount;
                }
                else
                {
                    ret = Difficulty *  reward.Bonus;
                }
            }

            if (reward.Round >0)
            {
                ret =  Math.Ceiling(ret/ reward.Round)*reward.Round;
            }

            return ret;
        }
        public static double InputAmount(this ContractorDefinitions.Quest quest)
        {
            //261
            var ret = 0D;
            var diff = quest.GetQuestDifficulty();
            {
                if (quest.Amount > 0)
                {
                    ret = quest.Amount;
                }
                else
                {
                    ret = diff * quest.Bonus;
                }
            }

            if (quest.Round > 0)
            {
                ret = Math.Ceiling(ret / quest.Round) * quest.Round;
            }


            return ret;
        }
    }
}
