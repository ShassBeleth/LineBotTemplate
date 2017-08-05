using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LineBotTemplate.Models.ReplyMessage;
using Newtonsoft.Json;

namespace LineBotTemplate.Services {

	/// <summary>
	/// リプライメッセージ
	/// </summary>
	public partial class ReplyMessageService {

		/// <summary>
		/// リクエストEntity
		/// </summary>
		private RequestOfReplyMessage Request { set; get; }

		/// <summary>
		/// メッセージの要素数
		/// </summary>
		private int MessagesIndex { set; get; }

		/// <summary>
		/// メッセージの最大要素数
		/// </summary>
		private readonly int MaxIndex = 5;

		/// <summary>
		/// チャンネルアクセストークン
		/// </summary>
		private string ChannelAccessToken { set; get; }

		/// <summary>
		/// コンストラクタ
		/// リプライトークンを保持する
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		public ReplyMessageService( string replyToken , string channelAccessToken ) {

			Trace.TraceInformation( "Reply Message Service Constractor Start" );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );

			this.Request = new RequestOfReplyMessage() {
				replyToken = replyToken ,
				messages = new RequestOfReplyMessage.Message[ 1 ]
			};
			this.MessagesIndex = 0;

			this.ChannelAccessToken = channelAccessToken;

			Trace.TraceInformation( "Reply Message Service Constractor End" );

		}

		/// <summary>
		/// テキストメッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="messageText">メッセージテキスト</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddTextMessage( string messageText ) {

