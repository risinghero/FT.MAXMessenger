namespace FT.MAXMessenger
{
    /// <summary>
    /// Запрос на редактирование сообщения.
    /// </summary>
    public class MaxEditMessageRequest : MaxNewMessageBody
    {
        /// <summary>
        /// Идентификатор сообщения для редактирования.
        /// </summary>
        public string MessageId { get; set; }
    }
}
