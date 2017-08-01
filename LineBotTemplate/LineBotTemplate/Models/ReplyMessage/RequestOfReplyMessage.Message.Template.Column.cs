namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class Template {

				/// <summary>
				/// カラム
				/// </summary>
				public class Column {

					/// <summary>
					/// 画像のURL
					/// </summary>
					public string thumbnailImageUrl;

					/// <summary>
					/// タイトル
					/// </summary>
					public string title;

					/// <summary>
					/// テキスト
					/// </summary>
					public string text;

					/// <summary>
					/// アクション
					/// </summary>
					public TemplateAction[] actions;

				}

			}
		}
	}
}