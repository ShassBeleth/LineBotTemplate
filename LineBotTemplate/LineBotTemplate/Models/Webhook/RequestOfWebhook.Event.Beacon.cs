namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {

			/// <summary>
			/// ビーコン情報
			/// </summary>
			public partial class Beacon {

				/// <summary>
				/// 発見したビーコンデバイスのハードウェアID
				/// </summary>
				public string hwid;

				/// <summary>
				/// 種別
				/// </summary>
				public BeaconType type;

				/// <summary>
				/// 発見したビーコンデバイスのデバイスメッセージ
				/// </summary>
				public string dm;

			}

		}
	}
}