namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class Template {

				/// <summary>
				/// ボタン押下時アクション
				/// </summary>
				public partial class TemplateAction {
				
					/// <summary>
					/// アクション種別
					/// </summary>
					public ActionType type;

					/// <summary>
					/// アクションの表示名
					/// </summary>
					public string label;

					/// <summary>
					/// postback eventに渡されるデータ
					/// </summary>
					public string data;

					/// <summary>
					/// アクション実行時に送信されるテキスト
					/// </summary>
					public string text;

					/// <summary>
					/// URI
					/// </summary>
					public string uri;


				}

			}
		}
	}
}