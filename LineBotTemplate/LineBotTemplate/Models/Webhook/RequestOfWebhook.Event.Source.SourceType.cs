namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {
			public partial class Source {

				/// <summary>
				/// イベント元種別
				/// </summary>
				public enum SourceType {

					/// <summary>
					/// ユーザ
					/// </summary>
					User,

					/// <summary>
					/// グループ
					/// </summary>
					Group,

					/// <summary>
					/// トークルーム
					/// </summary>
					Room,

				}

			}
		}
	}
}