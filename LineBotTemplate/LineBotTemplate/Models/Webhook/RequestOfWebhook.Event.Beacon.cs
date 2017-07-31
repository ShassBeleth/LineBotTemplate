namespace LineBotTemplate.Models.Webhook {

	/// <summary>
	/// ビーコン情報
	/// </summary>
	public class Beacon {

		/// <summary>
		/// 発見したビーコンデバイスのハードウェアID
		/// </summary>
		public string hwid;

		/// <summary>
		/// 種別
		/// </summary>
		public string type;

		/// <summary>
		/// 発見したビーコンデバイスのデバイスメッセージ
		/// </summary>
		public string dm;

	}

}