using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP;
using BP.Sys;
using BP.Web.Controls;
using System.Collections;

namespace CCFlow.WF.MapDef
{
    public partial class DDLFullCtrlUIUI : BP.Web.WebPage
    {
        #region 属性。
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string OperAttrKey
        {
            get
            {
                return this.Request.QueryString["OperAttrKey"];
            }
        }
       
        public string ExtType
        {
            get
            {
                return MapExtXmlList.DDLFullCtrl;
            }
        }

        public string Lab = null;
        #endregion 属性。
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack == false)
            {
                MapExt me = new MapExt();

                me.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData, MapExtAttr.ExtType, this.ExtType, MapExtAttr.AttrOfOper, this.RefNo);
                this.TB_SQL.Text = me.Doc;
                ArrayList arr = new ArrayList();
                SysEnums ens = new SysEnums("DBSrcType");

                foreach (SysEnum en in ens)
                {
                    arr.Add(en.Lab);

                }

                switch (me.FK_DBSrc)
                {
                    case "1":
                        this.DDL_DBSrc.SelectedValue = "SQLServer数据库";
                        break;
                    case "100":
                        this.DDL_DBSrc.SelectedValue = "WebService数据源";
                        break;
                    case "2":
                        this.DDL_DBSrc.SelectedValue = "Oracle数据库";
                        break;
                    case "3":
                        this.DDL_DBSrc.SelectedValue = "MySQL数据库";
                        break;
                    case "4":
                        this.DDL_DBSrc.SelectedValue = "Informix数据库";
                        break;
                    default:
                        this.DDL_DBSrc.SelectedValue = "应用系统主数据库(默认)";
                        break;

                }
                this.DDL_DBSrc.DataSource = arr;
                this.DDL_DBSrc.DataBind();


            }

        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            MapExt me = new MapExt();

            me.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData, MapExtAttr.ExtType, this.ExtType, MapExtAttr.AttrOfOper, this.RefNo);
            me.ExtType = this.ExtType;
            me.Doc = this.TB_SQL.Text;
            me.AttrOfOper = this.RefNo;
            me.FK_MapData = this.FK_MapData;
            me.MyPK = this.FK_MapData + "_" + me.ExtType + "_" + me.AttrOfOper;

            switch (this.DDL_DBSrc.Text)
            {
                case "应用系统主数据库(默认)":
                    me.FK_DBSrc = "0";
                    break;
                case "SQLServer数据库":
                    me.FK_DBSrc = "1";
                    break;
                case "WebService数据源":
                    me.FK_DBSrc = "100";
                    break;
                case "Oracle数据库":
                    me.FK_DBSrc = "2";
                    break;
                case "MySQL数据库":
                    me.FK_DBSrc = "3";
                    break;
                case "Informix数据库":
                    me.FK_DBSrc = "4";
                    break;
                default:
                    break;
            }

            me.Save();
        }

        protected void Btn_SaveAndClose_Click(object sender, EventArgs e)
        {
            Btn_Save_Click(null, null);
            BP.Sys.PubClass.WinClose();
        }

        protected void Btn_FullDtl_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_FullDDL_Click(object sender, EventArgs e)
        {

        }

        protected void Btn_Delete_Click(object sender, EventArgs e)
        {
            MapExt me = new MapExt();

            me.Retrieve(MapExtAttr.FK_MapData, this.FK_MapData, MapExtAttr.ExtType, this.ExtType, MapExtAttr.AttrOfOper, this.RefNo);
            me.Doc = "";
            me.Update();

            BP.Sys.PubClass.WinClose();
        }
    }
}