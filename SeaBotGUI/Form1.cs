// SeabotGUI
// Copyright (C) 2018 Weespin
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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SeaBotCore;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using Task = SeaBotCore.Task;

namespace SeaBotGUI
{
    public partial class Form1 : Form
    {
        public static Thread BotThread;
        public static Config _config = new Config();
        public static Thread BarrelThread; 
        public Form1()
        {
            InitializeComponent();
            ConfigSer.Load();
            this.MaximizeBox = false;
            textBox2.Text = _config.server_token;
            checkBox1.Checked = _config.debug;
            Core.Debug = _config.debug;
            chk_autofish.Checked = _config.collectfish;
            chk_prodfact.Checked = _config.prodfactory;
            chk_collectmat.Checked = _config.collectfactory;
            num_ironlimit.Value = _config.ironlimit;
            num_woodlimit.Value = _config.woodlimit;
            num_stonelimit.Value = _config.stonelimit;
            
            Logger.Event.LogMessageChat.OnLogMessage += LogMessageChat_OnLogMessage;
            linkLabel1.Links.Add(new LinkLabel.Link(){LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token"});
            //Check for cache
        }

        private void Inventory_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            FormateResources(SeaBotCore.Core.GolobalData);
        }

        private void LogMessageChat_OnLogMessage(Logger.Message e)
        {
            if (InvokeRequired)
            {
                MethodInvoker inv = delegate
                {
                    RichTextBoxExtensions.AppendText(richTextBox1, e.message + "\n", e.color);
                };
                richTextBox1.BeginInvoke(inv);
            }
            else
            {
                RichTextBoxExtensions.AppendText(richTextBox1, e.message + "\n", e.color);
            }
        }

        public static class RichTextBoxExtensions
        {
            public static void AppendText(RichTextBox box, string text, Color color)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;

                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
                box.ScrollToCaret();
            }
        }

        public void FormateResources(GlobalData data)
        {
            //Gold: Fish: Iron:
            //Gems: Wood: Rock:

            if (textBox1.InvokeRequired)
            {
                MethodInvoker inv = delegate
                {
                    if (data.Inventory != null)
                    {
                        StringBuilder txt = new StringBuilder();
                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Coins) != null)
                        {
                            txt.Append($"Gold: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Coins).Amount}");

                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Fish) != null)
                        {
                            txt.Append($" Fish: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Fish).Amount} ");
                        }
                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Iron) != null)
                        {
                            txt.Append($" Iron: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Iron).Amount} ");
                        }

