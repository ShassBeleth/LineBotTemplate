using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {
			public partial class Source {

				/// <summary>
				/// イベント元種別
				/// </summary>
				[JsonConverter( typeof( StringEnumConverter ) )]
				public enum SourceType {

					/// <summary>
					/// ユーザ
					/// </summary>
					[EnumMember( Value = "user" )]
					User,

					/// <summary>
					/// グループ
					/// </summary>
					[EnumMember( Value = "group" )]
					Group,

					/// <summary>
					/// トークルーム
					/// </summary>
					[EnumMember( Value = "room" )]
					Room,

				}

			}
		}
	}
}