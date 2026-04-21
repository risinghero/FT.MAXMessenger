using System.Collections.Generic;

namespace FT.MAXMessenger
{
    /// <summary>
    /// Базовая модель тела сообщения MAX.
    /// Используется при отправке, редактировании и ответах на callback.
    /// </summary>
    public class MaxNewMessageBody
    {
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Список вложений сообщения.
        /// </summary>
        public IList<MaxAttachment> Attachments { get; set; }

        /// <summary>
        /// Ссылка, прикреплённая к сообщению.
        /// </summary>
        public MaxMessageLink Link { get; set; }

        /// <summary>
        /// Признак необходимости отправки уведомления получателю.
        /// </summary>
        public bool? Notify { get; set; }

        /// <summary>
        /// Формат сообщения.
        /// </summary>
        public string Format { get; set; }
    }
}
