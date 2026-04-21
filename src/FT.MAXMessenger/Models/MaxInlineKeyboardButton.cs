namespace FT.MAXMessenger
{
    /// <summary>
    /// Кнопка inline-клавиатуры.
    /// </summary>
    public class MaxInlineKeyboardButton
    {
        /// <summary>
        /// Тип кнопки.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Отображаемый текст кнопки.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// URL, открываемый по нажатию кнопки.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Произвольная payload-строка кнопки.
        /// </summary>
        public string Payload { get; set; }
    }
}
