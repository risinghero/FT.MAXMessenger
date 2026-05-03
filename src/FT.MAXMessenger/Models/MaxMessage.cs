namespace FT.MAXMessenger
{
    /// <summary>
    /// Модель сообщения, возвращаемого API MAX.
    /// </summary>
    public class MaxMessage
    {
        /// <summary>
        /// Идентификатор сообщения.
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// Идентификатор чата, к которому относится сообщение.
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// Отправитель сообщения.
        /// </summary>
        public MaxUser Sender { get; set; }

        /// <summary>
        /// Получатель сообщения.
        /// </summary>
        public MaxMessageRecipient Recipient { get; set; }

        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public MaxNewMessageBody Body { get; set; }

        /// <summary>
        /// Временная метка сообщения.
        /// </summary>
        public long? Timestamp { get; set; }

        /// <summary>
        /// Ссылка на сообщение.
        /// </summary>
        public string Url { get; set; }
    }
}
