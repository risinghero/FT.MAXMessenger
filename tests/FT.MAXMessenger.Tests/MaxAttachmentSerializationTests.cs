using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FT.MAXMessenger.Tests
{
    public class MaxAttachmentSerializationTests
    {
        [Fact]
        public void AttachmentPayload_JToken_PreservesOriginalPropertyNames()
        {
            var request = new MaxSendMessageRequest
            {
                ChatId = "chat",
                Text = "text",
                Attachments = new[]
                {
                    new MaxAttachment
                    {
                        Type = MaxUploadTypes.File,
                        Payload = JToken.Parse("{\"fileId\":3379897271,\"token\":\"abc\"}")
                    }
                }
            };

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                {
                    NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            });

            Assert.Contains("\"payload\":{\"fileId\":3379897271,\"token\":\"abc\"}", json);
            Assert.DoesNotContain("file_id", json);
        }
    }
}
