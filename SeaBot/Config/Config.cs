using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SeaBotCore.Config
{
    public class Config : INotifyPropertyChanged
    {
        private string _serverToken = "";
        private bool _debug = false;
        private int _woodlimit = 0;
        private int _ironlimit = 0;
        private int _stonelimit = 0;
        private bool _collectfish = false;
        private bool _prodfactory = false;
        private bool _collectfactory = false;
        private bool _autoupgrade = false;
        private bool _autoship = false;
        private bool _finishupgrade = false;
        private bool _barrelhack = false;
        private bool _upgradeonlyfactory = false;
        private int _barrelinterval = 22;
        private int _hibernateinterval = 5;
        private string _telegramtoken = "";
        private string _autoshiptype = "coins";
        private bool _autoshipprofit = false;
        public event PropertyChangedEventHandler PropertyChanged;

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
            set { _autoship = value; OnPropertyChanged(new PropertyChangedEventArgs("autoship")); }
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

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }

    }

    class Configurator
    {
        public static void Save()
        {
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(Core.Config);
            File.WriteAllText("config.json", json);
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
