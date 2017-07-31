using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using LineBotTemplate.Models.Webhook;
using LineBotTemplate.Services;

namespace LineBotTemplate.Controllers {

	/// <summary>
	/// LINE BotからのWebhook送信先API
	/// </summary>
	public class WebhookController : ApiController {

		/// <summary>
		/// POSTメソッド
		/// </summary>
		/// <param name="requestToken">リクエストトークン</param>
		/// <returns>常にステータス200のみを返す</returns>
		public async Task<HttpResponseMessage> Post( JToken requestToken ) {

			Trace.TraceInformation( "Webhook API Start" );

			// リクエストトークンをオブジェクトに詰める
			Trace.TraceInformation( "Request Token is : " + requestToken );
			RequestOfWebhook request = requestToken.ToObject<RequestOfWebhook>();

			// TODO チャンネルアクセストークンの取得
			string channelAccessToken = "";

			foreach( Event eventObj in request.events ) {

				// 送信元IDの取得
				string sourceId
					= eventObj.source.type.Equals( "user" ) ? eventObj.source.userId
					: eventObj.source.type.Equals( "group" ) ? eventObj.source.groupId
					: eventObj.source.type.Equals( "room" ) ? eventObj.source.roomId
					: null;
				if( sourceId == null ) {
					Trace.TraceError( "Source Id Not Found" );
					break;
				}


				switch( eventObj.type ) {

					// 友達追加またはブロック解除
					case "follow":

					// グループまたはトークルームに追加
					case "join":
						await this.ExecuteJoinEvent(
							channelAccessToken ,
							eventObj.replyToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							sourceId
						);
						break;

					// ブロック
					case "unfollow":

					// グループから退出させられる
					case "leave":
						this.ExecuteLeaveEvent(
							channelAccessToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							eventObj.source.groupId
						);
						break;

					// メッセージ
					case "message":

						switch( eventObj.message.type ) {

							// テキスト
							case "text":
								await this.ExecuteTextMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.text
								);
								break;

							// 画像
							case "image":
								await this.ExecuteImageMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 動画
							case "video":
								await this.ExecuteVideoMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 音声
							case "audio":
								await this.ExecuteAudioMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);

								break;

							// ファイル
							case "file":
								await this.ExecuteFileMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.fileName ,
									eventObj.message.fileSize ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 位置情報
							case "location":

								if( eventObj.message.latitude == null || eventObj.message.longitude == null ) {
									Trace.TraceError( "Not Found Latitude Or Longitude" );
									break;
								}

								await this.ExecuteLocationMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.title ,
									eventObj.message.address ,
									eventObj.message.latitude ,
									eventObj.message.longitude
								);
								break;
								
							// Sticker
							case "sticker":
								await this.ExecuteStickerMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.packageId ,
									eventObj.message.stickerId
								);
								break;

							// 想定外のメッセージ種別の場合は何もしない
							default:
								Trace.TraceError( "Unexpected Message Type" );
								break;

						}

						break;

					// ポストバック
					case "postback":
						await this.ExecutePostbackEvent(
							channelAccessToken ,
							eventObj.replyToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							sourceId ,
							eventObj.postback.data
						);
						break;

					// ビーコン
					case "beacon":
						await this.ExecuteBeaconEvent(
							channelAccessToken ,
							eventObj.replyToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							sourceId ,
							eventObj.beacon.hwid ,
							eventObj.beacon.type ,
							eventObj.beacon.dm
						);

						break;

