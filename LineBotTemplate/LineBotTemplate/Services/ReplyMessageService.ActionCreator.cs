using System;
using System.Diagnostics;
using LineBotTemplate.Models.ReplyMessage;

namespace LineBotTemplate.Services {
	public partial class ReplyMessageService {

		/// <summary>
		/// テンプレートに使用するアクション作成クラス
		/// </summary>
		public class ActionCreator {

			/// <summary>
			/// アクション
			/// </summary>
			private RequestOfReplyMessage.Message.Template.TemplateAction[] actions;

			/// <summary>
			/// アクション配列の長さ
			/// </summary>
			private int ActionsIndex { set; get; }

			/// <summary>
			/// アクション配列の最大値
			/// </summary>
			private int MaxIndex { set; get; }

			/// <summary>
			/// アクション配列を作成する
			/// </summary>
			/// <param name="templateType">テンプレート種別</param>
			/// <returns>自身のオブジェクト</returns>
			public ActionCreator CreateAction( RequestOfReplyMessage.Message.Template.TemplateType templateType ) {

				Trace.TraceInformation( "Create Action Constractor Start" );
				Trace.TraceInformation( "Template Type is : " + templateType );

				this.actions = new RequestOfReplyMessage.Message.Template.TemplateAction[ 1 ];

				if( RequestOfReplyMessage.Message.Template.TemplateType.Buttons == templateType ) {
					this.MaxIndex = 4;
				}
				else if( RequestOfReplyMessage.Message.Template.TemplateType.Confirm == templateType ) {
					this.MaxIndex = 2;
				}
				else if( RequestOfReplyMessage.Message.Template.TemplateType.Carousel == templateType ) {
					this.MaxIndex = 3;
				}

				this.ActionsIndex = 0;

				Trace.TraceInformation( "Max Action Index is : " + this.MaxIndex );
				Trace.TraceInformation( "Create Action Constractor End" );

				return this;

			}

			/// <summary>
			/// タップ時にdataで指定された文字列がpostback eventとしてWebhookで通知されるアクションを追加する
			/// 2つめ以降のアクションは配列を作成しながら追加する
			/// アクションアイテムの上限を超えた場合は何もしない
			/// </summary>
			/// <param name="label">アクション表示名</param>
			/// <param name="data">Webhookに送信される文字列データ</param>
			/// <param name="text">アクション実行時に送信されるテキスト</param>
			/// <returns>自身のオブジェクト</returns>
			public ActionCreator AddPostbackAction( string label , string data , string text ) {

				Trace.TraceInformation( "Add PostBack Action Start" );
				Trace.TraceInformation( "Label is : " + label );
				Trace.TraceInformation( "Data is : " + data );
				Trace.TraceInformation( "Text is : " + text );

				if( this.ActionsIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Action Index == Max Index" );
					Trace.TraceInformation( "Add PostBack Action End" );
					return this;
				}

				Array.Resize( ref this.actions , this.ActionsIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.actions.Length );

				RequestOfReplyMessage.Message.Template.TemplateAction action = new RequestOfReplyMessage.Message.Template.TemplateAction() {
					type = RequestOfReplyMessage.Message.Template.TemplateAction.ActionType.Postback ,
					label = label ,
					data = data ,
					text = text
				};
				
				this.actions[ this.ActionsIndex ] = action;
				this.ActionsIndex++;

				Trace.TraceInformation( "Add PostBack Action End" );

				return this;

			}

			/// <summary>
			/// タップ時にtextで指定された文字列がユーザの発言として送信されるアクションを追加する
			/// 2つめ以降のアクションは配列を作成しながら追加する
			/// </summary>
			/// <param name="label">アクション表示名</param>
			/// <param name="text">アクション実行時に送信されるテキスト</param>
			/// <returns>自身のオブジェクト</returns>
			public ActionCreator AddMessageAction( string label , string text ) {

				Trace.TraceInformation( "Add Message Action Start" );
				Trace.TraceInformation( "Label is : " + label );
				Trace.TraceInformation( "Text is : " + text );

				if( this.ActionsIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Action Index == Max Index" );
					Trace.TraceInformation( "Add Message Action End" );
					return this;
				}

				Array.Resize( ref this.actions , this.ActionsIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.actions.Length );

				RequestOfReplyMessage.Message.Template.TemplateAction action = new RequestOfReplyMessage.Message.Template.TemplateAction() {
					type = RequestOfReplyMessage.Message.Template.TemplateAction.ActionType.Message ,
					label = label ,
					text = text
				};
				
				this.actions[ this.ActionsIndex ] = action;
				this.ActionsIndex++;

				Trace.TraceInformation( "Add Message Action End" );

				return this;

			}

			/// <summary>
			/// タップ時にuriで指定されたURIを開くアクションを追加する
			/// 2つめ以降のアクションは配列を作成しながら追加する
			/// </summary>
			/// <param name="label">アクション表示名</param>
			/// <param name="uri">URI</param>
			/// <returns>自身のオブジェクト</returns>
			public ActionCreator AddUriAction( string label , string uri ) {

				Trace.TraceInformation( "Add Uri Action Start" );
				Trace.TraceInformation( "Label is : " + label );
				Trace.TraceInformation( "Url is : " + uri );

				if( this.ActionsIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Action Index == Max Index" );
					Trace.TraceInformation( "Add Uri Action End" );
					return this;
				}
				Array.Resize( ref this.actions , this.ActionsIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.actions.Length );

				RequestOfReplyMessage.Message.Template.TemplateAction action = new RequestOfReplyMessage.Message.Template.TemplateAction() {
					type = RequestOfReplyMessage.Message.Template.TemplateAction.ActionType.Uri ,
					label = label ,
					uri = uri
				};

				this.actions[ this.ActionsIndex ] = action;
				this.ActionsIndex++;

				Trace.TraceInformation( "Add Uri Action End" );

				return this;

			}

			/// <summary>
			/// アクションの配列を返す
			/// </summary>
			/// <returns>アクションの配列</returns>
			public RequestOfReplyMessage.Message.Template.TemplateAction[] GetActions() => this.actions;

		}

	}
}