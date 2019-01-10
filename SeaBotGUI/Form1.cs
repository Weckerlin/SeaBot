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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using Exceptionless;
using Exceptionless.Configuration;
using Newtonsoft.Json;
using SeaBotCore;
using SeaBotCore.Data;
using SeaBotCore.Data.Materials;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using SeaBotGUI.GUIBinds;
using SeaBotGUI.TelegramBot;
using SeaBotGUI.Utils;

[assembly: Exceptionless("lVxMtZtAbEjXCOBSGWJ9DjHXlGg1w3808btZZ9Ug")]

namespace SeaBotGUI
{
    public partial class Form1 : Form
    {
        public static TeleConfigData _teleconfig = new TeleConfigData();

        public static Form1 instance;

        private bool barrelsaid;

        public Form1()
        {
            // bot = new WTGLib("a");
            ExceptionlessClient.Default.Configuration.IncludePrivateInformation = false;
            InitializeComponent();
            instance = this;
            TeleConfigSer.Load();
            MaximizeBox = false;
            CheckForUpdates();
            LoadControls();
            Logger.Event.LogMessageChat.OnLogMessage += LogMessageChat_OnLogMessage;
            if (!Core.Config.acceptedresponsibility)
            {
                var msg = MessageBox.Show(
                    "By clicking 'OK' you agree that neither the program nor the developer is responsible for your account.\r\nIn order not to get a ban, please do not use too small a number in the intervals of the barrel or just do not use them.",
                    "Welcome to the SeaBot!", MessageBoxButtons.OKCancel);
                if (msg == DialogResult.OK)
                    Core.Config.acceptedresponsibility = true;
                else
                    Environment.Exit(0);
            }

            //Check for cache
        }

        public DataGridView BuildingGrid { get; private set; }

        public DataGridView ShipGrid { get; private set; }

        public Label CoinsLabel { get; private set; }

        public Label FishLabel { get; private set; }

        public Label StoneLabel { get; private set; }

        public Label GemLabel { get; private set; }

        public Label IronLabel { get; private set; }

        public Label WoodLabel { get; private set; }

        public void LoadControls()
        {
            ExceptionlessClient.Default.Register(true);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
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
            BuildingGrid.DataSource = new BindingSource(GUIBinds.BuildingGrid.BuildingBinding.Buildings, null);
            ShipGrid.DataSource = new BindingSource(GUIBinds.ShipGrid.ShipBinding.Ships, null);
            num_ironlimit.Value = Core.Config.ironlimit;
            num_woodlimit.Value = Core.Config.woodlimit;
            num_stonelimit.Value = Core.Config.stonelimit;
            textBox3.Text = Core.Config.telegramtoken;
            num_barrelinterval.Value = Core.Config.barrelinterval;
            SeaBotCore.Events.Events.BotStoppedEvent.BotStopped.OnBotStoppedEvent += BotStopped_OnBotStoppedEvent;
            SeaBotCore.Events.Events.BotStartedEvent.BotStarted.OnBotStartedEvent += BotStarted_OnBotStartedEvent;
            lbl_startupcode.Text = TeleUtils.MacAdressCode.Substring(0, TeleUtils.MacAdressCode.Length / 2);
            if (Core.Config.autoshipprofit)
                radio_saveloot.Checked = true;
            else
                radio_savesailors.Checked = true;

            chk_smartsleep.Checked = Core.Config.smartsleepenabled;
            chk_sleepenabled.Checked = Core.Config.sleepenabled;
            num_sleepevery.Value = Core.Config.sleepevery;
            num_sleepfor.Value = Core.Config.sleepfor;
            if (Core.Config.sleepforhrs)
                radio_sleepforhrs.Checked = true;
            else
                radio_sleepformins.Checked = true;
            if (Core.Config.sleepeveryhrs)
                radio_sleepeveryhrs.Checked = true;
            else
                radio_sleepeverymin.Checked = true;
            linkLabel1.Links.Add(new LinkLabel.Link
                {LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token"});
            BuildingGrid.DefaultCellStyle.SelectionBackColor = BuildingGrid.DefaultCellStyle.BackColor;
            BuildingGrid.DefaultCellStyle.SelectionForeColor = BuildingGrid.DefaultCellStyle.ForeColor;
            UpdateButtons(Core.Config.autoshiptype);
            SeaBotCore.Events.Events.LoginedEvent.Logined.OnLoginedEvent += OnLogined;
            Core.Config.PropertyChanged += Config_PropertyChanged;
        }

        private void BotStarted_OnBotStartedEvent()
        {
            instance.Invoke(new Action(() =>
            {
                button3.Enabled = true;

                button2.Enabled = false;
            }));
        }

        private void BotStopped_OnBotStoppedEvent()
        {
            instance.Invoke(new Action(() =>
            {
                button3.Enabled = false;

                button2.Enabled = true;
            }));
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Fatal(e.ExceptionObject.ToString());
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "acceptedresponsibility") return;

            //Dont scream at me because this shit happened, its 4AM, i don't wont to bind
            instance.Invoke(new Action(() =>
            {
                if (e.PropertyName == "woodlimit") num_woodlimit.Value = Core.Config.woodlimit;

                if (e.PropertyName == "ironlimit") num_ironlimit.Value = Core.Config.ironlimit;


                if (e.PropertyName == "stonelimit") num_stonelimit.Value = Core.Config.stonelimit;

                if (e.PropertyName == "collectfish") chk_autofish.Checked = Core.Config.collectfish;

                if (e.PropertyName == "prodfactory") chk_prodfact.Checked = Core.Config.prodfactory;

                if (e.PropertyName == "collectfactory") chk_collectmat.Checked = Core.Config.collectfactory;

                if (e.PropertyName == "autoupgrade") chk_aupgrade.Checked = Core.Config.autoupgrade;

                if (e.PropertyName == "autoship") chk_autoshipupg.Checked = Core.Config.autoship;

                if (e.PropertyName == "finishupgrade") chk_finishupgrade.Checked = Core.Config.finishupgrade;

                if (e.PropertyName == "barrelhack") chk_barrelhack.Checked = Core.Config.barrelhack;

                if (e.PropertyName == "upgradeonlyfactory") chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;

                if (e.PropertyName == "barrelinterval") num_barrelinterval.Value = Core.Config.barrelinterval;

                if (e.PropertyName == "hibernateinterval")
                    num_hibernationinterval.Value = Core.Config.hibernateinterval;

                if (e.PropertyName == "autoshiptype") UpdateButtons(Core.Config.autoshiptype);

                if (e.PropertyName == "autoshipprofit")
                {
                    if (Core.Config.autoshipprofit)
                        radio_saveloot.Checked = true;
                    else
                        radio_savesailors.Checked = true;
                }
            }));
        }

        private void OnLogined()
        {
            FormatResources(Core.GlobalData);
            GUIBinds.BuildingGrid.Start();
            GUIBinds.ShipGrid.Start();
            Core.GlobalData.Inventory.CollectionChanged += Inventory_CollectionChanged;
            Core.GlobalData.Inventory.ItemPropertyChanged += Inventory_ItemPropertyChanged;
        }


        private void Inventory_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            FormatResources(Core.GlobalData);
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


        public void FormatResources(GlobalData data)
        {
            if (data.Inventory == null) return;

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
                    foreach (var list in a) listView1.Items.Add(list);
                };
                listView1.BeginInvoke(inv);
            }
            else
            {
                listView1.Items.Clear();
                foreach (var list in a) listView1.Items.Add(list);
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
                if (msg == DialogResult.Yes) CompUtils.OpenLink(data.HtmlUrl.ToString());
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
            FormatResources(Core.GlobalData);
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
            button2.Enabled = true;
            button3.Enabled = false;
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
            CompUtils.OpenLink("https://steamcommunity.com/id/wspin/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/nullcore");
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
            if (configAutoshiptype == "coins") radio_gold.Checked = true;

            if (configAutoshiptype == "fish") radioButton6.Checked = true;

            if (configAutoshiptype == "iron") radio_iron.Checked = true;

            if (configAutoshiptype == "wood") radio_wood.Checked = true;

            if (configAutoshiptype == "stone") radioButton7.Checked = true;
        }

