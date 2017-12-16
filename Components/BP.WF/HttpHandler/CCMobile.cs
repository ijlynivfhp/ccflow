﻿using System;
using System.Collections.Generic;
using System.Collections;
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
    public class CCMobile : DirectoryPageBase
    {
        /// <summary>
        /// 页面功能实体
        /// </summary>
        /// <param name="mycontext"></param>
        public CCMobile(HttpContext mycontext)
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
            switch (this.DoType)
            {

                case "DtlFieldUp": //字段上移
                    return "执行成功.";
                default:
                    break;
            }

            //找不不到标记就抛出异常.
            throw new Exception("@标记["+this.DoType+"]，没有找到.");
        }
        #endregion 执行父类的重写方法.

        public string Login_Init()
        {
            BP.WF.HttpHandler.WF_App_ACE ace = new WF_App_ACE(this.context);
            return ace.Login_Init();
        }

        public string Login_Submit()
        {
            BP.WF.HttpHandler.WF_App_ACE ace = new WF_App_ACE(this.context);
            return ace.Login_Submit();
        }
        /// <summary>
        /// 会签列表
        /// </summary>
        /// <returns></returns>
        public string HuiQianList_Init()
        {
            WF wf = new WF(this.context);
            return wf.HuiQianList_Init();
        }

        public string GetUserInfo()
        {
            BP.WF.HttpHandler.WF_App_ACE ace = new WF_App_ACE(this.context);
            return ace.GetUserInfo();
        }

        public string Home_Init()
        {
            Hashtable ht = new Hashtable();
            ht.Add("UserNo", BP.Web.WebUser.No);
            ht.Add("UserName", BP.Web.WebUser.Name);

            //系统名称.
            ht.Add("SysName", BP.Sys.SystemConfig.SysName);
            ht.Add("CustomerName", BP.Sys.SystemConfig.CustomerName);

            ht.Add("Todolist_EmpWorks", BP.WF.Dev2Interface.Todolist_EmpWorks);
            ht.Add("Todolist_Runing", BP.WF.Dev2Interface.Todolist_Runing);
            ht.Add("Todolist_Sharing", BP.WF.Dev2Interface.Todolist_Sharing);
            ht.Add("Todolist_CCWorks", BP.WF.Dev2Interface.Todolist_CCWorks);
            ht.Add("Todolist_Apply", BP.WF.Dev2Interface.Todolist_Apply); //申请下来的任务个数.
            ht.Add("Todolist_Draft", BP.WF.Dev2Interface.Todolist_Draft); //草稿数量.

            ht.Add("Todolist_HuiQian", BP.WF.Dev2Interface.Todolist_HuiQian); //会签数量.

            return BP.Tools.Json.ToJsonEntityModel(ht);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public string Home_Init_WorkCount()
        {
            string sql = "SELECT  TSpan as No, '' as Name, COUNT(WorkID) as Num, FROM WF_GenerWorkFlow WHERE Emps LIKE '%" + WebUser.No + "%' GROUP BY TSpan";
            DataSet ds = new DataSet();
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            ds.Tables.Add(dt);
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dt.Columns[0].ColumnName = "TSpan";
                dt.Columns[1].ColumnName = "Num";
            }

            sql = "SELECT IntKey as No, Lab as Name FROM Sys_Enum WHERE EnumKey='TSpan'";
            DataTable dt1 = BP.DA.DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow mydr in dt1.Rows)
                {
                    
                }
            }

            return BP.Tools.Json.ToJson(dt);
        }

        public string Runing_Init()
        {
            BP.WF.HttpHandler.WF wfPage = new WF(this.context);
          return  wfPage.Runing_Init();
        }
        /// <summary>
        /// 旧版本
        /// </summary>
        /// <returns></returns>
        public string Todolist_Init111()
        {
            BP.WF.HttpHandler.WF wfPage = new WF(this.context);
            return wfPage.Todolist_Init();
        }
        /// <summary>
        /// 新版本.
        /// </summary>
        /// <returns></returns>
        public string Todolist_Init()
        {
            string fk_node = this.GetRequestVal("FK_Node");
            DataTable dt = BP.WF.Dev2Interface.DB_Todolist(WebUser.No, this.FK_Node);
            return BP.Tools.Json.DataTableToJson(dt, false);
        }
        public string DB_GenerReturnWorks()
        {
            BP.WF.HttpHandler.WF_App_ACE ace = new WF_App_ACE(this.context);
            return ace.DB_GenerReturnWorks();
        }

        public string Start_Init()
        {
            BP.WF.HttpHandler.WF wfPage = new WF(this.context);
            return wfPage.Start_Init();
        }

        public string HandlerMapExt()
        {
            WF_CCForm en = new WF_CCForm(this.context);
            return en.HandlerMapExt();
        }

        /// <summary>
        /// 打开手机端
        /// </summary>
        /// <returns></returns>
        public string Do_OpenFlow()
        {
            string sid = this.GetRequestVal("SID");
            string[] strs = sid.Split('_');
            GenerWorkerList wl = new GenerWorkerList();
            int i = wl.Retrieve(GenerWorkerListAttr.FK_Emp, strs[0],
                GenerWorkerListAttr.WorkID, strs[1],
                GenerWorkerListAttr.IsPass, 0);

            if (i == 0)
            {
                return "err@提示:此工作已经被别人处理或者此流程已删除。";
            }

            BP.Port.Emp empOF = new BP.Port.Emp(wl.FK_Emp);
            Web.WebUser.SignInOfGener(empOF);
            return "MyFlow.htm?FK_Flow=" + wl.FK_Flow + "&WorkID=" + wl.WorkID + "&FK_Node=" + wl.FK_Node + "&FID=" + wl.FID;
        }
        /// <summary>
        /// 流程单表单查看.
        /// </summary>
        /// <returns>json</returns>
        public string FrmView_Init()
        {
            BP.WF.HttpHandler.WF wf = new WF(this.context);
            return wf.FrmView_Init();
        }
        /// <summary>
        /// 撤销发送
        /// </summary>
        /// <returns></returns>
        public string FrmView_UnSend()
        {
            BP.WF.HttpHandler.WF_WorkOpt_OneWork en = new WF_WorkOpt_OneWork(this.context);
            return en.OP_UnSend();
        }

        public string AttachmentUpload_Down()
        {
            WF_CCForm ccform = new WF_CCForm(this.context);
            return ccform.AttachmentUpload_Down();
        }


        #region 关键字查询.
        /// <summary>
        /// 打开表单
        /// </summary>
        /// <returns></returns>
        public string SearchKey_OpenFrm()
        {
            BP.WF.HttpHandler.WF_RptSearch search = new WF_RptSearch(this.context);
            return search.KeySearch_OpenFrm();
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <returns></returns>
        public string SearchKey_Query()
        {
            BP.WF.HttpHandler.WF_RptSearch search = new WF_RptSearch(this.context);
            return search.KeySearch_Query();
        }
        #endregion 关键字查询.

        #region 查询.
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public string Search_Init()
        {
            DataSet ds = new DataSet();
            string sql = "";

            string tSpan = this.GetRequestVal("TSpan");
            if (tSpan == "")
                tSpan = null;

            #region 1、获取时间段枚举/总数.
            SysEnums ses = new SysEnums("TSpan");
            DataTable dtTSpan = ses.ToDataTableField();
            dtTSpan.TableName = "TSpan";
            ds.Tables.Add(dtTSpan);

            if (this.FK_Flow == null)
            {
                sql = "SELECT  TSpan as No, COUNT(WorkID) as Num FROM WF_GenerWorkFlow WHERE Emps LIKE '%" + WebUser.No + "%' GROUP BY TSpan";
            }
            else
            {
                sql = "SELECT  TSpan as No, COUNT(WorkID) as Num FROM WF_GenerWorkFlow WHERE FK_Flow='" + this.FK_Flow + "' AND Emps LIKE '%" + WebUser.No + "%' GROUP BY TSpan";
            }

            DataTable dtTSpanNum = BP.DA.DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow drEnum in dtTSpan.Rows)
            {
                string no = drEnum["IntKey"].ToString();
                foreach (DataRow dr in dtTSpanNum.Rows)
                {
                    if (dr["No"].ToString() == no)
                    {
                        drEnum["Lab"] = drEnum["Lab"].ToString() + "(" + dr["Num"] + ")";
                        break;
                    }
                }
            }
            #endregion

            #region 2、处理流程类别列表.

            if (tSpan == null)
                sql = "SELECT  FK_Flow as No, FlowName as Name, COUNT(WorkID) as Num FROM WF_GenerWorkFlow WHERE  Emps LIKE '%" + WebUser.No + "%' GROUP BY FK_Flow, FlowName";
            else
                sql = "SELECT  FK_Flow as No, FlowName as Name, COUNT(WorkID) as Num FROM WF_GenerWorkFlow WHERE TSpan=" + tSpan + " AND Emps LIKE '%" + WebUser.No + "%' GROUP BY FK_Flow, FlowName";


            DataTable dtFlows = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dtFlows.Columns[0].ColumnName = "No";
                dtFlows.Columns[1].ColumnName = "Name";
                dtFlows.Columns[2].ColumnName = "Num";
            }
            dtFlows.TableName = "Flows";
            ds.Tables.Add(dtFlows);
            #endregion

            #region 3、处理流程实例列表.

            GenerWorkFlows gwfs = new GenerWorkFlows();
            BP.En.QueryObject qo = new QueryObject(gwfs);
            qo.AddWhere(GenerWorkFlowAttr.Emps, " LIKE ", "%" + BP.Web.WebUser.No + "%");

            if (tSpan != null)
            {
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.TSpan, tSpan);
            }

            if (this.FK_Flow != null)
            {
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.FK_Flow, this.FK_Flow);
            }
            qo.addOrderBy("WFSta");
            qo.addOrderByDesc("RDT");
            qo.Top = 50;


            DataTable mydt = null;
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                qo.DoQuery();
                mydt = gwfs.ToDataTableField("WF_GenerWorkFlow");
            }
            else
            {
                mydt = qo.DoQueryToTable();
                mydt.TableName = "WF_GenerWorkFlow";
            }
            #endregion

            ds.Tables.Add(mydt);

            return BP.Tools.Json.ToJson(ds);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public string Search_Search()
        {
            string TSpan = this.GetRequestVal("TSpan");
            string FK_Flow = this.GetRequestVal("FK_Flow");

            GenerWorkFlows gwfs = new GenerWorkFlows();
            QueryObject qo = new QueryObject(gwfs);
            qo.AddWhere(GenerWorkFlowAttr.Emps, " LIKE ", "%" + BP.Web.WebUser.No + "%");
            if (!string.IsNullOrEmpty(TSpan))
            {
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.TSpan, this.GetRequestVal("TSpan"));
            }
            if (!string.IsNullOrEmpty(FK_Flow))
            {
                qo.addAnd();
                qo.AddWhere(GenerWorkFlowAttr.FK_Flow, this.GetRequestVal("FK_Flow"));
            }
            qo.Top = 50;

            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                qo.DoQuery();
                DataTable dt = gwfs.ToDataTableField("Ens");
                return BP.Tools.Json.ToJson(dt);
            }
            else
            {
                DataTable dt = qo.DoQueryToTable();
                return BP.Tools.Json.ToJson(dt);
            }
        }

        #endregion

    }
}
