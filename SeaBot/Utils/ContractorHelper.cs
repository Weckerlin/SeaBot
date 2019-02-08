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
namespace SeaBotCore.Utils
{
    #region

    using System;
    using System.Linq;

    using SeaBotCore.Data.Definitions;

    #endregion

    internal static class ContractorHelper
    {
        public static double Amount(this ContractorDefinitions.Reward reward, double Difficulty)
        {
            double ret;
            if (reward.Type != "material")
            {
                ret = Math.Floor(
                    Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First().Xp / 1000D
                    * reward.XpPct);
            }
            else
            {
                if (reward.Amount > 0)
                {
                    ret = reward.Amount;
                }
                else
                {
                    ret = Difficulty * reward.Bonus;
                }
            }

            if (reward.Round > 0)
            {
                ret = Math.Ceiling(ret / reward.Round) * reward.Round;
            }

            return ret;
        }

        public static double GetQuestDifficulty(this ContractorDefinitions.Quest quest)
        {
            var median = 0;
            if (quest.ObjectiveTypeId != "sailor")
            {
                median = Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First()
                    .MedianCrew;
            }
            else
            {
                median = Definitions.LevelUpDef.Items.Item.Where(n => n.DefId == Core.GlobalData.Level).First()
                    .MedianCapacity;
            }

            return Math.Ceiling(median / 100D * quest.Difficulty);
        }

        public static double InputAmount(this ContractorDefinitions.Quest quest)
        {
            // 261
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