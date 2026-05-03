using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FT.MAXMessenger.Tests
{
    public class MaxClientIntegrationTests
    {
        private static MaxClient CreateClient()
        {
            return new MaxClient(MaxTestSettings.GetAccessToken());
        }

        private static bool CanRunIntegrationTests()
        {
            return !string.IsNullOrWhiteSpace(MaxTestSettings.GetAccessToken());
        }

        private static string? GetTestChatId()
        {
            return MaxTestSettings.GetTestChatId();
        }

        private static bool CanRunChatIntegrationTests()
        {
            return CanRunIntegrationTests() && !string.IsNullOrWhiteSpace(GetTestChatId());
        }

        [Fact]
        public async Task GetMe_ReturnsCurrentBot()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();

            var result = await client.GetMe();

            Assert.NotNull(result);
            Assert.True(result.UserId.HasValue);
            Assert.True(result.IsBot.GetValueOrDefault());
        }

        [Fact]
        public async Task GetSubscriptions_ReturnsResponse()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();

            var result = await client.GetSubscriptions();

            Assert.NotNull(result);
            Assert.NotNull(result.Subscriptions);
        }

        [Fact]
        public async Task GetChats_ReturnsResponse()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();

            var result = await client.GetChats();

            Assert.NotNull(result);
            Assert.NotNull(result.Chats);
        }

        [Fact]
        public async Task GetChat_ReturnsConfiguredChat()
        {
            if (!CanRunChatIntegrationTests())
                return;

            var client = CreateClient();
            var chatId = GetTestChatId();

            var result = await client.GetChat(chatId);

            Assert.NotNull(result);
            Assert.Equal(chatId, result.ChatId);
        }

        [Fact]
        public async Task GetMessages_ByChat_ReturnsResponse()
        {
            if (!CanRunChatIntegrationTests())
                return;

            var client = CreateClient();
            var chatId = GetTestChatId();

            var result = await client.GetMessages(new MaxMessagesQuery
            {
                ChatId = chatId,
                Count = 10
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Messages);
        }

        [Fact]
        public async Task GetMessage_WhenChatHasMessages_ReturnsMessage()
        {
            if (!CanRunChatIntegrationTests())
                return;

            var client = CreateClient();
            var chatId = GetTestChatId();
            var messages = await client.GetMessages(new MaxMessagesQuery
            {
                ChatId = chatId,
                Count = 1
            });

            if (messages == null || messages.Messages == null || messages.Messages.Count == 0)
                return;

            var result = await client.GetMessage(messages.Messages[0].MessageId);

            Assert.NotNull(result);
            Assert.Equal(messages.Messages[0].MessageId, result.MessageId);
        }

        [Fact]
        public async Task SendMessage_ToTestChat_ReturnsCreatedMessage()
        {
            if (!CanRunChatIntegrationTests())
                return;

            var client = CreateClient();
            var chatId = GetTestChatId();
            var text = $"Integration test message {System.DateTimeOffset.UtcNow:O}";

            var result = await client.SendMessage(new MaxSendMessageRequest
            {
                ChatId = chatId,
                Text = text
            });

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CreateUpload_ForVideo_ReturnsUploadUrlAndToken()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();

            var result = await client.CreateUpload(new MaxCreateUploadRequest
            {
                Type = "video"
            });

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.Url));
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task GetUpdates_ReturnsResponse()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();

            var result = await client.GetUpdates(new MaxUpdatesQuery
            {
                Limit = 10,
                Timeout = 1
            });

            Assert.NotNull(result);
            Assert.NotNull(result.Updates);
        }

        [Fact]
        public async Task GetUpdates_WithMarker_ReturnsConsistentResponse()
        {
            if (!CanRunIntegrationTests())
                return;

            var client = CreateClient();
            var initialUpdates = await client.GetUpdates(new MaxUpdatesQuery
            {
                Limit = 1,
                Timeout = 1
            });

            Assert.NotNull(initialUpdates);
            Assert.NotNull(initialUpdates.Updates);
            Assert.True(initialUpdates.Marker.HasValue);

            var nextUpdates = await client.GetUpdates(new MaxUpdatesQuery
            {
                Limit = 1,
                Timeout = 1,
                Marker = initialUpdates.Marker
            });

            Assert.NotNull(nextUpdates);
            Assert.NotNull(nextUpdates.Updates);
            Assert.True(nextUpdates.Marker.HasValue);
            Assert.True(nextUpdates.Marker.Value >= initialUpdates.Marker.Value);

            for (var i = 0; i < nextUpdates.Updates.Count; i++)
            {
                var update = nextUpdates.Updates[i];
                Assert.NotNull(update);
                Assert.False(string.IsNullOrWhiteSpace(update.UpdateType));
            }
        }

        [Fact]
        public async Task UploadFile_AndSendMessageWithAttachment_MessageListContainsFile()
        {
            if (!CanRunChatIntegrationTests())
                return;

            var client = CreateClient();
            var chatId = GetTestChatId();
            var upload = await client.CreateUpload(new MaxCreateUploadRequest
            {
                Type = MaxUploadTypes.File
            });

            Assert.NotNull(upload);
            Assert.False(string.IsNullOrWhiteSpace(upload.Url));

            var text = $"Integration test file message {System.DateTimeOffset.UtcNow:O}";
            object payload;

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
            {
                payload = await client.UploadFile(upload.Url, stream, "integration-test-file.txt", "text/plain");
            }

            Assert.NotNull(payload);

            var request = new MaxSendMessageRequest
            {
                ChatId = chatId,
                Text = text,
                Attachments = new[]
                {
                    new MaxAttachment
                    {
                        Type = MaxUploadTypes.File,
                        Payload = payload
                    }
                }
            };

            MaxMessage sentMessage = null;
            HttpRequestException sendException = null;

            for (var attempt = 0; attempt < 10; attempt++)
            {
                try
                {
                    sentMessage = await client.SendMessage(request);
                    sendException = null;
                    break;
                }
                catch (HttpRequestException ex)
                {
                    sendException = ex;
                    await Task.Delay(2000);
                }
            }

            if (sendException != null)
                throw sendException;

            Assert.NotNull(sentMessage);
            Assert.NotNull(sentMessage.Body);
            Assert.False(string.IsNullOrWhiteSpace(sentMessage.Body.Mid));
            Assert.Equal(text, sentMessage.Body.Text);

            MaxMessage matchedMessage = null;

            for (var attempt = 0; attempt < 10; attempt++)
            {
                var messages = await client.GetMessages(new MaxMessagesQuery
                {
                    MessageIds = new[]
                    {
                        sentMessage.Body.Mid
                    }
                });

                if (messages?.Messages != null)
                {
                    for (var i = 0; i < messages.Messages.Count; i++)
                    {
                        var message = messages.Messages[i];
                        if (message?.Body == null || message.Body.Text != text || message.Body.Attachments == null)
                            continue;

                        for (var j = 0; j < message.Body.Attachments.Count; j++)
                        {
                            var attachment = message.Body.Attachments[j];
                            if (attachment != null && attachment.Type == MaxUploadTypes.File)
                            {
                                matchedMessage = message;
                                break;
                            }
                        }

                        if (matchedMessage != null)
                            break;
                    }
                }

                if (matchedMessage != null)
                    break;

                await Task.Delay(2000);
            }

            Assert.NotNull(matchedMessage);
            Assert.NotNull(matchedMessage.Body);
            Assert.NotNull(matchedMessage.Body.Attachments);
        }
    }
}
