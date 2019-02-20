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
namespace SeaBotGUI.GUIBinds
{
    #region

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using SeaBotCore;
    using SeaBotCore.BotMethods;
    using SeaBotCore.BotMethods.ShipManagment.SendShip;
    using SeaBotCore.Cache;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Utils;

    using SeaBotGUI.Localization;

    #endregion

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

    public static class ResourcesBox
    {
        public static string ToKMB(this int num)
        {
            if (num > 999999999 || num < -999999999)
            {
                return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
            }

            if (num > 999999 || num < -999999)
            {
                return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
            }

            if (num > 999 || num < -999)
            {
                return num.ToString("0,.#K", CultureInfo.InvariantCulture);
            }

            return num.ToString(CultureInfo.InvariantCulture);
        }

        public static void Update()
        {
            var sailors = Core.LocalPlayer.Sailors;
            if (Form1.instance.SailorsLabel.InvokeRequired)
            {
                Form1.instance.SailorsLabel.Invoke(
                    new Action(() => { Form1.instance.SailorsLabel.Text = sailors.ToKMB(); }));
            }
            else
            {
                Form1.instance.CoinsLabel.Text = sailors.ToKMB();
            }

            var level = Core.LocalPlayer.Level;
            if (Form1.instance.LevelLabel.InvokeRequired)
            {
                Form1.instance.LevelLabel.Invoke(
                    new Action(() => { Form1.instance.LevelLabel.Text = level.ToString(); }));
            }
            else
            {
                Form1.instance.LevelLabel.Text = level.ToString();
            }

            var coins = Core.LocalPlayer.GetAmountItem("coins");
            if (Form1.instance.CoinsLabel.InvokeRequired)
            {
                Form1.instance.CoinsLabel.Invoke(new Action(() => { Form1.instance.CoinsLabel.Text = coins.ToKMB(); }));
            }
            else
            {
                Form1.instance.CoinsLabel.Text = coins.ToKMB();
            }

            var fish = Core.LocalPlayer.GetAmountItem("fish");
            if (Form1.instance.FishLabel.InvokeRequired)
            {
                Form1.instance.FishLabel.Invoke(new Action(() => { Form1.instance.FishLabel.Text = fish.ToKMB(); }));
            }
            else
            {
                Form1.instance.FishLabel.Text = fish.ToKMB();
            }

            var iron = Core.LocalPlayer.GetAmountItem("iron");
            if (Form1.instance.IronLabel.InvokeRequired)
            {
                Form1.instance.IronLabel.Invoke(new Action(() => { Form1.instance.IronLabel.Text = iron.ToKMB(); }));
            }
            else
            {
                Form1.instance.IronLabel.Text = iron.ToKMB();
            }

            var gem = Core.LocalPlayer.GetAmountItem("gem");
            if (Form1.instance.GemLabel.InvokeRequired)
            {
                Form1.instance.GemLabel.Invoke(new Action(() => { Form1.instance.GemLabel.Text = gem.ToKMB(); }));
            }
            else
            {
                Form1.instance.GemLabel.Text = gem.ToKMB();
            }

            var wood = Core.LocalPlayer.GetAmountItem("wood");
            if (Form1.instance.WoodLabel.InvokeRequired)
            {
                Form1.instance.WoodLabel.Invoke(new Action(() => { Form1.instance.WoodLabel.Text = wood.ToKMB(); }));
            }
            else
            {
                Form1.instance.WoodLabel.Text = wood.ToKMB();
            }

            var stone = Core.LocalPlayer.GetAmountItem("stone");
            if (Form1.instance.StoneLabel.InvokeRequired)
            {
                Form1.instance.StoneLabel.Invoke(new Action(() => { Form1.instance.StoneLabel.Text = stone.ToKMB(); }));
            }
            else
            {
                Form1.instance.StoneLabel.Text = stone.ToKMB();
            }
        }
    }
    public static class InventoryGrid
    {
        private static Thread InventoryGridThread;

        public static void Start()
        {
            if (InventoryGridThread == null)
            {
                InventoryGridThread = new Thread(UpdateGrid);
                InventoryGridThread.IsBackground = true;
                InventoryGridThread.Start();
                Core.LocalPlayer.Inventory.CollectionChanged += Inventory_CollectionChanged;
            }
        }

        private static void Inventory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
              UpdateGrid();
        }

        public static void Stop()
        {
            if (InventoryGridThread.IsAlive)
            {
                InventoryGridThread.Abort();
                InventoryGridThread = null;
            }
        }

