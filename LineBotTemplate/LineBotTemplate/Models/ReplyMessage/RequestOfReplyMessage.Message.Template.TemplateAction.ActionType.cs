using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class Template {
				public partial class TemplateAction {

					/// <summary>
					/// アクション種別
					/// </summary>
					[JsonConverter( typeof( StringEnumConverter ) )]
					public enum ActionType {

						/// <summary>
						/// タップ時にpostbackがwebhookで通知される
						/// </summary>
						[EnumMember( Value = "postback" )]
						Postback,

						/// <summary>
						/// テキストで指定された文字がユーザの発言として送信される
						/// </summary>
						[EnumMember( Value = "message" )]
						Message,

						/// <summary>
						/// URIを開く
						/// </summary>
						[EnumMember( Value = "uri" )]
						Uri,

					}

				}
			}
		}
	}
}