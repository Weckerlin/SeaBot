using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeaBotCore;
using SeaBotCore.Config;
using SeaBotCore.Data;
using SeaBotCore.Data.Definitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Utils;

namespace SeaBotGUI
{
    public partial class SelectMarketPlace : Form
    {
        private List<IgnoreMarketplaceData.IgnoreItem> locallist = new List<IgnoreMarketplaceData.IgnoreItem>();
        public SelectMarketPlace()
        {
            InitializeComponent();
          
            if (SeaBotCore.Core.GlobalData != null&&SeaBotCore.Core.GlobalData.Inventory!=null)
            {
                foreach (var item in AutoTools.GetUsableMarketplacePoints())
                {
                    
                    if (Core.Config.marketitems.Contains(item.Id))
                    {
                        locallist.Add(new IgnoreMarketplaceData.IgnoreItem(){Id = item.Id,Selected = true});
                    }
                    else
                    {
                        locallist.Add(new IgnoreMarketplaceData.IgnoreItem(){Id = item.Id,Selected = false});
                    }
                }

            }
            ((ListBox)this.checkedListBox1).DataSource = locallist;
            ((ListBox)this.checkedListBox1).DisplayMember = "Name";
            ((ListBox)this.checkedListBox1).ValueMember = "Selected";
             for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                IgnoreMarketplaceData.IgnoreItem obj = (IgnoreMarketplaceData.IgnoreItem)checkedListBox1.Items[i];
                checkedListBox1.SetItemChecked(i, obj.Selected);
            }
            this.checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var obj = (IgnoreMarketplaceData.IgnoreItem) checkedListBox1.Items[e.Index];
            if (obj != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (!Core.Config.marketitems.Contains(obj.Id))
                    {
                        var snapshot = Core.Config.marketitems;
                        snapshot.Add(obj.Id);
                        Core.Config.marketitems = snapshot;
                    }
                }
                else
                {
                    if (Core.Config.marketitems.Contains(obj.Id))
                    {
                        var snapshot = Core.Config.marketitems;
                        snapshot.Remove(obj.Id);
                        Core.Config.marketitems = snapshot;
                    }
                }
            }
        //    throw new NotImplementedException();
        }

     
    }
    public class IgnoreMarketplaceData
    {
        public class IgnoreItem
        {
           
            public int Id { get; set; }
            public bool Selected { get; set; }
            public string Name => MaterialDB.GetLocalizedName(Id);
        }
    }
}
