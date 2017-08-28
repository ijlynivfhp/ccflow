﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using BP.DA;
using BP.Sys;
using BP.Web;
using BP.Port;
using BP.En;
using BP.WF;
using BP.WF.Template;

namespace BP.WF.HttpHandler
{
    /// <summary>
    /// 页面功能实体
    /// </summary>
    public class WF_Admin_FoolFormDesigner_Rpt : DirectoryPageBase
    {
        /// <summary>
        /// 页面功能实体
        /// </summary>
        /// <param name="mycontext"></param>
        public WF_Admin_FoolFormDesigner_Rpt(HttpContext mycontext)
        {
            this.context = mycontext;
        }

        #region 执行父类的重写方法.
        /// <summary>
        /// 默认执行的方法
        /// </summary>
        /// <returns></returns>
        protected override string DoDefaultMethod()
        {
            string msg = "";
            try
            {
                switch (this.DoType)
                {
                    case "S0_RptList_Init": //初始化.
                        msg = this.S0_RptList_Init();
                        break;
                    case "S0_RptList_New": //创建一个报表.
                        msg = this.S0_RptList_New();
                        break;
                    case "S0_RptList_Delete": //删除报表.
                        msg = this.S0_RptList_Delete();
                        break;
                    case "S0_RptList_Edit": //删除报表.
                        msg = this.S0_RptList_Edit();
                        break;
                    case "S0_OneRpt_Init": //获得单个流程报表的初始化信息
                        msg = this.S0_OneRpt_Init();
                        break;
                    default:
                        msg = "err@没有判断的执行类型：" + this.DoType;
                        break;
                }
                context.Response.Write(msg);
            }
            catch (Exception ex)
            {
                context.Response.Write("err@" + ex.Message);
            }

            //找不不到标记就抛出异常.
            throw new Exception("@标记[" + this.DoType + "]，没有找到. @RowURL:" + context.Request.RawUrl);
        }
        #endregion 执行父类的重写方法.

        #region 报表设计器. - 第2步选择列.
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <returns></returns>
        public string S2ColsChose_Init()
        {
            DataSet ds = new DataSet();
            string rptNo = this.GetRequestVal("RptNo");

            //所有的字段.
            string fk_mapdata = "ND"+int.Parse(this.FK_Flow)+"Rpt";
            MapAttrs mattrs = new MapAttrs(fk_mapdata);
            ds.Tables.Add(mattrs.ToDataTableField("Sys_MapAttrOfAll"));

            //选择的字段,就是报表的字段.
            MapAttrs mattrsOfRpt = new MapAttrs(rptNo);
            ds.Tables.Add(mattrsOfRpt.ToDataTableField("Sys_MapAttrOfSelected"));

            //系统字段.
            MapAttrs mattrsOfSystem = new MapAttrs();
            var sysFields = BP.WF.Glo.FlowFields;
            foreach (MapAttr item in mattrs)
            {
                if (sysFields.Contains(item.KeyOfEn))
                    mattrsOfSystem.AddEntity(item);
            }
            ds.Tables.Add(mattrsOfSystem.ToDataTableField("Sys_MapAttrOfSystem"));

            //返回.
            return BP.Tools.Json.DataSetToJson(ds, false) ;
        }
        /// <summary>
        /// 选择列的保存.
        /// </summary>
        /// <returns></returns>
        public string S2ColsChose_Save()
        {
            //报表列表.
            string rptNo = this.GetRequestVal("RptNo");

            //保存的字段,从外面传递过来的值. 用逗号隔开的: 比如:  ,Name,Tel,Addr,
            string fields = ","+this.GetRequestVal("Fields")+",";

            //构造一个空的集合.
            MapAttrs mrattrsOfRpt = new MapAttrs();
            mrattrsOfRpt.Delete(MapAttrAttr.FK_MapData, rptNo);

            //所有的字段.
            string fk_mapdata = "ND" + int.Parse(this.FK_Flow) + "Rpt";
            MapAttrs allAttrs = new MapAttrs(fk_mapdata);

            foreach (MapAttr attr in allAttrs)
            {
                #region 处理特殊字段.
                if (attr.KeyOfEn == "FK_NY")
                {
                    attr.LGType = BP.En.FieldTypeS.FK;
                    attr.UIBindKey = "BP.Pub.NYs";
                    attr.UIContralType = BP.En.UIContralType.DDL;
                }

                if (attr.KeyOfEn == "FK_Dept")
                {
                    attr.LGType = BP.En.FieldTypeS.FK;
                    attr.UIBindKey = "BP.Port.Depts";
                    attr.UIContralType = BP.En.UIContralType.DDL;
                }
                #endregion 处理特殊字段.

                //增加上必要的字段.
                if (attr.KeyOfEn == "Title" ||
                    attr.KeyOfEn == "WorkID" ||
                    attr.KeyOfEn == "OID" ||
                    attr.KeyOfEn == "WFSta")
                {
                    attr.FK_MapData = rptNo;
                    attr.DirectInsert();
                    continue;
                }

                //如果包含了指定的字段，就执行插入操作.
                if (fields.Contains("," + attr.KeyOfEn + ",") == true)
                {
                    attr.FK_MapData = rptNo;
                    attr.MyPK = attr.FK_MapData + "_" + attr.KeyOfEn;
                    attr.DirectInsert();
                }
            }

            return "保存成功.";
        }
        #endregion

