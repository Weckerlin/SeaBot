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
#region

using Exceptionless.Configuration;

#endregion

[assembly: Exceptionless("lVxMtZtAbEjXCOBSGWJ9DjHXlGg1w3808btZZ9Ug")]

namespace SeaBotGUI
{
    #region

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

    using Newtonsoft.Json;

    using SeaBotCore;
    using SeaBotCore.BotMethods.ShipManagment.SendShip;
    using SeaBotCore.Config;
    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    using SeaBotGUI.Debug;
    using SeaBotGUI.GUIBinds;
    using SeaBotGUI.Localization;
    using SeaBotGUI.TelegramBot;
    using SeaBotGUI.Utils;

    #endregion

    public partial class Form1 : Form
    {
        public static TeleConfigData _teleconfig = new TeleConfigData();

        public static Form1 instance;

        private bool barrelsaid;

        public Form1()
        {
            // bot = new WTGLib("a");
            LocalizationController.SetLanguage(Core.Config.language);
            this.InitializeComponent();
            this.BuildingGrid = this.dataGridView1;
            this.ShipGrid = this.dataGridView2;
            this.CoinsLabel = this.lbl_coins;
            this.FishLabel = this.lbl_fish;
            this.StoneLabel = this.lbl_stone;
            this.TabControl = this.tabControl1;
            this.GemLabel = this.lbl_gems;
            this.IronLabel = this.lbl_iron;
            this.WoodLabel = this.lbl_wood;
            this.LevelLabel = this.lbl_lvl;
            this.SailorsLabel = this.lbl_sailors;
            this.NeededAlgoGrid = this.dataGridView3;
            this.InventoryGrid = this.dataGridView4;
            instance = this;
            TeleConfigSer.Load();
            this.MaximizeBox = false;
            this.CheckForUpdates();
            this.UpdateButtons(Core.Config.autoshiptype);
            this.LoadControls();
            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            Logger.Event.LogMessageChat.OnLogMessage += this.LogMessageChat_OnLogMessage;
            if (!Core.Config.acceptedresponsibility)
            {
                var msg = MessageBox.Show(
                    PrivateLocal.SEABOTGUI_WELCOME,
                    "Root to the SeaBot!",
                    MessageBoxButtons.OKCancel);
                if (msg == DialogResult.OK)
                {
                    Core.Config.acceptedresponsibility = true;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            Form1.instance.Resize += Instance_Resize;
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1].ToLower() == "-start")
                {
                    if (Core.Config.server_token != "")
                    {
                        Core.StartBot();
                    }
                    else
                    {
                        Logger.Error("Skipped command line startup because server token is null");
                    }
                }
            }

            // Check for cache
        }

