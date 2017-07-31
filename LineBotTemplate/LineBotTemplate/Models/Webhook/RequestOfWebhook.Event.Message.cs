namespace LineBotTemplate.Models.Webhook {

	/// <summary>
	/// メッセージ
	/// </summary>
	public class Message {

		/// <summary>
		/// メッセージID
		/// </summary>
		public string id;

		/// <summary>
		/// メッセージ種別
		/// </summary>
		public string type;

		/// <summary>
		/// メッセージ本文
		/// </summary>
		public string text;

		/// <summary>
		/// ファイル名
		/// </summary>
		public string fileName;

		/// <summary>
		/// ファイルのバイト数
		/// </summary>
		public string fileSize;

		/// <summary>
		/// タイトル
		/// </summary>
		public string title;

		/// <summary>
		/// 住所
		/// </summary>
		public string address;

		/// <summary>
		/// 緯度
		/// </summary>
		public string latitude;

		/// <summary>
		/// 経度
		/// </summary>
		public string longitude;

		/// <summary>
		/// パッケージ識別子
		/// </summary>
		public string packageId;

		/// <summary>
		/// Sticker識別子
		/// </summary>
		public string stickerId;


	}

}