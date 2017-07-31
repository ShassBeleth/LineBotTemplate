namespace LineBotTemplate.Models.ReplyMessage {

	/// <summary>
	/// メッセージ種別
	/// </summary>
	public enum MessageType {

		/// <summary>
		/// テキスト
		/// </summary>
		Text,

		/// <summary>
		/// 画像
		/// </summary>
		Image,

		/// <summary>
		/// 動画
		/// </summary>
		Video,

		/// <summary>
		/// 音声
		/// </summary>
		Audio,

		/// <summary>
		/// 位置情報
		/// </summary>
		Location,

		/// <summary>
		/// Sticker
		/// </summary>
		Sticker,

		/// <summary>
		/// リンク付き画像コンテンツ
		/// </summary>
		Imagemap,

		/// <summary>
		/// テンプレート
		/// </summary>
		Template,

	}

}