        private void updatecheck()
        {
            if (radio_gold.Checked) Core.Config.autoshiptype = "coins";

            if (radio_iron.Checked) Core.Config.autoshiptype = "iron";

            if (radio_wood.Checked) Core.Config.autoshiptype = "wood";

            if (radioButton6.Checked) Core.Config.autoshiptype = "fish";

            if (radioButton7.Checked) Core.Config.autoshiptype = "stone";
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
                MessageBox.Show("No telegram token");
            else
                try
                {
                    TelegramBotController.StartBot(Core.Config.telegramtoken);
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception.ToString());
                }
        }

        private void btn_removeitem_Click(object sender, EventArgs e)
        {
            var much = (int) num_removenum.Value;
            if (much <= 0) return;

            if (listView1.SelectedItems.Count > 0)
            {
                var picked = listView1.SelectedItems[0].SubItems[0].Text;
                var wehave = Core.GlobalData.GetAmountItem(picked);
                if (wehave != 0 && wehave >= much)
                {
                    var item = MaterialDB.GetItem(picked);
                    Logger.Info($"Removed {much} {item.Name}'s");
                    Networking.AddTask(
                        new Task.RemoveMaterialTask(item.DefId, much));
                    Core.GlobalData.Inventory.First(n => n.Id == item.DefId).Amount -=
                        much;
                }
            }
        }

        private void btn_dumpcore_Click(object sender, EventArgs e)
        {
            File.WriteAllText(DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss") + "DUMP.json",
                JsonConvert.SerializeObject(Core.GlobalData));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TelegramBotController.StopBot();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://github.com/weespin/SeaBot/wiki/Getting-Telegram-Token");
        }


        private void chk_smartsleep_CheckedChanged(object sender, EventArgs e)
        {
            groupBox15.Enabled = !chk_smartsleep.Checked;
            Core.Config.smartsleepenabled = chk_smartsleep.Checked;
        }

        private void chk_sleepenabled_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.sleepenabled = chk_sleepenabled.Checked;
        }

     

        private void num_sleepevery_ValueChanged(object sender, EventArgs e)
        {
            Core.Config.sleepevery = (int) num_sleepevery.Value;
        }

        private void radio_sleepforhrs_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_sleepforhrs.Checked)
                Core.Config.sleepforhrs = true;

            else
                Core.Config.sleepforhrs = false;
        }

        private void radio_sleepeveryhrs_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_sleepeveryhrs.Checked)
                Core.Config.sleepeveryhrs = true;

            else
                Core.Config.sleepeveryhrs = false;
        }

        private void num_barrelinterval_ValueChanged(object sender, EventArgs e)
        {
            if (num_barrelinterval.Value < 12 && !barrelsaid)
            {
                MessageBox.Show(
                    "Warning, at least 1 SeaBot user got banned for using interval, which is lower than 12.", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                barrelsaid = true;
            }

            if (num_barrelinterval.Value < 12)
                num_barrelinterval.ForeColor = Color.Red;
            else
                num_barrelinterval.ForeColor = Color.Black;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }
    }
}