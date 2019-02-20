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

    public static class Museum
    {
        public static void CollectMuseum()
        {
            // 12 = museum
            var museum = Core.LocalPlayer.Buildings.FirstOrDefault(n => n.DefId == 12);
            if (museum == null)
            {
                return;
            }

            var slot = Core.LocalPlayer.Slots.FirstOrDefault(n => n.Type == "museum_ship");
            if (slot == null)
            {
                return;
            }

            if (slot.SlotUsed == 0)
            {
                return;
            }

            var started = TimeUtils.FromUnixTime(slot.LastUsed);
            var b = LocalDefinitions.MuseumLevels.First(n => n.DefId == slot.Level);
            var turns = Math.Round((TimeUtils.FixedUTCTime - started).TotalSeconds / b.TurnTime);
            if (turns >= b.TurnCount)
            {
                slot.LastUsed = TimeUtils.GetEpochTime();
                museum.ProdStart = TimeUtils.GetEpochTime();
                Logger.Info(Localization.MUSEUM_COLLECT);
                Networking.AddTask(new Task.ConfirmMuseumTask(b.TurnCount));
            }
        }
    }
}