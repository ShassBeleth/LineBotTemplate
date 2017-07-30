namespace LineBotTemplate.Models.ReplyMessage {
	public partial class RequestOfReplyMessage {
		public partial class Message {
			public partial class ImageMapAction {

				/// <summary>
				/// タップ領域の定義
				/// </summary>
				public class ImageMapArea {

					/// <summary>
					/// タップ領域の横方向の位置
					/// </summary>
					public int x;

					/// <summary>
					/// タップ領域の縦方向の位置
					/// </summary>
					public int y;

					/// <summary>
					/// タップ領域の幅
					/// </summary>
					public int width;

					/// <summary>
					/// タップ領域の高さ
					/// </summary>
					public int height;

				}

			}
		}
	}
}