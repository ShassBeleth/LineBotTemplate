namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {

			/// <summary>
			/// イベント送信元を表すオブジェクト
			/// </summary>
			public partial class Source {

				/// <summary>
				/// 種別
				/// </summary>
				public string type;

				/// <summary>
				/// ユーザID
				/// </summary>
				public string userId;

				/// <summary>
				/// グループID
				/// </summary>
				public string groupId;

				/// <summary>
				/// ルームID
				/// </summary>
				public string roomId;

			}

		}
	}
}