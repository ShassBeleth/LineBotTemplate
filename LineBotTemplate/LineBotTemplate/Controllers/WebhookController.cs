using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace LineBotTemplate.Controllers {

	/// <summary>
	/// LINE BotからのWebhook送信先API
	/// </summary>
	public class WebhookController : ApiController {

		/// <summary>
		/// POSTメソッド
		/// </summary>
		/// <param name="requestToken">リクエストトークン</param>
		/// <returns>常にステータス200のみを返す</returns>
		public async Task<HttpResponseMessage> Post( JToken requestToken ) {

			Trace.TraceInformation( "Webhook API Start" );
			
			return new HttpResponseMessage( HttpStatusCode.OK );
		}

	}

}