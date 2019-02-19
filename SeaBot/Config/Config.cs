// SeaBotCore
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
namespace SeaBotCore.Config
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Web.Script.Serialization;

    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;

    #endregion

    public enum WorkshopType
    {
        MechanicalPart,

        Fuel,

        Concrete
    }
    public enum UpgradablyStrategy
    {
        Sailors,
        Loot
    }
    public enum ShipDestType
    {
        Upgradable,

        Outpost,

        Marketplace,

        Contractor,

        Auto,

        Wreck
    }
    public enum ChartData
    {
        Resources,

        PlayerInfo
    }

    public enum ChartType
    {
        Hour,

        Day
    }
    
    

    public class IgnoredDestination
    {
        public int DefId;
        public ShipDestType Destination;
    }
 

    public class Config : INotifyPropertyChanged
    {
        private bool _acceptedresponsibility;

        private bool _autoship;

        private ChartType _charttype = ChartType.Day;

        private ChartData _chartdata = ChartData.Resources;

        private UpgradablyStrategy _upgradablestrategy = UpgradablyStrategy.Sailors;

        private string _autoshiptype = "coins";

        private bool _autothresholdworkshop;

        private bool _autoupgrade;

        private bool _barrelhack;

        private int _barrelinterval = 22;

        private bool _collectfactory;

        private bool _collectfish;

        private bool _collectmuseum;

        private bool _debug;

        private bool _finishupgrade;

        private int _hibernateinterval = 5;

        private int _ironlimit;

        private LocalizationController.ELanguages _language = LocalizationController.GetDefaultLang();

        private List<int> _marketitems = new List<int>();

        private bool _prodfactory;

        private string _serverToken = string.Empty;

        private ShipDestType _shipdesttype = ShipDestType.Upgradable;

        private bool _sleepenabled;

        private int _sleepevery = 20;

        private bool _sleepeveryhrs;

        private int _sleepfor = 25;

        private bool _sleepforhrs;

        private bool _smartsleepenabled;

        private int _stonelimit;

        private string _telegramtoken = string.Empty;

        private int _thresholdconcrete;

        private int _thresholdfuel;

        private int _thresholdmechanical;

        private bool _upgradeonlyfactory;

        private int _woodlimit;

        private bool _exploitmode;

        private WorkshopType _workshoptype = WorkshopType.MechanicalPart;

        List<int> _ignoredships = new List<int>();

        List<IgnoredDestination> _ignoreddestination = new List<IgnoredDestination>();
      
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IgnoredDestination> ignoreddestination
        {
            get => this._ignoreddestination;
            set
            {
                this._ignoreddestination = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ignoreddestination"));

            }
        }
        public ChartType charttype
        {
            get => this._charttype;
            set
            {
                this._charttype = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("charttype"));
            }
        }

        public List<int> ignoredships
        {
            get => this._ignoredships;
            set
            {
                this._ignoredships = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ignoredships"));
            }
        }

        public ChartData chartdata
        {
            get => this._chartdata;
            set
            {
                this._chartdata = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("chartdata"));
            }
        }
        public bool exploitmode
        {
            get => this._exploitmode;
            set
            {
                this._exploitmode = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("exploitmode"));
            }
        }

        public bool acceptedresponsibility
        {
            get => this._acceptedresponsibility;
            set
            {
                this._acceptedresponsibility = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("acceptedresponsibility"));
            }
        }

        public bool autoship
        {
            get => this._autoship;
            set
            {
                this._autoship = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("autoship"));
            }
        }

        public UpgradablyStrategy upgradablestrategy
        {
            get => this._upgradablestrategy;
            set
            {
                this._upgradablestrategy = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("upgradablestrategy"));
            }
        }

        public string autoshiptype
        {
            get => this._autoshiptype;
            set
            {
                this._autoshiptype = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("autoshiptype"));
            }
        }

        public bool autothresholdworkshop
        {
            get => this._autothresholdworkshop;
            set
            {
                this._autothresholdworkshop = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("autothresholdworkshop"));
            }
        }

        public bool autoupgrade
        {
            get => this._autoupgrade;
            set
            {
                this._autoupgrade = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("autoupgrade"));
            }
        }

        public bool barrelhack
        {
            get => this._barrelhack;
            set
            {
                this._barrelhack = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("barrelhack"));
            }
        }

        public int barrelinterval
        {
            get => this._barrelinterval;
            set
            {
                this._barrelinterval = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("barrelinterval"));
            }
        }

        public bool collectfactory
        {
            get => this._collectfactory;
            set
            {
                this._collectfactory = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("collectfactory"));
            }
        }

        public bool collectfish
        {
            get => this._collectfish;
            set
            {
                this._collectfish = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("collectfish"));
            }
        }

        public bool collectmuseum
        {
            get => this._collectmuseum;
            set
            {
                this._collectmuseum = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("collectmuseum"));
            }
        }

        public bool debug
        {
            get => this._debug;
            set
            {
                this._debug = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("debug"));
            }
        }

        public bool finishupgrade
        {
            get => this._finishupgrade;
            set
            {
                this._finishupgrade = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("finishupgrade"));
            }
        }

        public int hibernateinterval
        {
            get => this._hibernateinterval;
            set
            {
                this._hibernateinterval = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("hibernateinterval"));
            }
        }

        public int ironlimit
        {
            get => this._ironlimit;
            set
            {
                this._ironlimit = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ironlimit"));
            }
        }

        public LocalizationController.ELanguages language
        {
            get => this._language;
            set
            {
                this._language = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("language"));
            }
        }

        public List<int> marketitems
        {
            get => this._marketitems;
            set
            {
                this._marketitems = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("marketitems"));
            }
        }

        public bool prodfactory
        {
            get => this._prodfactory;
            set
            {
                this._prodfactory = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("prodfactory"));
            }
        }

        public string server_token
        {
            get => this._serverToken;
            set
            {
                this._serverToken = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("server_token"));
            }
        }

        public ShipDestType shipdesttype
        {
            get => this._shipdesttype;
            set
            {
                this._shipdesttype = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("shipdesttype"));
            }
        }

        public bool sleepenabled
        {
            get => this._sleepenabled;
            set
            {
                this._sleepenabled = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("sleepenabled"));
            }
        }

        public int sleepevery
        {
            get => this._sleepevery;
            set
            {
                this._sleepevery = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("sleepevery"));
            }
        }

        public bool sleepeveryhrs
        {
            get => this._sleepeveryhrs;
            set
            {
                this._sleepeveryhrs = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("sleepeveryhrs"));
            }
        }

        public int sleepfor
        {
            get => this._sleepfor;
            set
            {
                this._sleepfor = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("sleepfor"));
            }
        }

        public bool sleepforhrs
        {
            get => this._sleepforhrs;
            set
            {
                this._sleepforhrs = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("sleepforhrs"));
            }
        }

        public bool smartsleepenabled
        {
            get => this._smartsleepenabled;
            set
            {
                this._smartsleepenabled = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("smartsleepenabled"));
            }
        }

        public int stonelimit
        {
            get => this._stonelimit;
            set
            {
                this._stonelimit = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("stonelimit"));
            }
        }

        public string telegramtoken
        {
            get => this._telegramtoken;
            set
            {
                this._telegramtoken = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("telegramtoken"));
            }
        }

        public int thresholdconcrete
        {
            get => this._thresholdconcrete;
            set
            {
                this._thresholdconcrete = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("thresholdconcrete"));
            }
        }

        public int thresholdfuel
        {
            get => this._thresholdfuel;
            set
            {
                this._thresholdfuel = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("thresholdfuel"));
            }
        }

        public int thresholdmechanical
        {
            get => this._thresholdmechanical;
            set
            {
                this._thresholdmechanical = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("thresholdmechanical"));
            }
        }

        public bool upgradeonlyfactory
        {
            get => this._upgradeonlyfactory;
            set
            {
                this._upgradeonlyfactory = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("upgradeonlyfactory"));
            }
        }

        public int woodlimit
        {
            get => this._woodlimit;
            set
            {
                this._woodlimit = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("woodlimit"));
            }
        }

        public WorkshopType workshoptype
        {
            get => this._workshoptype;
            set
            {
                this._workshoptype = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("workshoptype"));
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            this.PropertyChanged?.Invoke(this, eventArgs);
        }
    }

    internal class Configurator
    {
        public static void Load()
        {
            if (File.Exists("config.json"))
            {
                var ser = new JavaScriptSerializer();

                Core.Config = ser.Deserialize<Config>(File.ReadAllText("config.json"));
            }
        }

        public static void Save()
        {
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(Core.Config);
            try
            {
                File.WriteAllText("config.json", json);
            }
            catch (Exception)
            {
                Logger.Warning(Localization.CONFIG_CANT_SAVE);
            }
        }
    }
}