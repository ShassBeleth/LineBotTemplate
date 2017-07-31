namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// テンプレートオブジェクト
	/// </summary>
	public class Template {

		/// <summary>
		/// テンプレート種別
		/// </summary>
		public string type;

		/// <summary>
		/// 画像のURL
		/// </summary>
		public string thumbnailImageUrl;

		/// <summary>
		/// タイトル
		/// </summary>
		public string title;

		/// <summary>
		/// 説明文
		/// </summary>
		public string text;

		/// <summary>
		/// ボタン押下時アクション
		/// </summary>
		public TemplateAction[] actions;

		/// <summary>
		/// カラム
		/// </summary>
		public Column[] columns;

	}

}