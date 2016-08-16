﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP.DA;
using BP.WF.Data;
using BP.WF.Template;
using BP.Sys;
using BP.En;
using BP.Port;
using BP.Web;

namespace BP.WF
{
    /// <summary>
    /// 工作流程业务规则
    /// </summary>
    public class WorkFlowBuessRole
    {
        #region 生成标题的方法.
        /// <summary>
        /// 生成标题
        /// </summary>
        /// <param name="wk">工作</param>
        /// <param name="emp">人员</param>
        /// <param name="rdt">日期</param>
        /// <returns>生成string.</returns>
        public static string GenerTitle(Flow fl, Work wk, Emp emp, string rdt)
        {
            string titleRole = fl.TitleRole.Clone() as string;
            if (string.IsNullOrEmpty(titleRole))
            {
                // 为了保持与ccflow4.5的兼容,从开始节点属性里获取.
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name在@RDT发起.";
            }


            titleRole = titleRole.Replace("@WebUser.No", emp.No);
            titleRole = titleRole.Replace("@WebUser.Name", emp.Name);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", emp.FK_DeptText);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", emp.FK_Dept);
            titleRole = titleRole.Replace("@RDT", rdt);
            if (titleRole.Contains("@") == true)
            {
                Attrs attrs = wk.EnMap.Attrs;

                // 优先考虑外键的替换。
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                //在考虑其它的字段替换.
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "”");

            if (titleRole.Contains("@"))
            {
                /*如果没有替换干净，就考虑是用户字段拼写错误*/
                throw new Exception("@请检查是否是字段拼写错误，标题中有变量没有被替换下来. @" + titleRole);
            }

            if (titleRole.Contains("@"))
                titleRole = GenerTitleExt(fl, wk.NodeID, wk.OID, titleRole);

            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        /// <summary>
        /// 生成标题
        /// </summary>
        /// <param name="wk"></param>
        /// <returns></returns>
        public static string GenerTitle(Flow fl, Work wk)
        {
            string titleRole = fl.TitleRole.Clone() as string;
            if (string.IsNullOrEmpty(titleRole))
            {
                // 为了保持与ccflow4.5的兼容,从开始节点属性里获取.
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name在@RDT发起.";
            }

            if (titleRole == "@OutPara" || string.IsNullOrEmpty(titleRole) == true)
                titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name在@RDT发起.";

            titleRole = titleRole.Replace("@WebUser.No", wk.Rec);
            titleRole = titleRole.Replace("@WebUser.Name", wk.RecText);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", wk.RecOfEmp.FK_DeptText);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", wk.RecOfEmp.FK_Dept);
            titleRole = titleRole.Replace("@RDT", wk.RDT);

            if (titleRole.Contains("@"))
            {
                Attrs attrs = wk.EnMap.Attrs;

                // 优先考虑外键的替换 , 因为外键文本的字段的长度相对较长。
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;

                    string temp = wk.GetValStrByKey(attr.Key);
                    if (string.IsNullOrEmpty(temp))
                    {
                       #warning 为什么，加这个代码？牺牲了很多效率，我注销了. by zhoupeng 2016.8.15
                      //  wk.DirectUpdate();
                       // wk.RetrieveFromDBSources();
                    }

                    titleRole = titleRole.Replace("@" + attr.Key, temp);
                }

                //在考虑其它的字段替换.
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "”");

            if (titleRole.Contains("@"))
                titleRole = GenerTitleExt(fl, wk.NodeID, wk.OID, titleRole);

            // 为当前的工作设置title.
            wk.SetValByKey("Title", titleRole);

