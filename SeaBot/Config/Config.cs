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
        public event PropertyChangedEventHandler PropertyChanged;
        public string server_token = ""; //done
        public bool debug = false; //done
        public int woodlimit = 0;
        public int ironlimit = 0;
        public int stonelimit = 0;
        public bool collectfish = false;
        public bool prodfactory = false; 
        public bool collectfactory = false; 
        public bool autoupgrade = false;
        public bool autoship = false;
        public bool finishupgrade = false;
        public bool barrelhack = false;
        public bool upgradeonlyfactory = false;
        public int barrelinterval = 22;
        public int hibernateinterval = 5;
        public string telegramtoken = "";
        public string autoshiptype = "coins";
        public bool autoshipprofit = false;

        
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
