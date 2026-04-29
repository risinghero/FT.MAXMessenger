using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace FT.MAXMessenger.Tests
{
    public class MaxClientMediaTests
    {
        [Fact]
        public async Task CreateUpload_WithUnsupportedType_ThrowsArgumentException()
        {
            var client = new MaxClient(new HttpClient(new StubHttpMessageHandler(_ => throw new InvalidOperationException())), "token");

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.CreateUpload(new MaxCreateUploadRequest
            {
                Type = "photo"
            }));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task UploadFile_ReturnsPayloadFromResponse()
        {
            var handler = new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"token\":\"abc\"}", Encoding.UTF8, "application/json")
            });
            var client = new MaxClient(new HttpClient(handler), "token");

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));

            var result = await client.UploadFile("https://example.com/upload", stream, "test.txt", "text/plain");

            Assert.NotNull(result);
            Assert.Equal("abc", result.Value<string>("token"));
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

            public StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
            {
                _handler = handler;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_handler(request));
            }
        }
    }
}
