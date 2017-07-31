namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// アクション種別
	/// </summary>
	public enum ActionType {

		/// <summary>
		/// タップ時にpostbackがwebhookで通知される
		/// </summary>
		Postback,

		/// <summary>
		/// テキストで指定された文字がユーザの発言として送信される
		/// </summary>
		Message,

		/// <summary>
		/// URIを開く
		/// </summary>
		Uri,

	}

}