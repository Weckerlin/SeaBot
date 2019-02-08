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
using SeaBotCore.Cache;
using SeaBotCore.Config;
using SeaBotCore.Data;
using SeaBotCore.Data.Materials;
using SeaBotCore.Localizaion;
using SeaBotCore.Logger;
using SeaBotCore.Utils;
using SeaBotGUI.GUIBinds;
using SeaBotGUI.Localization;
using SeaBotGUI.Properties;
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
            LocalizationController.SetLanguage(Core.Config.language);
            InitializeComponent();
            BuildingGrid = dataGridView1;
            ShipGrid = dataGridView2;
            CoinsLabel = lbl_coins;
            FishLabel = lbl_fish;
            StoneLabel = lbl_stone;
            TabControl = tabControl1;
            GemLabel = lbl_gems;
            IronLabel = lbl_iron;
            WoodLabel = lbl_wood;
            LevelLabel = lbl_lvl;
            SailorsLabel = lbl_sailors;
            instance = this;
            TeleConfigSer.Load();
            MaximizeBox = false;
            CheckForUpdates();
            UpdateButtons(Core.Config.autoshiptype);
            LoadControls();
            Logger.Event.LogMessageChat.OnLogMessage += LogMessageChat_OnLogMessage;
            if (!Core.Config.acceptedresponsibility)
            {
                var msg = MessageBox.Show(
                    PrivateLocal.SEABOTGUI_WELCOME,
                    "Root to the SeaBot!", MessageBoxButtons.OKCancel);
                if (msg == DialogResult.OK)
                {
                    Core.Config.acceptedresponsibility = true;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            //Check for cache
        }

        public DataGridView BuildingGrid { get; private set; }

        public DataGridView ShipGrid { get; private set; }

        public Label CoinsLabel { get; private set; }

        public Label FishLabel { get; private set; }

        public TabControl TabControl { get; private set; }
        public Label StoneLabel { get; private set; }
        public Label LevelLabel { get; private set; }
        public Label SailorsLabel { get; private set; }
        public Label GemLabel { get; private set; }

        public Label IronLabel { get; private set; }

        public Label WoodLabel { get; private set; }

        public void LoadControls()
        {
            ExceptionlessClient.Default.Register(true);
            ExceptionlessClient.Default.SubmitLog("Logined");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            textBox2.Text = Core.Config.server_token;
            num_hibernationinterval.Value = Core.hibernation = Core.Config.hibernateinterval;
            checkBox1.Checked = Core.Config.debug;
       
            chk_autoshipupg.Checked = Core.Config.autoship;
            chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;
            chk_autofish.Checked = Core.Config.collectfish;
            chk_prodfact.Checked = Core.Config.prodfactory;
            chk_collectmat.Checked = Core.Config.collectfactory;
            chk_barrelhack.Checked = Core.Config.barrelhack;
            chk_finishupgrade.Checked = Core.Config.finishupgrade;
            chk_aupgrade.Checked = Core.Config.autoupgrade;
            chk_automuseum.Checked = Core.Config.collectmuseum;
            BuildingGrid.DataSource = new BindingSource(GUIBinds.BuildingGrid.BuildingBinding.Buildings, null);
            ShipGrid.DataSource = new BindingSource(GUIBinds.ShipGrid.ShipBinding.Ships, null);
            num_ironlimit.Value = Core.Config.ironlimit;
            num_woodlimit.Value = Core.Config.woodlimit;
            num_stonelimit.Value = Core.Config.stonelimit;
            num_limitfuel.Value = Core.Config.thresholdfuel;
            num_limitconcrete.Value = Core.Config.thresholdconcrete;
            num_limitmech.Value = Core.Config.thresholdmechanical;
            textBox3.Text = Core.Config.telegramtoken;
            num_barrelinterval.Value = Core.Config.barrelinterval;
            SeaBotCore.Events.Events.BotStoppedEvent.BotStopped.OnBotStoppedEvent += BotStopped_OnBotStoppedEvent;
            SeaBotCore.Events.Events.BotStartedEvent.BotStarted.OnBotStartedEvent += BotStarted_OnBotStartedEvent;
            lbl_startupcode.Text = TeleUtils.MacAdressCode.Substring(0, TeleUtils.MacAdressCode.Length / 2);
            if (Core.Config.autoshipprofit)
            {
                radio_saveloot.Checked = true;
            }
            else
            {
                radio_savesailors.Checked = true;
            }

            chk_smartsleep.Checked = Core.Config.smartsleepenabled;
            chk_sleepenabled.Checked = Core.Config.sleepenabled;
            num_sleepevery.Value = Core.Config.sleepevery;
            num_sleepfor.Value = Core.Config.sleepfor;
            if (Core.Config.sleepforhrs)
            {
                radio_sleepforhrs.Checked = true;
            }
            else
            {
                radio_sleepformins.Checked = true;
            }

            if (Core.Config.sleepeveryhrs)
            {
                radio_sleepeveryhrs.Checked = true;
            }
            else
            {
                radio_sleepeverymin.Checked = true;
            }

            if (Core.Config.autothresholdworkshop)
            {
                checkBox2.Checked = true;
            }

            if (Core.Config.workshoptype == WorkshopType.Fuel)
            {
                radioButton2.Checked = true;
            }
            if (Core.Config.workshoptype == WorkshopType.Concrete)
            {
                radioButton3.Checked = true;
            }
            if (Core.Config.workshoptype == WorkshopType.MechanicalPart)
            {
                radioButton1.Checked = true;
            }
            if(Core.Config.shipdesttype == ShipDestType.Auto)
            {
                radio_autoshipauto.Checked = true;
            }
            if (Core.Config.shipdesttype == ShipDestType.Contractor)
            {
                radio_contractor.Checked = true;
            }
            if (Core.Config.shipdesttype == ShipDestType.Marketplace)
            {
                radio_marketplace.Checked = true;
            }
            if (Core.Config.shipdesttype == ShipDestType.Outpost)
            {
                radio_outpost.Checked = true;
            }
            if (Core.Config.shipdesttype == ShipDestType.Upgradable)
            {
                radio_upgradable.Checked = true;
            }

            linkLabel1.Links.Add(new LinkLabel.Link
                {LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token"});
            BuildingGrid.DefaultCellStyle.SelectionBackColor = BuildingGrid.DefaultCellStyle.BackColor;
            BuildingGrid.DefaultCellStyle.SelectionForeColor = BuildingGrid.DefaultCellStyle.ForeColor;


            SeaBotCore.Events.Events.LoginedEvent.Logined.OnLoginedEvent += OnLogined;

            foreach (var lang in Enum.GetNames(typeof(LocalizationController.ELanguages)))
            {
                cbox_lang.Items.Add(lang);
            }

            cbox_lang.Text = Enum.GetName(typeof(LocalizationController.ELanguages), Core.Config.language);
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
            var exc = (Exception) e.ExceptionObject;
            exc.ToExceptionless().Submit();
            Logger.Fatal(e.ExceptionObject.ToString());
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "acceptedresponsibility")
            {
                return;
            }

            //Dont scream at me because this shit happened, its 4AM, i don't wont to bind
            instance.Invoke(new Action(() =>
            {
                if (e.PropertyName == "woodlimit")
                {
                    num_woodlimit.Value = Core.Config.woodlimit;
                }

                if (e.PropertyName == "ironlimit")
                {
                    num_ironlimit.Value = Core.Config.ironlimit;
                }


                if (e.PropertyName == "stonelimit")
                {
                    num_stonelimit.Value = Core.Config.stonelimit;
                }

                if (e.PropertyName == "collectfish")
                {
                    chk_autofish.Checked = Core.Config.collectfish;
                }

                if (e.PropertyName == "prodfactory")
                {
                    chk_prodfact.Checked = Core.Config.prodfactory;
                }

                if (e.PropertyName == "collectfactory")
                {
                    chk_collectmat.Checked = Core.Config.collectfactory;
                }

                if (e.PropertyName == "autoupgrade")
                {
                    chk_aupgrade.Checked = Core.Config.autoupgrade;
                }

                if (e.PropertyName == "autoship")
                {
                    chk_autoshipupg.Checked = Core.Config.autoship;
                }

                if (e.PropertyName == "finishupgrade")
                {
                    chk_finishupgrade.Checked = Core.Config.finishupgrade;
                }

                if (e.PropertyName == "barrelhack")
                {
                    chk_barrelhack.Checked = Core.Config.barrelhack;
                }

                if (e.PropertyName == "upgradeonlyfactory")
                {
                    chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;
                }

                if (e.PropertyName == "barrelinterval")
                {
                    num_barrelinterval.Value = Core.Config.barrelinterval;
                }

                if (e.PropertyName == "hibernateinterval")
                {
                    num_hibernationinterval.Value = Core.Config.hibernateinterval;
                }

                if (e.PropertyName == "autoshiptype")
                {
                    UpdateButtons(Core.Config.autoshiptype);
                }

                if (e.PropertyName == "autoshipprofit")
                {
                    if (Core.Config.autoshipprofit)
                    {
                        radio_saveloot.Checked = true;
                    }
                    else
                    {
                        radio_savesailors.Checked = true;
                    }
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
            if (data.Inventory == null)
            {
                return;
            }

            ResourcesBox.Update();
            var a = new List<ListViewItem>();
            //todo fix
            for (int i = 0; i < data.Inventory.Count; i++)
            {
                if (data.Inventory[i].Amount != 0)
                {
                    string[] row =
                    {
                      MaterialDB.GetLocalizedName(data.Inventory[i].Id),
                        data.Inventory[i].Amount.ToString()
                    };
                    a.Add(new ListViewItem(row));
                }
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
                label7.Text = string.Format(PrivateLocal.VERSION_OLD, version1);
                var msg = MessageBox.Show(PrivateLocal.VERSION_UPDATE_MBOX, "Update!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    CompUtils.OpenLink(data.HtmlUrl.ToString());
                }
            }
            else
            {
                label7.ForeColor = Color.DarkGreen;
                label7.Text = string.Format(PrivateLocal.VERSION_CURRENT, version1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Core.Config.server_token))
            {
                MessageBox.Show(PrivateLocal.TOKEN_EMPTY, "Error");
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
            if (configAutoshiptype == "coins")
            {
                radio_gold.Checked = true;
            }

            if (configAutoshiptype == "fish")
            {
                radio_fish.Checked = true;
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
                radio_stone.Checked = true;
            }
            if(configAutoshiptype == "Tier 2 - Oil")
            {
                radio_oil.Checked = true;
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

            if (radio_fish.Checked)
            {
                Core.Config.autoshiptype = "fish";
            }

            if (radio_stone.Checked)
            {
                Core.Config.autoshiptype = "stone";
            }
            if(radio_oil.Checked)
            {
                Core.Config.autoshiptype = "Tier 2 - Oil";
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
            if (Core.Config.telegramtoken == string.Empty)
            {
                MessageBox.Show(PrivateLocal.TELEGRAM_NO_TOKEN);
            }
            else
            {
                try
                {
                    TelegramBotController.StartBot(Core.Config.telegramtoken);
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception.ToString());
                }
            }
        }

        private void btn_removeitem_Click(object sender, EventArgs e)
        {
            var much = (int) num_removenum.Value;
            if (much <= 0)
            {
                return;
            }

            if (listView1.SelectedItems.Count > 0)
            {
                var picked = listView1.SelectedItems[0].SubItems[0].Text;
                var wehave = Core.GlobalData.GetAmountItem(picked);
                if (wehave != 0 && wehave >= much)
                {
                    var item = MaterialDB.GetItem(picked);
                    Logger.Info(string.Format(PrivateLocal.INVENTORY_REMOVED, much, item.Name));
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
                JsonConvert.SerializeObject(Core.GlobalData, Formatting.Indented));
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
            {
                Core.Config.sleepforhrs = true;
            }

            else
            {
                Core.Config.sleepforhrs = false;
            }
        }

        private void radio_sleepeveryhrs_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_sleepeveryhrs.Checked)
            {
                Core.Config.sleepeveryhrs = true;
            }

            else
            {
                Core.Config.sleepeveryhrs = false;
            }
        }

        private void num_barrelinterval_ValueChanged(object sender, EventArgs e)
        {
            if (num_barrelinterval.Value < 12 && !barrelsaid)
            {
                MessageBox.Show(
                    PrivateLocal.BARREL_INTERVAL_WARNING, "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                barrelsaid = true;
            }

            if (num_barrelinterval.Value < 12)
            {
                num_barrelinterval.ForeColor = Color.Red;
            }
            else
            {
                num_barrelinterval.ForeColor = Color.Black;
            }
        }

        private void cbox_lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbox_lang.SelectedItem != null)
            {
                LocalizationController.ELanguages lang;
                if (Enum.TryParse((string) cbox_lang.SelectedItem, out lang))
                {
                    if (lang == Core.Config.language)
                    {
                        return;
                    }

                    Core.Config.language = lang;
                    if (lang == LocalizationController.ELanguages.RU)
                    {
                        MessageBox.Show("Пожалуйста перезапустите программу чтобы поменять язык.");
                    }

                    if (lang == LocalizationController.ELanguages.EN)
                    {
                        MessageBox.Show("Please restart the program to change the language.");
                    }
                }
                else
                {
                    Core.Config.language = LocalizationController.ELanguages.EN;
                }
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://www.donationalerts.com/r/weespin");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://qiwi.me/seabot");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://steamcommunity.com/tradeoffer/new/?partner=83321528&token=2CIUp5N6");
        }

        private void chk_automuseum_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectmuseum = chk_automuseum.Checked;
        }

        private void Btn_stats_Click(object sender, EventArgs e)
        {
            Stats stats = new Stats();
            stats.Show();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autothresholdworkshop = checkBox2.Checked;
            if (checkBox2.Checked)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
            }
            else
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            WorkShopRadio();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            WorkShopRadio();
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            WorkShopRadio();
        }

        public void WorkShopRadio()
        {
            if (radioButton1.Checked)
            {
                Core.Config.workshoptype = WorkshopType.MechanicalPart;
            }

            if (radioButton2.Checked)
            {
                Core.Config.workshoptype = WorkshopType.Fuel;
            }
            if (radioButton3.Checked)
            {
                Core.Config.workshoptype = WorkshopType.Concrete;
            }
        }
        public void ShipDestReset()
        {
            if (radio_upgradable.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Upgradable;
            }
            if (radio_autoshipauto.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Auto;
            }
            if (radio_contractor.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Contractor;
            }
            if (radio_outpost.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Outpost;
            }
            if (radio_marketplace.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Marketplace;
            }




        }
     

        private void Num_limitmech_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdmechanical =(int) num_limitmech.Value;
        }

       

        private void Num_limitfuel_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdfuel = (int)num_limitfuel.Value;
        }

        private void Num_limitconcrete_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdconcrete = (int)num_limitconcrete.Value;
        }

        private void Label17_Click(object sender, EventArgs e)
        {

        }

        private void Radio_oil_CheckedChanged(object sender, EventArgs e)
        {
            updatecheck();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SelectMarketPlace s = new SelectMarketPlace();
            s.Show();
        }

        
    }
}