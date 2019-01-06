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

using Newtonsoft.Json;
using SeaBotCore;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using SeaBotGUI.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SeaBotGUI.TelegramBot;
using Task = System.Threading.Tasks.Task;

namespace SeaBotGUI
{
    public partial class Form1 : Form
    {
        public static Thread BotThread;
        public static Config _config = new Config();
        public static TeleConfigData _teleconfig = new TeleConfigData();

        public static WTGLib bot;
        public static bool TeleBotStarted;

        public static string GetDefMac()
        {
            var computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine("Interface information for {0}.{1}     ",
                computerProperties.HostName, computerProperties.DomainName);

            if (nics == null || nics.Length < 1)
            {
                Console.WriteLine("  No network interfaces found.");
                return "DEFCODE";
            }

            foreach (var adapter in nics)
            {
                var address = adapter.GetPhysicalAddress();
                var bytes = address.GetAddressBytes();
                var addr = "";
                for (var i = 0; i < bytes.Length; i++)
                {
                    // Display the physical address in hexadecimal.
                    addr += bytes[i].ToString("X2");
                    // Insert a hyphen after each byte, unless we are at the end of the
                }

                if (addr == "")
                {
                }
                else
                {
                    return addr;
                }
            }

            return "DEFCODE";
        }

        public static Form1 instance;
        public DataGridView BuildingGrid => dataGridView1;

