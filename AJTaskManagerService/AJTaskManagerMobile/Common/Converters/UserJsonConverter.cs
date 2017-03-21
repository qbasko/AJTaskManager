using System;
using AJTaskManagerMobile.Model.DTO;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Common.Converters
{
    public class UserJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(User);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var userAsString = serializer.Deserialize<string>(reader);
            return userAsString == null ? null : JsonConvert.DeserializeObject<User>(userAsString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var userAsString = JsonConvert.SerializeObject(value);
            serializer.Serialize(writer, userAsString);
        }
    }
}
