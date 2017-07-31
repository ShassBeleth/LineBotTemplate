namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// imagemapタップ時のアクション
	/// </summary>
	public class ImageMapAction {

		/// <summary>
		/// アクション種別
		/// </summary>
		public string type;

		/// <summary>
		/// WebページのURL
		/// </summary>
		public string linkUri;

		/// <summary>
		/// 送信するメッセージ
		/// </summary>
		public string text;

		/// <summary>
		/// タップ領域の定義
		/// </summary>
		public ImageMapArea area;

	}

}