        public Form1()
        {
            // bot = new WTGLib("a");
            InitializeComponent();
            instance = this;
            ConfigSer.Load();
            TeleConfigSer.Load();


            MaximizeBox = false;
            CheckForUpdates();
            textBox2.Text = _config.server_token;
            num_hibernationinterval.Value = Core.hibernation = _config.hibernateinterval;
            checkBox1.Checked = _config.debug;
            Core.Debug = _config.debug;
            chk_autoshipupg.Checked = _config.autoship;
            chk_onlyfactory.Checked = _config.upgradeonlyfactory;
            chk_autofish.Checked = _config.collectfish;
            chk_prodfact.Checked = _config.prodfactory;
            chk_collectmat.Checked = _config.collectfactory;
            chk_barrelhack.Checked = _config.barrelhack;
            chk_finishupgrade.Checked = _config.finishupgrade;
            chk_aupgrade.Checked = _config.autoupgrade;
            dataGridView1.DataSource = new BindingSource(GUIBinds.BuildingGrid.BuildingBinding.Buildings, null);
            num_ironlimit.Value = _config.ironlimit;
            num_woodlimit.Value = _config.woodlimit;
            num_stonelimit.Value = _config.stonelimit;
            textBox3.Text = _config.telegramtoken;
            num_barrelinterval.Value = _config.barrelinterval;
            var mac = GetDefMac();
            lbl_startupcode.Text = mac.Substring(0, mac.Length / 2);
            if (_config.autoshipprofit)
            {
                radio_saveloot.Checked = true;
            }
            else
            {
                radio_savesailors.Checked = true;
            }

            SeaBotCore.Events.Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += SyncFailed_OnSyncFailedEvent;
            UpdateButtons(_config.autoshiptype);
            Logger.Event.LogMessageChat.OnLogMessage += LogMessageChat_OnLogMessage;
            linkLabel1.Links.Add(new LinkLabel.Link
                {LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token"});
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            //Check for cache
        }


        private void SyncFailed_OnSyncFailedEvent(Enums.EErrorCode e)
        {
            new Task(() =>
            {
                if ((int) e == 4010 || e == 0 || e == Enums.EErrorCode.INVALID_SESSION)
                {
                    Networking._syncThread.Abort();
                    ThreadKill.KillTheThread(BotThread);
                    ThreadKill.KillTheThread(BarrelThread);
                    Core.GlobalData = null;
                    Networking.Login();
                    BarrelThread = new Thread(BarrelVoid) {IsBackground = true};
                    BarrelThread.Start();
                    Networking.StartThread();
                    BotThread = new Thread(BotVoid)
                    {
                        IsBackground = true
                    };
                    BotThread.Start();
                }
            }).Start();
        }

        private void Inventory_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            FormateResources(Core.GlobalData);
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
                        var txt = new StringBuilder();
                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("coins").DefId) != null)
                        {
                            txt.Append(
                                $"Gold: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("coins").DefId).Amount}");
                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("fish").DefId) != null)
                        {
                            txt.Append(
                                $" Fish: {data.Inventory.First(n => n.Id == (int) MaterialDB.GetItem("fish").DefId).Amount} ");
                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("iron").DefId) != null)
                        {
                            txt.Append(
                                $" Iron: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("iron").DefId).Amount} ");
                        }

                        txt.Append(Environment.NewLine);
                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("gem").DefId) != null)
                        {
                            txt.Append(
                                $" Gems: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("gem").DefId).Amount} ");
                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("wood").DefId) != null)
                        {
                            txt.Append(
                                $" Wood: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("wood").DefId).Amount}");
                        }

                        if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("stone").DefId) != null)
                        {
                            txt.Append(
                                $" Stone: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("stone").DefId).Amount}");
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
                    var txt = new StringBuilder();
                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("coins").DefId) != null)
                    {
                        txt.Append(
                            $"Gold: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("coins").DefId).Amount}");
                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("fish").DefId) != null)
                    {
                        txt.Append(
                            $" Fish: {data.Inventory.First(n => n.Id == (int) MaterialDB.GetItem("fish").DefId).Amount} ");
                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("iron").DefId) != null)
                    {
                        txt.Append(
                            $" Iron: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("iron").DefId).Amount} ");
                    }

                    txt.Append(Environment.NewLine);
                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("gem").DefId) != null)
                    {
                        txt.Append(
                            $" Gems: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("gem").DefId).Amount} ");
                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("wood").DefId) != null)
                    {
                        txt.Append(
                            $" Wood: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("wood").DefId).Amount}");
                    }

                    if (data.Inventory.FirstOrDefault(n => n.Id == MaterialDB.GetItem("stone").DefId) != null)
                    {
                        txt.Append(
                            $" Stone: {data.Inventory.First(n => n.Id == MaterialDB.GetItem("stone").DefId).Amount}");
                    }

                    textBox1.Text = txt.ToString();
                }
            }

            var a = new List<ListViewItem>();
            foreach (var dataa in data.Inventory.Where(n =>
                n.Id != 1 && n.Id != 2 &&
                n.Id != 3 && n.Id != 4 &&
                n.Id != 5 && n.Id != 6))
            {
                string[] row = {MaterialDB.GetItem(dataa.Id).Name, dataa.Amount.ToString()};
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

        private void CheckForUpdates()
        {
            var httpClient = new HttpClient();

            //specify to use TLS 1.2 as default connection
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");

            httpClient.DefaultRequestHeaders.Accept.Clear();
            var l = httpClient.GetAsync("http://api.github.com/repos/weespin/SeaBot/releases/latest").Result.Content
                .ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<GitHub_Data.Root>(l);
            var version1 =
                new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            var version2 = new Version(data.TagName);

            var result = version1.CompareTo(version2);
            if (result > 0)
            {
                label7.ForeColor = Color.DarkMagenta;
                label7.Text = $"[DEV] Version: {version1}";
            }

            else if (result < 0)
            {
                label7.ForeColor = Color.DarkRed;
                label7.Text = $"[Old] Version: {version1}";
                var msg = MessageBox.Show("A new update has been released, press OK to open download page!", "Update!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    CompUtils.OpenLink(data.HtmlUrl.ToString());
                }
            }
            else
            {
                label7.ForeColor = Color.DarkGreen;
                label7.Text = $"[Current] Version: {version1}";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_config.server_token))
            {
                MessageBox.Show("Empty server_token\nPlease fill server token in Settings tab", "Error");
                return;
            }

            if (!Directory.Exists("cache"))
            {
                Cache.DownloadCache();
            }

            button3.Enabled = true;
            button2.Enabled = false;
            Core.ServerToken = textBox2.Text;

            Networking.Login();
            FormateResources(Core.GlobalData);
            Core.GlobalData.Inventory.CollectionChanged += Inventory_CollectionChanged;
            Core.GlobalData.Inventory.ItemPropertyChanged += Inventory_ItemPropertyChanged;
            Networking.StartThread();
            BotThread = new Thread(BotVoid)
            {
                IsBackground = true
            };
            BotThread.Start();
            var a = Defenitions.BuildingDef;
        }

        private void Inventory_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            FormateResources(Core.GlobalData);
        }

        private void BarrelVoid()
        {
            while (true)
            {
                Thread.Sleep((int) num_barrelinterval.Value * 1000);

                if (chk_barrelhack.Checked)
                {
                    BotLoop.BotLoop.CollectBarrel();
                }
            }
        }

        private void BotVoid()
        {
            while (true)
            {
                if (chk_aupgrade.Checked)
                {
                    BotLoop.BotLoop.AutoUpgrade(_config.upgradeonlyfactory);
                }

                if (chk_autoshipupg.Checked)
                {
                    BotLoop.BotLoop.AutoShip(_config.autoshiptype, _config.autoshipprofit);
                }

                if (chk_autofish.Checked)
                {
                    BotLoop.BotLoop.CollectFish();
                }


                if (chk_collectmat.Checked)
                {
                    BotLoop.BotLoop.CollectMaterials();
                }

                if (chk_prodfact.Checked)
                {
                    BotLoop.BotLoop.ProduceFactories((int) num_ironlimit.Value, (int) num_stonelimit.Value,
                        (int) num_woodlimit.Value);
                }

                if (chk_finishupgrade.Checked)
                {
                    BotLoop.BotLoop.FinishUpgrade();
                }


                Thread.Sleep(20 * 1000);
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
            Core.StopBot();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink(e.Link.LinkData as string);
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

        private void chk_aupgrade_CheckedChanged(object sender, EventArgs e)
        {
            _config.autoupgrade = chk_aupgrade.Checked;
            ConfigSer.Save();
        }

        private void chk_finishupgrade_CheckedChanged(object sender, EventArgs e)
        {
            _config.finishupgrade = chk_finishupgrade.Checked;
            ConfigSer.Save();
        }

        private void chk_barrelhack_CheckedChanged(object sender, EventArgs e)
        {
            _config.barrelhack = chk_barrelhack.Checked;
            ConfigSer.Save();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://github.com/weespin/SeaBot");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/nullcore");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://steamcommunity.com/id/wspin/");
        }


        private void num_barrelinterval_Leave(object sender, EventArgs e)
        {
            _config.barrelinterval = (int) num_barrelinterval.Value;
            ConfigSer.Save();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/seabotdev");
        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            Core.hibernation = _config.hibernateinterval = (int) num_hibernationinterval.Value;
            ConfigSer.Save();
        }

        private void chk_onlyfactory_CheckedChanged(object sender, EventArgs e)
        {
            _config.upgradeonlyfactory = chk_onlyfactory.Checked;
            ConfigSer.Save();
        }

        private void chk_autoshipupg_CheckedChanged(object sender, EventArgs e)
        {
            _config.autoship = chk_autoshipupg.Checked;
            ConfigSer.Save();
        }

        private void radio_gold_CheckedChanged(object sender, EventArgs e)
        {
            updatecheck();
        }

        private void UpdateButtons(string configAutoshiptype)
        {
            if (configAutoshiptype == "coins")
            {
                radio_gold.Checked = true;
            }

            if (configAutoshiptype == "fish")
            {
                radioButton6.Checked = true;
            }

            if (configAutoshiptype == "iron")
            {
                radio_iron.Checked = true;
            }

            if (configAutoshiptype == "wood")
            {
                radio_wood.Checked = true;
            }

            if (configAutoshiptype == "stone")
            {
                radioButton7.Checked = true;
            }
        }

        private void updatecheck()
        {
            if (radio_gold.Checked)
            {
                _config.autoshiptype = "coins";
            }

            if (radio_iron.Checked)
            {
                _config.autoshiptype = "iron";
            }

            if (radio_wood.Checked)
            {
                _config.autoshiptype = "wood";
            }

            if (radioButton6.Checked)
            {
                //fish 
                _config.autoshiptype = "fish";
            }

            if (radioButton7.Checked)
            {
                //stone
                _config.autoshiptype = "stone";
            }

            ConfigSer.Save();
        }

        private void radio_iron_CheckedChanged(object sender, EventArgs e)
        {
            updatecheck();
        }

        private void radio_wood_CheckedChanged(object sender, EventArgs e)
        {
            updatecheck();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            //fish
            updatecheck();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            //stone
            updatecheck();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            //saveloot
            _config.autoshipprofit = radio_saveloot.Checked;
            ConfigSer.Save();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            _config.telegramtoken = textBox3.Text;
            ConfigSer.Save();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            if (_config.telegramtoken == "")
            {
                MessageBox.Show("No telegram token");
            }
            else
            {
                try
                {
                    bot = new WTGLib(_config.telegramtoken);
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception.ToString());
                }
            }
        }
    }
}