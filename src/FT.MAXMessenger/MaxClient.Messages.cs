using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>GET /messages</c>.
        /// Получает сообщения по идентификаторам либо из указанного чата.
        /// </summary>
        /// <param name="query">Параметры запроса сообщений.</param>
        public Task<MaxMessagesResponse> GetMessages(MaxMessagesQuery query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return SendAsync<MaxMessagesResponse>(HttpMethod.Get, "messages", query: CreateMessagesQuery(query));
        }

        /// <summary>
        /// Реализация метода документации <c>POST /messages</c>.
        /// Отправляет новое сообщение пользователю или в чат.
        /// </summary>
        /// <param name="request">Параметры отправки сообщения.</param>
        public async Task<MaxMessage> SendMessage(MaxSendMessageRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = CreateRecipientQuery(request.UserId, request.ChatId);
            var response = await SendAsync<MaxMessageEnvelope>(HttpMethod.Post, "messages", CreateMessageBody(request), query).ConfigureAwait(false);
            return response == null ? null : response.Message;
        }

        /// <summary>
        /// Реализация метода документации <c>PUT /messages</c>.
        /// Изменяет ранее отправленное сообщение по идентификатору <c>message_id</c>.
        /// </summary>
        /// <param name="request">Параметры редактирования сообщения.</param>
        public async Task<MaxMessage> EditMessage(MaxEditMessageRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.MessageId))
                throw new ArgumentException("Message id is required.", nameof(request));

            var query = new Dictionary<string, string>
            {
                { "message_id", request.MessageId }
            };

            var response = await SendAsync<MaxMessageEnvelope>(HttpMethod.Put, "messages", CreateMessageBody(request), query).ConfigureAwait(false);
            return response == null ? null : response.Message;
        }

        /// <summary>
        /// Реализация метода документации <c>GET /messages/{messageId}</c>.
        /// Получает сообщение по его идентификатору.
        /// </summary>
        /// <param name="messageId">Идентификатор сообщения.</param>
        public Task<MaxMessage> GetMessage(string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
                throw new ArgumentException("Message id is required.", nameof(messageId));

            return SendAsync<MaxMessage>(HttpMethod.Get, "messages/" + Uri.EscapeDataString(messageId));
        }

        private static IDictionary<string, string> CreateMessagesQuery(MaxMessagesQuery query)
        {
            var hasChatId =!string.IsNullOrWhiteSpace(query.ChatId);
            var hasMessageIds = query.MessageIds != null && query.MessageIds.Count > 0;

            if (hasChatId == hasMessageIds)
                throw new ArgumentException("Specify either chat id or message ids.", nameof(query));

            var parameters = new Dictionary<string, string>();
            if (hasChatId)
                parameters.Add("chat_id", query.ChatId);
            if (hasMessageIds)
                parameters.Add("message_ids", string.Join(",", query.MessageIds));
            if (query.From.HasValue)
                parameters.Add("from", query.From.Value.ToString());
            if (query.To.HasValue)
                parameters.Add("to", query.To.Value.ToString());
            if (query.Count.HasValue)
                parameters.Add("count", query.Count.Value.ToString());
            return parameters;
        }
    }
}
