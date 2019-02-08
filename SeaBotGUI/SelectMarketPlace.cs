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
namespace SeaBotGUI
{
    #region

    using System.Collections.Generic;
    using System.Windows.Forms;

    using SeaBotCore;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Utils;

    #endregion

    public partial class SelectMarketPlace : Form
    {
        private readonly List<IgnoreMarketplaceData.IgnoreItem> locallist = new List<IgnoreMarketplaceData.IgnoreItem>();

        public SelectMarketPlace()
        {
            this.InitializeComponent();

            if (Core.GlobalData != null && Core.GlobalData.Inventory != null)
            {
                foreach (var item in AutoTools.GetUsableMarketplacePoints())
                {
                    if (Core.Config.marketitems.Contains(item.Id))
                    {
                        this.locallist.Add(new IgnoreMarketplaceData.IgnoreItem { Id = item.Id, Selected = true });
                    }
                    else
                    {
                        this.locallist.Add(new IgnoreMarketplaceData.IgnoreItem { Id = item.Id, Selected = false });
                    }
                }
            }

            ((ListBox)this.checkedListBox1).DataSource = this.locallist;
            ((ListBox)this.checkedListBox1).DisplayMember = "Name";
            ((ListBox)this.checkedListBox1).ValueMember = "Selected";
            for (var i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                var obj = (IgnoreMarketplaceData.IgnoreItem)this.checkedListBox1.Items[i];
                this.checkedListBox1.SetItemChecked(i, obj.Selected);
            }

            this.checkedListBox1.ItemCheck += this.CheckedListBox1_ItemCheck;
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var obj = (IgnoreMarketplaceData.IgnoreItem)this.checkedListBox1.Items[e.Index];
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

            // throw new NotImplementedException();
        }
    }

    public class IgnoreMarketplaceData
    {
        public class IgnoreItem
        {
            public int Id { get; set; }

            public string Name => MaterialDB.GetLocalizedName(this.Id);

            public bool Selected { get; set; }
        }
    }
}