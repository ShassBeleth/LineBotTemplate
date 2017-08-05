namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// Reply Messageに使用するリクエストEntity
	/// </summary>
	public partial class RequestOfReplyMessage {

		/// <summary>
		/// 返信に必要なリプライトークン
		/// </summary>
		public string replyToken;

		/// <summary>
		/// リプライメッセージ(最大5通)
		/// </summary>
		public Message[] messages;

	}

}