            return titleRole;
        }
        /// <summary>
        /// 生成标题
        /// </summary>
        /// <param name="fl"></param>
        /// <param name="wk"></param>
        /// <returns></returns>
        public static string GenerTitle(Flow fl, GERpt wk)
        {
            string titleRole = fl.TitleRole.Clone() as string;
            if (string.IsNullOrEmpty(titleRole))
            {
                // 为了保持与ccflow4.5的兼容,从开始节点属性里获取.
                Attr myattr = wk.EnMap.Attrs.GetAttrByKey("Title");
                if (myattr == null)
                    myattr = wk.EnMap.Attrs.GetAttrByKey("Title");

                if (myattr != null)
                    titleRole = myattr.DefaultVal.ToString();

                if (string.IsNullOrEmpty(titleRole) || titleRole.Contains("@") == false)
                    titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name在@RDT发起.";
            }

            if (titleRole == "@OutPara" || string.IsNullOrEmpty(titleRole) == true)
                titleRole = "@WebUser.FK_DeptName-@WebUser.No,@WebUser.Name在@RDT发起.";


            titleRole = titleRole.Replace("@WebUser.No", wk.FlowStarter);
            titleRole = titleRole.Replace("@WebUser.Name", WebUser.Name);
            titleRole = titleRole.Replace("@WebUser.FK_DeptName", WebUser.FK_DeptName);
            titleRole = titleRole.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
            titleRole = titleRole.Replace("@RDT", wk.FlowStartRDT);
            if (titleRole.Contains("@"))
            {
                Attrs attrs = wk.EnMap.Attrs;

                // 优先考虑外键的替换,因为外键文本的字段的长度相对较长。
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }

                //在考虑其它的字段替换.
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                }
            }
            titleRole = titleRole.Replace('~', '-');
            titleRole = titleRole.Replace("'", "”");

            if (titleRole.Contains("@"))
                titleRole = GenerTitleExt(fl, int.Parse(fl.No + "01"), wk.OID, titleRole);

            // 为当前的工作设置title.
            wk.SetValByKey("Title", titleRole);
            return titleRole;
        }
        /// <summary>
        /// 如果从节点表单上没有替换下来，就考虑独立表单的替换.
        /// </summary>
        /// <param name="fl">流程</param>
        /// <param name="workid">工作ID</param>
        /// <returns>返回生成的标题</returns>
        private static string GenerTitleExt(Flow fl, int nodeId, Int64 workid, string titleRole)
        {
            FrmNodes nds = new FrmNodes(fl.No, nodeId);
            foreach (FrmNode item in nds)
            {
                GEEntity en = new GEEntity(item.FK_Frm);
                en.PKVal = workid;
                if (en.RetrieveFromDBSources() == 0)
                    continue;
                Attrs attrs = en.EnMap.Attrs;
                // 优先考虑外键的替换,因为外键文本的字段的长度相对较长。
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;
                    if (attr.IsRefAttr == false)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
                }

                //在考虑其它的字段替换.
                foreach (Attr attr in attrs)
                {
                    if (titleRole.Contains("@") == false)
                        break;

                    if (attr.IsRefAttr == true)
                        continue;
                    titleRole = titleRole.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
                }

                //如果全部已经替换完成.
                if (titleRole.Contains("@") == false)
                    return titleRole;
            }
            return titleRole;
        }
        #endregion 生成标题的方法.

        #region 产生单据编号
        /// <summary>
        /// 产生单据编号
        /// </summary>
        /// <param name="billFormat"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GenerBillNo(string billNo, Int64 workid, Entity en, string flowPTable)
        {
            if (string.IsNullOrEmpty(billNo))
                return "";
            if (billNo.Contains("@"))
                billNo = BP.WF.Glo.DealExp(billNo, en, null);

            /*如果，Bill 有规则 */
            billNo = billNo.Replace("{YYYY}", DateTime.Now.ToString("yyyy"));
            billNo = billNo.Replace("{yyyy}", DateTime.Now.ToString("yyyy"));

            billNo = billNo.Replace("{yy}", DateTime.Now.ToString("yy"));
            billNo = billNo.Replace("{YY}", DateTime.Now.ToString("YY"));

            billNo = billNo.Replace("{MM}", DateTime.Now.ToString("MM"));
            billNo = billNo.Replace("{mm}", DateTime.Now.ToString("mm"));

            billNo = billNo.Replace("{DD}", DateTime.Now.ToString("DD"));
            billNo = billNo.Replace("{dd}", DateTime.Now.ToString("dd"));
            billNo = billNo.Replace("{HH}", DateTime.Now.ToString("HH"));
            billNo = billNo.Replace("{hh}", DateTime.Now.ToString("HH"));

            billNo = billNo.Replace("{LSH}", workid.ToString());
            billNo = billNo.Replace("{WorkID}", workid.ToString());
            billNo = billNo.Replace("{OID}", workid.ToString());

            if (billNo.Contains("@WebUser.DeptZi"))
            {
                string val = DBAccess.RunSQLReturnStringIsNull("SELECT Zi FROM Port_Dept WHERE No='" + WebUser.FK_Dept + "'", "");
                billNo = billNo.Replace("@WebUser.DeptZi", val.ToString());
            }

            if (billNo.Contains("{ParentBillNo}"))
            {
                string pWorkID = DBAccess.RunSQLReturnStringIsNull("SELECT PWorkID FROM " + flowPTable + " WHERE   WFState >1 AND  OID=" + workid, "0");
                string parentBillNo = DBAccess.RunSQLReturnStringIsNull("SELECT BillNo FROM WF_GenerWorkFlow WHERE WorkID=" + pWorkID, "");
                billNo = billNo.Replace("{ParentBillNo}", parentBillNo);

                string sql = "";
                int num = 0;
                for (int i = 2; i < 7; i++)
                {
                    if (billNo.Contains("{LSH" + i + "}") == false)
                        continue;

                    sql = "SELECT COUNT(OID) FROM " + flowPTable + " WHERE PWorkID =" + pWorkID + " AND WFState >1 ";
                    num = BP.DA.DBAccess.RunSQLReturnValInt(sql, 0);
                    billNo = billNo + num.ToString().PadLeft(i, '0');
                    billNo = billNo.Replace("{LSH" + i + "}", "");
                    break;
                }
            }
            else
            {
                string sql = "";
                int num = 0;
                for (int i = 2; i < 7; i++)
                {
                    if (billNo.Contains("{LSH" + i + "}") == false)
                        continue;

                    billNo = billNo.Replace("{LSH" + i + "}", "");

                    sql = "SELECT COUNT(*) FROM " + flowPTable + " WHERE BillNo LIKE '" + billNo + "%' AND WFState >1 ";
                    if (DBAccess.AppCenterDBType == DBType.MSSQL)
                    {
                        //改为取最大值
                        sql = " SELECT isnull(convert(int,max(RIGHT(billno,len(billno)-len('" + billNo + "')-1))),0) FROM "
                            + flowPTable + " WHERE BillNo LIKE '" + billNo + "%' AND WFState >1 ";
                    }

                    num = BP.DA.DBAccess.RunSQLReturnValInt(sql, 0) + 1;
                    billNo = billNo + num.ToString().PadLeft(i, '0');
                }
            }
            return billNo;
        }
        #endregion 产生单据编号

        #region 获得下一个节点.
        /// <summary>
        /// 获得下一个节点
        /// </summary>
        /// <param name="currNode">当前的节点</param>
        /// <param name="workid">工作ID</param>
        /// <param name="currWorkFlow">当前的工作主表信息</param>
        /// <param name="enPara">参数</param>
        /// <returns>返回找到的节点</returns>
        public static Node RequestNextNode(Node currNode, Int64 workid, GenerWorkFlow currWorkFlow, GERpt enPara = null)
        {
            // 判断是否有用户选择的节点。
            if (currNode.CondModel == CondModel.ByUserSelected)
            {
                if (currWorkFlow == null)
                    throw new Exception("@参数错误:currWorkFlow");

                // 获取用户选择的节点.
                string nodes = currWorkFlow.Paras_ToNodes;
                if (string.IsNullOrEmpty(nodes))
                    throw new Exception("@用户没有选择发送到的节点.");

                string[] mynodes = nodes.Split(',');
                foreach (string item in mynodes)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    //排除到达自身节点
                    if (currNode.NodeID.ToString() == item)
                        continue;

                    return new Node(int.Parse(item));
                }

                //设置他为空,以防止下一次发送出现错误.
                currWorkFlow.Paras_ToNodes = "";
            }


            // 检查当前的状态是是否是退回，.
            Nodes nds = currNode.HisToNodes;
            if (nds.Count == 1)
            {
                Node toND = (Node)nds[0];
                return toND;
            }
            if (nds.Count == 0)
                throw new Exception("@没有找到它的下了步节点.");

            Conds dcsAll = new Conds();
            dcsAll.Retrieve(CondAttr.NodeID, currNode.NodeID, CondAttr.CondType, (int)CondType.Dir, CondAttr.PRI);
            if (dcsAll.Count == 0)
                throw new Exception("@没有为节点(" + currNode.NodeID + " , " + currNode.Name + ")设置方向条件");

            #region 获取能够通过的节点集合，如果没有设置方向条件就默认通过.
            Nodes myNodes = new Nodes();
            int toNodeId = 0;
            int numOfWay = 0;
            foreach (Node nd in nds)
            {
                Conds dcs = new Conds();
                foreach (Cond dc in dcsAll)
                {
                    if (dc.ToNodeID != nd.NodeID)
                        continue;

                    dc.WorkID = workid;
                    dc.FID = workid;

                    //如果当前的参数不为空.
                    if (enPara != null)
                        dc.en = enPara;

                    dcs.AddEntity(dc);
                }

                if (dcs.Count == 0)
                {
                    throw new Exception("@流程设计错误：从节点(" + currNode.Name + ")到节点(" + nd.Name + ")，没有设置方向条件，有分支的节点必须有方向条件。");
                    continue;
                }

                if (dcs.IsPass) // 如果通过了.
                    myNodes.AddEntity(nd);
            }
            #endregion 获取能够通过的节点集合，如果没有设置方向条件就默认通过.

            // 如果没有找到.
            if (myNodes.Count == 0)
                throw new Exception("@当前用户(" + BP.Web.WebUser.Name + "),定义节点的方向条件错误:从{" + currNode.NodeID + currNode.Name + "}节点到其它节点,定义的所有转向条件都不成立.");

            //如果找到1个.
            if (myNodes.Count == 1)
            {
                Node toND = myNodes[0] as Node;
                return toND;
            }

            //如果找到了多个.
            foreach (Cond dc in dcsAll)
            {
                foreach (Node myND in myNodes)
                {
                    if (dc.ToNodeID == myND.NodeID)
                    {
                        return myND;
                    }
                }
            }

            throw new Exception("@找到下一步节点.");
        }
        #endregion

        #region 找到下一个节点的接受人员
        /// <summary>
        /// 找到下一个节点的接受人员
        /// </summary>
        /// <param name="fl">流程</param>
        /// <param name="currNode">当前节点</param>
        /// <param name="toNode">到达节点</param>
        /// <returns>下一步工作人员No,Name格式的返回.</returns>
        public static DataTable RequetNextNodeWorkers(Flow fl, Node currNode, Node toNode, Entity enParas, Int64 workid)
        {
            if (toNode.IsGuestNode)
            {
                /*到达的节点是客户参与的节点. add by zhoupeng 2016.5.11*/
                DataTable mydt = new DataTable();
                mydt.Columns.Add("No", typeof(string));
                mydt.Columns.Add("Name", typeof(string));

                DataRow dr = mydt.NewRow();
                dr["No"] = "Guest";
                dr["Name"] = "外部用户";
                mydt.Rows.Add(dr);
                return mydt;
            }
             
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            string sql;
            string FK_Emp;

            //变量.
            string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;

            Paras ps = new Paras();
            // 按上一节点发送人处理。
            if (toNode.HisDeliveryWay == DeliveryWay.ByPreviousNodeEmp)
            {
                DataRow dr = dt.NewRow();
                dr[0] = BP.Web.WebUser.No;
                dt.Rows.Add(dr);
                return dt;
            }

            //首先判断是否配置了获取下一步接受人员的sql.
            if (toNode.HisDeliveryWay == DeliveryWay.BySQL
                || toNode.HisDeliveryWay == DeliveryWay.BySQLAsSubThreadEmpsAndData)
            {
                if (toNode.DeliveryParas.Length < 4)
                    throw new Exception("@您设置的当前节点按照SQL，决定下一步的接受人员，但是你没有设置SQL.");

                sql = toNode.DeliveryParas;
                sql = sql.Clone().ToString();

                //特殊的变量.
                sql = sql.Replace("@FK_Node", toNode.NodeID.ToString());
                sql = sql.Replace("@NodeID", toNode.NodeID.ToString());

                sql = Glo.DealExp(sql, enParas, null);
                if (sql.Contains("@"))
                {
                    if (Glo.SendHTOfTemp != null)
                    {
                        foreach (string key in Glo.SendHTOfTemp.Keys)
                        {
                            sql = sql.Replace("@" + key, Glo.SendHTOfTemp[key].ToString());
                        }
                    }
                }

                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0 && toNode.HisWhenNoWorker != WhenNoWorker.Skip)
                    throw new Exception("@没有找到可接受的工作人员。@技术信息：执行的SQL没有发现人员:" + sql);
                return dt;
            }

            #region 按绑定部门计算,该部门一人处理标识该工作结束(子线程)..
            if (toNode.HisDeliveryWay == DeliveryWay.BySetDeptAsSubthread)
            {
                if (toNode.HisRunModel != RunModel.SubThread)
                    throw new Exception("@您设置的节点接收人方式为：按绑定部门计算,该部门一人处理标识该工作结束(子线程)，但是当前节点非子线程节点。");

                sql = "SELECT No, Name,FK_Dept AS GroupMark FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + toNode.NodeID + ")";
                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0 && toNode.HisWhenNoWorker != WhenNoWorker.Skip)
                    throw new Exception("@没有找到可接受的工作人员,接受人方式为, ‘按绑定部门计算,该部门一人处理标识该工作结束(子线程)’ @技术信息：执行的SQL没有发现人员:" + sql);
                return dt;
            }
            #endregion 按绑定部门计算,该部门一人处理标识该工作结束(子线程)..


            #region 按照明细表,作为子线程的接收人.
            if (toNode.HisDeliveryWay == DeliveryWay.ByDtlAsSubThreadEmps)
            {
                if (toNode.HisRunModel != RunModel.SubThread)
                    throw new Exception("@您设置的节点接收人方式为：以分流点表单的明细表数据源确定子线程的接收人，但是当前节点非子线程节点。");

                BP.Sys.MapDtls dtls = new BP.Sys.MapDtls(currNode.NodeFrmID);
                string msg = null;
                foreach (BP.Sys.MapDtl dtl in dtls)
                {
                    try
                    {
                        string empFild = toNode.DeliveryParas;
                        if (string.IsNullOrEmpty(empFild))
                            empFild = " UserNo ";

                        ps = new Paras();
                        ps.SQL = "SELECT " + empFild + ", * FROM " + dtl.PTable + " WHERE RefPK=" + dbStr + "OID ORDER BY OID";
                        ps.Add("OID", workid);
                        dt = DBAccess.RunSQLReturnTable(ps);
                        if (dt.Rows.Count == 0 && toNode.HisWhenNoWorker != WhenNoWorker.Skip)
                            throw new Exception("@流程设计错误，到达的节点（" + toNode.Name + "）在指定的节点中没有数据，无法找到子线程的工作人员。");
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        msg += ex.Message;
                        //if (dtls.Count == 1)
                        //    throw new Exception("@估计是流程设计错误,没有在分流节点的明细表中设置");
                    }
                }
                throw new Exception("@没有找到分流节点的明细表作为子线程的发起的数据源，流程设计错误，请确认分流节点表单中的明细表是否有UserNo约定的系统字段。" + msg);
            }
            #endregion 按照明细表,作为子线程的接收人.

            #region 按节点绑定的人员处理.
            if (toNode.HisDeliveryWay == DeliveryWay.ByBindEmp)
            {
                ps = new Paras();
                ps.Add("FK_Node", toNode.NodeID);
                ps.SQL = "SELECT FK_Emp FROM WF_NodeEmp WHERE FK_Node=" + dbStr + "FK_Node ORDER BY FK_Emp";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                    throw new Exception("@流程设计错误:下一个节点(" + toNode.Name + ")没有绑定工作人员 . ");
                return dt;
            }
            #endregion 按节点绑定的人员处理.

            #region 按照选择的人员处理。
            if (toNode.HisDeliveryWay == DeliveryWay.BySelected
                || toNode.HisDeliveryWay == DeliveryWay.ByFEE)
            {
                ps = new Paras();
                ps.Add("FK_Node", toNode.NodeID);
                ps.Add("WorkID", workid);
                ps.SQL = "SELECT FK_Emp FROM WF_SelectAccper WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND AccType=0 ORDER BY IDX";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    /*从上次发送设置的地方查询. */
                    SelectAccpers sas = new SelectAccpers();
                    int i = sas.QueryAccepterPriSetting(toNode.NodeID);
                    if (i == 0)
                    {
                        if (toNode.HisDeliveryWay == DeliveryWay.BySelected)
                            throw new Exception("@请选择下一步骤工作(" + toNode.Name + ")接受人员。");
                        else
                            throw new Exception("@流程设计错误，请重写FEE，然后为节点(" + toNode.Name + ")设置接受人员，详细请参考cc流程设计手册。");
                    }

                    //插入里面.
                    foreach (SelectAccper item in sas)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item.FK_Emp;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
                return dt;
            }
            #endregion 按照选择的人员处理。

            #region 按照指定节点的处理人计算。
            if (toNode.HisDeliveryWay == DeliveryWay.BySpecNodeEmp
                || toNode.HisDeliveryWay == DeliveryWay.ByStarter)
            {
                /* 按指定节点岗位上的人员计算 */
                string strs = toNode.DeliveryParas;
                if (toNode.HisDeliveryWay == DeliveryWay.ByStarter)
                {
                    /*找开始节点的处理人员. */
                    strs = int.Parse(fl.No) + "01";
                    ps = new Paras();
                    ps.SQL = "SELECT FK_Emp FROM WF_GenerWorkerList WHERE WorkID=" + dbStr + "OID AND FK_Node=" + dbStr + "FK_Node AND IsPass=1 AND IsEnable=1 ";
                    ps.Add("FK_Node", int.Parse(strs));
                    ps.Add("OID", workid);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 1)
                        return dt;
                    else
                    {
                        /* 有可能当前节点就是第一个节点，那个时间还没有初始化数据，就返回当前人. */
                        DataRow dr = dt.NewRow();
                        dr[0] = BP.Web.WebUser.No;
                        dt.Rows.Add(dr);
                        return dt;
                    }
                }

                // 首先从本流程里去找。
                strs = strs.Replace(";", ",");
                string[] nds = strs.Split(',');
                foreach (string nd in nds)
                {
                    if (string.IsNullOrEmpty(nd))
                        continue;

                    if (DataType.IsNumStr(nd) == false)
                        throw new Exception("流程设计错误:您设置的节点(" + toNode.Name + ")的接收方式为按指定的节点岗位投递，但是您没有在访问规则设置中设置节点编号。");

                    ps = new Paras();
                    ps.SQL = "SELECT FK_Emp FROM WF_GenerWorkerList WHERE WorkID=" + dbStr + "OID AND FK_Node=" + dbStr + "FK_Node AND IsPass=1 AND IsEnable=1 ";
                    ps.Add("FK_Node", int.Parse(nd));
                    if (currNode.HisRunModel == RunModel.SubThread)
                        ps.Add("OID", workid);
                    else
                        ps.Add("OID", workid);

                    DataTable dt_ND = DBAccess.RunSQLReturnTable(ps);
                    //添加到结果表
                    if (dt_ND.Rows.Count != 0)
                    {
                        foreach (DataRow row in dt_ND.Rows)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = row[0].ToString();
                            dt.Rows.Add(dr);
                        }
                        //此节点已找到数据则不向下找，继续下个节点
                        continue;
                    }

                    //就要到轨迹表里查,因为有可能是跳过的节点.
                    ps = new Paras();
                    ps.SQL = "SELECT " + TrackAttr.EmpFrom + " FROM ND" + int.Parse(fl.No) + "Track WHERE (ActionType=" + dbStr + "ActionType1 OR ActionType=" + dbStr + "ActionType2 OR ActionType=" + dbStr + "ActionType3 OR ActionType=" + dbStr + "ActionType4 OR ActionType=" + dbStr + "ActionType5) AND NDFrom=" + dbStr + "NDFrom AND WorkID=" + dbStr + "WorkID";
                    ps.Add("ActionType1", (int)ActionType.Skip);
                    ps.Add("ActionType2", (int)ActionType.Forward);
                    ps.Add("ActionType3", (int)ActionType.ForwardFL);
                    ps.Add("ActionType4", (int)ActionType.ForwardHL);
                    ps.Add("ActionType5", (int)ActionType.Start);

                    ps.Add("NDFrom", int.Parse(nd));
                    ps.Add("WorkID", workid);
                    dt_ND = DBAccess.RunSQLReturnTable(ps);
                    if (dt_ND.Rows.Count != 0)
                    {
                        foreach (DataRow row in dt_ND.Rows)
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = row[0].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                }

                //本流程里没有有可能该节点是配置的父流程节点,也就是说子流程的一个节点与父流程指定的节点的工作人员一致.
                GenerWorkFlow gwf = new GenerWorkFlow(workid);
                if (gwf.PWorkID != 0)
                {
                    foreach (string pnodeiD in nds)
                    {
                        if (string.IsNullOrEmpty(pnodeiD))
                            continue;

                        Node nd = new Node(int.Parse(pnodeiD));
                        if (nd.FK_Flow != gwf.PFlowNo)
                            continue; // 如果不是父流程的节点，就不执行.

                        ps = new Paras();
                        ps.SQL = "SELECT FK_Emp FROM WF_GenerWorkerList WHERE WorkID=" + dbStr + "OID AND FK_Node=" + dbStr + "FK_Node AND IsPass=1 AND IsEnable=1 ";
                        ps.Add("FK_Node", nd.NodeID);
                        if (currNode.HisRunModel == RunModel.SubThread)
                            ps.Add("OID", gwf.PFID);
                        else
                            ps.Add("OID", gwf.PWorkID);

                        DataTable dt_PWork = DBAccess.RunSQLReturnTable(ps);
                        if (dt_PWork.Rows.Count != 0)
                        {
                            foreach (DataRow row in dt_PWork.Rows)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = row[0].ToString();
                                dt.Rows.Add(dr);
                            }
                            //此节点已找到数据则不向下找，继续下个节点
                            continue;
                        }

                        //就要到轨迹表里查,因为有可能是跳过的节点.
                        ps = new Paras();
                        ps.SQL = "SELECT " + TrackAttr.EmpFrom + " FROM ND" + int.Parse(fl.No) + "Track WHERE (ActionType=" + dbStr + "ActionType1 OR ActionType=" + dbStr + "ActionType2 OR ActionType=" + dbStr + "ActionType3 OR ActionType=" + dbStr + "ActionType4 OR ActionType=" + dbStr + "ActionType5) AND NDFrom=" + dbStr + "NDFrom AND WorkID=" + dbStr + "WorkID";
                        ps.Add("ActionType1", (int)ActionType.Start);
                        ps.Add("ActionType2", (int)ActionType.Forward);
                        ps.Add("ActionType3", (int)ActionType.ForwardFL);
                        ps.Add("ActionType4", (int)ActionType.ForwardHL);
                        ps.Add("ActionType5", (int)ActionType.Skip);

                        ps.Add("NDFrom", nd.NodeID);

                        if (currNode.HisRunModel == RunModel.SubThread)
                            ps.Add("OID", gwf.PFID);
                        else
                            ps.Add("OID", gwf.PWorkID);

                        dt_PWork = DBAccess.RunSQLReturnTable(ps);
                        if (dt_PWork.Rows.Count != 0)
                        {
                            foreach (DataRow row in dt_PWork.Rows)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = row[0].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                //返回指定节点的处理人
                if (dt.Rows.Count != 0)
                    return dt;

                throw new Exception("@流程设计错误，到达的节点（" + toNode.Name + "）在指定的节点(" + strs + ")中没有数据，无法找到工作的人员。 @技术信息如下: 投递方式:BySpecNodeEmp sql=" + ps.SQLNoPara);
            }
            #endregion 按照节点绑定的人员处理。

            #region 按照上一个节点表单指定字段的人员处理。
            if (toNode.HisDeliveryWay == DeliveryWay.ByPreviousNodeFormEmpsField)
            {
                // 检查接受人员规则,是否符合设计要求.
                string specEmpFields = toNode.DeliveryParas;
                if (string.IsNullOrEmpty(specEmpFields))
                    specEmpFields = "SysSendEmps";

                if (enParas.EnMap.Attrs.Contains(specEmpFields) == false)
                    throw new Exception("@您设置的当前节点按照指定的人员，决定下一步的接受人员，但是你没有在节点表单中设置该表单" + specEmpFields + "字段。");

                //获取接受人并格式化接受人, 
                string emps = enParas.GetValStringByKey(specEmpFields);
                emps = emps.Replace(" ", "");
                if (emps.Contains(",") && emps.Contains(";"))
                {
                    /*如果包含,; 例如 zhangsan,张三;lisi,李四;*/
                    string[] myemps1 = emps.Split(';');
                    foreach (string str in myemps1)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        string[] ss = str.Split(',');
                        DataRow dr = dt.NewRow();
                        dr[0] = ss[0];
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count == 0)
                        throw new Exception("@输入的接受人员信息错误;[" + emps + "]。");
                    else
                        return dt;
                }

                emps = emps.Replace(";", ",");
                emps = emps.Replace("；", ",");
                emps = emps.Replace("，", ",");
                emps = emps.Replace("、", ",");
                emps = emps.Replace("@", ",");

                if (string.IsNullOrEmpty(emps))
                    throw new Exception("@没有在字段[" + enParas.EnMap.Attrs.GetAttrByKey(specEmpFields).Desc + "]中指定接受人，工作无法向下发送。");

                // 把它加入接受人员列表中.
                string[] myemps = emps.Split(',');
                foreach (string s in myemps)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    //if (BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(NO) AS NUM FROM Port_Emp WHERE NO='" + s + "' or name='"+s+"'", 0) == 0)
                    //    continue;

                    DataRow dr = dt.NewRow();
                    dr[0] = s;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            #endregion 按照上一个节点表单指定字段的人员处理。

            string prjNo = "";
            FlowAppType flowAppType = currNode.HisFlow.HisFlowAppType;
            sql = "";
            if (currNode.HisFlow.HisFlowAppType == FlowAppType.PRJ)
            {
                prjNo = "";
                try
                {
                    prjNo = enParas.GetValStrByKey("PrjNo");
                }
                catch (Exception ex)
                {
                    throw new Exception("@当前流程是工程类流程，但是在节点表单中没有PrjNo字段(注意区分大小写)，请确认。@异常信息:" + ex.Message);
                }
            }

            #region 按部门与岗位的交集计算.
            if (toNode.HisDeliveryWay == DeliveryWay.ByDeptAndStation)
            {
                //added by liuxc,2015.6.29.
                //区别集成与BPM模式
                if (BP.WF.Glo.OSModel == BP.Sys.OSModel.OneOne)
                {
                    sql = " SELECT a.No,a.Name FROM Port_Emp A, WF_NodeDept B, WF_NodeStation C, Port_EmpStation D ";
                    sql += " WHERE A.FK_Dept=B.FK_Dept AND A.No=D.FK_Emp AND C.FK_Station=D.FK_Station AND B.FK_Node=C.FK_Node ";
                    sql += " AND B.FK_Node=" + dbStr + "FK_Node";

                    ps = new Paras();
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.SQL = sql;
                    dt = DBAccess.RunSQLReturnTable(ps);
                }
                else
                {
                    sql = "SELECT pdes.FK_Emp AS No"
                          + " FROM   Port_DeptEmpStation pdes"
                          + " INNER JOIN WF_NodeDept wnd ON wnd.FK_Dept = pdes.FK_Dept"
                          + " AND wnd.FK_Node = " + toNode.NodeID
                          + " INNER JOIN WF_NodeStation wns ON  wns.FK_Station = pdes.FK_Station"
                          + " AND wns.FK_Node =" + toNode.NodeID
                          + " ORDER BY pdes.FK_Emp";

                    dt = DBAccess.RunSQLReturnTable(sql);
                }

                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@节点访问规则(" + toNode.HisDeliveryWay.ToString() + ")错误:节点(" + toNode.NodeID + "," + toNode.Name + "), 按照岗位与部门的交集确定接受人的范围错误，没有找到人员:SQL=" + sql);
            }
            #endregion 按部门与岗位的交集计算.

            #region 判断节点部门里面是否设置了部门，如果设置了就按照它的部门处理。
            if (toNode.HisDeliveryWay == DeliveryWay.ByDept)
            {
                ps = new Paras();
                ps.Add("FK_Node", toNode.NodeID);
                ps.Add("WorkID", workid);
                ps.SQL = "SELECT FK_Emp FROM WF_SelectAccper WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND AccType=0 ORDER BY IDX";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count > 0)
                    return dt;

                if (flowAppType == FlowAppType.Normal)
                {
                    ps = new Paras();
                    ps.SQL = "SELECT  A.No, A.Name  FROM Port_Emp A, WF_NodeDept B WHERE A.FK_Dept=B.FK_Dept AND B.FK_Node=" + dbStr + "FK_Node";
                    ps.Add("FK_Node", toNode.NodeID);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count > 0 && toNode.HisWhenNoWorker != WhenNoWorker.Skip)
                        return dt;
                    else
                        throw new Exception("@按部门确定接受人的范围,没有找到人员.");
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    sql = " SELECT A.No,A.Name FROM Port_Emp A, WF_NodeDept B, Prj_EmpPrjStation C, WF_NodeStation D ";
                    sql += "  WHERE A.FK_Dept=B.FK_Dept AND A.No=C.FK_Emp AND C.FK_Station=D.FK_Station AND B.FK_Node=D.FK_Node ";
                    sql += "  AND C.FK_Prj=" + dbStr + "FK_Prj  AND D.FK_Node=" + dbStr + "FK_Node";

                    ps = new Paras();
                    ps.Add("FK_Prj", prjNo);
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.SQL = sql;

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /* 如果项目组里没有工作人员就提交到公共部门里去找。*/
                        sql = "SELECT NO FROM Port_Emp WHERE NO IN ";
                        sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                        sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                        sql += ")";
                        sql += "AND NO IN ";
                        sql += "(";
                        sql += "SELECT FK_Emp FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN ";
                        sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node2)";
                        sql += ")";
                        sql += " ORDER BY No";

                        ps = new Paras();
                        ps.Add("FK_Node1", toNode.NodeID);
                        ps.Add("FK_Node2", toNode.NodeID);
                        ps.SQL = sql;
                    }
                    else
                    {
                        return dt;
                    }

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count > 0)
                        return dt;
                }
            }
            #endregion 判断节点部门里面是否设置了部门，如果设置了，就按照它的部门处理。

            #region 仅按岗位计算
            if (toNode.HisDeliveryWay == DeliveryWay.ByStationOnly)
            {
                sql = "SELECT A.FK_Emp FROM " + BP.WF.Glo.EmpStation + " A, WF_NodeStation B WHERE A.FK_Station=B.FK_Station AND B.FK_Node=" + dbStr + "FK_Node ORDER BY A.FK_Emp";
                ps = new Paras();
                ps.Add("FK_Node", toNode.NodeID);
                ps.SQL = sql;
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@节点访问规则错误:节点(" + toNode.NodeID + "," + toNode.Name + "), 仅按岗位计算，没有找到人员:SQL=" + ps.SQLNoPara);
            }
            #endregion

            #region 按岗位计算(以部门集合为纬度).
            if (toNode.HisDeliveryWay == DeliveryWay.ByStationAndEmpDept)
            {
                /* 考虑当前操作人员的部门, 如果本部门没有这个岗位就不向上寻找. */
                //sql = "SELECT No FROM Port_Emp WHERE NO IN "
                //      + "(SELECT  FK_Emp  FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node) )"
                //      + " AND  FK_Dept IN "
                //      + "(SELECT  FK_Dept  FROM Port_EmpDept WHERE FK_Emp=" + dbStr + "FK_Emp)";
                //sql += " ORDER BY No ";

                ps = new Paras();

                sql = "SELECT No,Name FROM Port_Emp WHERE No=" + dbStr + "FK_Emp ";
                ps.Add("FK_Emp", WebUser.No);
                dt = DBAccess.RunSQLReturnTable(ps);

                //sql = "SELECT a.FK_Emp as No FROM Port_EmpDept A , Port_EmpStation B, WF_NodeStation C  ";
                //sql += " WHERE A.FK_Emp=B.FK_Emp AND B.FK_Station=C.FK_Station AND C.FK_Node="+dbStr+"FK_Node ";
                //sql += " AND A.FK_Dept IN ( SELECT FK_Dept from Port_EmpDept WHERE FK_Emp=" + dbStr + "FK_Emp ) ";
                //ps.SQL = sql;
                //ps.Add("FK_Node", toNode.NodeID);
                //ps.Add("FK_Emp", WebUser.No);
                //dt = DBAccess.RunSQLReturnTable(ps);

                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@节点访问规则(" + toNode.HisDeliveryWay.ToString() + ")错误:节点(" + toNode.NodeID + "," + toNode.Name + "), 按岗位计算(以部门集合为纬度)。技术信息,执行的SQL=" + ps.SQLNoPara);
            }
            #endregion

            string empNo = WebUser.No;
            string empDept = WebUser.FK_Dept;

            #region 按指定的节点的人员岗位，做为下一步骤的流程接受人。
            if (toNode.HisDeliveryWay == DeliveryWay.BySpecNodeEmpStation)
            {
                /* 按指定的节点的人员岗位 */
                string para = toNode.DeliveryParas;
                para = para.Replace("@", "");

                if (DataType.IsNumStr(para) == true)
                {
                    ps = new Paras();
                    ps.SQL = "SELECT FK_Emp,FK_Dept FROM WF_GenerWorkerList WHERE WorkID=" + dbStr + "OID AND FK_Node=" + dbStr + "FK_Node ";
                    ps.Add("OID", workid);
                    ps.Add("FK_Node", int.Parse(para));

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count != 1)
                        throw new Exception("@流程设计错误，到达的节点（" + toNode.Name + "）在指定的节点中没有数据，无法找到工作的人员。");

                    empNo = dt.Rows[0][0].ToString();
                    empDept = dt.Rows[0][1].ToString();
                }
                else
                {
                    if (enParas.Row.ContainsKey(para) == false)
                        throw new Exception("@在找人接收人的时候错误@字段{" + para + "}不包含在rpt里，流程设计错误。");

                    empNo = enParas.GetValStrByKey(para);
                    if (string.IsNullOrEmpty(empNo))
                        throw new Exception("@字段{" + para + "}不能为空，没有取出来处理人员。");

                    BP.Port.Emp em = new BP.Port.Emp(empNo);
                    empDept = em.FK_Dept;
                }
            }
            #endregion 按指定的节点人员，做为下一步骤的流程接受人。

            #region 最后判断 - 按照岗位来执行。
            if (currNode.IsStartNode == false)
            {
                ps = new Paras();
                if (flowAppType == FlowAppType.Normal || flowAppType == FlowAppType.DocFlow)
                {
                    // 如果当前的节点不是开始节点， 从轨迹里面查询。
                    sql = "SELECT DISTINCT FK_Emp  FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN "
                       + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + toNode.NodeID + ") "
                       + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(toNode.GroupStaNDs, true) + ") )";

                    sql += " ORDER BY FK_Emp ";

                    ps.SQL = sql;
                    ps.Add("WorkID", workid);
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    // 如果当前的节点不是开始节点， 从轨迹里面查询。
                    sql = "SELECT DISTINCT FK_Emp  FROM Prj_EmpPrjStation WHERE FK_Station IN "
                       + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) AND FK_Prj=" + dbStr + "FK_Prj "
                       + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(toNode.GroupStaNDs, true) + ") )";
                    sql += " ORDER BY FK_Emp ";

                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.Add("FK_Prj", prjNo);
                    ps.Add("WorkID", workid);

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /* 如果项目组里没有工作人员就提交到公共部门里去找。*/
                        sql = "SELECT DISTINCT FK_Emp  FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN "
                         + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) "
                         + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(toNode.GroupStaNDs, true) + ") )";
                        sql += " ORDER BY FK_Emp ";

                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", toNode.NodeID);
                        ps.Add("WorkID", workid);
                    }
                    else
                    {
                        return dt;
                    }
                }

                dt = DBAccess.RunSQLReturnTable(ps);
                // 如果能够找到.
                if (dt.Rows.Count >= 1)
                {
                    if (dt.Rows.Count == 1)
                    {
                        /*如果人员只有一个的情况，说明他可能要 */
                    }
                    return dt;
                }
            }

            /* 如果执行节点 与 接受节点岗位集合一致 */
            if (currNode.GroupStaNDs == toNode.GroupStaNDs)
            {
                /* 说明，就把当前人员做为下一个节点处理人。*/
                DataRow dr = dt.NewRow();
                dr[0] = WebUser.No;
                dt.Rows.Add(dr);
                return dt;
            }

            /* 如果执行节点 与 接受节点岗位集合不一致 */
            if (currNode.GroupStaNDs != toNode.GroupStaNDs)
            {
                /* 没有查询到的情况下, 先按照本部门计算。*/
                if (flowAppType == FlowAppType.Normal)
                {
                    if (BP.Sys.SystemConfig.OSDBSrc == Sys.OSDBSrc.Database)
                    {
                        if (BP.WF.Glo.OSModel == BP.Sys.OSModel.OneMore)
                            sql = "SELECT FK_Emp as No FROM Port_DeptEmpStation A, WF_NodeStation B         WHERE A.FK_Station=B.FK_Station AND B.FK_Node=" + dbStr + "FK_Node AND A.FK_Dept=" + dbStr + "FK_Dept";
                        else
                            sql = "SELECT FK_Emp as No FROM Port_EmpStation A, WF_NodeStation B, Port_Emp C WHERE A.FK_Station=B.FK_Station AND A.FK_Emp=C.No  AND B.FK_Node=" + dbStr + "FK_Node AND C.FK_Dept=" + dbStr + "FK_Dept";
                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", toNode.NodeID);
                        ps.Add("FK_Dept", empDept);
                    }

                    if (BP.Sys.SystemConfig.OSDBSrc == Sys.OSDBSrc.WebServices)
                    {
                        DataTable dtStas = BP.DA.DBAccess.RunSQLReturnTable("SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + toNode.NodeID);
                        string stas = DBAccess.GenerWhereInPKsString(dtStas);
                        var ws = DataType.GetPortalInterfaceSoapClientInstance();
                        return ws.GenerEmpsBySpecDeptAndStats(empDept, stas);
                    }
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    sql = "SELECT  FK_Emp  FROM Prj_EmpPrjStation WHERE FK_Prj=" + dbStr + "FK_Prj1 AND FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node)"
                    + " AND  FK_Prj=" + dbStr + "FK_Prj2 ";
                    sql += " ORDER BY FK_Emp ";

                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Prj1", prjNo);
                    ps.Add("FK_Node", toNode.NodeID);
                    ps.Add("FK_Prj2", prjNo);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /* 如果项目组里没有工作人员就提交到公共部门里去找。 */
                        sql = "SELECT No FROM Port_Emp WHERE NO IN "
                      + "(SELECT  FK_Emp  FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node))"
                      + " AND  NO IN "
                      + "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept =" + dbStr + "FK_Dept)";

                        sql += " ORDER BY No ";

                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", toNode.NodeID);
                        ps.Add("FK_Dept", empDept);
                        //  dt = DBAccess.RunSQLReturnTable(ps);
                    }
                    else
                    {
                        return dt;
                    }
                }

                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    NodeStations nextStations = toNode.NodeStations;
                    if (nextStations.Count == 0)
                        throw new Exception("@节点没有岗位:" + toNode.NodeID + "  " + toNode.Name);
                }
                else
                {
                    bool isInit = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString() == BP.Web.WebUser.No)
                        {
                            /* 如果岗位分组不一样，并且结果集合里还有当前的人员，就说明了出现了当前操作员，拥有本节点上的岗位也拥有下一个节点的工作岗位
                             导致：节点的分组不同，传递到同一个人身上。 */
                            isInit = true;
                        }
                    }

