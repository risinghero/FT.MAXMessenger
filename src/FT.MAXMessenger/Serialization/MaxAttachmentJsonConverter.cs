using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace FT.MAXMessenger
{
    internal sealed class MaxAttachmentJsonConverter : JsonConverter<MaxAttachment>
    {
        public override void WriteJson(JsonWriter writer, MaxAttachment value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(value.Type);

            if (value.Payload != null)
            {
                writer.WritePropertyName("payload");
                if (value.Payload is JToken token)
                    token.WriteTo(writer);
                else
                    serializer.Serialize(writer, value.Payload);
            }

            writer.WriteEndObject();
        }

        public override MaxAttachment ReadJson(JsonReader reader, Type objectType, MaxAttachment existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Null)
                return null;

            return new MaxAttachment
            {
                Type = token["type"]?.Value<string>(),
                Payload = token["payload"]
            };
        }
    }
}
