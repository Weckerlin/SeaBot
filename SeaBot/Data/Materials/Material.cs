using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBotCore.Data.Materials
{
 public static class MaterialDB
    {
        public static MaterialsData.Item GetItem(int id)
        {
            return Cache.GetMaterials().Items.Item.FirstOrDefault(n => n.DefId == id);
        }
        public static MaterialsData.Item GetItem(long id)
        {
            return Cache.GetMaterials().Items.Item.FirstOrDefault(n => n.DefId == id);
        }
        public static MaterialsData.Item GetItem(string name)
        {
        
            return Cache.GetMaterials().Items.Item.FirstOrDefault(n => n.Name==name);
        }

        public static List<MaterialsData.Item> GetAllItems()
        {
            return Cache.GetMaterials().Items.Item.ToList();
        }
    }
}
