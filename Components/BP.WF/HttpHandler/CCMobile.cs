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

        public string Todolist_Init()
        {
            BP.WF.HttpHandler.WF wfPage = new WF(this.context);
            return wfPage.Todolist_Init();
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

    }
}
