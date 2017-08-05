using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class ImageMapAction {

				/// <summary>
				/// アクション種別
				/// </summary>
				[JsonConverter( typeof( StringEnumConverter ) )]
				public enum ImageMapActionType {

					/// <summary>
					/// URL
					/// </summary>
					[EnumMember( Value = "url" )]
					Url,

					/// <summary>
					/// メッセージ
					/// </summary>
					[EnumMember( Value = "message" )]
					Message,

				}

			}
		}
	}
}
