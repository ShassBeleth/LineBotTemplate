namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class Template {
				public partial class TemplateAction {

					/// <summary>
					/// アクション種別
					/// </summary>
					public enum ActionType {

						/// <summary>
						/// タップ時にpostbackがwebhookで通知される
						/// </summary>
						Postback,

						/// <summary>
						/// テキストで指定された文字がユーザの発言として送信される
						/// </summary>
						Message,

						/// <summary>
						/// URIを開く
						/// </summary>
						Uri,

					}

				}
			}
		}
	}
}