using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>POST /answers</c>.
        /// Отправляет ответ на callback по идентификатору <c>callback_id</c>.
        /// </summary>
        /// <param name="callbackId">Идентификатор callback.</param>
        /// <param name="message">Сообщение, которое будет отправлено в ответ.</param>
        public Task AnswerCallback(string callbackId, MaxNewMessageBody message)
        {
            if (string.IsNullOrWhiteSpace(callbackId))
                throw new ArgumentException("Callback id is required.", nameof(callbackId));
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var query = new Dictionary<string, string>
            {
                { "callback_id", callbackId }
            };

            return SendAsync<object>(HttpMethod.Post, "answers", new MaxCallbackAnswerRequest { Message = message }, query);
        }
    }
}
