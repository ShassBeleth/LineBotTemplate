namespace LineBotTemplate.Models.Webhook {

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