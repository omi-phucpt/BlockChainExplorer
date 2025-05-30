using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlockchainExplorer.Models.Common
{
    public class Chains
    {
        public List<Chain> networks { get; set; }
    }

    public class Chain
    {
        public int chainIndex { get; set; }
        public string chortName { get; set; }
        public string name { get; set; }
        public string logoUrl { get; set; }
    }

    public class Network
    {
        [Key]
        [JsonConverter(typeof(StringToIntConverter))]
        public int ChainIndex { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }

    public class StringToIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return int.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
