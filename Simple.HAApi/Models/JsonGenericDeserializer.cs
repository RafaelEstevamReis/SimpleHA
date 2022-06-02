using Newtonsoft.Json;
using System;

namespace Simple.HAApi.Models
{
    public class JsonGenericDeserializer : JsonConverter
    {
        // from https://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm

        public override bool CanConvert(Type objType) => true;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) { }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
            {
                return JsonSerializer.CreateDefault().Deserialize(reader).ToString();
            }

            return reader.Value?.ToString();
        }
    }
}