        public static void UpdateGrid()
        {
            var _lastupdatedTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(50);
                if ((DateTime.Now - _lastupdatedTime).TotalSeconds >= 1)
                {     
                    if (Form1.instance.WindowState == FormWindowState.Minimized)
                    {
                        continue;
                    }

                    _lastupdatedTime = DateTime.Now;
                    if (Form1.instance.BuildingGrid.InvokeRequired)
                    {
                        var newbuild = InventoryBinding.GetItems();
                        MethodInvoker meth = () =>
                            {
                                foreach (DataGridViewTextBoxColumn clmn in Form1.instance.InventoryGrid.Columns)
                                {
                                    clmn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                    clmn.Resizable = DataGridViewTriState.False;
                                }

                                foreach (var bld in newbuild)
                                {
                                    if (InventoryBinding.Items.Where(n => n.ID == bld.ID).FirstOrDefault() == null)
                                    {
                                        InventoryBinding.Items.Add(bld);
                                    }
                                    else
                                    {
                                        var old = InventoryBinding.Items.First(n => n.ID == bld.ID);
                                        if (old.Amount != bld.Amount)
                                        {
                                            old.Amount = bld.Amount;
                                        }
                                    }
                                }

                                Form1.instance.BuildingGrid.Refresh();
                                Form1.instance.BuildingGrid.Update();
                            };

                        Form1.instance.BuildingGrid.BeginInvoke(meth);
                    }
                }
            }
        }

        public static class InventoryBinding
        {
            public static BindingList<Item> Items = new BindingList<Item>();

            public static BindingList<Item> GetItems()
            {
                var ret = new BindingList<Item>();
                if (Core.LocalPlayer == null)
                {
                    return ret;
                }

                if (Core.LocalPlayer.Inventory == null)
                {
                    return ret;
                }

                foreach (var item in Core.LocalPlayer.Inventory)
                {
                    if (item.Amount == 0)
                    {
                        continue;
                    }
                    var pitem = new Item();
                    pitem.ID = item.Id;
                    pitem.Name = MaterialDB.GetLocalizedName(item.Id);
                    pitem.Amount = item.Amount;
                    ret.Add(pitem);
                }

                return ret;
            }

            public class Item
            {
               

                public int ID { get; set; }

                public string Name { get; set; }

                public int Amount { get; set; }

                public int Startup
                {
                    get
                    {
                        var am = Form1.instance.StartupInv.Where(n => n.Id == this.ID).FirstOrDefault();
                        if (am == null)
                        {
                            return 0;
                        }
                        return am.Amount;
                    }
                }

              
                public char Changed
                {
                    get
                    {
                        if (Form1.instance.StartupInv.Any(n => n.Id == ID))
                        {
                            var startupamount = Form1.instance.StartupInv.Where(n => n.Id == ID).FirstOrDefault()
                                ?.Amount;
                            if (startupamount > Amount)
                            {
                                return '↘';
                            }

                            if (startupamount == Amount)
                            {
                                return '=';
                            }
                            
                            if (startupamount < Amount)
                            {
                                return '↗';
                            }
                        }
                        else
                        {
                            return '↗';
                        }

                        return '?';
                    }
                  
                }
            }
        }
    }

    public static class BuildingGrid
    {
        private static Thread BuildingThread;

        public static void Start()
        {
            if (BuildingThread == null)
            {
                BuildingThread = new Thread(UpdateGrid);
                BuildingThread.IsBackground = true;
                BuildingThread.Start();
            }
        }

        public static void Stop()
        {
            if (BuildingThread.IsAlive)
            {
                BuildingThread.Abort();
                BuildingThread = null;
            }
        }

        public static void UpdateGrid()
        {
            var _lastupdatedTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(50);
                if ((DateTime.Now - _lastupdatedTime).TotalSeconds >= 1)
                {
                    if ((string)Form1.instance.Invoke(
                            new Func<string>(() => Form1.instance.TabControl.SelectedTab.Name)) != "tabPage5")
                    {
                        continue;
                    }

                    if (Form1.instance.WindowState == FormWindowState.Minimized)
                    {
                        continue;
                    }

                    _lastupdatedTime = DateTime.Now;
                    if (Form1.instance.BuildingGrid.InvokeRequired)
                    {
                        var newbuild = BuildingBinding.GetBuildings();
                        MethodInvoker meth = () =>
                            {
                                foreach (DataGridViewTextBoxColumn clmn in Form1.instance.BuildingGrid.Columns)
                                {
                                    clmn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                    clmn.Resizable = DataGridViewTriState.False;
                                }

                                foreach (var bld in newbuild)
                                {
                                    if (BuildingBinding.Buildings.Where(n => n.ID == bld.ID).FirstOrDefault() == null)
                                    {
                                        BuildingBinding.Buildings.Add(bld);
                                    }
                                    else
                                    {
                                        var old = BuildingBinding.Buildings.First(n => n.ID == bld.ID);
                                        if (old.Level != bld.Level)
                                        {
                                            old.Level = bld.Level;
                                        }

                                        if (old.Producing != bld.Producing)
                                        {
                                            old.Producing = bld.Producing;
                                        }

                                        if (old.Upgrade != bld.Upgrade)
                                        {
                                            old.Upgrade = bld.Upgrade;
                                        }

                                        // edit
                                    }
                                }

                                Form1.instance.BuildingGrid.Refresh();
                                Form1.instance.BuildingGrid.Update();
                            };

                        Form1.instance.BuildingGrid.BeginInvoke(meth);
                    }
                }
            }
        }

        public static class BuildingBinding
        {
            public static BindingList<Building> Buildings = new BindingList<Building>();

            public static BindingList<Building> GetBuildings()
            {
                var ret = new BindingList<Building>();
                if (Core.LocalPlayer == null)
                {
                    return ret;
                }

                if (Core.LocalPlayer.Buildings == null)
                {
                    return ret;
                }

                foreach (var building in Core.LocalPlayer.Buildings)
                {
                    var Building = new Building();
                    Building.ID = building.InstId;
                    Building.Name = LocalizationCache.GetNameFromLoc(
                        SeaBotCore.Data.Definitions.LocalDefinitions.Buildings.Where(n => n.DefId == building.DefId).FirstOrDefault()
                            ?.NameLoc,
                        LocalDefinitions.Buildings.Where(n => n.DefId == building.DefId).FirstOrDefault()
                            ?.Name);
                    Building.Level = building.Level;
                    var producing = string.Empty;
                    if (building.ProdStart != 0)
                    {
                        var willbeproducedat = building.ProdStart + LocalDefinitions.Buildings
                                                   .Where(n => n.DefId == building.DefId).FirstOrDefault()?.BuildingLevels.Level
                                                   .Where(n => n.Id == building.Level).FirstOrDefault()?.ProdOutputs
                                                   .ProdOutput[0].Time;
                        if (willbeproducedat.HasValue)
                        {
                            producing = (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(willbeproducedat.Value))
                                .ToString(@"hh\:mm\:ss");
                        }

                        // lol xD
                    }

                    var upgrade = string.Empty;
                    if (building.UpgStart != 0)
                    {
                        var willbeproducedat = building.UpgStart + LocalDefinitions.Buildings
                                                   .Where(n => n.DefId == building.DefId).FirstOrDefault()?.BuildingLevels.Level
                                                   .Where(n => n.Id == building.Level + 1).FirstOrDefault()
                                                   ?.UpgradeTime;
                        if (willbeproducedat.HasValue)
                        {
                            upgrade = (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime((long)willbeproducedat))
                                .ToString(@"hh\:mm\:ss");
                        }
                    }

                    if (building.DefId == 12)
                    {
                        if (building.UpgStart == 0)
                        {
                            var slot = Core.LocalPlayer.Slots.FirstOrDefault(n => n.Type == "museum_ship");
                            if (slot == null)
                            {
                                continue;
                            }

                            if (slot.SlotUsed == 0)
                            {
                                continue;
                            }

                            var started = TimeUtils.FromUnixTime(slot.LastUsed);

                            var b = LocalDefinitions.MuseumLevels.First(n => n.DefId == building.Level);

                            producing = (TimeUtils.FixedUTCTime - started.AddSeconds(b.TurnCount * b.TurnTime))
                                .ToString(@"hh\:mm\:ss");
                        }
                    }

                    Building.UpgradeIgnore = false;

                    Building.Producing = producing;
                    Building.Upgrade = upgrade;
                    ret.Add(Building);
                }

                return ret;
            }

            public class Building
            {
                public static bool ProdIgnore { get; set; }

                public static bool UpgradeIgnore { get; set; }

                public int ID { get; set; }

                public int Level { get; set; }

                public string Name { get; set; }

                public string Producing { get; set; }

                public string Upgrade { get; set; }
            }
        }
    }

    public class ShipGrid
    {
        public static BindingList<Ship> Ships = new BindingList<Ship>();

        public static Thread ShipThread;

        public static void Start()
        {
            if (ShipThread == null)
            {
                ShipThread = new Thread(UpdateGrid);
                ShipThread.IsBackground = true;
                ShipThread.Start();
            }
        }

        public static void Stop()
        {
            if (ShipThread.IsAlive)
            {
                ShipThread.Abort();
                ShipThread = null;
            }
        }

        public static void UpdateGrid()
        {
            var _lastupdatedTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(50);
                if ((DateTime.Now - _lastupdatedTime).TotalSeconds >= 1)
                {
                    if ((string)Form1.instance.Invoke(
                            new Func<string>(() => Form1.instance.TabControl.SelectedTab.Name)) != "tabPage6")
                    {
                        continue;
                    }

                    if (Form1.instance.WindowState == FormWindowState.Minimized)
                    {
                        continue;
                    }

                    _lastupdatedTime = DateTime.Now;
                    if (Form1.instance.ShipGrid.InvokeRequired)
                    {
                        var newships = ShipBinding.GetShips();
                        MethodInvoker meth = () =>
                            {
                                foreach (var clmn in Form1.instance.ShipGrid.Columns)
                                {
                                    if ((clmn as DataGridViewTextBoxColumn) != null)
                                    {
                                        var b = (DataGridViewTextBoxColumn)clmn;

                                        b.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                        b.Resizable = DataGridViewTriState.False;
                                        b.ReadOnly = true;
                                    }
                                }
                               
                               

                                foreach (var bld in newships)
                                {
                                    if (ShipBinding.Ships.Where(n => n.ID == bld.ID).FirstOrDefault() == null)
                                    {
                                        var bld2 = bld;

                                        ShipBinding.Ships.Add(bld2);
                                    }
                                    else
                                    {
                                        var old = ShipBinding.Ships.First(n => n.ID == bld.ID);
                                        if (old.InPortAt != bld.InPortAt)
                                        {
                                            old.InPortAt = bld.InPortAt;
                                        }

                                        if (old.Route != bld.Route)
                                        {
                                            old.Route = bld.Route;
                                        }

                                        // edit
                                    }
                                }

                                Form1.instance.ShipGrid.Refresh();
                                Form1.instance.ShipGrid.Update();
                            };

                        Form1.instance.ShipGrid.BeginInvoke(meth);
                    }
                }
            }
        }

        public static class ShipBinding
        {
            public static BindingList<Ship> Ships = new BindingList<Ship>();

            public static BindingList<Ship> GetShips()
            {
                var ret = new BindingList<Ship>();
                if (Core.LocalPlayer == null)
                {
                    return ret;
                }

                if (Core.LocalPlayer.Buildings == null)
                {
                    return ret;
                }

                foreach (var ship in Core.LocalPlayer.Ships.Where(n => n.Activated != 0))
                {
                    
                    var name = ship.GetShipName();
                    var Ship = new Ship();
                    Ship.ID = ship.InstId;
                    Ship.Name = name;

                    var willatportat = string.Empty;
                    if (ship.Sent != 0)
                    {
                        try
                        {
                            Ship.Route = LocalizationCache.GetNameFromLoc(ship.GetTravelName(), string.Empty);
                            if (ship.Type == "social_contract")
                            {
                                Ship.Route = PrivateLocal.SHIPS_SOCIAL_CONTRACT;
                            }

                            var willatportattime = ship.Sent + ship.GetTravelTime();

                            // lol xD 
                            if ((TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(willatportattime)).TotalSeconds > 0)
                            {
                                willatportat = "--:--:--";
                            }
                            else
                            {
                                willatportat =
                                    (TimeUtils.FixedUTCTime - TimeUtils.FromUnixTime(willatportattime)).ToString(
                                        @"hh\:mm\:ss");
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    Ship.InPortAt = willatportat;
                    ret.Add(Ship);
                }

                return ret;
            }
        }

        public class Ship
        {
            private bool _ignored;

            public int ID { get; set; }

            public string Name { get; set; }

            public string InPortAt { get; set; }

            public string Route { get; set; }

            public bool Ignored
            {
                get
                {
                    return Core.Config.ignoredships.Contains(ID);
                }
                set
                {
                    if (value)
                    {
                        if (!Core.Config.ignoredships.Contains(ID))
                        {
                            var snapshot = Core.Config.ignoredships;
                            snapshot.Add(ID);
                            Core.Config.ignoredships = snapshot;
                        }
                        
                    }
                    else
                    {
                        if (Core.Config.ignoredships.Contains(ID))
                        {
                            var snapshot = Core.Config.ignoredships;
                            snapshot.Remove(ID);
                            Core.Config.ignoredships = snapshot;
                        } 
                    }
                    this._ignored = value;
                }
            }
        }
    }
    public class RadioGroupBox : GroupBox
    {
        public event EventHandler SelectedChanged = delegate { };

        int _selected;
        public int Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                int val = 0;
                var radioButton = this.Controls.OfType<RadioButton>()
                    .FirstOrDefault(radio =>
                        radio.Tag != null 
                        && int.TryParse(radio.Tag.ToString(), out val) && val == value);

                if (radioButton != null)
                {
                    radioButton.Checked = true;
                    _selected = val;
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            var radioButton = e.Control as RadioButton;
            if (radioButton != null)
                radioButton.CheckedChanged += radioButton_CheckedChanged;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radio = (RadioButton)sender;
            int val = 0;
            if (radio.Checked && radio.Tag != null 
                              && int.TryParse(radio.Tag.ToString(), out val))
            {
                _selected = val;
                SelectedChanged(this, new EventArgs());
            }
        }
    }
}