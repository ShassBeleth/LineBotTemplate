using System.Collections.Generic;

namespace LineBotTemplate.Models.Webhook {

	/// <summary>
	/// Webhookに使用するリクエストEntity
	/// </summary>
	public class RequestOfWebhook {

		/// <summary>
		/// イベントリスト
		/// </summary>
		public Event[] events;

	}

}