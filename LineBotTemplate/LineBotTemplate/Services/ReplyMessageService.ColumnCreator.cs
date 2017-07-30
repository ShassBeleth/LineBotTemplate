using System;
using System.Diagnostics;
using LineBotTemplate.Models.ReplyMessage;

namespace LineBotTemplate.Services {
	public partial class ReplyMessageService {

		/// <summary>
		/// カラム作成クラス
		/// </summary>
		public class ColumnCreator {

			/// <summary>
			/// カラム配列
			/// </summary>
			private RequestOfReplyMessage.Message.Template.Column[] columns;

			/// <summary>
			/// カラム配列の長さ
			/// </summary>
			private int ColumnIndex { set; get; }

			/// <summary>
			/// カラム配列の最大値
			/// </summary>
			private int MaxIndex { set; get; }

			/// <summary>
			/// カラム配列を作成する
			/// </summary>
			/// <returns>自身のオブジェクト</returns>
			public ColumnCreator CreateColumn() {

				Trace.TraceInformation( "Create Column Constractor Start" );

				this.columns = new RequestOfReplyMessage.Message.Template.Column [ 1 ];
				this.MaxIndex = 5;
				this.ColumnIndex = 0;

				Trace.TraceInformation( "Max Column Index is : " + this.MaxIndex );

				Trace.TraceInformation( "Create Column Constractor End" );

				return this;

			}

			/// <summary>
			/// カラムを追加する
			/// 2つめ以降のカラムは配列を作成しながら追加する
			/// </summary>
			/// <param name="thumbnailImageUrl">画像のURL</param>
			/// <param name="title">タイトル</param>
			/// <param name="text">説明文</param>
			/// <param name="actions">ボタン</param>
			/// <returns></returns>
			public ColumnCreator AddColumn(
				string thumbnailImageUrl ,
				string title ,
				string text ,
				RequestOfReplyMessage.Message.Template.TemplateAction[] actions
			) {

				Trace.TraceInformation( "Add Column Start" );
				Trace.TraceInformation( "Thumbnail Image Url is : " + thumbnailImageUrl );
				Trace.TraceInformation( "Title is : " + title );
				Trace.TraceInformation( "Text is : " + text );
				Trace.TraceInformation( "Actions Length is : " + actions.Length );

				if( this.ColumnIndex == this.MaxIndex ) {
					Trace.TraceWarning( "Column Index == Max Index" );
					Trace.TraceInformation( "Add Column End" );
					return this;
				}

				Array.Resize( ref this.columns , this.ColumnIndex + 1 );
				Trace.TraceInformation( "Actions Size is : " + this.columns.Length );

				RequestOfReplyMessage.Message.Template.Column column = new RequestOfReplyMessage.Message.Template.Column() {
					thumbnailImageUrl = thumbnailImageUrl ,
					title = title ,
					text = text ,
					actions = actions
				};
				
				this.columns[ this.ColumnIndex ] = column;
				this.ColumnIndex++;

				Trace.TraceInformation( "Add Column End" );

				return this;

			}

			/// <summary>
			/// カラムの配列を返す
			/// </summary>
			/// <returns>カラムの配列を返す</returns>
			public RequestOfReplyMessage.Message.Template.Column[] GetColumns() => this.columns;

		}

	}
}