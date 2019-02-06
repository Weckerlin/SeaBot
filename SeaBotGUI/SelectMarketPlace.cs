using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeaBotCore.Data.Materials;

namespace SeaBotGUI
{
    public partial class SelectMarketPlace : Form
    {
        public SelectMarketPlace()
        {
            InitializeComponent();
            if (SeaBotCore.Core.GlobalData != null&&SeaBotCore.Core.GlobalData.Inventory!=null)
            {
                foreach (var item in SeaBotCore.Core.GlobalData.Inventory)
                {
                    checkedListBox1.Items.Add(MaterialDB.GetLocalizedName(item.Id));
                }
            }
        }
    }
}