			Trace.TraceInformation( "Add Text Message Start" );
			Trace.TraceInformation( "Message Text is : " + messageText );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Text Message End" );
				return this;
			}
			
			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Text ,
				text = messageText
			};
			
			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Text Message End" );

			return this;

		}

		/// <summary>
		/// 画像メッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="originalContentUrl">画像URL</param>
		/// <param name="previewImageUrl">プレビュー画像URL</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddImageMessage( 
			string originalContentUrl , 
			string previewImageUrl 
		) {

			Trace.TraceInformation( "Add Image Message Start" );
			Trace.TraceInformation( "Original Content Url is : " + originalContentUrl );
			Trace.TraceInformation( "Preview Content Url is : " + previewImageUrl );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Image Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Image ,
				originalContentUrl = originalContentUrl ,
				previewImageUrl = previewImageUrl
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Image Message End" );

			return this;

		}

		/// <summary>
		/// 動画メッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="originalContentUrl">動画URL</param>
		/// <param name="previewImageUrl">プレビュー画像URL</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddVideoMessage(
			string originalContentUrl ,
			string previewImageUrl
		) {

			Trace.TraceInformation( "Add Video Message Start" );
			Trace.TraceInformation( "Original Content Url is : " + originalContentUrl );
			Trace.TraceInformation( "Preview Content Url is : " + previewImageUrl );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Video Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Video ,
				originalContentUrl = originalContentUrl ,
				previewImageUrl = previewImageUrl
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Video Message End" );

			return this;

		}

		/// <summary>
		/// 音声メッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="originalContentUrl">音声ファイルのURL</param>
		/// <param name="duration">音声ファイルの時間長さ</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddAudioMessage(
			string originalContentUrl ,
			int duration
		) {

			Trace.TraceInformation( "Add Audio Message Start" );
			Trace.TraceInformation( "Original Content Url is : " + originalContentUrl );
			Trace.TraceInformation( "Duration is : " + duration );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Audio Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Audio ,
				originalContentUrl = originalContentUrl ,
				duration = duration
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Audio Message End" );

			return this;

		}

		/// <summary>
		/// 位置情報メッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="title">タイトル</param>
		/// <param name="address">アドレス</param>
		/// <param name="latitude">緯度</param>
		/// <param name="longitude">経度</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddLocationMessage(
			string title ,
			string address ,
			double latitude ,
			double longitude
		) {

			Trace.TraceInformation( "Add Location Message Start" );
			Trace.TraceInformation( "Title is : " + title );
			Trace.TraceInformation( "Address is : " + address );
			Trace.TraceInformation( "Latitude is : " + latitude );
			Trace.TraceInformation( "Longitude is : " + longitude );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Location Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Location ,
				title = title ,
				address = address ,
				latitude = latitude ,
				longitude = longitude
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Location Message End" );

			return this;

		}

		/// <summary>
		/// Stickerメッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="packageId">パッケージ識別子</param>
		/// <param name="stickerId">Sticker識別子</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddStickerMessage(
			string packageId ,
			string stickerId
		) {

			Trace.TraceInformation( "Add Sticker Message Start" );
			Trace.TraceInformation( "Package ID is : " + packageId );
			Trace.TraceInformation( "Sticker ID is : " + stickerId );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Sticker Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Sticker ,
				packageId = packageId ,
				stickerId = stickerId
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Sticker Message End" );

			return this;

		}

		/// <summary>
		/// リンク付き画像コンテンツを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// </summary>
		/// <param name="baseUrl">画像のURL</param>
		/// <param name="altText">代替テキスト</param>
		/// <param name="width">幅</param>
		/// <param name="height">高さ</param>
		/// <param name="actions">タップ時のアクション</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddImagemapMessage(
			string baseUrl ,
			string altText ,
			int width ,
			int height ,
			RequestOfReplyMessage.Message.ImageMapAction[] actions
		) {

			Trace.TraceInformation( "Add Imagemap Message Start" );
			Trace.TraceInformation( "Base Url is : " + baseUrl );
			Trace.TraceInformation( "Alt Text is : " + altText );
			Trace.TraceInformation( "Width is : " + width );
			Trace.TraceInformation( "Height is : " + height );
			Trace.TraceInformation( "Action Length is : " + actions.Length );
			
			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Sticker Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Imagemap ,
				baseUrl = baseUrl ,
				altText = altText ,
				baseSize = new RequestOfReplyMessage.Message.BaseSize() {
					width = width ,
					height = height
				} ,
				actions = actions
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Sticker Message End" );

			return this;

		}

		/// <summary>
		/// 画像、タイトル、テキスト、複数のアクションボタンを組み合わせたテンプレートメッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// アクションボタンは最大4つ
		/// </summary>
		/// <param name="altText">代替テキスト</param>
		/// <param name="thumbnailImageUrl">画像のURL</param>
		/// <param name="title">タイトル</param>
		/// <param name="text">説明文</param>
		/// <param name="actions">ボタン押下時アクション</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddButtonsMessage(
			string altText ,
			string thumbnailImageUrl ,
			string title ,
			string text ,
			RequestOfReplyMessage.Message.Template.TemplateAction[] actions
		) {

			Trace.TraceInformation( "Add Button Message Start" );
			Trace.TraceInformation( "Alt Text is : " + altText );
			Trace.TraceInformation( "Thumbnail Image Url is : " + thumbnailImageUrl );
			Trace.TraceInformation( "Title is : " + title );
			Trace.TraceInformation( "Text is : " + text );
			Trace.TraceInformation( "Actions Length is : " + actions.Length );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Button Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message.Template template = new RequestOfReplyMessage.Message.Template() {
				type = RequestOfReplyMessage.Message.Template.TemplateType.Buttons ,
				thumbnailImageUrl = thumbnailImageUrl ,
				title = title ,
				text = text ,
				actions = actions
			};

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Template ,
				altText = altText ,
				template = template
			};
			
			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Button Message End" );

			return this;

		}

		/// <summary>
		/// 2つのアクションボタンを提示するテンプレートメッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// アクションボタンは最大2つ
		/// </summary>
		/// <param name="altText">代替テキスト</param>
		/// <param name="text">説明文</param>
		/// <param name="actions">ボタン押下時アクション</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddConfirmMessage(
			string altText ,
			string text ,
			RequestOfReplyMessage.Message.Template.TemplateAction[] actions
		) {

			Trace.TraceInformation( "Add Confirm Message Start" );
			Trace.TraceInformation( "Alt Text is : " + altText );
			Trace.TraceInformation( "Text is : " + text );
			Trace.TraceInformation( "Actions Length is : " + actions.Length );
			
			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Confirm Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message.Template template = new RequestOfReplyMessage.Message.Template() {
				type = RequestOfReplyMessage.Message.Template.TemplateType.Confirm ,
				text = text ,
				actions = actions
			};

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Template ,
				altText = altText ,
				template = template
			};

			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Confirm Message End" );

			return this;

		}

		/// <summary>
		/// 複数の情報を並べて提示できるカルーセル型のテンプレートメッセージを送信リストに追加する
		/// 初回以外は配列を拡大させながら追加する
		/// 5通目以降は追加されない
		/// カラムは最大5つ
		/// </summary>
		/// <param name="altText">代替テキスト</param>
		/// <param name="columns">カラム</param>
		/// <returns>自身のオブジェクト</returns>
		public ReplyMessageService AddCarouselMessage(
			string altText ,
			RequestOfReplyMessage.Message.Template.Column[] columns
		) {

			Trace.TraceInformation( "Add Carousel Message Start" );
			Trace.TraceInformation( "Alt Text is : " + altText );
			Trace.TraceInformation( "Column Length is : " + columns.Length );

			if( this.MessagesIndex == this.MaxIndex ) {
				Trace.TraceWarning( "Message Index == Max Index" );
				Trace.TraceInformation( "Add Carousel Message End" );
				return this;
			}

			Array.Resize( ref this.Request.messages , this.MessagesIndex + 1 );
			Trace.TraceInformation( "Messages Size is : " + this.Request.messages.Length );

			RequestOfReplyMessage.Message.Template template = new RequestOfReplyMessage.Message.Template() {
				type = RequestOfReplyMessage.Message.Template.TemplateType.Carousel ,
				columns = columns
			};

			RequestOfReplyMessage.Message message = new RequestOfReplyMessage.Message() {
				type = RequestOfReplyMessage.Message.MessageType.Template ,
				altText = altText ,
				template = template
			};
			
			this.Request.messages[ this.MessagesIndex ] = message;
			this.MessagesIndex++;

			Trace.TraceInformation( "Add Carousel Message End" );

			return this;

		}

		/// <summary>
		/// メッセージの送信
		/// </summary>
		/// <returns></returns>
		public async Task Send() {

			Trace.TraceInformation( "Reply Message Send Start" );

			string jsonRequest = JsonConvert.SerializeObject( this.Request );
			Trace.TraceInformation( "Reply Message Request is : " + jsonRequest );
			StringContent content = new StringContent( jsonRequest );
			content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
			client.DefaultRequestHeaders.Add( "Authorization" , "Bearer {" + this.ChannelAccessToken + "}" );

			try {

				HttpResponseMessage response = await client.PostAsync( "https://api.line.me/v2/bot/message/reply" , content );

				switch( response.StatusCode ) {

					// 200 リクエスト成功
					case System.Net.HttpStatusCode.OK:
						string result = await response.Content.ReadAsStringAsync();
						Trace.TraceInformation( "Status is : 200" );
						break;

					// 400 パラメータ誤り
					case System.Net.HttpStatusCode.BadRequest:
						Trace.TraceError( "Status is : 400 Bad Request" );
						break;

					// 401 権限ヘッダが正しくない
					case System.Net.HttpStatusCode.Unauthorized:
						Trace.TraceError( "Status is : 401 Unauthorized" );
						break;

					// 403 APIの利用権限がない
					case System.Net.HttpStatusCode.Forbidden:
						Trace.TraceError( "Status is : 403 Forbidden" );
						break;

					// 500 APIサーバ側のエラー
					case System.Net.HttpStatusCode.InternalServerError:
						Trace.TraceError( "Status is : 500 Internal Server Error" );
						break;

					default:

						// 429 アクセス頻度制限越え
						if( (int)response.StatusCode == 429 ) {
							Trace.TraceError( "Status is : 429 Too Many Requests" );
						}
						// その他エラー
						else {
							Trace.TraceError( "Status is not 200" );
							throw new Exception();
						}
						break;

				}

				response.Dispose();
			}
			catch( ArgumentNullException e ) {
				Trace.TraceError( "Reply Message Send Argument Null Exception " + e.Message );
			}
			catch( Exception e ) {
				Trace.TraceError( "Reply Message Send 予期せぬ例外 " + e.Message );
			}

			content.Dispose();
			client.Dispose();

			Trace.TraceInformation( "Reply Message Send End" );

		}

	}

}