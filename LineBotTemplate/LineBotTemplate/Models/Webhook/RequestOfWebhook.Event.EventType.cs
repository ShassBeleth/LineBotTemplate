using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {

			/// <summary>
			/// イベント種別
			/// </summary>
			[JsonConverter( typeof( StringEnumConverter ) )]
			public enum EventType {

				/// <summary>
				/// メッセージ
				/// </summary>
				[EnumMember( Value = "message" )]
				Message,

				/// <summary>
				/// 友達追加またはブロック解除
				/// </summary>
				[EnumMember( Value = "follow" )]
				Follow,

				/// <summary>
				/// ブロック
				/// </summary>
				[EnumMember( Value = "unfollow" )]
				Unfollow,

				/// <summary>
				/// グループまたはトークルームに追加
				/// </summary>
				[EnumMember( Value = "join" )]
				Join,

				/// <summary>
				/// グループから退出させられる
				/// </summary>
				[EnumMember( Value = "leave" )]
				Leave,

				/// <summary>
				/// ポストバック
				/// </summary>
				[EnumMember( Value = "postback" )]
				Postback,

				/// <summary>
				/// ビーコン
				/// </summary>
				[EnumMember( Value = "beacon" )]
				Beacon,

			}

		}
	}
}