using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {

			/// <summary>
			/// メッセージ種別
			/// </summary>
			[JsonConverter( typeof( StringEnumConverter ) )]
			public enum MessageType {

				/// <summary>
				/// テキスト
				/// </summary>
				[EnumMember( Value = "text" )]
				Text,

				/// <summary>
				/// 画像
				/// </summary>
				[EnumMember( Value = "image" )]
				Image,

				/// <summary>
				/// 動画
				/// </summary>
				[EnumMember( Value = "video" )]
				Video,

				/// <summary>
				/// 音声
				/// </summary>
				[EnumMember( Value = "audio" )]
				Audio,

				/// <summary>
				/// 位置情報
				/// </summary>
				[EnumMember( Value = "location" )]
				Location,

				/// <summary>
				/// Sticker
				/// </summary>
				[EnumMember( Value = "sticker" )]
				Sticker,

				/// <summary>
				/// リンク付き画像コンテンツ
				/// </summary>
				[EnumMember( Value = "imagemap" )]
				Imagemap,

				/// <summary>
				/// テンプレート
				/// </summary>
				[EnumMember( Value = "template" )]
				Template,

			}

		}
	}
}