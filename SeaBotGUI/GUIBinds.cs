// SeabotGUI
// Copyright (C) 2019 Weespin
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore;
using SeaBotCore.Data;
using SeaBotCore.Utils;
using Task = System.Threading.Tasks.Task;

namespace SeaBotGUI.GUIBinds
{
    public static class GUIBinds
    {
        public static class BuildingBinding
        {
            public static BindingList<Building> Buildings = new BindingList<Building>();

            public static BindingList<Building> GetBuildings()
            {
                var ret = new BindingList<Building>();
                if (Core.GlobalData == null)
                {
                    return ret;
                }

                if (Core.GlobalData.Buildings == null)
                {
                    return ret;
                }

                foreach (var building in Core.GlobalData.Buildings)
                {
                    var Building = new Building();
                    Building.ID = building.InstId;
                    Building.Name = Cache.GetBuildingDefenitions().Items.Item.Where(n => n.DefId == building.DefId)
                        .First().Name;
                    Building.Level = building.Level;
                    var producing = "";
                    if (building.ProdStart != 0)
                    {
                        var willbeproducedat = building.ProdStart + Cache.GetBuildingDefenitions().Items.Item
                                                   .Where(n => n.DefId == building.DefId).First().Levels.Level
                                                   .Where(n => n.Id == (long) building.Level).First().ProdOutputs
                                                   .ProdOutput[0].Time;
                        //lol xD

                        producing =
                            (DateTime.UtcNow - TimeUtils.FromUnixTime(willbeproducedat))
                            .ToString(@"hh\:mm\:ss");
                    }

                    Building.Producing = producing;
                    var upgrade = "";
                    if (building.UpgStart != 0)
                    {
                        var willbeproducedat = building.UpgStart + Cache.GetBuildingDefenitions().Items.Item
                                                   .Where(n => n.DefId == building.DefId).First().Levels.Level
                                                   .Where(n => n.Id == (long) building.Level + 1).First().UpgradeTime;

                        upgrade = (DateTime.UtcNow - TimeUtils.FromUnixTime(willbeproducedat))
                            .ToString(@"hh\:mm\:ss");
                    }

                    Building.Upgrade = upgrade;
                    ret.Add(Building);
                }

                return ret;
            }

            public class Building
            {
                public int ID { get; set; }
                public string Name { get; set; }
                public int Level { get; set; }
                public string Producing { get; set; }
                public string Upgrade { get; set; }
            }
        }
    }
}