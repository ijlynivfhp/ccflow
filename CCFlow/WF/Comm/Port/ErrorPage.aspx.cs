using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Web.UC;
using BP.Web;


namespace CCFlow.Web.Comm
{
	/// <summary>
	/// ErrPage 的摘要说明。
	/// </summary>
	public partial class ErrPage: BP.Web.WebPage
	{

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.AddHeader("P3P", "CP=CAO PSA OUR");
            this.UCSys1.Add(this.Msg);
        }

        private string Msg
        {
            get
            {
                string msg = this.Session["info"] as string;
                if (msg == null)
                    msg = this.Application["info" + WebUser.No] as string;
                if (msg == null)
                {
                    msg = "@提示信息丢失。"; // "@没有找到信息，请在在途工作中找到它。";
                }
                return msg;
            }
        }
		/// <summary>
		/// DealPage
		/// </summary>
		private void DealPage()
		{
//			string mess ; 
//			switch (this.ErrorId)
//			{
//				case "NoUserNoSession":
//					mess="你的能录时间太长";
//				case "ddd":
//				default :
//			}
//			this.LabMess.Text=mess;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//this.IsAuthenticate=false ;
			//
			// CODEGEN：该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		private void Btn1_Click(object sender, System.EventArgs e)
		{
			this.WinClose();
			 
		}

		private void Btn2_Click(object sender, System.EventArgs e)
		{
			this.Session["info"]=this.Msg;
			this.Response.Redirect("../FAQ/Ask.aspx",true);
		}

		private void Btn3_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
