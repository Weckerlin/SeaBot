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
using SeaBotCore.Data.Definitions;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public static class Museum
    {
        public static void CollectMuseum()
        {
            //12 = museum
            var museum = Core.GlobalData.Buildings.FirstOrDefault(n => n.DefId == 12);
            if (museum == null)
            {
                return;
            }

            var started = TimeUtils.FromUnixTime(museum.ProdStart);
            var b = Definitions.MuseumLvlDef.Items.Item.First(n => n.DefId == museum.Level);

            var turns = Math.Round((TimeUtils.FixedUTCTime - started).TotalSeconds / b.TurnTime);
            if (turns >= b.TurnCount)
            {
                Networking.AddTask(new Task.ConfirmMuseumTask((int) b.TurnCount));
            }
        }
    }
}