					// 想定外のイベント種別の場合は何もしない
					default:
						Trace.TraceError( "Unexpected Type" );
						break;

				}

			}

			Trace.TraceInformation( "Webhook API End" );
			return new HttpResponseMessage( HttpStatusCode.OK );

		}

		/// <summary>
		/// 追加時イベント
		/// 友達登録、ブロック解除時、グループ追加時、トークルーム追加時
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">イベント送信元ID</param>
		private async Task ExecuteJoinEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string type ,
			string id
		) {

			Trace.TraceInformation( "Join Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "追加されました" )
				.Send();


			Trace.TraceInformation( "Join Event End" );

		}

		/// <summary>
		/// 退出時イベント
		/// ブロック時、グループ退出時
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">ユーザIDまたはグループID</param>
		private void ExecuteLeaveEvent(
			string channelAccessToken ,
			string timestamp ,
			string type ,
			string id
		) {

			Trace.TraceInformation( "Leave Event Start" );
			
			// ここにイベント内容を記載

			Trace.TraceInformation( "Leave Event End" );

		}

		/// <summary>
		/// テキストメッセージイベント
		/// </summary>
 		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="text">テキスト</param>
		private async Task ExecuteTextMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			string text
		) {

			Trace.TraceInformation( "Text Message Event Start" );
			
			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "テキスト" )
				.AddTextMessage( text )
				.Send();

			Trace.TraceInformation( "Text Message Event End" );

		}

		/// <summary>
		/// 画像メッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="binaryImage">バイナリ画像</param>
		private async Task ExecuteImageMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			byte[] binaryImage
		) {

			Trace.TraceInformation( "Image Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "画像" )
				.AddTextMessage( "バイナリサイズ : " + binaryImage.Length )
				.Send();

			Trace.TraceInformation( "Image Message Event End" );

		}

		/// <summary>
		/// 動画メッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="binaryVideo">バイナリ動画</param>
		private async Task ExecuteVideoMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			byte[] binaryVideo
		) {

			Trace.TraceInformation( "Video Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "動画" )
				.AddTextMessage( "バイナリサイズ : " + binaryVideo.Length )
				.Send();

			Trace.TraceInformation( "Video Message Event End" );

		}

		/// <summary>
		/// 音声メッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="binaryAudio">バイナリ音声</param>
		private async Task ExecuteAudioMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			byte[] binaryAudio
		) {

			Trace.TraceInformation( "Audio Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "音声" )
				.AddTextMessage( "バイナリサイズ : " + binaryAudio.Length )
				.Send();

			Trace.TraceInformation( "Audio Message Event End" );

		}

		/// <summary>
		/// ファイルメッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="fileName">ファイル名</param>
		/// <param name="fileSize">ファイルのバイト数</param>
		/// <param name="binaryFile">バイナリファイル</param>
		private async Task ExecuteFileMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			string fileName ,
			string fileSize ,
			byte[] binaryFile
		) {

			Trace.TraceInformation( "File Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "ファイル" )
				.AddTextMessage( 
					"ファイル名 : " + fileName + "\n" +
					"ファイルサイズ : " + fileSize + "\n" +
					"バイナリサイズ : " + binaryFile.Length 
				)
				.Send();

			Trace.TraceInformation( "File Message Event End" );

		}

		/// <summary>
		/// 位置情報メッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="title">タイトル</param>
		/// <param name="address">住所</param>
		/// <param name="latitude">緯度</param>
		/// <param name="longitude">経度</param>
		private async Task ExecuteLocationMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			string title ,
			string address ,
			string latitude ,
			string longitude
		) {

			Trace.TraceInformation( "Location Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "位置情報" )
				.AddTextMessage( 
					"タイトル : " + title + "\n" +
					"アドレス : " + address + "\n" +
					"緯度 : " + latitude + "\n" +
					"経度 : " + longitude
				)
				.Send();

			Trace.TraceInformation( "Location Message Event End" );

		}

		/// <summary>
		/// Stickerメッセージイベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="packageId">パッケージ識別子</param>
		/// <param name="stickerId">Sticker識別子</param>
		private async Task ExecuteStickerMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string messageId ,
			string packageId ,
			string stickerId
		) {

			Trace.TraceInformation( "Sticker Message Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "Sticker" )
				.AddTextMessage(
					"パッケージ識別子 : " + packageId + "\n" +
					"Sticker識別子 : " + stickerId
				)
				.Send();

			Trace.TraceInformation( "Sticker Message Event End" );

		}

		/// <summary>
		/// ポストバック送信時イベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">ユーザIDまたはグループIDまたはトークルームID</param>
		/// <param name="data">ポストバックデータ</param>
		private async Task ExecutePostbackEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string type ,
			string id ,
			string data
		) {

			Trace.TraceInformation( "Postback Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "ポストバック" )
				.AddTextMessage(
					"データ : " + data
				)
				.Send();


			Trace.TraceInformation( "Postback Event End" );

		}

		/// <summary>
		/// ビーコンデバイスの受信圏内出入り時イベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">ユーザIDまたはグループIDまたはトークルームID</param>
		/// <param name="hardWareId">ハードウェア識別子</param>
		/// <param name="beaconType">ビーコン種別</param>
		/// <param name="deviceMessage">デバイスメッセージ</param>
		private async Task ExecuteBeaconEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			string sourceType ,
			string sourceId ,
			string hardWareId ,
			string beaconType ,
			string deviceMessage
		) {

			Trace.TraceInformation( "Beacon Event Start" );

			// ここにイベント内容を記載
			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( "ビーコン" )
				.AddTextMessage(
					"ハードウェア識別子 : " + hardWareId + "\n" +
					"ビーコン種別 : " + beaconType + "\n" +
					"デバイスメッセージ : " + deviceMessage
				)
				.Send();

			Trace.TraceInformation( "Beacon Event End" );

		}

	}

}