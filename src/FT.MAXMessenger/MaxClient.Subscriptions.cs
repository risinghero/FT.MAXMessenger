using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>GET /subscriptions</c>.
        /// Получает список текущих WebHook-подписок.
        /// </summary>
        public Task<MaxSubscriptionsResponse> GetSubscriptions()
        {
            return SendAsync<MaxSubscriptionsResponse>(HttpMethod.Get, "subscriptions");
        }

        /// <summary>
        /// Реализация метода документации <c>POST /subscriptions</c>.
        /// Создаёт подписку на обновления через WebHook.
        /// </summary>
        /// <param name="request">Параметры подписки.</param>
        public Task<MaxOperationResult> CreateSubscription(MaxCreateSubscriptionRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Url))
                throw new ArgumentException("Url is required.", nameof(request));

            return SendAsync<MaxOperationResult>(HttpMethod.Post, "subscriptions", request);
        }

        /// <summary>
        /// Реализация метода документации <c>DELETE /subscriptions</c>.
        /// Удаляет подписку на WebHook по URL.
        /// </summary>
        /// <param name="url">URL подписки, которую нужно удалить.</param>
        public Task<MaxOperationResult> DeleteSubscription(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url is required.", nameof(url));

            var query = new Dictionary<string, string>
            {
                { "url", url }
            };

            return SendAsync<MaxOperationResult>(HttpMethod.Delete, "subscriptions", query: query);
        }

        /// <summary>
        /// Реализация метода документации <c>GET /updates</c>.
        /// Получает обновления бота через Long Polling.
        /// </summary>
        /// <param name="query">Параметры получения обновлений.</param>
        public Task<MaxUpdatesResponse> GetUpdates(MaxUpdatesQuery query = null)
        {
            return SendAsync<MaxUpdatesResponse>(HttpMethod.Get, "updates", query: CreateUpdatesQuery(query));
        }

        private static IDictionary<string, string> CreateUpdatesQuery(MaxUpdatesQuery query)
        {
            if (query == null)
                return null;

            var parameters = new Dictionary<string, string>();
            if (query.Limit.HasValue)
                parameters.Add("limit", query.Limit.Value.ToString());
            if (query.Timeout.HasValue)
                parameters.Add("timeout", query.Timeout.Value.ToString());
            if (query.Marker.HasValue)
                parameters.Add("marker", query.Marker.Value.ToString());
            if (query.Types != null && query.Types.Count > 0)
                parameters.Add("types", string.Join(",", query.Types));
            return parameters;
        }
    }
}