                        txt.Append(Environment.NewLine);
                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Gems) != null)
                        {
                            txt.Append($" Gems: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Gems).Amount} ");

                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Wood) != null)
                        {
                            txt.Append($" Wood: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Wood).Amount}");
                        }
                        if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Stone) != null)
                        {
                            txt.Append($" Stone: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Stone).Amount}");
                        }

                        textBox1.Text = txt.ToString();
                    }
                };
                textBox1.Invoke(inv);
            }
            else
            {
                if (data.Inventory != null)
                {
                    StringBuilder txt  = new StringBuilder();
                    if (data.Inventory.FirstOrDefault(n => n.Id == (int) Enums.EMaterial.Coins) != null)
                    {
                        txt .Append($"Gold: {data.Inventory.First(n => n.Id == (int) Enums.EMaterial.Coins).Amount}");
                    
                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == (int) Enums.EMaterial.Fish) != null)
                    {
                        txt.Append($" Fish: {data.Inventory.First(n => n.Id == (int) Enums.EMaterial.Fish).Amount} ");
                    }
                    if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Iron) != null)
                    {
                        txt .Append( $" Iron: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Iron).Amount} ");
                    }

                    txt.Append(Environment.NewLine);
                    if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Gems) != null)
                    {
                        txt.Append($" Gems: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Gems).Amount} ");

                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Wood) != null)
                    {
                        txt .Append( $" Wood: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Wood).Amount}");
                    }
                    if (data.Inventory.FirstOrDefault(n => n.Id == (int)Enums.EMaterial.Stone) != null)
                    {
                        txt .Append( $" Stone: {data.Inventory.First(n => n.Id == (int)Enums.EMaterial.Stone).Amount}");
                    }

                    textBox1.Text = txt.ToString();
                }
            }

            var a = new List<ListViewItem>();
            foreach (var dataa in data.Inventory.Where(n =>
                n.Id != (int) Enums.EMaterial.Coins && n.Id != (int) Enums.EMaterial.Iron &&
                n.Id != (int) Enums.EMaterial.Gems && n.Id != (int) Enums.EMaterial.Wood &&
                n.Id != (int) Enums.EMaterial.Stone && n.Id != (int) Enums.EMaterial.Fish))
            {
                string[] row = {((Enums.EMaterial) dataa.Id).ToString(), dataa.Amount.ToString()};
                a.Add(new ListViewItem(row));
            }

            if (listView1.InvokeRequired)
            {
                MethodInvoker inv = delegate
                {
                    listView1.Items.Clear();
                    foreach (var list in a)
                    {
                        listView1.Items.Add(list);
                    }
                };
                listView1.BeginInvoke(inv);
            }
            else
            {
                listView1.Items.Clear();
                foreach (var list in a)
                {
                    listView1.Items.Add(list);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_config.server_token))
            {
                MessageBox.Show("Empty server_token\nPlease fill server token in Settings tab", "Error");
                return;
            }
            button3.Enabled = true;
            button2.Enabled = false;
            Core.ServerToken = textBox2.Text;
            Networking.Login();
            FormateResources(Core.GolobalData);
           Core.GolobalData.Inventory.CollectionChanged += Inventory_CollectionChanged;
            Core.GolobalData.Inventory.ItemPropertyChanged += Inventory_ItemPropertyChanged;
            BarrelThread = new Thread(BarrelVoid){IsBackground = true};
            BarrelThread.Start();
            BotThread = new Thread(BotVoid);
            BotThread.IsBackground = true;
            BotThread.Start();
            var a = Defenitions.BuildingDef;
        }

        private void Inventory_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            FormateResources(Core.GolobalData);
        }

        void BarrelVoid()
        {
            while (true)
            {
                Thread.Sleep(10*1000);

                if (chk_barrelhack.Checked)
                {
                    var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                        .Where(n => n.DefId == 21).First());
                    Logger.Info(
                        $"Barrel! Collecting {bar.Amount} {((Enums.EMaterial)bar.Definition.Id).ToString()}");
                    Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(), bar.Definition.Id.ToString(), Core.GolobalData.Level.ToString()));
                }
            }
        }
        void BotVoid()
        {
            while (true)
            {
                
                if (chk_autofish.Checked)
                {
                    var totalfish = 0;
                    foreach (var boat in Core.GolobalData.Boats)
                    {
                        var started = TimeUtils.FromUnixTime(boat.ProdStart);
                        var b = XmlProcessor.GetBoatLevels().level.First(n => n.id ==Core.GolobalData.BoatLevel);
                        var turns = Math.Round((DateTime.UtcNow - started).TotalSeconds / b.turn_time);
                        if (turns > 5)
                        {
                            totalfish += (int) (b.output_amount * turns);
                            Networking.AddTask(new Task.TakeFish(boat));
                        }
                    }

                    if (totalfish > 0)
                    {
                        Logger.Info($"Collecting {totalfish} fish");
                    }
                }


                bool cltd = false;
                foreach (var data in Core.GolobalData.Buildings)
                {
                    if (chk_collectmat.Checked)
                    {
                        if (data.UpgStart == 0 && data.ProdStart != 0)
                        {
                            var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                            if (def.Type != "factory")
                            {
                                continue;
                            }

                            var defs = def.Levels.Level.First(n => n.Id == data.Level);
                            var started = TimeUtils.FromUnixTime(data.ProdStart);
                            if ((DateTime.UtcNow - started).TotalSeconds > defs.ProdOutputs.ProdOutput[0].Time)
                            {
                                Logger.Info(
                                    $"Сollecting {defs.ProdOutputs.ProdOutput[0].Amount} {((Enums.EMaterial) defs.ProdOutputs.ProdOutput[0].MaterialId).ToString()}");

                                Networking.AddTask(new Task.FinishBuildingProductionTask(data.InstId.ToString()));
                                cltd = true;
                                data.ProdStart = 0;
                            }
                        }
                    }
                }

                if (!cltd)
                {
                    foreach (var data in Core.GolobalData.Buildings)
                    {
                        if (chk_prodfact.Checked)
                        {
                            if (data.UpgStart == 0 && data.ProdStart == 0)
                            {
                                var def = Defenitions.BuildingDef.Items.Item.First(n => n.DefId == data.DefId);
                                if (def.Type != "factory")
                                {
                                    continue;
                                }

                                //lets start?
                                //DO WE HAVE ENOUGH RESOURCES
                                var needed = def.Levels.Level.First(n => n.Id == data.Level);
                                var input = needed.ProdOutputs.ProdOutput[0].Inputs.Input;
                                var can = false;
                                foreach (var material in input)
                                {
                                    if (Core.GolobalData.Inventory.Any(n => n.Id == material.Id))
                                    {
                                        var mat = Core.GolobalData.Inventory
                                            .First(n => n.Id == material.Id);
                                        if (mat.Amount > material.Amount)
                                        {
                                            can = true;
                                            mat.Amount -= (int) material.Amount;
                                        }
                                        else
                                        {
                                            can = false;
                                        }
                                    }
                                }

                                if (can)
                                {
                                    //
                                    var output = needed.ProdOutputs.ProdOutput[0].MaterialId;
                                    switch ((Enums.EMaterial) output)
                                    {
                                        case Enums.EMaterial.Wood:
                                            var amount =
                                                Core.GolobalData.Inventory.Where(n =>
                                                    n.Id == (int) Enums.EMaterial.Wood).First();
                                            if (amount.Amount > (int) num_woodlimit.Value)
                                            {
                                                if ((int) num_woodlimit.Value == 0)
                                                {
                                                    can = true;
                                                    break;
                                                }

                                                can = false;
                                            }

                                            break;
                                        case Enums.EMaterial.Iron:
                                            amount =
                                                Core.GolobalData.Inventory.Where(n =>
                                                    n.Id == (int) Enums.EMaterial.Iron).First();
                                            if (amount.Amount > (int) num_ironlimit.Value)
                                            {
                                                if ((int) num_ironlimit.Value == 0)
                                                {
                                                    can = true;
                                                    break;
                                                }

                                                can = false;
                                            }

                                            break;
                                        case Enums.EMaterial.Stone:
                                            amount =
                                               Core.GolobalData.Inventory.Where(n =>
                                                    n.Id == (int) Enums.EMaterial.Stone).First();
                                            if (amount.Amount > (int) num_stonelimit.Value)
                                            {
                                                if ((int) num_stonelimit.Value == 0)
                                                {
                                                    can = true;
                                                    break;
                                                }

                                                can = false;
                                            }


                                            break;
                                    }
                                }

                                if (can)
                                {
                                    Logger.Info(
                                        $"Started producing {((Enums.EMaterial) needed.ProdOutputs.ProdOutput[0].MaterialId).ToString()}");
                                    Networking.AddTask(new Task.StartBuildingProductionTask(data.InstId.ToString(),
                                        data.ProdId.ToString()));
                                    data.ProdStart = TimeUtils.GetEpochTime();
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(60 * 1000);
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _config.debug = checkBox1.Checked;
            Core.Debug = checkBox1.Checked;
            ConfigSer.Save();
        }

        private void chk_autofish_CheckedChanged(object sender, EventArgs e)
        {
            _config.collectfish = chk_autofish.Checked;
            ConfigSer.Save();
        }

        private void chk_prodfact_CheckedChanged(object sender, EventArgs e)
        {
            _config.prodfactory = chk_prodfact.Checked;
            ConfigSer.Save();
        }

        private void chk_collectmat_CheckedChanged(object sender, EventArgs e)
        {
            _config.collectfactory = chk_collectmat.Checked;
            ConfigSer.Save();
        }


        private void num_woodlimit_Leave(object sender, EventArgs e)
        {
            _config.woodlimit = (int) num_woodlimit.Value;
            ConfigSer.Save();
        }


        private void num_ironlimit_Leave(object sender, EventArgs e)
        {
            _config.ironlimit = (int) num_ironlimit.Value;
            ConfigSer.Save();
        }

        private void num_stonelimit_Leave(object sender, EventArgs e)
        {
            _config.stonelimit = (int) num_stonelimit.Value;
            ConfigSer.Save();
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            _config.server_token = textBox2.Text;
            ConfigSer.Save();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            Core.StopBot();
            BotThread.Abort();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            _config.debug = checkBox1.Checked;
            Core.Debug = checkBox1.Checked;
            ConfigSer.Save();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
          //  var seed = textBox3.Text;
           // BarrelController.SetSeed(Convert.ToDouble(seed));
           // BarrelController.GetNextBarrel()
        }
    }
}