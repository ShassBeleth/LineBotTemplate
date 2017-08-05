namespace LineBotTemplate.Models.Webhook {

	/// <summary>
	/// Webhookに使用するリクエストEntity
	/// </summary>
	public partial class RequestOfWebhook {

		/// <summary>
		/// イベントリスト
		/// </summary>
		public Event[] events;

	}

}