        #region 报表设计器. - 第3步设置列的顺序.
        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <returns></returns>
        public string S3ColsLabel_Init()
        {
            string rptNo = this.GetRequestVal("RptNo");

            //选择的字段,就是报表的字段.
            MapAttrs mattrsOfRpt = new MapAttrs(rptNo);
            return mattrsOfRpt.ToJson();
        }
        /// <summary>
        /// 保存列的顺序名称.
        /// </summary>
        /// <returns></returns>
        public string S3ColsLabel_Save()
        {
            string orders = this.GetRequestVal("Orders");
            //格式为  @KeyOfEn,Lable,idx  比如： @DianHua,电话,1@Addr,地址,2
            
            string rptNo=this.GetRequestVal("RptNo");

            string[] strs = orders.Split('@');
            foreach (string item in strs)
            {
                if (string.IsNullOrEmpty(item)==true)
                    continue;

                string[] vals=item.Split(',');

                string mypk = rptNo + "_" + vals[0];

                MapAttr attr = new MapAttr();
                attr.MyPK = mypk;
                attr.Retrieve();

                attr.Name = vals[1];
                attr.Idx = int.Parse(vals[2]);

                attr.Update(); //执行更新.
            }

            return "保存成功..";
        }
        #endregion

        #region 报表设计器 - 第4步骤.
        public string S5SearchCond_Init()
        {
            return "";
        }
        #endregion

        /// <summary>
        /// 获得流程报表列表
        /// </summary>
        /// <returns></returns>
        public string S0_RptList_Init()
        {
            BP.WF.Rpt.MapRpts ens = new BP.WF.Rpt.MapRpts();
            ens.Retrieve(BP.WF.Rpt.MapRptAttr.FK_Flow, this.FK_Flow);
            if (ens.Count == 0)
            {
                BP.WF.Rpt.MapRpt en = new BP.WF.Rpt.MapRpt();
                en.No = "ND" + int.Parse(this.FK_Flow) + "MyRpt";
                en.Name = "流程报表默认";
                en.FK_Flow = this.FK_Flow;
                en.Insert();
                ens.Retrieve(BP.WF.Rpt.MapRptAttr.FK_Flow, this.FK_Flow);
            }

            return ens.ToJson();
        }

        /// <summary>
        /// 获得单个流程报表的初始化信息
        /// </summary>
        /// <returns></returns>
        public string S0_OneRpt_Init()
        {
            string no = this.GetRequestVal("No");
            BP.WF.Rpt.MapRpt ens = new BP.WF.Rpt.MapRpt();
            ens.Retrieve(BP.WF.Rpt.MapRptAttr.No, no);
            if (ens == null)
            {
                BP.WF.Rpt.MapRpt en = new BP.WF.Rpt.MapRpt();
                en.No = "ND" + int.Parse(this.FK_Flow) + "MyRpt";
                en.Name = "流程报表默认";
                en.FK_Flow = this.FK_Flow;
                en.Insert();
                ens.Retrieve(BP.WF.Rpt.MapRptAttr.No, no);
            }
            return ens.ToJson();
        }

