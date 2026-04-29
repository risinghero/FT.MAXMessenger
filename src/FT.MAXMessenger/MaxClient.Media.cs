using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        private static readonly HashSet<string> SupportedUploadTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            MaxUploadTypes.Image,
            MaxUploadTypes.Video,
            MaxUploadTypes.Audio,
            MaxUploadTypes.File
        };

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
            if (!SupportedUploadTypes.Contains(request.Type))
                throw new ArgumentException("Supported upload types are: image, video, audio, file.", nameof(request));

            var query = new Dictionary<string, string>
            {
                { "type", request.Type.ToLowerInvariant() }
            };

            return SendAsync<MaxUpload>(HttpMethod.Post, "uploads", query: query);
        }

        /// <summary>
        /// Загружает файл по ранее полученному URL из метода <c>POST /uploads</c>.
        /// Возвращает JSON-объект, который можно использовать как payload вложения.
        /// </summary>
        /// <param name="uploadUrl">URL для загрузки файла.</param>
        /// <param name="content">Содержимое файла.</param>
        /// <param name="fileName">Имя файла для multipart-запроса.</param>
        /// <param name="contentType">MIME-тип файла.</param>
        public async Task<JToken> UploadFile(string uploadUrl, Stream content, string fileName, string contentType = null)
        {
            if (string.IsNullOrWhiteSpace(uploadUrl))
                throw new ArgumentException("Upload URL is required.", nameof(uploadUrl));
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name is required.", nameof(fileName));

            using var multipartContent = new MultipartFormDataContent();
            using var fileContent = new StreamContent(content);

            if (!string.IsNullOrWhiteSpace(contentType))
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            multipartContent.Add(fileContent, "data", fileName);

            using var response = await _httpClient.PostAsync(uploadUrl, multipartContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonConvert.DeserializeObject<JToken>(json);
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
