namespace FT.MAXMessenger
{
    /// <summary>
    /// Универсальная модель вложения сообщения.
    /// </summary>
    public class MaxAttachment
    {
        /// <summary>
        /// Тип вложения.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Полезная нагрузка вложения.
        /// </summary>
        public object Payload { get; set; }
    }
}
