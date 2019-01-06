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
using SeaBotGUI.GUIBinds;
using SeaBotGUI.TelegramBot;
using Task = System.Threading.Tasks.Task;

namespace SeaBotGUI
{
    public partial class Form1 : Form
    {
        public static Thread BotThread;

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
        public Label CoinsLabel => lbl_coins;
        public Label FishLabel => lbl_fish;
        public Label StoneLabel => lbl_stone;
        public Label GemLabel => lbl_gems;
        public Label IronLabel => lbl_iron;
        public Label WoodLabel => lbl_wood;
        public void LoadControls()
        {
           
            textBox2.Text = Core.Config.server_token;
            num_hibernationinterval.Value = Core.hibernation = Core.Config.hibernateinterval;
            checkBox1.Checked = Core.Config.debug;
            Core.Debug = Core.Config.debug;
            chk_autoshipupg.Checked = Core.Config.autoship;
            chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;
            chk_autofish.Checked = Core.Config.collectfish;
            chk_prodfact.Checked = Core.Config.prodfactory;
            chk_collectmat.Checked = Core.Config.collectfactory;
            chk_barrelhack.Checked = Core.Config.barrelhack;
            chk_finishupgrade.Checked = Core.Config.finishupgrade;
            chk_aupgrade.Checked = Core.Config.autoupgrade;
            dataGridView1.DataSource = new BindingSource(GUIBinds.BuildingGrid.BuildingBinding.Buildings, null);
            num_ironlimit.Value = Core.Config.ironlimit;
            num_woodlimit.Value = Core.Config.woodlimit;
            num_stonelimit.Value = Core.Config.stonelimit;
            textBox3.Text = Core.Config.telegramtoken;
            num_barrelinterval.Value = Core.Config.barrelinterval;
            var mac = GetDefMac();
            lbl_startupcode.Text = mac.Substring(0, mac.Length / 2);
            if (Core.Config.autoshipprofit)
            {
                radio_saveloot.Checked = true;
            }
            else
            {
                radio_savesailors.Checked = true;
            }
            linkLabel1.Links.Add(new LinkLabel.Link
                { LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token" });
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            UpdateButtons(Core.Config.autoshiptype);
            SeaBotCore.Events.Events.LoginedEvent.Logined.OnLoginedEvent += OnLogined;
        }

        void OnLogined()
        {
            FormateResources(Core.GlobalData);
            Core.GlobalData.Inventory.CollectionChanged += Inventory_CollectionChanged;
            Core.GlobalData.Inventory.ItemPropertyChanged += Inventory_ItemPropertyChanged;
        }
        public Form1()
        {
            SeaBotCore.Events.Events.SyncFailedEvent.SyncFailed.OnSyncFailedEvent += SyncFailed_OnSyncFailedEvent;
            // bot = new WTGLib("a");
            InitializeComponent();
            instance = this;      
            TeleConfigSer.Load();
            MaximizeBox = false;
            CheckForUpdates();
            LoadControls();
            Logger.Event.LogMessageChat.OnLogMessage += LogMessageChat_OnLogMessage;

            //Check for cache
        }


        private void SyncFailed_OnSyncFailedEvent(Enums.EErrorCode e)
        {
            new Task(() =>
            {
                if ((int) e == 4010 || e == 0 || e == Enums.EErrorCode.INVALID_SESSION)
                {
                   Core.StopBot();
                   Core.StartBot();
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

     
        public void FormateResources(GlobalData data)
        {
            if (data.Inventory == null)
            { return;}
            ResourcesBox.Update();
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
            if (string.IsNullOrEmpty(Core.Config.server_token))
            {
                MessageBox.Show("Empty server_token\nPlease fill server token in Settings tab", "Error");
                return;
            }
            button3.Enabled = true;
            button2.Enabled = false;
            Core.StartBot();
          

        }

        private void Inventory_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            FormateResources(Core.GlobalData);
        }

     

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.debug = checkBox1.Checked;
            Core.Debug = checkBox1.Checked;
          
        }

        private void chk_autofish_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectfish = chk_autofish.Checked;
          
        }

        private void chk_prodfact_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.prodfactory = chk_prodfact.Checked;
          
        }

        private void chk_collectmat_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectfactory = chk_collectmat.Checked;
          
        }


        private void num_woodlimit_Leave(object sender, EventArgs e)
        {
            Core.Config.woodlimit = (int) num_woodlimit.Value;
          
        }


        private void num_ironlimit_Leave(object sender, EventArgs e)
        {
            Core.Config.ironlimit = (int) num_ironlimit.Value;
          
        }

        private void num_stonelimit_Leave(object sender, EventArgs e)
        {
            Core.Config.stonelimit = (int) num_stonelimit.Value;
          
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            Core.Config.server_token = textBox2.Text;
          
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
            Core.Config.debug = checkBox1.Checked;
            Core.Debug = checkBox1.Checked;
          
        }

        private void chk_aupgrade_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autoupgrade = chk_aupgrade.Checked;
          
        }

        private void chk_finishupgrade_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.finishupgrade = chk_finishupgrade.Checked;
          
        }

        private void chk_barrelhack_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.barrelhack = chk_barrelhack.Checked;
          
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
            Core.Config.barrelinterval = (int) num_barrelinterval.Value;
          
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/seabotdev");
        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            Core.hibernation = Core.Config.hibernateinterval = (int) num_hibernationinterval.Value;
          
        }

        private void chk_onlyfactory_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.upgradeonlyfactory = chk_onlyfactory.Checked;
          
        }

        private void chk_autoshipupg_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autoship = chk_autoshipupg.Checked;
          
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
                Core.Config.autoshiptype = "coins";
            }

            if (radio_iron.Checked)
            {
                Core.Config.autoshiptype = "iron";
            }

            if (radio_wood.Checked)
            {
                Core.Config.autoshiptype = "wood";
            }

            if (radioButton6.Checked)
            {
                //fish 
                Core.Config.autoshiptype = "fish";
            }

            if (radioButton7.Checked)
            {
                //stone
                Core.Config.autoshiptype = "stone";
            }

          
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
            Core.Config.autoshipprofit = radio_saveloot.Checked;
          
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Core.Config.telegramtoken = textBox3.Text;
          
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            if (Core.Config.telegramtoken == "")
            {
                MessageBox.Show("No telegram token");
            }
            else
            {
                try
                {
                    bot = new WTGLib(Core.Config.telegramtoken);
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception.ToString());
                }
            }
        }
    }
}