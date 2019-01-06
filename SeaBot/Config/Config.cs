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
        public string server_token { get; set; } = ""; //done
        public bool debug { get; set; } = false; //done
        public int woodlimit { get; set; } = 0;
        public int ironlimit { get; set; } = 0;
        public int stonelimit { get; set; } = 0;
        public bool collectfish { get; set; } = false;
        public bool prodfactory { get; set; } = false; 
        public bool collectfactory { get; set; } = false; 
        public bool autoupgrade { get; set; } = false;
        public bool autoship { get; set; } = false;
        public bool finishupgrade { get; set; } = false;
        public bool barrelhack { get; set; } = false;
        public bool upgradeonlyfactory { get; set; } = false;
        public int barrelinterval { get; set; } = 22;
        public int hibernateinterval { get; set; } = 5;
        public string telegramtoken { get; set; } = "";
        public string autoshiptype { get; set; } = "coins";
        public bool autoshipprofit { get; set; } = false;

        
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
