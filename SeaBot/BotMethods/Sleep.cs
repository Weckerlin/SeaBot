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
using System.Threading;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public class Sleeping
    {
        public static void Sleep()
        {
            if (Core.Config.sleepevery != 0)
                if ((DateTime.Now - Core.lastsleep).TotalMinutes >= (Core.Config.sleepeveryhrs
                        ? Core.Config.sleepevery * 60
                        : Core.Config.sleepevery))
                {
                    var sleeptimeinmin = 0;

                    Core.lastsleep = DateTime.Now;
                    if (Core.Config.smartsleepenabled)
                    {
                        //10 min
                        var thresholdinmin = 20;
                        var DelayMinList = new List<int>();
                        foreach (var ship in Core.GlobalData.Ships.Where(n => n.Activated != 0))
                            if (ship.Sent != 0)
                                try
                                {
                                    var shipdef =
                                        Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
                                    if (shipdef == null) continue;

                                    if (Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                                            ?.Levels == null)
                                        continue;

                                    if (Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)
                                            ?.Levels.Level
                                            .Count == 0)
                                        continue;

                                    var lvl = Defenitions.UpgrDef.Items.Item
                                        .FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels.Level
                                        .FirstOrDefault(n => n.Id == ship.TargetLevel);
                                    if (lvl == null) continue;


                                    var willatportattime = ship.Sent + lvl.TravelTime;
                                    //lol xD 
                                    if (!((DateTime.UtcNow - TimeUtils.FromUnixTime(willatportattime)).TotalSeconds >
                                          0))
                                        DelayMinList.Add((int) Math.Ceiling(
                                            (TimeUtils.FromUnixTime(willatportattime) - DateTime.UtcNow).TotalMinutes));
                                }
                                catch (Exception e)
                                {
                                    Logger.Logger.Debug(
                                        $"Again fucking exception -> Ship def id = {ship.DefId} Destination = {ship.TargetId} Level = {ship.TargetLevel}");
                                }

                        foreach (var building in Core.GlobalData.Buildings)
                        {
                            if (building.ProdStart != 0)
                            {
                                var willbeproducedat =
                                    building.ProdStart + Cache.GetBuildingDefenitions().Items.Item
                                        .First(n => n.DefId == building.DefId).Levels.Level
                                        .First(n => n.Id == (long) building.Level).ProdOutputs
                                        .ProdOutput[0].Time;
                                //lol xD

                                DelayMinList.Add((int) Math.Ceiling(
                                    (TimeUtils.FromUnixTime(willbeproducedat) - DateTime.UtcNow).TotalMinutes));
                            }


                            if (building.UpgStart != 0)
                            {
                                var willbeproducedat = building.UpgStart + Cache.GetBuildingDefenitions().Items.Item
                                                           .Where(n => n.DefId == building.DefId).First().Levels.Level
                                                           .Where(n => n.Id == (long) building.Level + 1).First()
                                                           .UpgradeTime;


                                DelayMinList.Add((int) Math.Ceiling(
                                    (TimeUtils.FromUnixTime(willbeproducedat) - DateTime.UtcNow).TotalMinutes));
                            }
                        }

                        //Find center
                        var a = DelayMinList.Where(n => n > thresholdinmin).GroupBy(i => i);
                        var b =
                            a.OrderByDescending(grp => grp.Count());
                        var mostlikely = b
                            .Select(grp => grp.Key).FirstOrDefault();
                        var avg = DelayMinList.Average();
                        if (mostlikely > avg)
                        {
                            if (mostlikely > thresholdinmin)
                                sleeptimeinmin = mostlikely;
                            else
                                sleeptimeinmin = thresholdinmin;
                        }
                        else
                        {
                            if (avg > thresholdinmin)
                                sleeptimeinmin = (int) avg;
                            else
                                sleeptimeinmin = thresholdinmin;
                        }
                    }
                    else
                    {
                        sleeptimeinmin = Core.Config.sleepforhrs
                            ? Core.Config.sleepfor * 60
                            : Core.Config.sleepfor;
                    }

                    new System.Threading.Tasks.Task(() =>
                    {
                        Core.StopBot();
                        Logger.Logger.Info($"Started sleepin'. Waking up after {sleeptimeinmin} min.");
                        Thread.Sleep(sleeptimeinmin * 1000);
                        Core.StartBot();
                    }).Start();
                    //StartSleeping
                }
        }
    }
}