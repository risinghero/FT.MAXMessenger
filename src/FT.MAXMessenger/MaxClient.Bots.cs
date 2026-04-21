using System.Net.Http;
using System.Threading.Tasks;

namespace FT.MAXMessenger
{
    public partial class MaxClient
    {
        /// <summary>
        /// Реализация метода документации <c>GET /me</c>.
        /// Возвращает информацию о боте, от имени которого выполняются запросы.
        /// </summary>
        public Task<MaxUser> GetMe()
        {
            return SendAsync<MaxUser>(HttpMethod.Get, "me");
        }
    }
}
