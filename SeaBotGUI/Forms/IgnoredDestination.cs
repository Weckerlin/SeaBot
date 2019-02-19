using SeaBotCore;
using SeaBotCore.BotMethods.ShipManagment.SendShip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBotGUI.Forms
{
    using SeaBotCore.Config;

    public partial class IgnoredDestination : Form
    {
        private readonly List<IgnoredDest> locallist = new List<IgnoredDest>();

        private class IgnoredDest
        {

            public ShipDestType desttype;

            public int DefId { get; set; }

            public string Name { get; set; }

            public bool Selected =>
                Core.Config.ignoreddestination
                    .Count(n => n.Destination == this.desttype && n.DefId == this.DefId) != 0;
        }

        SeaBotCore.Config.ShipDestType DestinationType;
        public IgnoredDestination(SeaBotCore.Config.ShipDestType type)
        {
            InitializeComponent();
            DestinationType = type;
            switch(DestinationType)
            {
                case SeaBotCore.Config.ShipDestType.Outpost:
                    BindOutpost();
                    break;
                case SeaBotCore.Config.ShipDestType.Upgradable:
                    BindUpgradable();
                    break;
                case SeaBotCore.Config.ShipDestType.Contractor:
                    BindContractor();
                    break;
            }

            ((ListBox)this.checkedListBox1).DataSource = this.locallist;
            ((ListBox)this.checkedListBox1).DisplayMember = "Name";
            ((ListBox)this.checkedListBox1).ValueMember = "Selected";
            for (var i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                var obj = (IgnoredDest)this.checkedListBox1.Items[i];
                this.checkedListBox1.SetItemChecked(i, obj.Selected);
            }
            checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
        }

        private void BindContractor()
        {
            if (Core.GlobalData != null && Core.GlobalData.Inventory != null)
            {
                var definitions = SeaBotCore.Data.Definitions.Definitions.ConDef.Items.Item.Where(n => (Core.GlobalData.Contracts.Count(b => b.DefId == n.DefId) != 0));
                foreach (var def in definitions)
                {
                    IgnoredDest item = new IgnoredDest();
                    item.desttype = this.DestinationType;
                    item.DefId = def.DefId;
                    item.Name = SeaBotCore.Cache.LocalizationCache.GetNameFromLoc(def.NameLoc, def.Name);
                    locallist.Add(item);
                }
            }
        }

        private void BindUpgradable()
        {
            if (Core.GlobalData != null && Core.GlobalData.Inventory != null)
            {
                var sito = 
              Core.GlobalData.Upgradeables.Where(
                              n => n.Amount != 0 && n.Progress < n.Amount
                                 );
                var definitions = SeaBotCore.Data.Definitions.Definitions.UpgrDef.Items.Item.Where(n => (sito.Count(b=>b.DefId==n.DefId)!=0));
                foreach (var def in definitions)
                {
                    IgnoredDest item = new IgnoredDest();
                    item.desttype = this.DestinationType;
                    item.DefId = def.DefId;
                    item.Name = SeaBotCore.Cache.LocalizationCache.GetNameFromLoc(def.NameLoc, def.Name);
                    locallist.Add(item);
                }
            }
        }

        public void BindOutpost()
        {
            if (Core.GlobalData != null && Core.GlobalData.Inventory != null)
            {
                var openedoutpost = Core.GlobalData.Outposts.Where(n => !n.Done && n.Crew < n.RequiredCrew);
                var lockedoutposts = SendingHelper.GetUnlockableOutposts(); 
                foreach(var opened in openedoutpost)
                {
                   IgnoredDest dest = new IgnoredDest();
                    var item = SeaBotCore.Data.Definitions.Definitions.OutpostDef.Items.Item.Where(n => n.DefId == opened.DefId).FirstOrDefault();
                    if(item == null)
                    {
                        continue;
                    }
                    dest.Name = SeaBotCore.Cache.LocalizationCache.GetNameFromLoc(item.NameLoc, item.Name);
                    dest.DefId = opened.DefId;
                    dest.desttype = this.DestinationType;
                    locallist.Add(dest);
                }
                foreach (var lockedoutpost in lockedoutposts )
                {
                    IgnoredDest item = new IgnoredDest();
                    item.DefId = lockedoutpost.DefId;
                    item.Name = SeaBotCore.Cache.LocalizationCache.GetNameFromLoc(lockedoutpost.NameLoc, lockedoutpost.Name);
                       locallist.Add(item);
                       item.desttype = this.DestinationType;
                }
            }

        }
        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var obj = (IgnoredDest)this.checkedListBox1.Items[e.Index];
            if (obj != null)
            {
                var exists = Core.Config.ignoreddestination.Count(n => n.Destination == DestinationType && n.DefId == obj.DefId) != 0;
                if (e.NewValue == CheckState.Checked)
                {
                    if (!exists)
                    {
                        var snapshot = Core.Config.ignoreddestination;
                        snapshot.Add(new SeaBotCore.Config.IgnoredDestination() {DefId = obj.DefId,Destination = DestinationType });
                        Core.Config.ignoreddestination = snapshot;
                    }
                }
                else
                {
                    if (exists)
                    {
                        var snapshot = Core.Config.ignoreddestination;
                        snapshot.Remove(snapshot.Where(n=>n.DefId==obj.DefId&&n.Destination == DestinationType).FirstOrDefault());
                        Core.Config.ignoreddestination = snapshot;
                    }
                }
            }

            // throw new NotImplementedException();
        }
     
    }
}
