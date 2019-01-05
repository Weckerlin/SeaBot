using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using R = Newtonsoft.Json.Required;
using N = Newtonsoft.Json.NullValueHandling;
namespace SeaBotCore.Data.Defenitions
{
    public class EventsDefenitions
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
            [J("player_level")] public long PlayerLevel { get; set; }
            [J("start_time")] public long StartTime { get; set; }
            [J("sleep_time")] public long SleepTime { get; set; }
            [J("end_time")] public long EndTime { get; set; }
            [J("contractors")] public string Contractors { get; set; }
            [J("upgradeables")] public string Upgradeables { get; set; }
            [J("dealers")] public string Dealers { get; set; }
            [J("outposts")] public string Outposts { get; set; }
            [J("treasures")] public string Treasures { get; set; }
            [J("global_contractors")] public string GlobalContractors { get; set; }
            [J("crossroads")] public string Crossroads { get; set; }
            [J("barrel")] public Barrel Barrel { get; set; }
            [J("social_contract")] public long SocialContract { get; set; }
            [J("token_upgrade")] public string TokenUpgrade { get; set; }
            [J("city")] public string City { get; set; }
            [J("map")] public string Map { get; set; }
        }

        public partial struct Barrel
        {
            public long? Integer;
            public string String;

            public static implicit operator Barrel(long Integer) => new Barrel { Integer = Integer };
            public static implicit operator Barrel(string String) => new Barrel { String = String };
        }
        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    BarrelConverter.Singleton,
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
            };
        }
        internal class BarrelConverter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Barrel) || t == typeof(Barrel?);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                        var integerValue = serializer.Deserialize<long>(reader);
                        return new Barrel { Integer = integerValue };
                    case JsonToken.String:
                    case JsonToken.Date:
                        var stringValue = serializer.Deserialize<string>(reader);
                        return new Barrel { String = stringValue };
                }
                throw new Exception("Cannot unmarshal type Barrel");
            }

            public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
            {
                var value = (Barrel)untypedValue;
                if (value.Integer != null)
                {
                    serializer.Serialize(writer, value.Integer.Value);
                    return;
                }
                if (value.String != null)
                {
                    serializer.Serialize(writer, value.String);
                    return;
                }
                throw new Exception("Cannot marshal type Barrel");
            }

            public static readonly BarrelConverter Singleton = new BarrelConverter();
        }



    }
}
