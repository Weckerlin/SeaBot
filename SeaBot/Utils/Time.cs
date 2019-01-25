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
using SeaBotCore.Data.Definitions;

namespace SeaBotCore.Utils
{
    public static class TimeUtils
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        public static DateTime FromUnixTime(long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        public static int GetEpochTime()
        {
            var utcDate = DateTime.Now.ToUniversalTime();
            var baseTicks = 621355968000000000;
            var tickResolution = 10000000;
            var epoch = (int) ((utcDate.Ticks - baseTicks) / tickResolution);
            return epoch;
        }

        public static EventsDefenitions.Item GetCurrentEvent()
        {
            var stl = new Dictionary<EventsDefenitions.Item, long>();
            foreach (var item in Defenitions.EvntDef.Items.Item)
            {
                var x = GetEpochTime();
                if (x >= item.StartTime && x <= item.EndTime)
                {
                    return item;
                }
            }

            return Defenitions.EvntDef.Items.Item.OrderBy(x => Math.Abs(x.EndTime - GetEpochTime())).First();
        }
    }
}