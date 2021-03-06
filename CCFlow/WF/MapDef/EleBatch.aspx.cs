using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class EleCopy : BP.Web.WebPage
    {
        #region 属性.
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string KeyOfEn
        {
            get
            {
                return this.Request.QueryString["KeyOfEn"];
            }
        }
        public string EleType
        {
            get
            {
                return this.Request.QueryString["EleType"];
            }
        }
        public string FK_Flow
        {
            get
            {
                string str = this.FK_MapData;
                str = str.Replace("ND", "");
                str = str.PadLeft(5, '0');
                str = str.Substring(0, 3);
                return str;
            }
        }
        #endregion 属性.

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.FK_MapData.Substring(0, 2) != "ND")
            {
                this.Pub1.AddFieldSetRed("错误","err:只有节点表单才可以执行 ");
                return;
            }

            #region 菜单
            this.Left.AddHR();
            this.Left.AddUL();
            if (this.DoType == "Copy")
                this.Left.AddLiB("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Copy", "批量复制");
            else
                this.Left.AddLi("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Copy", "批量复制");

            if (this.DoType == "Update")
                this.Left.AddLiB("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Update", "批量更新");
            else
                this.Left.AddLi("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Update", "批量更新");

            if (this.DoType == "Delete")
                this.Left.AddLiB("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Delete", "批量删除");
            else
                this.Left.AddLi("EleBatch.aspx?EleType=" + this.EleType + "&FK_MapData=" + this.FK_MapData + "&KeyOfEn=" + this.KeyOfEn + "&DoType=Delete", "批量删除");
            this.Left.AddULEnd();
            #endregion


            if (this.DoType == null)
            {
                this.Pub1.AddFieldSet("表单元素的批量处理",
                    "仅用节点表单，它包括如下几种处理方式<BR>1，批量更新元素属性。<BR>2，批量增加。<BR>3、批量删除。");
                return;
            }

            switch (this.EleType)
            {
                case "MapAttr":
                    this.MapAttr();
                    break;
                default:
                    break;
            }
        }
        public MapDatas GetMDs
        {
            get
            {
                string sql = "SELECT NodeID FROM WF_Node WHERE FK_Flow='" + this.FK_Flow + "'";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);

                string nds = "";
                foreach (DataRow dr in dt.Rows)
                    nds += ",'ND" + dr[0].ToString() + "'";

                sql = "SELECT No FROM Sys_MapData WHERE No IN (" + nds.Substring(1) + ")";
                dt = DBAccess.RunSQLReturnTable(sql);

                MapDatas mds = new MapDatas();
                mds.RetrieveInSQL(sql);

                return mds;
            }
        }
        public string Label
        {
            get
            {
                switch (this.DoType)
                {
                    case "Copy":
                        return "复制";
                    case "Update":
                        return "更新";
                    case "Delete":
                        return "删除";
                    default:
                        return "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void MapAttr()
        {
            MapDatas mds = this.GetMDs;

            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeftTX("批量:"+this.Label);

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("表单ID");
            this.Pub1.AddTDTitle("名称");
            this.Pub1.AddTDTitle("操作");
            this.Pub1.AddTREnd();

            foreach (MapData md in mds)
            {
                switch (this.DoType)
                {
                    case "Copy":
                        if (md.MapAttrs.Contains(MapAttrAttr.KeyOfEn, this.KeyOfEn) == true)
                            continue;
                        break;
                    case "Update":
                        if (md.MapAttrs.Contains(MapAttrAttr.KeyOfEn, this.KeyOfEn) == false)
                            continue;
                        if (md.No == this.FK_MapData)
                            continue;

                        break;
                    case "Delete":
                        if (md.MapAttrs.Contains(MapAttrAttr.KeyOfEn, this.KeyOfEn) == false)
                            continue;
                        break;
                    default:
                        break;
                }
                

                this.Pub1.AddTR();
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + md.No;
                cb.Text = md.Name;
                this.Pub1.AddTD(cb);
                this.Pub1.AddTD(md.Name);
                this.Pub1.AddTD("<a href=''>预览自由表单</a> - <a href=''>设计自由表单</a>");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();

            this.Pub1.AddHR();
            Button btn = new Button();
            btn.ID = "Btn";
            btn.Text = "执行批量["+this.Label+"]操作";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            MapDatas mds = this.GetMDs;
            MapAttr mattrOld = new MapAttr(this.FK_MapData, this.KeyOfEn);
            MapAttr mattr = new MapAttr(this.FK_MapData, this.KeyOfEn);
            foreach (MapData md in mds)
            {
                CheckBox cb = this.Pub1.GetCBByID("CB_" + md.No);
                if (cb==null)
                    continue;

                if (cb.Checked == false)
                    continue;

                if (this.DoType == "Copy")
                {
                    /*执行批量Copy*/
                    mattr.FK_MapData = md.No;
                    mattr.Insert();
                    mattr.Idx = mattrOld.Idx;
                }

                if (this.DoType == "Update")
                {
                    /*执行批量Update*/
                    MapAttr mattrUpdate = new MapAttr(md.No, this.KeyOfEn);
                    int gID = mattrUpdate.GroupID;
                    mattrUpdate.Copy(mattrOld);
                    mattrUpdate.FK_MapData = md.No;
                    mattrUpdate.GroupID = gID;
                    mattrUpdate.Update();
                }

                if (this.DoType == "Delete")
                {
                    /*执行批量 Delete */
                    MapAttr mattrDelete = new MapAttr(md.No, this.KeyOfEn);
                    mattrDelete.Delete();
                }

            }
            // 转向.
            this.Response.Redirect(this.Request.RawUrl, true);
        }
    }
}