#warning edit by peng, 用来确定不同岗位集合的传递包含同一个人的处理方式。

                    //  if (isInit == false || isInit == true)
                    return dt;
                }
            }

            /*这里去掉了向下级别寻找的算法. */


            /* 没有查询到的情况下, 按照最大匹配数 提高一个级别计算，递归算法未完成。
             * 因为:以上已经做的岗位的判断，就没有必要在判断其它类型的节点处理了。
             * */
            string nowDeptID = empDept.Clone() as string;
            while (true)
            {
                BP.Port.Dept myDept = new BP.Port.Dept(nowDeptID);
                nowDeptID = myDept.ParentNo;
                if (nowDeptID == "-1" || nowDeptID.ToString() == "0")
                {
                    break; /*一直找到了最高级仍然没有发现，就跳出来循环从当前操作员人部门向下找。*/
                    throw new Exception("@按岗位计算没有找到(" + toNode.Name + ")接受人.");
                }

                //检查指定的部门下面是否有该人员.
                DataTable mydtTemp = RequetNextNodeWorkers_DiGui(nowDeptID, empNo,toNode);
                if (mydtTemp == null)
                {
                    /*如果父亲级没有，就找父级的平级. */
                    BP.Port.Depts myDepts = new BP.Port.Depts();
                    myDepts.Retrieve(BP.Port.DeptAttr.ParentNo, myDept.ParentNo);
                    foreach (BP.Port.Dept item in myDepts)
                    {
                        if (item.No == nowDeptID)
                            continue;
                        mydtTemp = RequetNextNodeWorkers_DiGui(item.No, empNo,toNode);
                        if (mydtTemp == null)
                            continue;
                        else
                            return mydtTemp;
                    }

                    continue; /*如果平级也没有，就continue.*/
                }
                else
                    return mydtTemp;
            }

            /*如果向上找没有找到，就考虑从本级部门上向下找。 */
            nowDeptID = empDept.Clone() as string;
            BP.Port.Depts subDepts = new BP.Port.Depts(nowDeptID);

            //递归出来子部门下有该岗位的人员.
            DataTable mydt123 = RequetNextNodeWorkers_DiGui_ByDepts(subDepts, empNo, toNode);
            if (mydt123 == null)
                throw new Exception("@按岗位计算没有找到(" + toNode.Name + ")接受人.");
            return mydt123;
            #endregion  按照岗位来执行。
        }
        /// <summary>
        /// 递归出来子部门下有该岗位的人员
        /// </summary>
        /// <param name="subDepts"></param>
        /// <param name="empNo"></param>
        /// <returns></returns>
        private static DataTable RequetNextNodeWorkers_DiGui_ByDepts(BP.Port.Depts subDepts, string empNo, Node toNode)
        {
            foreach (BP.Port.Dept item in subDepts)
            {
                DataTable dt = RequetNextNodeWorkers_DiGui(item.No, empNo, toNode);
                if (dt != null)
                    return dt;

                dt = RequetNextNodeWorkers_DiGui_ByDepts(item.HisSubDepts, empNo, toNode);
                if (dt != null)
                    return dt;
            }
            return null;
        }
        /// <summary>
        /// 根据部门获取下一步的操作员
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="emp1"></param>
        /// <returns></returns>
        private static DataTable RequetNextNodeWorkers_DiGui(string deptNo, string empNo,Node toNode)
        {
            string sql;
            string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;

            if (BP.WF.Glo.OSModel == BP.Sys.OSModel.OneMore)
                sql = "SELECT FK_Emp as No FROM Port_DeptEmpStation A, WF_NodeStation B WHERE A.FK_Station=B.FK_Station AND B.FK_Node=" + dbStr + "FK_Node AND A.FK_Dept=" + dbStr + "FK_Dept AND A.FK_Emp!=" + dbStr + "FK_Emp";
            else
                sql = "SELECT FK_Emp as No FROM Port_EmpStation A, WF_NodeStation B, Port_Emp C WHERE A.FK_Station=B.FK_Station AND A.FK_Emp=C.No AND B.FK_Node=" + dbStr + "FK_Node AND C.FK_Dept=" + dbStr + "FK_Dept AND A.FK_Emp!=" + dbStr + "FK_Emp";
            Paras ps = new Paras();
            ps.SQL = sql;
            ps.Add("FK_Node", toNode.NodeID);
            ps.Add("FK_Dept", deptNo);
            ps.Add("FK_Emp", empNo);

            DataTable dt = DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
            {
                NodeStations nextStations = toNode.NodeStations;
                if (nextStations.Count == 0)
                    throw new Exception("@节点没有岗位:" + toNode.NodeID + "  " + toNode.Name);

                sql = "SELECT No FROM Port_Emp WHERE No IN ";
                sql += "(SELECT  FK_Emp  FROM " + BP.WF.Glo.EmpStation + " WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )";
                sql += " AND No IN ";

                if (deptNo == "1")
                {
                    sql += "(SELECT No as FK_Emp FROM Port_Emp WHERE No!=" + dbStr + "FK_Emp ) ";
                }
                else
                {
                    BP.Port.Dept deptP = new BP.Port.Dept(deptNo);
                    sql += "(SELECT No as FK_Emp FROM Port_Emp WHERE No!=" + dbStr + "FK_Emp AND FK_Dept = '" + deptP.ParentNo + "')";
                }

                ps = new Paras();
                ps.SQL = sql;
                ps.Add("FK_Node", toNode.NodeID);
                ps.Add("FK_Emp", empNo);
                dt = DBAccess.RunSQLReturnTable(ps);

                if (dt.Rows.Count == 0)
                    return null;
                return dt;
            }
            else
            {
                return dt;
            }
            return null;
        }
        #endregion 找到下一个节点的接受人员

    }

}
