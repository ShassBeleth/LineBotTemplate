using System;
using System.Diagnostics;
using LineBotTemplate.Models.ReplyMessage;

namespace LineBotTemplate.Services {
	public partial class ReplyMessageService {

		/// <summary>
		/// Imagemapに使用するアクション作成クラス
		/// </summary>
		public class ImagemapActionCreator {

			/// <summary>
			/// アクション
			/// </summary>
			private RequestOfReplyMessage.Message.ImageMapAction[] actions;

			/// <summary>
			/// アクション配列の長さ
			/// </summary>
			private int ActionsIndex { set; get; }

			/// <summary>
			/// アクション配列の最大値
			/// </summary>
			private readonly int MaxIndex = 50;

			/// <summary>
			/// アクション配列を作成する
			/// </summary>
			/// <param name="imagemapType">imagemap種別</param>
			/// <returns>自身のオブジェクト</returns>
			public ImagemapActionCreator CreateAction( RequestOfReplyMessage.Message.ImageMapAction.ImageMapActionType imagemapType ) {

				Trace.TraceInformation( "Create Imagemap Action Constractor Start" );
				Trace.TraceInformation( "Imagemap Type is : " + imagemapType );

				this.actions = new RequestOfReplyMessage.Message.ImageMapAction[ 1 ];
				this.ActionsIndex = 0;
				
				Trace.TraceInformation( "Create ImagemapAction Constractor End" );

				return this;

			}

			/// <summary>
			/// タップ時に指定のURLを開くアクションを追加する
			/// 2つめ以降のアクションは配列を作成しながら追加する
			/// アクションアイテムの上限を超えた場合は何もしない
			/// </summary>
			/// <param name="linkUri">WebページのURL</param>
			/// <param name="x">タップ領域の横方向の位置</param>
			/// <param name="y">タップ領域の縦方向の位置</param>
			/// <param name="width">タップ領域の幅</param>
			/// <param name="height">タップ領域の高さ</param>
			/// <returns>自身のオブジェクト</returns>
			public ImagemapActionCreator AddUrlAction( 
				string linkUri , 
				int x ,
				int y ,
				int width ,
				int height
			) {

				Trace.TraceInformation( "Add Url Action Start" );
				Trace.TraceInformation( "Link Uri is : " + linkUri );
				Trace.TraceInformation( "X is : " + x );
				Trace.TraceInformation( "Y is : " + y );
				Trace.TraceInformation( "Width is : " + width );
				Trace.TraceInformation( "Height is : " + height );

				if( this.ActionsIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Action Index == Max Index" );
					Trace.TraceInformation( "Add Url Action End" );
					return this;
				}

				Array.Resize( ref this.actions , this.ActionsIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.actions.Length );

				RequestOfReplyMessage.Message.ImageMapAction action = new RequestOfReplyMessage.Message.ImageMapAction() {
					type = RequestOfReplyMessage.Message.ImageMapAction.ImageMapActionType.Url ,
					linkUri = linkUri ,
					area = new RequestOfReplyMessage.Message.ImageMapAction.ImageMapArea() {
						x = x ,
						y = y ,
						width = width ,
						height = height
					}
				};

				this.actions[ this.ActionsIndex ] = action;
				this.ActionsIndex++;

				Trace.TraceInformation( "Add Url Action End" );

				return this;

			}

			/// <summary>
			/// タップ時に特定のメッセージ送信を行うアクションを追加する
			/// 2つめ以降のアクションは配列を作成しながら追加する
			/// アクションアイテムの上限を超えた場合は何もしない
			/// </summary>
			/// <param name="text">送信するメッセージ</param>
			/// <param name="x">タップ領域の横方向の位置</param>
			/// <param name="y">タップ領域の縦方向の位置</param>
			/// <param name="width">タップ領域の幅</param>
			/// <param name="height">タップ領域の高さ</param>
			/// <returns>自身のオブジェクト</returns>
			public ImagemapActionCreator AddMessageAction(
				string text ,
				int x ,
				int y ,
				int width ,
				int height
			) {

				Trace.TraceInformation( "Add Message Action Start" );
				Trace.TraceInformation( "Text is : " + text );
				Trace.TraceInformation( "X is : " + x );
				Trace.TraceInformation( "Y is : " + y );
				Trace.TraceInformation( "Width is : " + width );
				Trace.TraceInformation( "Height is : " + height );

				if( this.ActionsIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Action Index == Max Index" );
					Trace.TraceInformation( "Add Message Action End" );
					return this;
				}

				Array.Resize( ref this.actions , this.ActionsIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.actions.Length );

				RequestOfReplyMessage.Message.ImageMapAction action = new RequestOfReplyMessage.Message.ImageMapAction() {
					type = RequestOfReplyMessage.Message.ImageMapAction.ImageMapActionType.Message ,
					text = text ,
					area = new RequestOfReplyMessage.Message.ImageMapAction.ImageMapArea() {
						x = x ,
						y = y ,
						width = width ,
						height = height
					}
				};

				this.actions[ this.ActionsIndex ] = action;
				this.ActionsIndex++;

				Trace.TraceInformation( "Add Message Action End" );

				return this;

			}
			
			/// <summary>
			/// アクションの配列を返す
			/// </summary>
			/// <returns>アクションの配列</returns>
			public RequestOfReplyMessage.Message.ImageMapAction[] GetActions() => this.actions;

		}

	}
}