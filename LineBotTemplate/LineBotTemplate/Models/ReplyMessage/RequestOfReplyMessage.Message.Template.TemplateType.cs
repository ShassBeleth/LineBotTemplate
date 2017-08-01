using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class Template {

				/// <summary>
				/// テンプレート種別
				/// </summary>
				[JsonConverter( typeof( StringEnumConverter ) )]
				public enum TemplateType {

					/// <summary>
					/// 画像、タイトル、テキスト、アクションボタンを組み合わせたテンプレート
					/// </summary>
					[EnumMember( Value = "buttons" )]
					Buttons,

					/// <summary>
					/// 2つのアクションボタンを提示するテンプレート
					/// </summary>
					[EnumMember( Value = "confirm" )]
					Confirm,

					/// <summary>
					/// 複数の情報を並べて提示できるカルーセル型テンプレート
					/// </summary>
					[EnumMember( Value = "carousel" )]
					Carousel,

				}

			}
		}
	}
}