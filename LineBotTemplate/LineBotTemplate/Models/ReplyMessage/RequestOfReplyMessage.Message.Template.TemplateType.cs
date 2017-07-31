namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// テンプレート種別
	/// </summary>
	public enum TemplateType {

		/// <summary>
		/// 画像、タイトル、テキスト、アクションボタンを組み合わせたテンプレート
		/// </summary>
		Buttons,

		/// <summary>
		/// 2つのアクションボタンを提示するテンプレート
		/// </summary>
		Confirm,

		/// <summary>
		/// 複数の情報を並べて提示できるカルーセル型テンプレート
		/// </summary>
		Carousel,

	}

}