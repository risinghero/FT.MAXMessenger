using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    /// <summary>
    /// Клиент для работы с HTTP API MAX.
    /// 
    /// В текущей реализации поддержаны следующие методы из документации:
    /// - <c>GET /me</c> — получение информации о боте.
    /// - <c>POST /messages</c> — отправка нового сообщения.
    /// - <c>PUT /messages</c> — редактирование сообщения по <c>message_id</c>.
    /// - <c>GET /messages/{messageId}</c> — получение сообщения по идентификатору.
    /// - <c>POST /answers</c> — ответ на callback по <c>callback_id</c>.
    /// 
    /// Полный список реализованных и пока не реализованных методов вынесен в файл
    /// <c>MAXMessenger/API_METHODS.md</c>.
    /// </summary>
    /// <remarks>
    /// Создаёт клиент MAX с переданным экземпляром <see cref="HttpClient"/>.
    /// </remarks>
    /// <param name="httpClient">HTTP-клиент для выполнения запросов.</param>
    /// <param name="accessToken">Токен доступа к API.</param>
    public partial class MaxClient(HttpClient httpClient, string accessToken)
    {
        /// <summary>
        /// Базовый адрес API MAX.
        /// </summary>
        private const string BaseUrl = "https://platform-api.max.ru/";

        private static readonly HttpMethod PatchMethod = new("PATCH");

        /// <summary>
        /// Общий HTTP-клиент для сценария, когда внешний экземпляр <see cref="HttpClient"/> не передан.
        /// </summary>
        private static readonly HttpClient SharedHttpClient = new();

        /// <summary>
        /// Настройки сериализации JSON в формате, ожидаемом API MAX.
        /// </summary>
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        /// <summary>
        /// Создаёт клиент MAX с общим экземпляром <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="accessToken">Токен доступа к API.</param>
        public MaxClient(string accessToken)
            : this(SharedHttpClient, accessToken)
        {
        }

        /// <summary>
        /// Токен доступа, используемый для авторизации запросов к API MAX.
        /// </summary>
        public string AccessToken { get; } = string.IsNullOrWhiteSpace(accessToken)
                ? throw new ArgumentException("Access token is required.", nameof(accessToken))
                : accessToken;

        /// <summary>
        /// Выполняет HTTP-запрос к API MAX, при необходимости сериализует тело запроса
        /// и десериализует JSON-ответ в указанный тип.
        /// </summary>
        private async Task<T> SendAsync<T>(HttpMethod method, string urlPart, object body = null, IDictionary<string, string> query = null)
        {
            using var request = new HttpRequestMessage(method, BaseUrl + urlPart + BuildQueryString(query));
            request.Headers.Add("Authorization", AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, JsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using (var response = await _httpClient.SendAsync(request).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                if (response.Content == null)
                    return default(T);

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(json))
                    return default(T);

                return JsonConvert.DeserializeObject<T>(json, JsonSettings);
            }
        }

        /// <summary>
        /// Формирует параметры получателя сообщения.
        /// По правилам API должен быть указан либо пользователь, либо чат.
        /// </summary>
        private static IDictionary<string, string> CreateRecipientQuery(long? userId, string chatId)
        {
            var hasChatId = !string.IsNullOrWhiteSpace(chatId);
            if (userId.HasValue == hasChatId)
                throw new ArgumentException("Specify either user id or chat id.");

            var query = new Dictionary<string, string>();
            if (userId.HasValue)
                query.Add("user_id", userId.Value.ToString());
            if (hasChatId)
                query.Add("chat_id", chatId);
            return query;
        }

        /// <summary>
        /// Создаёт тело запроса для операций отправки и редактирования сообщения.
        /// </summary>
        private static MaxNewMessageBody CreateMessageBody(MaxNewMessageBody request)
        {
            return new MaxNewMessageBody
            {
                Text = request.Text,
                Attachments = request.Attachments,
                Link = request.Link,
                Notify = request.Notify,
                Format = request.Format
            };
        }

        private static IDictionary<string, string> CreatePagingQuery(int? count, long? marker)
        {
            var query = new Dictionary<string, string>();
            if (count.HasValue)
                query.Add("count", count.Value.ToString());
            if (marker.HasValue)
                query.Add("marker", marker.Value.ToString());
            return query;
        }

        private static IDictionary<string, string> CreateChatMembersQuery(MaxChatMembersQuery query)
        {
            if (query == null)
                return null;

            var parameters = new Dictionary<string, string>();
            if (query.Count.HasValue)
                parameters.Add("count", query.Count.Value.ToString());
            if (query.Marker.HasValue)
                parameters.Add("marker", query.Marker.Value.ToString());
            if (query.UserIds != null && query.UserIds.Count > 0)
                parameters.Add("user_ids", string.Join(",", query.UserIds));
            return parameters;
        }

        private static string CreateChatPath(string chatId, params string[] segments)
        {
            var builder = new StringBuilder("chats/");
            builder.Append(chatId);

            if (segments != null)
            {
                for (var i = 0; i < segments.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(segments[i]))
                        continue;

                    builder.Append("/");
                    builder.Append(Uri.EscapeDataString(segments[i]));
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Собирает query string из словаря параметров.
        /// Пустые значения пропускаются.
        /// </summary>
        private static string BuildQueryString(IDictionary<string, string> query)
        {
            if (query == null || query.Count == 0)
                return string.Empty;

            var builder = new StringBuilder("?");
            var isFirst = true;
            foreach (var item in query)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                    continue;

                if (!isFirst)
                    builder.Append("&");

                builder.Append(Uri.EscapeDataString(item.Key));
                builder.Append("=");
                builder.Append(Uri.EscapeDataString(item.Value));
                isFirst = false;
            }

            return isFirst ? string.Empty : builder.ToString();
        }

    }
}
