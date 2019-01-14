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

using System;
using System.ComponentModel;
using System.IO;
using System.Web.Script.Serialization;
using SeaBotCore.Localizaion;

namespace SeaBotCore.Config
{
    public class Config : INotifyPropertyChanged
    {
        private bool _acceptedresponsibility;
        private bool _autoship;
        private bool _autoshipprofit;
        private string _autoshiptype = "coins";
        private bool _autoupgrade;
        private bool _barrelhack;
        private int _barrelinterval = 22;
        private bool _collectfactory;
        private bool _collectfish;
        private bool _debug;
        private bool _finishupgrade;
        private int _hibernateinterval = 5;
        private int _ironlimit;
        private bool _prodfactory;
        private string _serverToken = string.Empty;
        private bool _sleepenabled;
        private int _sleepevery = 20;
        private bool _sleepeveryhrs;
        private int _sleepfor = 25;
        private bool _sleepforhrs;
        private bool _smartsleepenabled;
        private int _stonelimit;
        private string _telegramtoken = string.Empty;
        private bool _upgradeonlyfactory;
        private int _woodlimit;

        public bool sleepenabled
        {
            get => _sleepenabled;
            set
            {
                _sleepenabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("sleepenabled"));
            }
        } //done

        public bool smartsleepenabled
        {
            get => _smartsleepenabled;
            set
            {
                _smartsleepenabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("smartsleepenabled"));
            }
        } //done

        public int sleepevery
        {
            get => _sleepevery;
            set
            {
                _sleepevery = value;
                OnPropertyChanged(new PropertyChangedEventArgs("sleepevery"));
            }
        } //done

        public int sleepfor
        {
            get => _sleepfor;
            set
            {
                _sleepfor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("sleepfor"));
            }
        } //done

        public bool sleepforhrs
        {
            get => _sleepforhrs;
            set
            {
                _sleepforhrs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("sleepforhrs"));
            }
        } //done

        public bool sleepeveryhrs
        {
            get => _sleepeveryhrs;
            set
            {
                _sleepeveryhrs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("sleepeveryhrs"));
            }
        } //done

        public bool acceptedresponsibility
        {
            get => _acceptedresponsibility;
            set
            {
                _acceptedresponsibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("acceptedresponsibility"));
            }
        } //done

        public string server_token
        {
            get => _serverToken;
            set
            {
                _serverToken = value;
                OnPropertyChanged(new PropertyChangedEventArgs("server_token"));
            }
        } //done

        public bool debug
        {
            get => _debug;
            set
            {
                _debug = value;
                OnPropertyChanged(new PropertyChangedEventArgs("debug"));
            }
        } //done

        public int woodlimit
        {
            get => _woodlimit;
            set
            {
                _woodlimit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("woodlimit"));
            }
        }

        public int ironlimit
        {
            get => _ironlimit;
            set
            {
                _ironlimit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ironlimit"));
            }
        }

        public int stonelimit
        {
            get => _stonelimit;
            set
            {
                _stonelimit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("stonelimit"));
            }
        }

        public bool collectfish
        {
            get => _collectfish;
            set
            {
                _collectfish = value
                    ;
                OnPropertyChanged(new PropertyChangedEventArgs("collectfish"));
            }
        }

        public bool prodfactory
        {
            get => _prodfactory;
            set
            {
                _prodfactory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("prodfactory"));
            }
        }

        public bool collectfactory
        {
            get => _collectfactory;
            set
            {
                _collectfactory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("collectfactory"));
            }
        }


        public bool autoupgrade
        {
            get => _autoupgrade;
            set
            {
                _autoupgrade = value;
                OnPropertyChanged(new PropertyChangedEventArgs("autoupgrade"));
            }
        }

        public bool autoship
        {
            get => _autoship;
            set
            {
                _autoship = value;
                OnPropertyChanged(new PropertyChangedEventArgs("autoship"));
            }
        }

        public bool finishupgrade
        {
            get => _finishupgrade;
            set
            {
                _finishupgrade = value;
                OnPropertyChanged(new PropertyChangedEventArgs("finishupgrade"));
            }
        }

        public bool barrelhack
        {
            get => _barrelhack;
            set
            {
                _barrelhack = value;
                OnPropertyChanged(new PropertyChangedEventArgs("barrelhack"));
            }
        }

        public bool upgradeonlyfactory
        {
            get => _upgradeonlyfactory;
            set
            {
                _upgradeonlyfactory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("upgradeonlyfactory"));
            }
        }

        public int barrelinterval
        {
            get => _barrelinterval;
            set
            {
                _barrelinterval = value;
                OnPropertyChanged(new PropertyChangedEventArgs("barrelinterval"));
            }
        }

        public int hibernateinterval
        {
            get => _hibernateinterval;
            set
            {
                _hibernateinterval = value;
                OnPropertyChanged(new PropertyChangedEventArgs("hibernateinterval"));
            }
        }

        public string telegramtoken
        {
            get => _telegramtoken;
            set
            {
                _telegramtoken = value;
                OnPropertyChanged(new PropertyChangedEventArgs("telegramtoken"));
            }
        }

        public string autoshiptype
        {
            get => _autoshiptype;
            set
            {
                _autoshiptype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("autoshiptype"));
            }
        }

        public bool autoshipprofit
        {
            get => _autoshipprofit;
            set
            {
                _autoshipprofit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("autoshipprofit"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }

    internal class Configurator
    {
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
                Logger.Logger.Warning(
                    Localization.CONFIG_CANT_SAVE);
            }
        }

        public static void Load()
        {
            if (File.Exists("config.json"))
            {
                var ser = new JavaScriptSerializer();

                Core.Config = ser.Deserialize<Config>(File.ReadAllText("config.json"));
            }
        }
    }
}