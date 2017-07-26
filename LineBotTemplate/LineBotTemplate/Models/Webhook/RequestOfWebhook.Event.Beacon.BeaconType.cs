namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {

			public partial class Beacon {
				/// <summary>
				/// ビーコン種別
				/// </summary>
				public enum BeaconType {

					/// <summary>
					/// ビーコンデバイスの受信圏内に入った
					/// </summary>
					Enter,

					/// <summary>
					/// ビーコンデバイスの受信圏内から出た
					/// </summary>
					Leave,

					/// <summary>
					/// Beacon Bannerをタップした
					/// </summary>
					Banner,

				}

			}
		}
	}
}