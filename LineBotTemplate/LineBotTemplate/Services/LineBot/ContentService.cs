using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LineBotTemplate.Services.LineBot {

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
				result = await response.Content.ReadAsByteArrayAsync();
				response.Dispose();
				client.Dispose();
				Trace.TraceInformation( "Get Binary Image is : " + result != null ? "SUCCESS" : "FAILED" );
			}
			catch( ArgumentNullException e ) {
				Trace.TraceError( "Get Content Argument Null Exception : " + e.Message );
				client.Dispose();
				result = null;
			}
			catch( HttpRequestException e ) {
				Trace.TraceError( "Get Content Http Request Exception : " + e.Message );
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