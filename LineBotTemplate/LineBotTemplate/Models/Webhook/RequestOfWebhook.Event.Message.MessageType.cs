namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {
			public partial class Message {

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
					/// ファイル
					/// </summary>
					File,

					/// <summary>
					/// 位置情報
					/// </summary>
					Location,

					/// <summary>
					/// Sticker
					/// </summary>
					Sticker,

				}

			}
		}
	}
}