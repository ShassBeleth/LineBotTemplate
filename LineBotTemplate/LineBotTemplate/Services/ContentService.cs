using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LineBotTemplate.Services {

	/// <summary>
	/// Contentsに関わるServiceクラス
	/// </summary>
	public class ContentService {

		/// <summary>
		/// Contentから画像、動画、音声にアクセスするAPIを呼び、バイナリデータを返す
		/// </summary>
		/// <param name="messageId">メッセージID</param>
		/// <param name="channelAccessToken">チャンネルアクセストークン</param>
		/// <returns>バイナリデータ</returns>
		public async Task<byte[]> GetContent(
			string messageId ,
			string channelAccessToken
		) {

			Trace.TraceInformation( "Get Content Start" );
			Trace.TraceInformation( "Message Id is : " + messageId );
			Trace.TraceInformation( "Channel Access Token is : " + channelAccessToken );

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
			client.DefaultRequestHeaders.Add( "Authorization" , "Bearer {" + channelAccessToken + "}" );

			byte[] result = null;
			try {
				HttpResponseMessage response = await client.GetAsync( "https://api.line.me/v2/bot/message/" + messageId + "/content" );
				switch( response.StatusCode ) {

					// 200 リクエスト成功
					case System.Net.HttpStatusCode.OK:
						result = await response.Content.ReadAsByteArrayAsync();
						Trace.TraceInformation( "Get Binary Image is : " + result != null ? "SUCCESS" : "FAILED" );
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
				client.Dispose();
				
			}
			catch( ArgumentNullException e ) {
				Trace.TraceError( "Get Content Argument Null Exception : " + e.Message );
				client.Dispose();
				result = null;
			}
			catch( Exception e ) {
				Trace.TraceError( "Get Content 予期せぬ例外 : " + e.Message );
				client.Dispose();
				result = null;
			}

			Trace.TraceInformation( "Get Content End" );
			return result;

		}

	}

}