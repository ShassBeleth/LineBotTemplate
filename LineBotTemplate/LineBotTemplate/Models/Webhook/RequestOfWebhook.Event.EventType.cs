namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {

			/// <summary>
			/// イベント種別
			/// </summary>
			public enum EventType {

				/// <summary>
				/// メッセージ
				/// </summary>
				Message,

				/// <summary>
				/// 友達追加またはブロック解除
				/// </summary>
				Follow,

				/// <summary>
				/// ブロック
				/// </summary>
				Unfollow,

				/// <summary>
				/// グループまたはトークルームに追加
				/// </summary>
				Join,

				/// <summary>
				/// グループから退出させられる
				/// </summary>
				Leave,

				/// <summary>
				/// ポストバック
				/// </summary>
				Postback,

				/// <summary>
				/// ビーコン
				/// </summary>
				Beacon,

			}

		}
	}
}