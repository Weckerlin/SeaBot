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

    using System;
    using System.Linq;

    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    public static class FishPier
    {
        public static void CollectFish()
        {
            var totalfish = 0;
            foreach (var boat in Core.GlobalData.Boats)
            {
                var started = TimeUtils.FromUnixTime(boat.ProdStart);
                var b = Definitions.BoatDef.Items.Item.FirstOrDefault(n => n.DefId == 1)?.Levels.Level
                    .FirstOrDefault(n => n.Id == Core.GlobalData.BoatLevel);
                if (b == null)
                {
                    continue;
                }

                var turns = Math.Round((TimeUtils.FixedUTCTime - started).TotalSeconds / b.TurnTime);
                if (turns > b.TurnCount / 2)
                {
                    totalfish += (int)(b.OutputAmount * turns);
                    Networking.AddTask(new Task.TakeFish(boat));
                }
            }

            if (totalfish > 0)
            {
                Logger.Info(string.Format(Localization.FISH_COLLECTING, totalfish));
            }
        }
    }
}