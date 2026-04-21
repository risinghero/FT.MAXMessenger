using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>GET /chats</c>.
        /// Получает список групповых чатов с поддержкой постраничной навигации.
        /// </summary>
        /// <param name="count">Количество чатов на странице.</param>
        /// <param name="marker">Маркер следующей страницы.</param>
        public Task<MaxChatsResponse> GetChats(int? count = null, long? marker = null)
        {
            return SendAsync<MaxChatsResponse>(HttpMethod.Get, "chats", query: CreatePagingQuery(count, marker));
        }

        /// <summary>
        /// Реализация метода документации <c>GET /chats/{chatId}</c>.
        /// Получает информацию о групповом чате.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task<MaxChat> GetChat(string chatId)
        {
            return SendAsync<MaxChat>(HttpMethod.Get, CreateChatPath(chatId));
        }

        /// <summary>
        /// Реализация метода документации <c>PATCH /chats/{chatId}</c>.
        /// Обновляет свойства группового чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="request">Параметры обновления чата.</param>
        public Task<MaxChat> UpdateChat(string chatId, MaxUpdateChatRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return SendAsync<MaxChat>(PatchMethod, CreateChatPath(chatId), request);
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /chats/{chatId}</c>.
        /// Удаляет групповой чат.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task DeleteChat(string chatId)
        {
            return SendAsync<object>(HttpMethod.Delete, CreateChatPath(chatId));
        }

        /// <summary>
        /// Реализация метода документации <c>POST /chats/{chatId}/actions</c>.
        /// Отправляет действие бота в групповой чат.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="request">Параметры действия.</param>
        public Task SendChatAction(string chatId, MaxChatActionRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Action))
                throw new ArgumentException("Action is required.", nameof(request));

            return SendAsync<object>(HttpMethod.Post, CreateChatPath(chatId, "actions"), request);
        }

        /// <summary>
        /// Реализация метода документации <c>GET /chats/{chatId}/pin</c>.
        /// Получает закреплённое сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public async Task<MaxMessage> GetPinnedMessage(string chatId)
        {
            var response = await SendAsync<MaxMessageEnvelope>(HttpMethod.Get, CreateChatPath(chatId, "pin")).ConfigureAwait(false);
            return response == null ? null : response.Message;
        }

        /// <summary>
        /// Реализация метода документации <c>PUT /chats/{chatId}/pin</c>.
        /// Закрепляет сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="request">Параметры закрепления сообщения.</param>
        public Task<MaxOperationResult> SetPinnedMessage(string chatId, MaxChatPinRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.MessageId))
                throw new ArgumentException("Message id is required.", nameof(request));

            return SendAsync<MaxOperationResult>(HttpMethod.Put, CreateChatPath(chatId, "pin"), request);
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /chats/{chatId}/pin</c>.
        /// Удаляет закреплённое сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task DeletePinnedMessage(string chatId)
        {
            return SendAsync<object>(HttpMethod.Delete, CreateChatPath(chatId, "pin"));
        }

        /// <summary>
        /// Реализация метода документации <c>GET /chats/{chatId}/members/me</c>.
        /// Получает информацию о членстве текущего бота в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task<MaxChatMember> GetMyChatMembership(string chatId)
        {
            return SendAsync<MaxChatMember>(HttpMethod.Get, CreateChatPath(chatId, "members", "me"));
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /chats/{chatId}/members/me</c>.
        /// Удаляет текущего бота из чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task LeaveChat(string chatId)
        {
            return SendAsync<object>(HttpMethod.Delete, CreateChatPath(chatId, "members", "me"));
        }

        /// <summary>
        /// Реализация метода документации <c>GET /chats/{chatId}/members/admins</c>.
        /// Получает список администраторов чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        public Task<MaxChatMembersResponse> GetChatAdmins(string chatId)
        {
            return SendAsync<MaxChatMembersResponse>(HttpMethod.Get, CreateChatPath(chatId, "members", "admins"));
        }

        /// <summary>
        /// Реализация метода документации <c>POST /chats/{chatId}/members/admins</c>.
        /// Назначает администраторов группового чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="request">Параметры добавления администраторов.</param>
        public Task<MaxOperationResult> AddChatAdmins(string chatId, MaxAddChatAdminsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return SendAsync<MaxOperationResult>(HttpMethod.Post, CreateChatPath(chatId, "members", "admins"), request);
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /chats/{chatId}/members/admins/{userId}</c>.
        /// Снимает права администратора у участника чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        public Task RemoveChatAdmin(string chatId, long userId)
        {
            return SendAsync<object>(HttpMethod.Delete, CreateChatPath(chatId, "members", "admins", userId.ToString()));
        }

        /// <summary>
        /// Реализация метода документации <c>GET /chats/{chatId}/members</c>.
        /// Получает список участников группового чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="query">Параметры выборки участников.</param>
        public Task<MaxChatMembersResponse> GetChatMembers(string chatId, MaxChatMembersQuery query = null)
        {
            return SendAsync<MaxChatMembersResponse>(HttpMethod.Get, CreateChatPath(chatId, "members"), query: CreateChatMembersQuery(query));
        }

        /// <summary>
        /// Реализация метода документации <c>POST /chats/{chatId}/members</c>.
        /// Добавляет участников в групповой чат.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="request">Параметры добавления участников.</param>
        public Task<MaxOperationResult> AddChatMembers(string chatId, MaxAddChatMembersRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return SendAsync<MaxOperationResult>(HttpMethod.Post, CreateChatPath(chatId, "members"), request);
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /chats/{chatId}/members</c>.
        /// Удаляет участника из группового чата.
        /// </summary>
        /// <param name="chatId">Идентификатор чата.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="block">Признак блокировки пользователя после удаления.</param>
        public Task RemoveChatMember(string chatId, long userId, bool? block = null)
        {
            var query = new Dictionary<string, string>
            {
                { "user_id", userId.ToString() }
            };

            if (block.HasValue)
                query.Add("block", block.Value ? "true" : "false");

            return SendAsync<object>(HttpMethod.Delete, CreateChatPath(chatId, "members"), query: query);
        }
    }
}
