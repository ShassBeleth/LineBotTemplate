﻿namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {

			/// <summary>
			/// テンプレートオブジェクト
			/// </summary>
			public partial class Template {

				/// <summary>
				/// テンプレート種別
				/// </summary>
				public TemplateType type;

				/// <summary>
				/// 画像のURL
				/// </summary>
				public string thumbnailImageUrl;

				/// <summary>
				/// タイトル
				/// </summary>
				public string title;

				/// <summary>
				/// 説明文
				/// </summary>
				public string text;

				/// <summary>
				/// ボタン押下時アクション
				/// </summary>
				public TemplateAction[] actions;

				/// <summary>
				/// カラム
				/// </summary>
				public Column[] columns;

			}

		}
	}
}