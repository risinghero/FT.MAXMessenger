namespace FT.MAXMessenger
{
    /// <summary>
    /// Модель ссылки, прикреплённой к сообщению.
    /// </summary>
    public class MaxMessageLink
    {
        /// <summary>
        /// Тип ссылки.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// URL ссылки.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Текст ссылки.
        /// </summary>
        public string Text { get; set; }
    }
}
