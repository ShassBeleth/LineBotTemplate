namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// ボタン押下時アクション
	/// </summary>
	public class TemplateAction {

		/// <summary>
		/// アクション種別
		/// </summary>
		public ActionType type;

		/// <summary>
		/// アクションの表示名
		/// </summary>
		public string label;

		/// <summary>
		/// postback eventに渡されるデータ
		/// </summary>
		public string data;

		/// <summary>
		/// アクション実行時に送信されるテキスト
		/// </summary>
		public string text;

		/// <summary>
		/// URI
		/// </summary>
		public string uri;


	}

}