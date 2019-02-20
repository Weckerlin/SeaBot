// SeabotGUI
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
namespace SeaBotGUI
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Forms.Integration;

    using LiveCharts;
    using LiveCharts.Wpf;

    using Newtonsoft.Json;

    using SeaBotCore;
    using SeaBotCore.Config;
    using SeaBotCore.Data;

    using SeaBotGUI.Localization;

    using Telegram.Bot.Types.InlineQueryResults.Abstractions;

    #endregion

    public partial class Stats : Form
    {
       

        public Stats()
        {
           

            this.InitializeComponent();
            if (Core.Config.chartdata == ChartData.Resources)
            {
                this.radio_res.Checked = true;
            }
            else
            {
                this.radioButton1.Checked = true;
            }
            if (Core.Config.charttype == ChartType.Day)
            {
                this.radio_days.Checked = true;
            }
            else
            {
                this.radio_hours.Checked = true;
            }
            this.DrawChart();
            this.cartesianChart1.LegendLocation = LegendLocation.Right;
            this.cartesianChart1.Zoom = ZoomingOptions.X;
        }

    

        public void DrawChart()
        {
            var data = this.GetGlobalData(Core.Config.charttype).OrderBy(x => x.createtime).ToList();
            if (Core.Config.chartdata == ChartData.Resources)
            {
                this.cartesianChart1.Series = new SeriesCollection
                                                  {
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_COINS,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 1)
                                                                              .FirstOrDefault()?.Amount)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_FISH,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 3)
                                                                              .FirstOrDefault()?.Amount)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_STONE,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 5)
                                                                              .FirstOrDefault()?.Amount)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_IRON,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 6)
                                                                              .FirstOrDefault()?.Amount)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_WOOD,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 4)
                                                                              .FirstOrDefault()?.Amount)))
                                                          }
                                                  };
            }

            if (Core.Config.chartdata == ChartData.PlayerInfo)
            {
                this.cartesianChart1.Series = new SeriesCollection
                                                  {
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_LEVEL,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(n => Convert.ToDouble(n.data.Level)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_GEMS,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(
                                                                      n => Convert.ToDouble(
                                                                          n.data.Inventory.Where(a => a.Id == 2)
                                                                              .FirstOrDefault()?.Amount)))
                                                          },
                                                      new LineSeries
                                                          {
                                                              Title = PrivateLocal.STAT_SAILORS,
                                                              Values = new ChartValues<double>(
                                                                  data.Select(n => Convert.ToDouble(n.data.Sailors)))
                                                          }
                                                  };
            }

            this.cartesianChart1.AxisX.Clear();
            this.cartesianChart1.AxisX.Add(
                new Axis
                    {
                        Title = PrivateLocal.STAT_TIME, Labels = data.Select(n => n.createtime.ToString()).ToList()
                    });
            this.cartesianChart1.AxisY.Clear();
            this.cartesianChart1.AxisY.Add(new Axis { Title = PrivateLocal.STAT_AMOUNT });
        }

        public Dictionary<PlayerData, DateTime> LoadAllStats()
        {
            var l = new Dictionary<PlayerData, DateTime>();
            if (Directory.Exists("stats"))
            {
                var files = Directory.GetFiles("stats");
                foreach (var file in files)
                {
                    try
                    {
                        var datestr = file.Replace(@"stats\", string.Empty);
                        var date = DateTime.ParseExact(datestr, @"yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
                        l.Add(JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(file)), date);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return l;
        }

        private void Btn_zoomreset_Click(object sender, EventArgs e)
        {
            this.ClearZoom();
        }

        private void CartesianChart1_ChildChanged(object sender, ChildChangedEventArgs e)
        {
        }

        private void ClearZoom()
        {
            // to clear the current zoom/pan just set the axis limits to double.NaN
            this.cartesianChart1.AxisX[0].MinValue = double.NaN;
            this.cartesianChart1.AxisX[0].MaxValue = double.NaN;
            this.cartesianChart1.AxisY[0].MinValue = double.NaN;
            this.cartesianChart1.AxisY[0].MaxValue = double.NaN;
        }

        private List<GraphGlobalData> GetGlobalData(ChartType t)
        {
            var array = this.LoadAllStats();
            var l = new Dictionary<PlayerData, DateTime>();

            foreach (var gd in array)
            {
                var d = false;
                if (t == ChartType.Hour)
                {
                    d = l.All(n => (gd.Value - n.Value).Duration().TotalMinutes >= 59);
                }

                if (t == ChartType.Day)
                {
                    d = l.All(n => (gd.Value - n.Value).Duration().TotalDays >= 0.9);
                }

                if (d)
                {
                    l.Add(gd.Key, gd.Value);
                }
            }

            var list = new List<GraphGlobalData>();
            foreach (var entry in l)
            {
                list.Add(new GraphGlobalData(entry.Value, entry.Key));
            }

            return list.OrderByDescending(n=>n.createtime).Take(50).ToList();
        }

        private void Radio_days_CheckedChanged(object sender, EventArgs e)
        {
            // charttype = ChartType.Day;
        }

        private void Radio_hours_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.radio_days.Checked)
            {
               
                Core.Config.charttype = ChartType.Hour;
            }
            else
            {
                Core.Config.charttype = ChartType.Day;
            }

            this.DrawChart();
        }

        private void Radio_res_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radio_res.Checked)
            {
                Core.Config.chartdata = ChartData.Resources;
            }
            else
            {
                Core.Config.chartdata = ChartData.PlayerInfo;
            }

            this.DrawChart();
        }

        internal struct GraphGlobalData
        {
            internal PlayerData data;

            internal DateTime createtime;

            internal GraphGlobalData(DateTime time, PlayerData gdata)
            {
                this.data = gdata;
                this.createtime = time;
            }
        }
    }
}