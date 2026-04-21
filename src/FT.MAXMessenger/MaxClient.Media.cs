using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>POST /uploads</c>.
        /// Получает ссылку для загрузки медиафайла указанного типа.
        /// </summary>
        /// <param name="request">Параметры создания загрузки.</param>
        public Task<MaxUpload> CreateUpload(MaxCreateUploadRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Type))
                throw new ArgumentException("Type is required.", nameof(request));

            var query = new Dictionary<string, string>
            {
                { "type", request.Type }
            };

            return SendAsync<MaxUpload>(HttpMethod.Post, "uploads", query: query);
        }

        /// <summary>
        /// Реализация метода документации <c>GET /videos/{videoToken}</c>.
        /// Получает информацию о ранее загруженном видео.
        /// </summary>
        /// <param name="videoToken">Токен видео.</param>
        public Task<MaxVideo> GetVideo(string videoToken)
        {
            if (string.IsNullOrWhiteSpace(videoToken))
                throw new ArgumentException("Video token is required.", nameof(videoToken));

            return SendAsync<MaxVideo>(HttpMethod.Get, "videos/" + Uri.EscapeDataString(videoToken));
        }
    }
}
