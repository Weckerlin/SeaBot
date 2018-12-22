using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Defenitions
{
    public class BarrelDefenitions
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
        [J("version_id")] public long VersionId { get; set; }
        [J("name")] public string Name { get; set; }
        [J("player_level")] public long PlayerLevel { get; set; }
        [J("event_id")] public long EventId { get; set; }
        [J("model_name")] public string ModelName { get; set; }
        [J("bounds_min")] public string BoundsMin { get; set; }
        [J("bounds_max")] public string BoundsMax { get; set; }
        [J("spawn_time")] public string SpawnTime { get; set; }
        [J("lifespan")] public long Lifespan { get; set; }
        [J("sound")] public string Sound { get; set; }
        [J("positions")] public string Positions { get; set; }
        [J("materials")] public Materials Materials { get; set; }
    }

    public partial class Materials
    {
        [J("material")] public List<Material> Material { get; set; }
    }

        public partial class Material
        {
            public string DefId { get; set; }
        [J("id")] public long Id { get; set; }
        [J("type")] public string Type { get; set; }
        [J("koef")] public double Koef { get; set; }
        [J("exponent_min")] public double ExponentMin { get; set; }
        [J("exponent_max")] public double ExponentMax { get; set; }
        [J("offset_min")] public long OffsetMin { get; set; }
        [J("offset_max")] public long OffsetMax { get; set; }
    }
}
}
