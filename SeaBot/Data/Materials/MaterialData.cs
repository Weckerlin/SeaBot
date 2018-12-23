using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Materials
{
    public class MaterialsData
    {
        public partial class Root
        {
            [J("items")] public Items Items { get; set; }
        }

        public partial class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public partial class Item
        {
            [J("def_id")] public long DefId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("limited")] public long Limited { get; set; }
            [J("disposable")] public long Disposable { get; set; }
            [J("texture")] public string Texture { get; set; }
        }
    }
}
