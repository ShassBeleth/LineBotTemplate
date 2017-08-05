using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LineBotTemplate.Models.Webhook {
	public partial class RequestOfWebhook {
		public partial class Event {
			public partial class Beacon {

				/// <summary>
				/// ビーコン種別
				/// </summary>
				[JsonConverter( typeof( StringEnumConverter ) )]
				public enum BeaconType {

					/// <summary>
					/// ビーコンデバイスの受信圏内に入った
					/// </summary>
					[EnumMember( Value = "enter" )]
					Enter,

					/// <summary>
					/// ビーコンデバイスの受信圏内から出た
					/// </summary>
					[EnumMember( Value = "leave" )]
					Leave,

					/// <summary>
					/// Beacon Bannerをタップした
					/// </summary>
					[EnumMember( Value = "banner" )]
					Banner,

				}

			}
		}
	}
}