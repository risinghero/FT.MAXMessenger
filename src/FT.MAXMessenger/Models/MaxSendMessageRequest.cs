namespace FT.MAXMessenger
{
    /// <summary>
    /// Запрос на отправку сообщения.
    /// Должен содержать либо <see cref="UserId"/>, либо <see cref="ChatId"/>.
    /// </summary>
    public class MaxSendMessageRequest : MaxNewMessageBody
    {
        /// <summary>
        /// Идентификатор пользователя-получателя.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Идентификатор чата-получателя.
        /// </summary>
        public string ChatId { get; set; }
    }
}
