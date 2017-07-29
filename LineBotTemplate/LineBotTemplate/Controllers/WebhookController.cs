using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using LineBotTemplate.Models.Webhook;
using System;

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
		public HttpResponseMessage Post( JToken requestToken ) {

			Trace.TraceInformation( "Webhook API Start" );

			// リクエストトークンをオブジェクトに詰める
			Trace.TraceInformation( "Request Token is : " + requestToken );
			RequestOfWebhook request = requestToken.ToObject<RequestOfWebhook>();

			foreach( RequestOfWebhook.Event eventObj in request.events ) {

				// 送信元IDの取得
				string sourceId
					= eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.User ? eventObj.source.userId
					: eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.Group ? eventObj.source.groupId
					: eventObj.source.type == RequestOfWebhook.Event.Source.SourceType.Room ? eventObj.source.roomId
					: null;
				if( sourceId == null ) {
					Trace.TraceError( "Error : Source Id Not Found" );
					break;
				}


				switch( eventObj.type ) {

					// 友達追加またはブロック解除
					case RequestOfWebhook.Event.EventType.Follow:

					// グループまたはトークルームに追加
					case RequestOfWebhook.Event.EventType.Join:
						this.ExecuteJoinEvent(
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
								this.ExecuteTextMessageEvent(
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.text
								);
								break;

							// 画像
							case RequestOfWebhook.Event.Message.MessageType.Image:
								this.ExecuteImageMessageEvent(
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id
								);
								break;

							// 動画
							case RequestOfWebhook.Event.Message.MessageType.Video:
								this.ExecuteVideoMessageEvent(
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id
								);
								break;

							// 音声
							case RequestOfWebhook.Event.Message.MessageType.Audio:
								this.ExecuteAudioMessageEvent(
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id
								);

								break;

							// ファイル
							case RequestOfWebhook.Event.Message.MessageType.File:
								this.ExecuteFileMessageEvent(
									eventObj.replyToken ,
									eventObj.timestamp ,
									eventObj.source.type ,
									sourceId ,
									eventObj.message.id ,
									eventObj.message.fileName ,
									eventObj.message.fileSize
								);
								break;

							// 位置情報
							case RequestOfWebhook.Event.Message.MessageType.Location: {

									if( !eventObj.message.latitude.HasValue || !eventObj.message.longitude.HasValue ) {
										Trace.TraceError( "Error : Not Found Latitude Or Longitude" );
										break;
									}

									this.ExecuteLocationMessageEvent(
										eventObj.replyToken ,
										eventObj.timestamp ,
										eventObj.source.type ,
										sourceId ,
										eventObj.message.id ,
										eventObj.message.title ,
										eventObj.message.address ,
										eventObj.message.latitude.Value ,
										eventObj.message.longitude.Value
									);
									break;

								}

							// Sticker
							case RequestOfWebhook.Event.Message.MessageType.Sticker:
								this.ExecuteStickerMessageEvent(
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
								Trace.TraceError( "Error : Unexpected Message Type" );
								break;

						}

						break;

					// ポストバック
					case RequestOfWebhook.Event.EventType.Postback:
						this.ExecutePostbackEvent(
							eventObj.replyToken ,
							eventObj.timestamp ,
							eventObj.source.type ,
							sourceId ,
							eventObj.postback.data
						);
						break;

					// ビーコン
					case RequestOfWebhook.Event.EventType.Beacon:
						this.ExecuteBeaconEvent(
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
						Trace.TraceError( "Error : Unexpected Type" );
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
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">イベント送信元ID</param>
		private void ExecuteJoinEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType type ,
			string id
		) {

			Trace.TraceInformation( "Join Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Join Event End" );

		}

		/// <summary>
		/// 退出時イベント
		/// ブロック時、グループ退出時
		/// </summary>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">ユーザIDまたはグループID</param>
		private void ExecuteLeaveEvent(
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType type ,
			string id
		) {

			Trace.TraceInformation( "Leave Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Leave Event End" );

		}

		/// <summary>
		/// テキストメッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="text">テキスト</param>
		private void ExecuteTextMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId ,
			string text
		) {

			Trace.TraceInformation( "Text Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Text Message Event End" );

		}

		/// <summary>
		/// 画像メッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		private void ExecuteImageMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId
		) {

			Trace.TraceInformation( "Image Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Image Message Event End" );

		}

		/// <summary>
		/// 動画メッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		private void ExecuteVideoMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId
		) {

			Trace.TraceInformation( "Video Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Video Message Event End" );

		}

		/// <summary>
		/// 音声メッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		private void ExecuteAudioMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId
		) {

			Trace.TraceInformation( "Audio Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Audio Message Event End" );

		}

		/// <summary>
		/// ファイルメッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="fileName">ファイル名</param>
		/// <param name="fileSize">ファイルのバイト数</param>
		private void ExecuteFileMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId ,
			string fileName ,
			string fileSize
		) {

			Trace.TraceInformation( "File Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "File Message Event End" );

		}

		/// <summary>
		/// 位置情報メッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="title">タイトル</param>
		/// <param name="address">住所</param>
		/// <param name="latitude">緯度</param>
		/// <param name="longitude">経度</param>
		private void ExecuteLocationMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId ,
			string title ,
			string address ,
			double latitude ,
			double longitude
		) {

			Trace.TraceInformation( "Location Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Location Message Event End" );

		}

		/// <summary>
		/// Stickerメッセージイベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="sourceType">イベント送信元種別</param>
		/// <param name="sourceId">イベント送信元ID</param>
		/// <param name="messageId">メッセージ識別子</param>
		/// <param name="packageId">パッケージ識別子</param>
		/// <param name="stickerId">Sticker識別子</param>
		private void ExecuteStickerMessageEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string messageId ,
			string packageId ,
			string stickerId
		) {

			Trace.TraceInformation( "Sticker Message Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Sticker Message Event End" );

		}

		/// <summary>
		/// ポストバック送信時イベント
		/// </summary>
		/// <param name="replyToken">リプライトークン</param>
		/// <param name="timestamp">Webhook受信日時</param>
		/// <param name="type">イベント送信元種別</param>
		/// <param name="id">ユーザIDまたはグループIDまたはトークルームID</param>
		/// <param name="data">ポストバックデータ</param>
		private void ExecutePostbackEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType type ,
			string id ,
			string data
		) {

			Trace.TraceInformation( "Postback Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Postback Event End" );

		}

		/// <summary>
		/// ビーコンデバイスの受信圏内出入り時イベント
		/// </summary>
		private void ExecuteBeaconEvent(
			string replyToken ,
			int timestamp ,
			RequestOfWebhook.Event.Source.SourceType sourceType ,
			string sourceId ,
			string hardWareId ,
			RequestOfWebhook.Event.Beacon.BeaconType beaconType ,
			string deviceMessage
		) {

			Trace.TraceInformation( "Beacon Event Start" );

			// ここにイベント内容を記載

			Trace.TraceInformation( "Beacon Event End" );

		}

	}

}