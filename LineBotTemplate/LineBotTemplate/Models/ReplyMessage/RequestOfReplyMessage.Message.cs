namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {

		/// <summary>
		/// リプライメッセージ
		/// </summary>
		public partial class Message {

			/// <summary>
			/// メッセージ種別
			/// </summary>
			public MessageType type;

			/// <summary>
			/// メッセージ本文
			/// </summary>
			public string text;

			/// <summary>
			/// 画像、動画、音声のURL
			/// </summary>
			public string originalContentUrl;

			/// <summary>
			/// プレビュー画像のURL
			/// </summary>
			public string previewImageUrl;

			/// <summary>
			/// 音声ファイルの時間の長さ
			/// </summary>
			public int? duration;

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
			public double? latitude;

			/// <summary>
			/// 経度
			/// </summary>
			public double? longitude;

			/// <summary>
			/// パッケージ識別子
			/// </summary>
			public string packageId;

			/// <summary>
			/// Sticker識別子
			/// </summary>
			public string stickerId;

			/// <summary>
			/// imagemapに使用する画像のURL
			/// </summary>
			public string baseUrl;

			/// <summary>
			/// 代替テキスト
			/// </summary>
			public string altText;

			/// <summary>
			/// 基本比率サイズ
			/// </summary>
			public BaseSize baseSize;

			/// <summary>
			/// imagemapタップ時のアクション
			/// </summary>
			public ImageMapAction[] actions;

			/// <summary>
			/// テンプレートオブジェクト
			/// </summary>
			public Template template;

		}

	}
}