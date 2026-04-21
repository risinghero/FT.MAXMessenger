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
    }
}
