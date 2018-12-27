using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;
namespace SeaBotCore.Data.Defenitions
{
    class ShipDefenitions
    {
        public partial class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }
        public partial class Root
        {
            [J("items")] public Items Items { get; set; }
        }
        public partial class Item
        {

            [J("def_id")] public long DefId { get; set; }
            [J("version_id")] public long VersionId { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("desc_loc")] public string DescLoc { get; set; }
            [J("ship_type")] public string ShipType { get; set; }
            [J("propulsion")] public string Propulsion { get; set; }
            [J("slot_usage")] public long SlotUsage { get; set; }
            [J("mass")] [JsonConverter(typeof(ParseStringConverter))] public long Mass { get; set; }
            [J("start_level")] public long StartLevel { get; set; }
            [J("player_level")] public long PlayerLevel { get; set; }
            [J("req_type")] public string ReqType { get; set; }
            [J("req_id")] public long ReqId { get; set; }
            [J("req_level")] public long ReqLevel { get; set; }
            [J("max_count")] public long MaxCount { get; set; }
            [J("shop_order_id")] public long ShopOrderId { get; set; }
            [J("event_id")] public long EventId { get; set; }
            [J("museum_xp")] public long MuseumXp { get; set; }
            [J("hide")] public long Hide { get; set; }
            [J("bounds_min")] public string BoundsMin { get; set; }
            [J("bounds_max")] public string BoundsMax { get; set; }
            [J("animations", NullValueHandling = N.Ignore)] public Animations Animations { get; set; }
            [J("levels", NullValueHandling = N.Ignore)] public LevelsClass Levels { get; set; }
            [J("capacity_levels", NullValueHandling = N.Ignore)] public Levels CapacityLevels { get; set; }
            [J("sailors_levels", NullValueHandling = N.Ignore)] public Levels SailorsLevels { get; set; }
            [J("particles", NullValueHandling = N.Ignore)] public Particles Particles { get; set; }
            [J("particles_new", NullValueHandling = N.Ignore)] public ParticlesNew ParticlesNew { get; set; }
        }

        public partial class Animations
        {
            [J("animation")] public List<Animation> Animation { get; set; }
        }

        public partial class Animation
        {
            [J("id")] public long Id { get; set; }
            [J("data")] public string Data { get; set; }
        }

        public partial class Levels
        {
            [J("level")] public List<CapacityLevelsLevel> Level { get; set; }
        }

        public partial class CapacityLevelsLevel
        {
            [J("id")] public long Id { get; set; }
            [J("xp")] public long Xp { get; set; }
            [J("gem_price")] public long GemPrice { get; set; }
            [J("capacity", NullValueHandling = N.Ignore)] public long? Capacity { get; set; }
            [J("materials")] public Materials Materials { get; set; }
            [J("sailors", NullValueHandling = N.Ignore)] public long? Sailors { get; set; }
        }

        public partial class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public partial class Material
        {
            [J("id")] public long Id { get; set; }
            [J("amount")] public long Amount { get; set; }
        }

        public partial class LevelsClass
        {
            [J("level")] public List<LevelsLevel> Level { get; set; }
        }

        public partial class LevelsLevel
        {
            [J("id")] public long Id { get; set; }
            [J("xp")] public long Xp { get; set; }
            [J("gem_price")] public long GemPrice { get; set; }
            [J("sailors")] public long Sailors { get; set; }
            [J("capacity")] public long Capacity { get; set; }
            [J("speed")] public long Speed { get; set; }
            [J("materials", NullValueHandling = N.Ignore)] public Materials Materials { get; set; }
        }

        public partial class Particles
        {
            [J("particle")] public List<Animation> Particle { get; set; }
        }

        public partial class ParticlesNew
        {
            [J("particle")] public List<Particle> Particle { get; set; }
        }

        public partial class Particle
        {
            [J("particle_id")] public long ParticleId { get; set; }
            [J("data")] public string Data { get; set; }
        }
        internal class ParseStringConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null) return null;
                var value = serializer.Deserialize<string>(reader);
                long l;
                if (Int64.TryParse(value, out l))
                {
                    return l;
                }
                throw new Exception("Cannot unmarshal type long");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                if (untypedValue == null)
                {
                    serializer.Serialize(writer, null);
                    return;
                }
                var value = (long)untypedValue;
                serializer.Serialize(writer, value.ToString());
                return;
            }

            public static readonly ParseStringConverter Singleton = new ParseStringConverter();
        }
    }
}
