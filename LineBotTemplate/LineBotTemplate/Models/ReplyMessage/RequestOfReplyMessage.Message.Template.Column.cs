namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// カラム
	/// </summary>
	public class Column {

		/// <summary>
		/// 画像のURL
		/// </summary>
		public string thumbnailImageUrl;

		/// <summary>
		/// タイトル
		/// </summary>
		public string title;

		/// <summary>
		/// テキスト
		/// </summary>
		public string text;

		/// <summary>
		/// アクション
		/// </summary>
		public TemplateAction[] actions;

	}

}