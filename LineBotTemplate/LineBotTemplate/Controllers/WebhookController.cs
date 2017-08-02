using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using LineBotTemplate.Models.Webhook;
using LineBotTemplate.Services;
using LineBotTemplate.Models.Profile;

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
			string channelAccessToken = this.GetChannelAccessToken();

			foreach( RequestOfWebhook.Event eventObj in request.events ) {

				// 送信元IDの取得
				string sourceId
					= eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.User ? eventObj.source.userId
					: eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.Group ? eventObj.source.groupId
					: eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.Room ? eventObj.source.roomId
					: null;
				if( sourceId == null ) {
					Trace.TraceError( "Source Id Not Found" );
					break;
				}
				
				switch( eventObj.type ) {

					// 友達追加またはブロック解除
					case RequestOfWebhook.Event.EventType.Follow:

					// グループまたはトークルームに追加
					case RequestOfWebhook.Event.EventType.Join:
						await this.ExecuteJoinEvent(
							channelAccessToken ,
							eventObj.replyToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							sourceId
						);
						break;

					// ブロック
					case RequestOfWebhook.Event.EventType.Unfollow:

					// グループから退出させられる
					case RequestOfWebhook.Event.EventType.Leave:
						this.ExecuteLeaveEvent(
							channelAccessToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							eventObj.source.groupId
						);
						break;

					// メッセージ
					case RequestOfWebhook.Event.EventType.Message:

						switch( eventObj.message.type ) {

							// テキスト
							case RequestOfWebhook.Event.Message.MessageType.Text:
								await this.ExecuteTextMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.text
								);
								break;

							// 画像
							case RequestOfWebhook.Event.Message.MessageType.Image:
								await this.ExecuteImageMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 動画
							case RequestOfWebhook.Event.Message.MessageType.Video:
								await this.ExecuteVideoMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 音声
							case RequestOfWebhook.Event.Message.MessageType.Audio:
								await this.ExecuteAudioMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);

								break;

							// ファイル
							case RequestOfWebhook.Event.Message.MessageType.File:
								await this.ExecuteFileMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.fileName ,
									eventObj.message.fileSize ,
									await new ContentService().GetContent( eventObj.message.id , channelAccessToken )
								);
								break;

							// 位置情報
							case RequestOfWebhook.Event.Message.MessageType.Location:

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
									eventObj.message.title ,
									eventObj.message.address ,
									eventObj.message.latitude ,
									eventObj.message.longitude
								);
								break;

							// Sticker
							case RequestOfWebhook.Event.Message.MessageType.Sticker:
								await this.ExecuteStickerMessageEvent(
									channelAccessToken ,
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
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
					case RequestOfWebhook.Event.EventType.Postback:
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
					case RequestOfWebhook.Event.EventType.Beacon:
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
		/// チャンネルアクセストークンの取得
		/// </summary>
		/// <returns>チャンネルアクセストークン</returns>
		private string GetChannelAccessToken() {

			string token = "FMEYNCzDFwMSzMErx5VMh6xeePaZR7n+zQ3NJckfElYFsoULEytM6DFqrVyIbtUXrrVaFYOZVkm0PCZ7ENeyq2ai7wt7nIfcIFmiEXHF+5UrLyrm10McfYYFf30bknRV1I0uIpKPhxj9RRYHG1Y2AgdB04t89/1O/w1cDnyilFU=";

			// TODO ここに実装方法を記述
			// 例：app.configから取得　定数クラスから取得等

			return token;

		}

		/// <summary>
		/// 追加時イベント
		/// 友達登録、ブロック解除時、グループ追加時、トークルーム追加時
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		private async Task ExecuteJoinEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId
		) {

			Trace.TraceInformation( "Join Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( channelAccessToken , replyToken , "追加されました" );
			
			Trace.TraceInformation( "Join Event End" );

		}

		/// <summary>
		/// 退出時イベント
		/// ブロック時、グループ退出時
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">ユーザIDまたはグループID</param>
		private void ExecuteLeaveEvent(
			string channelAccessToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId
		) {

			Trace.TraceInformation( "Leave Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );

			// TODO ここにイベント内容を記載

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
		/// <param name="text">テキスト</param>
		private async Task ExecuteTextMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string text
		) {

			Trace.TraceInformation( "Text Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Text is : " + text );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( replyToken , channelAccessToken , "テキスト\n" + text );

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
		/// <param name="binaryImage">バイナリ画像</param>
		private async Task ExecuteImageMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			byte[] binaryImage
		) {

			Trace.TraceInformation( "Image Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Binary Image Length is : " + binaryImage.Length );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( replyToken , channelAccessToken , "画像" );

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
		/// <param name="binaryVideo">バイナリ動画</param>
		private async Task ExecuteVideoMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			byte[] binaryVideo
		) {

			Trace.TraceInformation( "Video Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Binary Video Length is : " + binaryVideo.Length );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( replyToken , channelAccessToken , "動画" );

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
		/// <param name="binaryAudio">バイナリ音声</param>
		private async Task ExecuteAudioMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			byte[] binaryAudio
		) {

			Trace.TraceInformation( "Audio Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Binary Audio Length is : " + binaryAudio.Length );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( replyToken , channelAccessToken , "音声" );

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
		/// <param name="fileName">ファイル名</param>
		/// <param name="fileSize">ファイルのバイト数</param>
		/// <param name="binaryFile">バイナリファイル</param>
		private async Task ExecuteFileMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string fileName ,
			string fileSize ,
			byte[] binaryFile
		) {

			Trace.TraceInformation( "File Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "File Name Id is : " + fileName );
			Trace.TraceInformation( "File Size Id is : " + fileSize );
			Trace.TraceInformation( "Binary File Length is : " + binaryFile.Length );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( 
				replyToken , 
				channelAccessToken , 
				"ファイル\n" +
				"ファイル名:" + fileName + "\n" +
				"ファイルサイズ:" + fileSize
			);

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
		/// <param name="title">タイトル</param>
		/// <param name="address">住所</param>
		/// <param name="latitude">緯度</param>
		/// <param name="longitude">経度</param>
		private async Task ExecuteLocationMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string title ,
			string address ,
			string latitude ,
			string longitude
		) {

			Trace.TraceInformation( "Location Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Title is : " + title );
			Trace.TraceInformation( "Address is : " + address );
			Trace.TraceInformation( "Latitude is : " + latitude );
			Trace.TraceInformation( "Longitude is : " + longitude );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( 
				replyToken , 
				channelAccessToken , 
				"位置情報\n" +  
				"タイトル:" + title +
				"住所:" + address +
				"緯度:" + latitude + 
				"経度:" + longitude
			);

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
		/// <param name="packageId">パッケージ識別子</param>
		/// <param name="stickerId">Sticker識別子</param>
		private async Task ExecuteStickerMessageEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string packageId ,
			string stickerId
		) {

			Trace.TraceInformation( "Sticker Message Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Package Id is : " + packageId );
			Trace.TraceInformation( "Sticker Id is : " + stickerId );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( 
				replyToken , 
				channelAccessToken , 
				"Sticker\n" +
				"パッケージ:" + packageId +
				"Sticker:" + stickerId
			);

			Trace.TraceInformation( "Sticker Message Event End" );

		}

		/// <summary>
		/// ポストバック送信時イベント
		/// </summary>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceI">イベント送信元種別</param>
		/// <param name="sourceId">ユーザIDまたはグループIDまたはトークルームID</param>
		/// <param name="data">ポストバックデータ</param>
		private async Task ExecutePostbackEvent(
			string channelAccessToken ,
			string replyToken ,
			string timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string data
		) {

			Trace.TraceInformation( "Postback Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Data is : " + data );

			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( replyToken , channelAccessToken , "ポストバック\n" + data );

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
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string hardWareId ,
			RequestOfWebhook.Event.Beacon.BeaconType beaconType ,
			string deviceMessage
		) {

			Trace.TraceInformation( "Beacon Event Start" );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );
			Trace.TraceInformation( "Reply Token is : " + replyToken );
			Trace.TraceInformation( "Timestamp is : " + timestamp );
			Trace.TraceInformation( "Source Type is : " + sourceType );
			Trace.TraceInformation( "Source Id is : " + sourceId );
			Trace.TraceInformation( "Hard Ware Id is : " + hardWareId );
			Trace.TraceInformation( "Beacon Type is : " + beaconType );
			Trace.TraceInformation( "Device Message is : " + deviceMessage );


			// TODO ここにイベント内容を記載
			// 以下サンプル
			await this.ReplyTextMessageSampleEvent( 
				replyToken , 
				channelAccessToken , 
				"ビーコン\n" +
				"ハードウェア:" + hardWareId + "\n" +
				"種別:" + beaconType + "\n" +
				"デバイスメッセージ" + deviceMessage
			);

			Trace.TraceInformation( "Beacon Event End" );

		}

		/// <summary>
		/// テキストサンプル
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="text">テキスト</param>
		/// <returns></returns>
		private async Task ReplyTextMessageSampleEvent(
			string replyToken ,
			string channelAccessToken ,
			string text
		) => await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( text )
				.Send();

		/// <summary>
		/// プロフィールを表示するサンプル
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <param name="userId">ユーザID</param>
		/// <returns></returns>
		private async Task ReplyProfileMessageSampleEvent(
			string replyToken ,
			string channelAccessToken ,
			string userId
		) {

			ResponseOfProfile profile = await new ProfileService().GetProfile( userId , channelAccessToken );

			await new ReplyMessageService( replyToken , channelAccessToken )
				.AddTextMessage( 
					"プロフィール\n" +
					"表示名：" + profile.displayName + "\n" +
					"ステータスメッセージ：" + profile.statusMessage
				)
				.Send();

		}

	}

}