        private void Instance_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  
            {  
                Hide();  
                this.notifyIcon1.Visible = true;                  
            }  
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();  
            this.WindowState = FormWindowState.Normal;  
            this.notifyIcon1.Visible = false;  

        }

        public DataGridView NeededAlgoGrid { get; }

        public DataGridView BuildingGrid { get; }

        public DataGridView InventoryGrid { get; }

        public Label CoinsLabel { get; }

        public Label FishLabel { get; }

        public Label GemLabel { get; }

        public Label IronLabel { get; }

        public Label LevelLabel { get; }

        public Label SailorsLabel { get; }

        public DataGridView ShipGrid { get; }

        public Label StoneLabel { get; }

        public TabControl TabControl { get; }

        public Label WoodLabel { get; }

        public void FormatResources(GlobalData data)
        {
            if (data.Inventory == null)
            {
                return;
            }

            ResourcesBox.Update();
            
        }

        public void LoadControls()
        {
            ExceptionlessClient.Default.Register(true);
            ExceptionlessClient.Default.SubmitLog("Logined");
            AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

            this.textBox2.Text = Core.Config.server_token;
            this.num_hibernationinterval.Value = Core.hibernation = Core.Config.hibernateinterval;
            this.checkBox1.Checked = Core.Config.debug;
            this.chk_exploit.Checked = Core.Config.exploitmode;
            this.chk_autoshipupg.Checked = Core.Config.autoship;
            this.chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;
            this.chk_autofish.Checked = Core.Config.collectfish;
            this.chk_prodfact.Checked = Core.Config.prodfactory;
            this.chk_collectmat.Checked = Core.Config.collectfactory;
            this.chk_barrelhack.Checked = Core.Config.barrelhack;
            if (Core.Config.sleepeveryhrs)
            {
                this.radio_sleepeveryhrs.Checked = true;
            }
            else
            {
                this.radio_sleepeverymin.Checked = true;
            }

            if (Core.Config.sleepforhrs)
            {
                this.radio_sleepforhrs.Checked = true;
            }
            else
            {
                this.radio_sleepformins.Checked = true;
            }
            this.chk_finishupgrade.Checked = Core.Config.finishupgrade;
            this.chk_aupgrade.Checked = Core.Config.autoupgrade;
            this.chk_automuseum.Checked = Core.Config.collectmuseum;
            this.BuildingGrid.DataSource = new BindingSource(GUIBinds.BuildingGrid.BuildingBinding.Buildings, null);
            this.ShipGrid.DataSource = new BindingSource(GUIBinds.ShipGrid.ShipBinding.Ships, null);
            this.NeededAlgoGrid.DataSource = new BindingSource(DebugGUI.Algos,null);
            this.InventoryGrid.DataSource = new BindingSource(GUIBinds.InventoryGrid.InventoryBinding.Items,null);
            this.num_ironlimit.Value = Core.Config.ironlimit;
            this.num_woodlimit.Value = Core.Config.woodlimit;
            this.num_stonelimit.Value = Core.Config.stonelimit;
            this.num_limitfuel.Value = Core.Config.thresholdfuel;
            this.num_limitconcrete.Value = Core.Config.thresholdconcrete;
            this.num_limitmech.Value = Core.Config.thresholdmechanical;
            this.textBox3.Text = Core.Config.telegramtoken;
            this.num_barrelinterval.Value = Core.Config.barrelinterval;
            BindDestination();
            SeaBotCore.Events.Events.BotStoppedEvent.BotStopped.OnBotStoppedEvent += this.BotStopped_OnBotStoppedEvent;
            SeaBotCore.Events.Events.BotStartedEvent.BotStarted.OnBotStartedEvent += this.BotStarted_OnBotStartedEvent;
            this.lbl_startupcode.Text = TeleUtils.MacAdressCode.Substring(0, TeleUtils.MacAdressCode.Length / 2);
           
            this.chk_smartsleep.Checked = Core.Config.smartsleepenabled;
            this.chk_sleepenabled.Checked = Core.Config.sleepenabled;
            this.num_sleepevery.Value = Core.Config.sleepevery;
            this.num_sleepfor.Value = Core.Config.sleepfor;
            if (Core.Config.sleepforhrs)
            {
                this.radio_sleepforhrs.Checked = true;
            }
            else
            {
                this.radio_sleepformins.Checked = true;
            }

            if (Core.Config.sleepeveryhrs)
            {
                this.radio_sleepeveryhrs.Checked = true;
            }
            else
            {
                this.radio_sleepeverymin.Checked = true;
            }

            if (Core.Config.autothresholdworkshop)
            {
                this.checkBox2.Checked = true;
            }

            if (Core.Config.workshoptype == WorkshopType.Fuel)
            {
                this.radioButton2.Checked = true;
            }

            if (Core.Config.workshoptype == WorkshopType.Concrete)
            {
                this.radioButton3.Checked = true;
            }

            if (Core.Config.workshoptype == WorkshopType.MechanicalPart)
            {
                this.radioButton1.Checked = true;
            }

            if (Core.Config.shipdesttype == ShipDestType.Auto)
            {
                this.radio_autoshipauto.Checked = true;
            }

            if (Core.Config.shipdesttype == ShipDestType.Contractor)
            {
                this.radio_contractor.Checked = true;
            }

            if (Core.Config.shipdesttype == ShipDestType.Marketplace)
            {
                this.radio_marketplace.Checked = true;
            }

            if (Core.Config.shipdesttype == ShipDestType.Outpost)
            {
                this.radio_outpost.Checked = true;
            }

            if (Core.Config.shipdesttype == ShipDestType.Upgradable)
            {
                this.radio_upgradable.Checked = true;
            }

            if (Core.Config.upgradablestrategy == UpgradablyStrategy.Loot)
            {
                this.radio_saveloot.Checked = true;
            }
            else
            {
                this.radio_savesailors.Checked = true;
            }

            this.linkLabel1.Links.Add(
                new LinkLabel.Link { LinkData = "https://github.com/weespin/SeaBot/wiki/Getting-server_token" });
            this.BuildingGrid.DefaultCellStyle.SelectionBackColor = this.BuildingGrid.DefaultCellStyle.BackColor;
            this.BuildingGrid.DefaultCellStyle.SelectionForeColor = this.BuildingGrid.DefaultCellStyle.ForeColor;

            SeaBotCore.Events.Events.LoginedEvent.Logined.OnLoginedEvent += this.OnLogined;

            foreach (var lang in Enum.GetNames(typeof(LocalizationController.ELanguages)))
            {
                this.cbox_lang.Items.Add(lang);
            }

            this.cbox_lang.Text = Enum.GetName(typeof(LocalizationController.ELanguages), Core.Config.language);
            Core.Config.PropertyChanged += this.Config_PropertyChanged;
        }

        private void BindDestination()
        {
            switch (Core.Config.shipdesttype)
            {
                case ShipDestType.Upgradable:
                    this.radio_upgradable.Checked = true;
                    break;
                case ShipDestType.Outpost:
                    this.radio_outpost.Checked = true;
                    break;
                case ShipDestType.Marketplace:
                    this.radio_marketplace.Checked = true;
                    break;
                case ShipDestType.Contractor:
                    this.radio_contractor.Checked = true;
                    break;
                case ShipDestType.Auto:
                    this.radio_autoshipauto.Checked = true;
                    break;
                case ShipDestType.Wreck:
                    this.radio_wreck.Checked = true;
                    break;
                
            }
        }

       
        public void ShipDestReset()
        {
            if (this.radio_upgradable.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Upgradable;
            }

            if (this.radio_autoshipauto.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Auto;
            }

            if (this.radio_contractor.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Contractor;
            }

            if (this.radio_outpost.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Outpost;
            }

            if (this.radio_marketplace.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Marketplace;
            }
        }

        public void WorkShopRadio()
        {
            if (this.radioButton1.Checked)
            {
                Core.Config.workshoptype = WorkshopType.MechanicalPart;
            }

            if (this.radioButton2.Checked)
            {
                Core.Config.workshoptype = WorkshopType.Fuel;
            }

            if (this.radioButton3.Checked)
            {
                Core.Config.workshoptype = WorkshopType.Concrete;
            }
        }

        private void BotStarted_OnBotStartedEvent()
        {
            instance.Invoke(
                new Action(
                    () =>
                        {
                            this.button3.Enabled = true;

                            this.button2.Enabled = false;
                        }));
        }

        private void BotStopped_OnBotStoppedEvent()
        {
            instance.Invoke(
                new Action(
                    () =>
                        {
                            this.button3.Enabled = false;

                            this.button2.Enabled = true;
                        }));
        }

        private void btn_dumpcore_Click(object sender, EventArgs e)
        {
            File.WriteAllText(
                DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss") + "DUMP.json",
                JsonConvert.SerializeObject(Core.GlobalData, Formatting.Indented));
        }

        private void btn_removeitem_Click(object sender, EventArgs e)
        {
            var much = (int)this.num_removenum.Value;
            if (much <= 0)
            {
                return;
            }

            if (this.dataGridView4.CurrentRow!=null)
            {

                if (dataGridView4.CurrentRow != null)
                {
                    var picked = (GUIBinds.InventoryGrid.InventoryBinding.Item)dataGridView4.CurrentRow.DataBoundItem;
                    var wehave = picked.Amount;
                    if (wehave != 0 && wehave >= much)
                    {
                        var item = MaterialDB.GetItem(picked.ID);
                        Logger.Info(string.Format(PrivateLocal.INVENTORY_REMOVED, much, item.Name));
                        Networking.AddTask(new Task.RemoveMaterialTask(item.DefId, much));
                        Core.GlobalData.Inventory.First(n => n.Id == item.DefId).Amount -= much;
                    }
                }
            }
        }

        private void Btn_stats_Click(object sender, EventArgs e)
        {
            var stats = new Stats();
            stats.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var s = new Marketplace();
            s.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(Core.Config.server_token))
            {
                MessageBox.Show(PrivateLocal.TOKEN_EMPTY, "Error");
                return;
            }

            this.button3.Enabled = true;
            this.button2.Enabled = false;
            Core.StartBot();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = true;
            this.button3.Enabled = false;
            Core.StopBot();
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

        private void button5_Click(object sender, EventArgs e)
        {
            TelegramBotController.StopBot();
        }

        private void cbox_lang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbox_lang.SelectedItem != null)
            {
                LocalizationController.ELanguages lang;
                if (Enum.TryParse((string)this.cbox_lang.SelectedItem, out lang))
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

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Core.Config.debug = this.checkBox1.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autothresholdworkshop = this.checkBox2.Checked;
            if (this.checkBox2.Checked)
            {
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = false;
                this.radioButton3.Enabled = false;
            }
            else
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = true;
            }
        }

        private void CheckForUpdates()
        {
            try
            {
                var httpClient = new HttpClient();

                // specify to use TLS 1.2 as default connection
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");

                httpClient.DefaultRequestHeaders.Accept.Clear();
                var l = httpClient.GetAsync("http://api.github.com/repos/weespin/SeaBot/releases/latest").Result.Content
                    .ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<GitHub_Data.Root>(l);
                var version1 = new Version(
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
                var version2 = new Version(data.TagName);

                var result = version1.CompareTo(version2);
                if (result > 0)
                {

                    this.Text += $" [DEV] Version: {version1}";
                }
                else if (result < 0)
                {

                    this.Text += string.Format(" " + PrivateLocal.VERSION_OLD, version1);
                    var msg = MessageBox.Show(
                        PrivateLocal.VERSION_UPDATE_MBOX,
                        "Update!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (msg == DialogResult.Yes)
                    {
                        CompUtils.OpenLink(data.HtmlUrl.ToString());
                    }
                }
                else
                {

                    this.Text += string.Format(" " + PrivateLocal.VERSION_CURRENT, version1);
                }
            }
            catch (Exception)
            {

                this.Text += $"no internet";
                //ignored
            }
        }

        private void chk_aupgrade_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autoupgrade = this.chk_aupgrade.Checked;
        }

        private void chk_autofish_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectfish = this.chk_autofish.Checked;
        }

        private void chk_automuseum_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectmuseum = this.chk_automuseum.Checked;
        }

        private void chk_autoshipupg_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.autoship = this.chk_autoshipupg.Checked;
        }

        private void chk_barrelhack_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.barrelhack = this.chk_barrelhack.Checked;
        }

        private void chk_collectmat_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.collectfactory = this.chk_collectmat.Checked;
        }

        private void chk_finishupgrade_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.finishupgrade = this.chk_finishupgrade.Checked;
        }

        private void chk_onlyfactory_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.upgradeonlyfactory = this.chk_onlyfactory.Checked;
        }

        private void chk_prodfact_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.prodfactory = this.chk_prodfact.Checked;
        }

        private void chk_sleepenabled_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.sleepenabled = this.chk_sleepenabled.Checked;
        }

        private void chk_smartsleep_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox15.Enabled = !this.chk_smartsleep.Checked;
            Core.Config.smartsleepenabled = this.chk_smartsleep.Checked;
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "acceptedresponsibility")
            {
                return;
            }

            // Dont scream at me because this shit happened, its 4AM, i don't wont to bind
            instance.Invoke(
                new Action(
                    () =>
                        {
                            if (e.PropertyName == "woodlimit")
                            {
                                this.num_woodlimit.Value = Core.Config.woodlimit;
                            }

                            if (e.PropertyName == "ironlimit")
                            {
                                this.num_ironlimit.Value = Core.Config.ironlimit;
                            }

                            if (e.PropertyName == "stonelimit")
                            {
                                this.num_stonelimit.Value = Core.Config.stonelimit;
                            }

                            if (e.PropertyName == "collectfish")
                            {
                                this.chk_autofish.Checked = Core.Config.collectfish;
                            }

                            if (e.PropertyName == "prodfactory")
                            {
                                this.chk_prodfact.Checked = Core.Config.prodfactory;
                            }

                            if (e.PropertyName == "collectfactory")
                            {
                                this.chk_collectmat.Checked = Core.Config.collectfactory;
                            }

                            if (e.PropertyName == "autoupgrade")
                            {
                                this.chk_aupgrade.Checked = Core.Config.autoupgrade;
                            }

                            if (e.PropertyName == "autoship")
                            {
                                this.chk_autoshipupg.Checked = Core.Config.autoship;
                            }

                            if (e.PropertyName == "finishupgrade")
                            {
                                this.chk_finishupgrade.Checked = Core.Config.finishupgrade;
                            }

                            if (e.PropertyName == "barrelhack")
                            {
                                this.chk_barrelhack.Checked = Core.Config.barrelhack;
                            }

                            if (e.PropertyName == "upgradeonlyfactory")
                            {
                                this.chk_onlyfactory.Checked = Core.Config.upgradeonlyfactory;
                            }

                            if (e.PropertyName == "barrelinterval")
                            {
                                this.num_barrelinterval.Value = Core.Config.barrelinterval;
                            }

                            if (e.PropertyName == "hibernateinterval")
                            {
                                this.num_hibernationinterval.Value = Core.Config.hibernateinterval;
                            }

                            if (e.PropertyName == "autoshiptype")
                            {
                                this.UpdateButtons(Core.Config.autoshiptype);
                            }

                            
                        }));
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exc = (Exception)e.ExceptionObject;
            exc.ToExceptionless().Submit();
            Logger.Fatal(e.ExceptionObject.ToString());
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Inventory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.FormatResources(Core.GlobalData);
        }

        private void Inventory_ItemPropertyChanged(object sender, ItemPropertyChangedEventArgs e)
        {
            this.FormatResources(Core.GlobalData);
        }

        private void Label17_Click(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink(e.Link.LinkData as string);
        }

        private void LinkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/seabotdev");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://github.com/weespin/SeaBot");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/nullcore");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://steamcommunity.com/id/wspin/");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://t.me/seabotdev");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://github.com/weespin/SeaBot/wiki/Getting-Telegram-Token");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://qiwi.me/seabot");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://www.donationalerts.com/r/weespin");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CompUtils.OpenLink("https://steamcommunity.com/tradeoffer/new/?partner=83321528&token=2CIUp5N6");
        }

        private void LogMessageChat_OnLogMessage(Logger.Message e)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker inv = delegate
                    {
                        RichTextBoxExtensions.AppendText(this.richTextBox1, e.message + "\n", e.color);
                    };
                this.richTextBox1.BeginInvoke(inv);
            }
            else
            {
                RichTextBoxExtensions.AppendText(this.richTextBox1, e.message + "\n", e.color);
            }
        }

        private void num_barrelinterval_Leave(object sender, EventArgs e)
        {
            Core.Config.barrelinterval = (int)this.num_barrelinterval.Value;
        }

        private void num_barrelinterval_ValueChanged(object sender, EventArgs e)
        {
            if (this.num_barrelinterval.Value < 12 && !this.barrelsaid)
            {
                MessageBox.Show(
                    PrivateLocal.BARREL_INTERVAL_WARNING,
                    "Alert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                this.barrelsaid = true;
            }

            if (this.num_barrelinterval.Value < 12)
            {
                this.num_barrelinterval.ForeColor = Color.Red;
            }
            else
            {
                this.num_barrelinterval.ForeColor = Color.Black;
            }
        }

        private void num_ironlimit_Leave(object sender, EventArgs e)
        {
            Core.Config.ironlimit = (int)this.num_ironlimit.Value;
        }

        private void Num_limitconcrete_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdconcrete = (int)this.num_limitconcrete.Value;
        }

        private void Num_limitfuel_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdfuel = (int)this.num_limitfuel.Value;
        }

        private void Num_limitmech_Leave(object sender, EventArgs e)
        {
            Core.Config.thresholdmechanical = (int)this.num_limitmech.Value;
        }

        private void num_sleepevery_ValueChanged(object sender, EventArgs e)
        {
            Core.Config.sleepevery = (int)this.num_sleepevery.Value;
        }

        private void num_stonelimit_Leave(object sender, EventArgs e)
        {
            Core.Config.stonelimit = (int)this.num_stonelimit.Value;
        }

        private void num_woodlimit_Leave(object sender, EventArgs e)
        {
            Core.Config.woodlimit = (int)this.num_woodlimit.Value;
        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            Core.hibernation = Core.Config.hibernateinterval = (int)this.num_hibernationinterval.Value;
        }

        private void OnLogined()
        {
            this.FormatResources(Core.GlobalData);
            GUIBinds.BuildingGrid.Start();
            GUIBinds.ShipGrid.Start();
            SeaBotGUI.Debug.DebugGUI.Start();
            GUIBinds.InventoryGrid.Start();
            Core.GlobalData.Inventory.CollectionChanged += this.Inventory_CollectionChanged;
            Core.GlobalData.Inventory.ItemPropertyChanged += this.Inventory_ItemPropertyChanged;
        }

        private void radio_gold_CheckedChanged(object sender, EventArgs e)
        {
            this.updatecheck();
        }

        private void radio_iron_CheckedChanged(object sender, EventArgs e)
        {
            this.updatecheck();
        }

        private void Radio_oil_CheckedChanged(object sender, EventArgs e)
        {
            this.updatecheck();
        }

        private void radio_sleepeveryhrs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radio_sleepeveryhrs.Checked)
            {
                Core.Config.sleepeveryhrs = true;
            }
            else
            {
                Core.Config.sleepeveryhrs = false;
            }
        }

        private void radio_sleepforhrs_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radio_sleepforhrs.Checked)
            {
                Core.Config.sleepforhrs = true;
            }
            else
            {
                Core.Config.sleepforhrs = false;
            }
        }

        private void radio_wood_CheckedChanged(object sender, EventArgs e)
        {
            this.updatecheck();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.WorkShopRadio();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.WorkShopRadio();
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.WorkShopRadio();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
            if(this.radio_saveloot.Checked)
            {
                Core.Config.upgradablestrategy = UpgradablyStrategy.Loot;
            }
            else
            {
                Core.Config.upgradablestrategy = UpgradablyStrategy.Sailors;
            }
           
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            // fish
            this.updatecheck();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            // stone
            this.updatecheck();
        }

        private void textBox2_Leave_1(object sender, EventArgs e)
        {
            Core.Config.server_token = this.textBox2.Text;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Core.Config.telegramtoken = this.textBox3.Text;
        }

        private void UpdateButtons(string configAutoshiptype)
        {
            if (configAutoshiptype == "coins")
            {
                this.radio_gold.Checked = true;
            }

            if (configAutoshiptype == "fish")
            {
                this.radio_fish.Checked = true;
            }

            if (configAutoshiptype == "iron")
            {
                this.radio_iron.Checked = true;
            }

            if (configAutoshiptype == "wood")
            {
                this.radio_wood.Checked = true;
            }

            if (configAutoshiptype == "stone")
            {
                this.radio_stone.Checked = true;
            }

            if (configAutoshiptype == "Tier 2 - Oil")
            {
                this.radio_oil.Checked = true;
            }
        }

        private void updatecheck()
        {
            if (this.radio_gold.Checked)
            {
                Core.Config.autoshiptype = "coins";
            }

            if (this.radio_iron.Checked)
            {
                Core.Config.autoshiptype = "iron";
            }

            if (this.radio_wood.Checked)
            {
                Core.Config.autoshiptype = "wood";
            }

            if (this.radio_fish.Checked)
            {
                Core.Config.autoshiptype = "fish";
            }

            if (this.radio_stone.Checked)
            {
                Core.Config.autoshiptype = "stone";
            }

            if (this.radio_oil.Checked)
            {
                Core.Config.autoshiptype = "Tier 2 - Oil";
            }
        }


        private void Radio_wreck_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void UpdateDestCfg()
        {
            if (this.radio_autoshipauto.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Auto;
            }
            if (this.radio_wreck.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Wreck;
            }
            if (this.radio_contractor.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Contractor;
            }
            if (this.radio_marketplace.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Marketplace;
            }
            if (this.radio_outpost.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Outpost;
            }
            if (this.radio_upgradable.Checked)
            {
                Core.Config.shipdesttype = ShipDestType.Upgradable;
            }

        }

        private void Radio_autoshipauto_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void Radio_contractor_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void Radio_upgradable_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void Radio_outpost_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void Radio_marketplace_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDestCfg();
        }

        private void Chk_exploit_CheckedChanged(object sender, EventArgs e)
        {
            Core.Config.exploitmode = this.chk_exploit.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Num_sleepfor_ValueChanged(object sender, EventArgs e)
        {
            Core.Config.sleepfor = (int)this.num_sleepfor.Value;
        }
    }
}