        /// <summary>
        /// 创建新报表
        /// </summary>
        /// <returns></returns>
        public string S0_RptList_New()
        {
            string no = this.GetRequestVal("No");
            string name = this.GetRequestVal("Name");
            string note = this.GetRequestVal("Note");
            //BP.WF.Rpt.MapRpt en = new BP.WF.Rpt.MapRpt();
            //en.No = no;
            //if (en.RetrieveFromDBSources() == 1)
            //{
            //    return "err@编号{" + en.No + "}已经存在.";
            //}

            //en.Name = name;
            //en.Note = note;
            //en.Insert();

            string mapRpt = no;
            MapData mapData = new MapData();
            mapData.No = mapRpt;
            mapData.Name = name;
            mapData.Note = mapData.Note;
            mapData.Insert();


            BP.WF.Rpt.MapRpt rpt = new BP.WF.Rpt.MapRpt();
            rpt.No = mapRpt;
            rpt.FK_Flow = FK_Flow;
            rpt.Note = note;
            rpt.Name = name;
            Flow flow = new Flow(this.FK_Flow);
            rpt.PTable = flow.PTable == "" ? "ND" + this.FK_Flow.TrimStart('0') + "RPT" : flow.PTable;
            rpt.Update();

            string sql = "";

            sql = "insert into Sys_MapAttr  select '" + no + "_'+KeyOfEn,'" + no + "',KeyOfEn,Name,DefVal,UIContralType,MyDataType,LGType,UIWidth,UIHeight,MinLen,MaxLen,UIBindKey,UIRefKey,UIRefKeyText,UIVisible,UIIsEnable,UIIsLine,UIIsInput,Idx,GroupID,IsSigan,x,y,GUID,Tag,EditType,ColSpan,AtPara from Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' + cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + flow.No + "')";
            //DBAccess.RunSQL(sql); // 写入 Sys_MapAttr
            //
            sql = "select * from Sys_MapAttr WHERE FK_MapData IN ( SELECT 'ND' + cast(NodeID as varchar(20)) FROM WF_Node WHERE FK_Flow='" + flow.No + "')";

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            sql = "SELECT KeyOfEn FROM Sys_MapAttr WHERE FK_MapData='" + no + "'";
            DataTable dtExits = DBAccess.RunSQLReturnTable(sql);
            string pks = "@";
            foreach (DataRow dr in dtExits.Rows)
                pks += dr[0] + "@";

            foreach (DataRow dr in dt.Rows)
            {
                string mypk = dr["MyPK"].ToString();
                if (pks.Contains("@" + dr["KeyOfEn"].ToString() + "@"))
                    continue;

                pks += dr["KeyOfEn"].ToString() + "@";

                BP.Sys.MapAttr ma = new BP.Sys.MapAttr(mypk);
                ma.MyPK = no + "_" + ma.KeyOfEn;
                ma.FK_MapData = no;
                ma.UIIsEnable = false;

                if (ma.DefValReal.Contains("@"))
                {
                    /*如果是一个有变量的参数.*/
                    ma.DefVal = "";
                }

                try
                {
                    ma.Insert();
                }
                catch
                {
                }
            }

            //DoCheck_CheckRpt(no, this.FK_Flow, name, rpt.PTable);
            return "@创建成功.";
        }

        /// <summary>
        /// 删除报表
        /// </summary>
        /// <returns></returns>
        public string S0_RptList_Delete()
        {
            string no = this.GetRequestVal("No");
            if (no == "ND" + int.Parse(this.FK_Flow) + "MyRpt")
                return "err@默认报表，不能删除。";

            BP.WF.Rpt.MapRpt en = new BP.WF.Rpt.MapRpt();
            en.No = no;
            en.Delete();
            return "@删除成功.";
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public string S0_RptList_Edit()
        {
            string no = this.GetRequestVal("No");

            BP.WF.Rpt.MapRpt en = new BP.WF.Rpt.MapRpt();
            en.No = no;
            en.Retrieve();

            en.Name = this.GetValFromFrmByKey("Name");
            en.Note = this.GetValFromFrmByKey("Note");
            en.Update();

            return "@保存成功.";
        }

    }
}