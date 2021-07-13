using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Json
{
    public class DictionaryJsonConverter : JsonConverter<IDictionary>
    {
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter     writer,
                                       IDictionary    value,
                                       JsonSerializer serializer)
        {
            var namingStrategy = (serializer.ContractResolver as DefaultContractResolver)?.NamingStrategy;

            var array = new JObject();
            foreach (DictionaryEntry entry in value)
            {
                var key = namingStrategy != null
                              ? namingStrategy.GetPropertyName(entry.Key.ToString(), false)
                              : entry.Key.ToString();
                var token = JToken.FromObject(entry.Value, serializer);
                array.Add(new JProperty(key, token));
            }

            array.WriteTo(writer);
        }

        public override IDictionary ReadJson(JsonReader     reader,
                                             Type           objectType,
                                             IDictionary    existingValue,
                                             bool           hasExistingValue,
                                             JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanRead is false. The type will skip the converter.");
        }
    }
}
