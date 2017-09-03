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
    public class WF_RptSearch : DirectoryPageBase
    {
        /// <summary>
        /// 页面功能实体
        /// </summary>
        /// <param name="mycontext"></param>
        public WF_RptSearch(HttpContext mycontext)
        {
            this.context = mycontext;
        }

        #region 流程分布.
        public string DistributedOfMy_Init()
        {
            DataSet ds = new DataSet();

            //我发起的流程.
            string sql = "";
            sql = "select FK_Flow, FlowName,Count(WorkID) as Num FROM WF_GenerWorkFlow  WHERE Starter='" + BP.Web.WebUser.No + "' GROUP BY FK_Flow, FlowName ";
            System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Start";
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dt.Columns["FK_FLOW"].ColumnName = "FK_Flow";
                dt.Columns["FLOWNAME"].ColumnName = "FlowName";
                dt.Columns["NUM"].ColumnName = "Num";
            }
            ds.Tables.Add(dt);

            //待办.
            sql = "select FK_Flow, FlowName,Count(WorkID) as Num FROM wf_empworks  WHERE FK_Emp='" + BP.Web.WebUser.No + "' GROUP BY FK_Flow, FlowName ";
            System.Data.DataTable dtTodolist = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dtTodolist.TableName = "Todolist";
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dtTodolist.Columns["FK_FLOW"].ColumnName = "FK_Flow";
                dtTodolist.Columns["FLOWNAME"].ColumnName = "FlowName";
                dtTodolist.Columns["NUM"].ColumnName = "Num";
            }

            ds.Tables.Add(dtTodolist);

            //正在运行的流程.
            System.Data.DataTable dtRuning = BP.WF.Dev2Interface.DB_TongJi_Runing();
            dtRuning.TableName = "Runing";
            ds.Tables.Add(dtRuning);


            //归档的流程.
            System.Data.DataTable dtOK = BP.WF.Dev2Interface.DB_TongJi_FlowComplete();
            dtOK.TableName = "OK";
            ds.Tables.Add(dtOK);

            //返回结果.
            return BP.Tools.Json.DataSetToJson(ds, false);
        }
        #endregion

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
            throw new Exception("@标记[" + this.DoType + "]，没有找到. @RowURL:" + context.Request.RawUrl);
        }
        #endregion 执行父类的重写方法.

        public string Flowlist_Init()
        {
            DataSet ds = new DataSet();

            string sql = "SELECT No,Name,ParentNo FROM WF_FlowSort ORDER BY ParentNo, Idx";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Sort";
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dt.Columns["NO"].ColumnName = "No";
                dt.Columns["NAME"].ColumnName = "Name";
                dt.Columns["PARENTNO"].ColumnName = "ParentNo";
            }
            ds.Tables.Add(dt);


            sql = "SELECT No,Name,FK_FlowSort FROM WF_Flow ORDER BY FK_FlowSort, Idx";
            dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "Flows";
            if (SystemConfig.AppCenterDBType == DBType.Oracle)
            {
                dt.Columns["NO"].ColumnName = "No";
                dt.Columns["NAME"].ColumnName = "Name";
                dt.Columns["FK_FLOWSORT"].ColumnName = "FK_FlowSort";
            }
            ds.Tables.Add(dt);

            return BP.Tools.Json.DataSetToJson(ds, false);
        }

        #region xxx 界面 .
        #endregion xxx 界面方法.

    }
}