using Newtonsoft.Json.Linq;

namespace FT.MAXMessenger
{
    /// <summary>
    /// Модель пользователя или бота в API MAX.
    /// </summary>
    public class MaxUser
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Признак того, что объект является ботом.
        /// </summary>
        public bool? IsBot { get; set; }

        /// <summary>
        /// Время последней активности.
        /// </summary>
        public long? LastActivityTime { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Описание профиля.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ссылка на аватар.
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Ссылка на полный размер аватара.
        /// </summary>
        public string FullAvatarUrl { get; set; }

        /// <summary>
        /// Команды бота или пользователя в сыром JSON-виде.
        /// </summary>
        public JToken Commands { get; set; }
    }
}
