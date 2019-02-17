using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBotGUI.Debug
{
    using System.ComponentModel;

    using SeaBotCore;

    public static class DebugGUI
    {
            public static  BindingList<BestAlgo> Algos = new BindingList<BestAlgo>();
            private static Thread BestAlgoThread;

            public static void Start()
            {
                if (BestAlgoThread == null)
                {
                BestAlgoThread = new Thread(UpdateBestStrategy);
                BestAlgoThread.IsBackground = true;
                BestAlgoThread.Start();
                }
            }

            public static void Stop()
            {
                if (BestAlgoThread.IsAlive)
                {
                BestAlgoThread.Abort();
                BestAlgoThread = null;
                }
            }
        public static void UpdateBestStrategy()
        {
            var _lastupdatedTime = DateTime.Now;
            while (true)
            {
                Thread.Sleep(50);
                if ((DateTime.Now - _lastupdatedTime).TotalSeconds >= 1)
                {
                if (SeaBotCore.Core.GlobalData == null || !Core.Config.debug)
                {
                    continue;
                }
                _lastupdatedTime = DateTime.Now;
                var list = SeaBotCore.Utils.AutoTools.NeededItemsForUpgrade();
                var plist = SeaBotCore.Utils.AutoTools.NeededItemsForUpgradePercentage();
                var bestalgolist = new List<BestAlgo>();
                foreach (var item in list)
                {
                    var ret = new BestAlgo();
                    ret.Amount = item.Value;
                    ret.ID = item.Key;
                    var pctg = plist.Where(n => n.Key == item.Key).FirstOrDefault();
                    ret.Percentage = pctg.Value;
                    ret.Name = SeaBotCore.Data.Materials.MaterialDB.GetLocalizedName(item.Key);
                    bestalgolist.Add(ret);
                }

                if (Form1.instance.NeededAlgoGrid.InvokeRequired)
                {

                    MethodInvoker meth = () =>
                        {
                            foreach (DataGridViewTextBoxColumn clmn in Form1.instance.NeededAlgoGrid.Columns)
                            {
                                clmn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                clmn.Resizable = DataGridViewTriState.False;
                            }

                            foreach (var bld in bestalgolist)
                            {
                                if (DebugGUI.Algos.Where(n => n.ID == bld.ID).FirstOrDefault() == null)
                                {
                                    Algos.Add(bld);
                                }
                                else
                                {
                                    var old = Algos.First(n => n.ID == bld.ID);
                                    if (old.Amount != bld.Amount)
                                    {
                                        old.Amount = bld.Amount;
                                    }

                                    if (old.Percentage != bld.Percentage)
                                    {
                                        old.Percentage = bld.Percentage;
                                    }

                                    // edit
                                }
                            }

                            Form1.instance.NeededAlgoGrid.Refresh();
                            Form1.instance.NeededAlgoGrid.Update();
                        };

                    Form1.instance.NeededAlgoGrid.BeginInvoke(meth);

                }
                }
            }
        }
            public class BestAlgo
        {
            public string Name {
                get;set;  }

            public int ID { get; set; }

            public int Amount { get; set; }

            public decimal Percentage { get; set; }
            
        }
    }
    }

