namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Внутренняя модель ответа API, в котором сообщение вложено в поле <c>message</c>.
        /// </summary>
        private class MaxMessageEnvelope
        {
            /// <summary>
            /// Сообщение, возвращённое API.
            /// </summary>
            public MaxMessage Message { get; set; }
        }

        /// <summary>
        /// Внутренняя модель тела запроса для ответа на callback.
        /// </summary>
        private class MaxCallbackAnswerRequest
        {
            /// <summary>
            /// Сообщение, отправляемое в ответ на callback.
            /// </summary>
            public MaxNewMessageBody Message { get; set; }
        }
    }
}
