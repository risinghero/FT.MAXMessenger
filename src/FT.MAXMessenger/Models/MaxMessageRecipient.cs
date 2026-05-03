namespace FT.MAXMessenger
{
    /// <summary>
    /// Получатель сообщения в API MAX.
    /// </summary>
    public class MaxMessageRecipient
    {
        /// <summary>
        /// Идентификатор чата-получателя.
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// Тип чата-получателя.
        /// </summary>
        public string ChatType { get; set; }

        /// <summary>
        /// Идентификатор пользователя-получателя.
        /// </summary>
        public long? UserId { get; set; }
    }
}
