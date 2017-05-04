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
    /// 初始化函数
    /// </summary>
    public class WF_App_ACE : WebContralBase
    {
        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="mycontext"></param>
        public WF_App_ACE(HttpContext mycontext)
        {
            this.context = mycontext;
        }
        /// <summary>
        /// 获得发起流程
        /// </summary>
        /// <returns></returns>
        public string Start_Init()
        {
            DataTable dt = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(WebUser.No);
            return BP.Tools.Json.DataTableToJson(dt,true);
        }
        /// <summary>
        /// 获得待办
        /// </summary>
        /// <returns></returns>
        public string Todolist_Init()
        {
            string fk_node = this.GetRequestVal("FK_Node");
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, this.FK_Node);
            return BP.Tools.Json.DataTableToJson(dt,true);
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="UserNo">人员编号</param>
        /// <param name="fk_flow">流程编号</param>
        /// <returns>运行中的流程</returns>
        public string Runing_Init()
        {
            DataTable dt = null;
            dt = BP.WF.Dev2Interface.DB_GenerRuning();
            return BP.Tools.Json.DataTableToJson(dt,true);
        }

        /// <summary>
        /// 初始化赋值.
        /// </summary>
        /// <returns></returns>
        public string Home_Init()
        {
            Hashtable ht = new Hashtable();
            ht.Add("UserNo", BP.Web.WebUser.No);
            ht.Add("UserName", BP.Web.WebUser.Name);

            //系统名称.
            ht.Add("SysName", BP.Sys.SystemConfig.SysName);


            ht.Add("Todolist_EmpWorks", BP.WF.Dev2Interface.Todolist_EmpWorks);
            ht.Add("Todolist_Runing", BP.WF.Dev2Interface.Todolist_Runing);
            ht.Add("Todolist_Sharing", BP.WF.Dev2Interface.Todolist_Sharing);

            return BP.Tools.Json.ToJsonEntityModel(ht);
        }
        /// <summary>
        /// 转换成菜单.
        /// </summary>
        /// <returns></returns>
        public string Home_Menu()
        {
            DataSet ds = new DataSet();

            BP.WF.XML.ClassicMenus menus = new XML.ClassicMenus();
            menus.RetrieveAll();

           DataTable dtMain=  menus.ToDataTable();
           dtMain.TableName = "ClassicMenus";
           ds.Tables.Add(dtMain);

           BP.WF.XML.ClassicMenuAdvFuncs advMenms = new XML.ClassicMenuAdvFuncs();
           advMenms.RetrieveAll();

           DataTable dtMenuAdv = advMenms.ToDataTable();
           dtMenuAdv.TableName = "ClassicMenusAdv";
           ds.Tables.Add(dtMenuAdv);

           return BP.Tools.Json.ToJson(ds);
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

        #region 控制台.
        /// <summary>
        /// 控制台信息.
        /// </summary>
        /// <returns></returns>
        public string Index_Init()
        {
            Hashtable ht = new Hashtable();
            ht.Add("Todolist_Runing", BP.WF.Dev2Interface.Todolist_Runing); //运行中.
            ht.Add("Todolist_EmpWorks", BP.WF.Dev2Interface.Todolist_EmpWorks); //待办
            ht.Add("Todolist_CCWorks", BP.WF.Dev2Interface.Todolist_CCWorks); //抄送.

            //本周.
            ht.Add("TodayNum", BP.WF.Dev2Interface.Todolist_CCWorks); //抄送.

            return BP.Tools.Json.ToJsonEntityModel(ht);
        }
        #endregion 控制台.

        /// <summary>
        /// 设置
        /// </summary>
        /// <returns></returns>
        public string Setting_Init()
        {
            return "";
        }

        #region 登录界面.
        /// <summary>
        /// 登录.
        /// </summary>
        /// <returns></returns>
        public string Login_Submit()
        {
            string userNo = this.GetRequestVal("TB_UserNo");
            string pass = this.GetRequestVal("TB_Pass");

            BP.Port.Emp emp = new Emp();
            emp.No = userNo;
            if (emp.RetrieveFromDBSources() ==0)
                return "err@用户名或者密码错误.";

            if (emp.Pass !=pass )
                return "err@用户名或者密码错误.";

            //调用登录方法.
            BP.WF.Dev2Interface.Port_Login(emp.No, emp.Name, emp.FK_Dept, emp.FK_DeptText);

            return "登录成功.";

        }
        public string Login_Init()
        {
            Hashtable ht = new Hashtable();
            ht.Add("SysName", SystemConfig.SysName);
            ht.Add("ServiceTel", SystemConfig.ServiceTel);
            ht.Add("UserNo", WebUser.No);
            ht.Add("UserName", WebUser.Name);

            return BP.Tools.Json.ToJsonEntityModel(ht);
        }
        #endregion 登录界面.

        #region 草稿.
        /// <summary>
        /// 草稿.
        /// </summary>
        /// <returns></returns>
        public string Draft_Init()
        {
            System.Data.DataTable dt = BP.WF.Dev2Interface.DB_GenerDraftDataTable(this.FK_Flow);

            return BP.Tools.Json.ToJson(dt);
        }
        #endregion 草稿.

        #region 抄送流程.
        /// <summary>
        /// 抄送流程
        /// </summary>
        /// <returns></returns>
        public string cc_Init()
        {
            string sta = this.GetRequestVal("Sta");
            if (sta == null || sta == "")
                sta = "-1";

            int pageSize = 6;// int.Parse(pageSizeStr);

            string pageIdxStr = this.GetRequestVal("PageIdx");
            if (pageIdxStr == null)
                pageIdxStr = "1";
            int pageIdx = int.Parse(pageIdxStr);

            //实体查询.
            BP.WF.SMSs ss = new BP.WF.SMSs();
            BP.En.QueryObject qo = new BP.En.QueryObject(ss);

            System.Data.DataTable dt = null;
            if (sta == "-1")
                dt = BP.WF.Dev2Interface.DB_CCList(BP.Web.WebUser.No);
            if (sta == "0")
                dt = BP.WF.Dev2Interface.DB_CCList_UnRead(BP.Web.WebUser.No);
            if (sta == "1")
                dt = BP.WF.Dev2Interface.DB_CCList_Read(BP.Web.WebUser.No);
            if (sta == "2")
                dt = BP.WF.Dev2Interface.DB_CCList_Delete(BP.Web.WebUser.No);

            int allNum = qo.GetCount();
            qo.DoQuery(BP.WF.SMSAttr.MyPK, pageSize, pageIdx);

            //绑定分页
            // this.Pub1.BindPageIdx(allNum, pageSize, pageIdx, "CC.aspx?Sta=" + sta);

            /*BP.WF.XML.CCMenus tps = new BP.WF.XML.CCMenus();
            tps.RetrieveAll();
            string link = ""; // "<a href='?MsgType=All'>全部</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            foreach (BP.WF.XML.CCMenu tp in tps)
            {
                link += "<a href='?Sta=" + tp.No + "'>" + tp.Name + "</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            }*/

            return BP.Tools.Json.DataTableToJson(dt,true);
        }
        #endregion 抄送.

        #region 我的关注流程.
        /// <summary>
        /// 我的关注流程
        /// </summary>
        /// <returns></returns>
        public string Focus_Init()
        {
            string flowNo = this.GetRequestVal("FK_Flow");

            int idx = 0;
            //获得关注的数据.
            System.Data.DataTable dt = BP.WF.Dev2Interface.DB_Focus(flowNo, BP.Web.WebUser.No);
            SysEnums stas = new SysEnums("WFSta");
            string[] tempArr;
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                int wfsta = int.Parse(dr["WFSta"].ToString());
                //edit by liuxc,2016-10-22,修复状态显示不正确问题
                string wfstaT = (stas.GetEntityByKey(SysEnumAttr.IntKey, wfsta) as SysEnum).Lab;
                string currEmp = string.Empty;

                if (wfsta != (int)BP.WF.WFSta.Complete)
                {
                    //edit by liuxc,2016-10-24,未完成时，处理当前处理人，只显示处理人姓名
                    foreach (string emp in dr["ToDoEmps"].ToString().Split(';'))
                    {
                        tempArr = emp.Split(',');

                        currEmp += tempArr.Length > 1 ? tempArr[1] : tempArr[0] + ",";
                    }

                    currEmp = currEmp.TrimEnd(',');

                    //currEmp = dr["ToDoEmps"].ToString();
                    //currEmp = currEmp.TrimEnd(';');
                }
                dr["ToDoEmps"] = currEmp;
                dr["FlowNote"] = wfstaT;
                dr["AtPara"] = (wfsta == (int)BP.WF.WFSta.Complete ? dr["Sender"].ToString().TrimStart('(').TrimEnd(')').Split(',')[1] : "");
            }
            return BP.Tools.Json.DataTableToJson(dt,true);
        }
        #endregion 我的关注.

        #region 取消关注流程.
        /// <summary>
        /// 我的关注流程
        /// </summary>
        /// <returns></returns>
        public string Focus_Delete()
        {
            BP.WF.Dev2Interface.Flow_Focus(Int64.Parse(this.GetRequestVal("WorkID")));
            return "您已取消关注！";
        }
        #endregion 取消关注流程.
    }
   
}
