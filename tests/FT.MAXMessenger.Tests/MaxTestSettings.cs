using Microsoft.Extensions.Configuration;
using System;

namespace FT.MAXMessenger.Tests
{
    internal static class MaxTestSettings
    {
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
            .AddUserSecrets<MaxClientIntegrationTests>()
            .Build();

        public static string GetAccessToken()
        {
            return Configuration["MAX:AccessToken"] ?? Environment.GetEnvironmentVariable("MAX_API_TOKEN");
        }

        public static string? GetTestChatId()
        {
            var value = Configuration["MAX:TestChatId"] ?? Environment.GetEnvironmentVariable("MAX_TEST_CHAT_ID");
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            return null;
        }
    }
}
