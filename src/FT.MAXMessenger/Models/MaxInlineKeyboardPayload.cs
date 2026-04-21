using System.Collections.Generic;

namespace FT.MAXMessenger
{
    /// <summary>
    /// Полезная нагрузка inline-клавиатуры.
    /// </summary>
    public class MaxInlineKeyboardPayload
    {
        /// <summary>
        /// Строки кнопок inline-клавиатуры.
        /// </summary>
        public IList<IList<MaxInlineKeyboardButton>> Buttons { get; set; }
    }
}
