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
    using System.Collections.Generic;
    using System.Linq;

    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;

    #endregion

    public static class TimeUtils
    {
        public static object locker = new object();

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

        private static TimeSpan _timeOffset = new TimeSpan(0);

        public static DateTime FixedUTCTime => DateTime.UtcNow + _timeOffset;

        public static void CheckForTimeMismatch(long time)
        {
           
                var timedelay = (DateTime.UtcNow + _timeOffset - FromUnixTime(time)).TotalMinutes;

                // Logger.Logger.Info(FromUnixTime(time).ToString());
                if (timedelay >= 3 || timedelay <= -3)
                {
                    Logger.Warning(string.Format(Localization.TIMEUTIL_TIMEMISMATCH, timedelay));
                    _timeOffset = TimeSpan.FromMinutes(timedelay).Negate();
                    Logger.Debug("Time offset (min) = " + _timeOffset.TotalMinutes);

                    // Time is really delayed!
                }
            
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        public static EventsDefenitions.Event GetCurrentEvent()
        {
            var stl = new Dictionary<EventsDefenitions.Event, long>();
            foreach (var item in Definitions.EvntDef.Events.Item)
            {
                var x = GetEpochTime();
                if (x >= item.StartTime && x <= item.EndTime)
                {
                    return item;
                }
            }

            return Definitions.EvntDef.Events.Item.OrderBy(x => Math.Abs(x.EndTime - GetEpochTime())).First();
        }

        public static int GetEpochTime()
        {
            var utcDate = FixedUTCTime;
            var baseTicks = 621355968000000000;
            var tickResolution = 10000000;
            var epoch = (int)((utcDate.Ticks - baseTicks) / tickResolution);
            return epoch;
        }
    }
}