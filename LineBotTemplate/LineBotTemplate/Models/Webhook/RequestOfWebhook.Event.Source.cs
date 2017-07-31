namespace LineBotTemplate.Models.Webhook {

	/// <summary>
	/// イベント送信元を表すオブジェクト
	/// </summary>
	public class Source {

		/// <summary>
		/// 種別
		/// </summary>
		public string type;

		/// <summary>
		/// ユーザID
		/// </summary>
		public string userId;

		/// <summary>
		/// グループID
		/// </summary>
		public string groupId;

		/// <summary>
		/// ルームID
		/// </summary>
		public string roomId;

	}

}