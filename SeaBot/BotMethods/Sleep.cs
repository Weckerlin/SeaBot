using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public class Sleeping
    {
        public static void Sleep()
        {
            if (Core.Config.sleepevery != 0)
            {
                if ((DateTime.Now - Core.lastsleep).TotalMinutes >= (Core.Config.sleepeveryhrs
                        ? Core.Config.sleepevery * 60
                        : Core.Config.sleepevery))
                {

                    int sleeptimeinmin = 0;
                  
                    Core.lastsleep = DateTime.Now;
                    if (Core.Config.smartsleepenabled)
                    {
                        //10 min
                        int thresholdinmin = 20 ; 
                        List<int> DelayMinList = new List<int>();
                        foreach (var ship in Core.GlobalData.Ships.Where(n => n.Activated != 0))
                        {
                          
                            if (ship.Sent != 0)
                            {
                                try
                                {
                                    var shipdef = Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId);
                                    if (shipdef == null)
                                    {
                                        continue;
                                    }

                                    if (Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels == null)
                                    {
                                        continue;
                                    }

                                    if (Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels.Level
                                            .Count == 0)
                                    {
                                        continue;
                                    }

                                    var lvl = Defenitions.UpgrDef.Items.Item.FirstOrDefault(n => n.DefId == ship.TargetId)?.Levels.Level
                                        .FirstOrDefault(n => n.Id == ship.TargetLevel);
                                    if (lvl == null)
                                    {
                                        continue;
                                    }

                                   
                                    var willatportattime = ship.Sent + lvl.TravelTime;
                                    //lol xD 
                                    if (!((DateTime.UtcNow - TimeUtils.FromUnixTime(willatportattime)).TotalSeconds >
                                          0))
                                    {
                                        DelayMinList.Add((int) Math.Ceiling(
                                            (DateTime.UtcNow - TimeUtils.FromUnixTime(willatportattime)).TotalMinutes));
                                    }
                                    
                                }
                                catch (Exception e)
                                {
                                    Logger.Logger.Debug($"Again fucking exception -> Ship def id = {ship.DefId} Destination = {ship.TargetId} Level = {ship.TargetLevel}");

                                }
                            }

                        }
                        foreach (var building in Core.GlobalData.Buildings)
                        {

                          
                            if (building.ProdStart != 0)
                            {
                                var willbeproducedat = building.ProdStart + Cache.GetBuildingDefenitions().Items.Item.First(n => n.DefId == building.DefId).Levels.Level.First(n => n.Id == (long)building.Level).ProdOutputs
                                                           .ProdOutput[0].Time;
                                //lol xD

                                DelayMinList.Add((int)Math.Ceiling(
                                    (DateTime.UtcNow - TimeUtils.FromUnixTime(willbeproducedat)).TotalMinutes));
                            }

                        
                            if (building.UpgStart != 0)
                            {
                                var willbeproducedat = building.UpgStart + Cache.GetBuildingDefenitions().Items.Item
                                                           .Where(n => n.DefId == building.DefId).First().Levels.Level
                                                           .Where(n => n.Id == (long)building.Level + 1).First().UpgradeTime;


                                DelayMinList.Add((int)Math.Ceiling(
                                    (DateTime.UtcNow - TimeUtils.FromUnixTime(willbeproducedat)).TotalMinutes));
                            }
                        }
                        //Find center
                        var mostlikely = DelayMinList.Where(n=>n> thresholdinmin).GroupBy(i => i).OrderByDescending(grp => grp.Count())
                            .Select(grp => grp.Key).First();
                        var avg = DelayMinList.Average();
                        if (mostlikely > avg)
                        {
                            if (mostlikely > thresholdinmin)
                            {
                                sleeptimeinmin = (int)mostlikely;
                            }
                            else
                            {

                                sleeptimeinmin = thresholdinmin;
                            }
                        }
                        else
                        {
                            if (avg > thresholdinmin)
                            {
                                sleeptimeinmin = (int)avg ;
                            }
                            else
                            {
                                sleeptimeinmin = (int)thresholdinmin;
                            }
                        }
                       

                    }
                    else
                    {
                        sleeptimeinmin = Core.Config.sleepforhrs
                            ? Core.Config.sleepfor * 60
                            : Core.Config.sleepfor;
                    }
                  new  System.Threading.Tasks.Task(() =>
                  {
                      SeaBotCore.Core.StopBot();
                      Logger.Logger.Info($"Started sleepin'. Waking up after {sleeptimeinmin} min.");
                      Thread.Sleep(sleeptimeinmin*1000);
                      SeaBotCore.Core.StartBot();
                  }).Start();
                        //StartSleeping
                        
                }
            }
        }
    }
}
