using System;
using System.Collections;
using System.Data;
using BP.En;
using BP.Web;
using BP.DA;
using BP.Port;
using BP.Sys;
using BP.WF.XML;
using BP.WF.Template;

namespace BP.WF
{
    /// <summary>
    /// 撤销发送
    /// </summary>
    public class WorkUnSend
    {
        #region 属性.
        private string _AppType = null;
        /// <summary>
        /// 虚拟目录的路径
        /// </summary>
        public string AppType
        {
            get
            {
                if (_AppType == null)
                {
                    if (BP.Sys.SystemConfig.IsBSsystem == false)
                    {
                        _AppType = "WF";
                    }
                    else
                    {
                        if (BP.Web.WebUser.IsWap)
                            _AppType = "WF/WAP";
                        else
                        {
                            bool b = BP.Sys.Glo.Request.RawUrl.ToLower().Contains("oneflow");
                            if (b)
                                _AppType = "WF/OneFlow";
                            else
                                _AppType = "WF";
                        }
                    }
                }
                return _AppType;
            }
        }
        private string _VirPath = null;
        /// <summary>
        /// 虚拟目录的路径
        /// </summary>
        public string VirPath
        {
            get
            {
                if (_VirPath == null)
                {
                    if (BP.Sys.SystemConfig.IsBSsystem)
                        _VirPath = Glo.CCFlowAppPath;//BP.Sys.Glo.Request.ApplicationPath;
                    else
                        _VirPath = "";
                }
                return _VirPath;
            }
        }
        public string FlowNo = null;
        private Flow _HisFlow = null;
        public Flow HisFlow
        {
            get
            {
                if (_HisFlow == null)
                    this._HisFlow = new Flow(this.FlowNo);
                return this._HisFlow;
            }
        }
        /// <summary>
        /// 工作ID
        /// </summary>
        public Int64 WorkID = 0;
        /// <summary>
        /// FID
        /// </summary>
        public Int64 FID = 0;
        /// <summary>
        /// 是否是干流
        /// </summary>
        public bool IsMainFlow
        {
            get
            {
                if (this.FID != 0 && this.FID != this.WorkID)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        /// <summary>
        /// 撤销发送
        /// </summary>
        public WorkUnSend(string flowNo, Int64 workID)
        {
            this.FlowNo = flowNo;
            this.WorkID = workID;
        }
        /// <summary>
        /// 得到当前的进行中的工作。
        /// </summary>
        /// <returns></returns>		 
        public WorkNode GetCurrentWorkNode()
        {
            int currNodeID = 0;
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            gwf.WorkID = this.WorkID;
            if (gwf.RetrieveFromDBSources() == 0)
            {
                // this.DoFlowOver(ActionType.FlowOver, "非正常结束，没有找到当前的流程记录。");
                throw new Exception("@" + string.Format("工作流程{0}已经完成。", this.WorkID));
            }

            Node nd = new Node(gwf.FK_Node);
            Work work = nd.HisWork;
            work.OID = this.WorkID;
            work.NodeID = nd.NodeID;
            work.SetValByKey("FK_Dept", BP.Web.WebUser.FK_Dept);
            if (work.RetrieveFromDBSources() == 0)
            {
                Log.DefaultLogWriteLineError("@WorkID=" + this.WorkID + ",FK_Node=" + gwf.FK_Node + ".不应该出现查询不出来工作."); // 没有找到当前的工作节点的数据，流程出现未知的异常。
                work.Rec = BP.Web.WebUser.No;
                try
                {
                    work.Insert();
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError("@没有找到当前的工作节点的数据，流程出现未知的异常" + ex.Message + ",不应该出现"); // 没有找到当前的工作节点的数据
                }
            }
            work.FID = gwf.FID;
            WorkNode wn = new WorkNode(work, nd);
            return wn;
        }
        /// <summary>
        /// 执行撤消
        /// </summary>
        public string DoUnSend()
        {
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            if (gwf.FID != 0)
            {
                /*
                 * 说明该流程是一个子线程，如果子线程的撤销，就需要删除该子线程。
                 */
                BP.WF.Dev2Interface.Flow_DeleteSubThread(this.FlowNo, this.WorkID,"撤销方式删除");
                return "@子线程已经被删除.";
            }


            // 如果停留的节点是分合流。
            Node nd = new Node(gwf.FK_Node);
            if (nd.HisCancelRole == CancelRole.None)
            {
                /*该节点不允许退回.*/
                throw new Exception("当前节点，不允许撤销。");
            }

            switch (nd.HisNodeWorkType)
            {
                case NodeWorkType.WorkFHL:
                    throw new Exception("分合流点不允许撤消。");
                case NodeWorkType.WorkFL:
                    /*判断权限,当前的发送人是谁，如果是自己，就可以撤销.*/
                    if (gwf.Sender != WebUser.No)
                        throw new Exception("@分流点不是您发送的，您不能执行撤销。");
                   
                     return this.DoUnSendFeiLiu(gwf);
                case NodeWorkType.StartWorkFL:
                    return this.DoUnSendFeiLiu(gwf);
                case NodeWorkType.WorkHL:
                    if (this.IsMainFlow)
                    {
                        /* 首先找到与他最近的一个分流点，并且判断当前的操作员是不是分流点上的工作人员。*/
                        return this.DoUnSendHeiLiu_Main(gwf);
                    }
                    else
                    {
                        return this.DoUnSendSubFlow(gwf); //是子流程时.
                    }
                    break;
                case NodeWorkType.SubThreadWork:
                    break;
                default:
                    break;
            }

            if (nd.IsStartNode)
                throw new Exception("当前节点是开始节点，所以您不能撤销。");

            //定义当前的节点.
            WorkNode wn = this.GetCurrentWorkNode();

            #region 求的撤销的节点.
            int cancelToNodeID = 0;

            if (nd.HisCancelRole == CancelRole.SpecNodes)
            {
                /*指定的节点可以撤销,首先判断当前人员是否有权限.*/
                NodeCancels ncs = new NodeCancels();
                ncs.Retrieve(NodeCancelAttr.FK_Node, wn.HisNode.NodeID);
                if (ncs.Count == 0)
                    throw new Exception("@流程设计错误, 您设置了当前节点(" + wn.HisNode.Name + ")可以让指定的节点人员撤销，但是您没有设置指定的节点.");

                /* 查询出来. */
                string sql = "SELECT FK_Node FROM WF_GenerWorkerList WHERE FK_Emp='" + WebUser.No + "' AND IsPass=1 AND IsEnable=1 AND WorkID=" + wn.HisWork.OID + " ORDER BY RDT DESC ";
                DataTable dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0)
                    throw new Exception("@撤销流程错误,您没有权限执行撤销发送.");

                // 找到将要撤销到的NodeID.
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (NodeCancel nc in ncs)
                    {
                        if (nc.CancelTo == int.Parse(dr[0].ToString()))
                        {
                            cancelToNodeID = nc.CancelTo;
                            break;
                        }
                    }

                    if (cancelToNodeID != 0)
                        break;
                }

                if (cancelToNodeID == 0)
                    throw new Exception("@撤销流程错误,您没有权限执行撤销发送,没有找到可以撤销的节点.");
            }

            if (nd.HisCancelRole == CancelRole.OnlyNextStep)
            {
                /*如果仅仅允许撤销上一步骤.*/
                WorkNode wnPri = wn.GetPreviousWorkNode();

                GenerWorkerList wl = new GenerWorkerList();
                int num = wl.Retrieve(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No,
                    GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID);
                if (num == 0)
                    throw new Exception("@您不能执行撤消发送，因为当前工作不是您发送的或下一步工作已处理。");
                cancelToNodeID = wnPri.HisNode.NodeID;
            }

            if (cancelToNodeID == 0)
                throw new Exception("@没有求出要撤销到的节点.");
            #endregion 求的撤销的节点.

            /********** 开始执行撤销. **********************/
            Node cancelToNode = new Node(cancelToNodeID);
            WorkNode wnOfCancelTo = new WorkNode(this.WorkID, cancelToNodeID);

            // 调用撤消发送前事件。
            string msg = nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, nd, wn.HisWork, null);

            #region 删除当前节点数据。

            // 删除产生的工作列表。
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node );

            // 删除工作信息,如果是按照ccflow格式存储的。
            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            // 删除附件信息。
            DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB WHERE FK_MapData='ND" + gwf.FK_Node + "' AND RefPKVal='" + this.WorkID + "'");
            #endregion 删除当前节点数据。

            // 更新.
            gwf.FK_Node = cancelToNode.NodeID;
            gwf.NodeName = cancelToNode.Name;

            if (cancelToNode.IsEnableTaskPool && Glo.IsEnableTaskPool)
                gwf.TaskSta = TaskSta.Takeback;
            else
                gwf.TaskSta = TaskSta.None;

            gwf.TodoEmps = WebUser.No + "," + WebUser.Name;
            gwf.Update();

            if (cancelToNode.IsEnableTaskPool && Glo.IsEnableTaskPool)
            {
                //设置全部的人员不可用。
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0,  IsEnable=-1 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);

                //设置当前人员可用。
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0,  IsEnable=1  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node + " AND FK_Emp='" + WebUser.No + "'");
            }
            else
            {
                BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            }

            //更新当前节点，到rpt里面。
            BP.DA.DBAccess.RunSQL("UPDATE " + this.HisFlow.PTable + " SET FlowEndNode=" + gwf.FK_Node + " WHERE OID=" + this.WorkID);
            
            // 记录日志..
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, cancelToNode.NodeID, cancelToNode.Name, "无");

            // 删除数据.
            if (wn.HisNode.IsStartNode)
            {
                DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE WorkID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND FK_Node=" + nd.NodeID);
            }

            if (wn.HisNode.IsEval)
            {
                /*如果是质量考核节点，并且撤销了。*/
                DBAccess.RunSQL("DELETE FROM WF_CHEval WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.WorkID);
            }

            #region 恢复工作轨迹，解决工作抢办。
            if (cancelToNode.IsStartNode == false && cancelToNode.IsEnableTaskPool == false)
            {
                WorkNode ppPri = wnOfCancelTo.GetPreviousWorkNode();
                GenerWorkerList wl = new GenerWorkerList();
                wl.Retrieve(GenerWorkerListAttr.FK_Node, wnOfCancelTo.HisNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);
                // BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + backtoNodeID + " AND WorkID=" + this.WorkID);
                RememberMe rm = new RememberMe();
                rm.Retrieve(RememberMeAttr.FK_Node, wnOfCancelTo.HisNode.NodeID, RememberMeAttr.FK_Emp, ppPri.HisWork.Rec);

                string[] empStrs = rm.Objs.Split('@');
                foreach (string s in empStrs)
                {
                    if (s == "" || s == null)
                        continue;

                    if (s == wl.FK_Emp)
                        continue;
                    GenerWorkerList wlN = new GenerWorkerList();
                    wlN.Copy(wl);
                    wlN.FK_Emp = s;

                    WF.Port.WFEmp myEmp = new Port.WFEmp(s);
                    wlN.FK_EmpText = myEmp.Name;

                    wlN.Insert();
                }
            }
            #endregion 恢复工作轨迹，解决工作抢办。

            #region 如果是开始节点, 检查此流程是否有子线程，如果有则删除它们。
            if (nd.IsStartNode)
            {
                /*要检查一个是否有 子流程，如果有，则删除它们。*/
                GenerWorkFlows gwfs = new GenerWorkFlows();
                gwfs.Retrieve(GenerWorkFlowAttr.PWorkID, this.WorkID);
                if (gwfs.Count > 0)
                {
                    foreach (GenerWorkFlow item in gwfs)
                    {
                        /*删除每个子线程.*/
                        BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(item.FK_Flow, item.WorkID, true);
                    }
                }
            }
            #endregion

            //调用撤消发送后事件。
            msg += nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, nd, wn.HisWork, null);

            if (wnOfCancelTo.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    if (wnOfCancelTo.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@撤消发送执行成功，您可以点这里<a href='" + VirPath + "WF/Wap/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + VirPath + "WF/Wap/MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                    else
                        return "@撤销成功." + msg;
                }
                else
                {
                    switch (wnOfCancelTo.HisNode.HisFormType)
                    {
                        case NodeFormType.FixForm:
                        case NodeFormType.FreeForm:
                            return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + this.VirPath + this.AppType + "/Do.aspx?ActionType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                            break;
                        default:
                            return "撤销成功" + msg;
                            break;
                    }
                }
            }
            else
            {
                // 更新是否显示。
                //  DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + cancelToNode.NodeID);
                switch (wnOfCancelTo.HisNode.HisFormType)
                {
                    case NodeFormType.FixForm:
                    case NodeFormType.FreeForm:
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>  . " + msg;
                        break;
                    default:
                        return "撤销成功:" + msg;
                        break;
                }
            }
            return "工作已经被您撤销到:" + cancelToNode.Name;
        }
        /// <summary>
        /// 撤消分流点
        /// 1, 把分流节点的人员设置成待办。
        /// 2，删除所有该分流点发起的子线程。
        /// </summary>
        /// <param name="gwf"></param>
        /// <returns></returns>
        private string DoUnSendFeiLiu(GenerWorkFlow gwf)
        {
            //首先要检查，当前的处理人是否是分流节点的处理人？如果是，就要把，未走完的所有子线程都删除掉。
            GenerWorkerList gwl = new GenerWorkerList();
            int i=gwl.Retrieve(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node, GenerWorkerListAttr.FK_Emp, WebUser.No);
            if (i == 0)
                throw new Exception("@您不能执行撤消发送，因为当前工作不是您发送的。");

            //处理事件.
            Node nd = new Node(gwf.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = gwf.WorkID;
            wk.RetrieveFromDBSources();

            string msg = nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, nd, wk, null);

            // 记录日志..
            WorkNode wn = new WorkNode(wk, nd);
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, gwf.FK_Node, gwf.NodeName, "");

            // 删除分合流记录。
            if (nd.IsStartNode)
                DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);

            //删除上一个节点的数据。
            foreach (Node ndNext in nd.HisToNodes)
            {
                i = DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + this.WorkID + " AND FK_Node=" + ndNext.NodeID);
                if (i == 0)
                    continue;

                if (ndNext.HisRunModel == RunModel.SubThread)
                {
                    /*如果到达的节点是子线程,就查询出来发起的子线程。*/
                    GenerWorkFlows gwfs = new GenerWorkFlows();
                    gwfs.Retrieve(GenerWorkFlowAttr.FID, this.WorkID);
                    foreach (GenerWorkFlow en in gwfs)
                        BP.WF.Dev2Interface.Flow_DeleteSubThread(gwf.FK_Flow, en.WorkID, "撤销发送删除");
                    continue;
                }

                // 删除工作记录。
                Works wks = ndNext.HisWorks;
                if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                    wks.Delete(GenerWorkerListAttr.FID, this.WorkID);
            }

            //设置当前节点。
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node + " AND IsPass=1");
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerFH SET FK_Node=" + gwf.FK_Node + " WHERE FID=" + this.WorkID);

            // 设置当前节点的状态.
            Node cNode = new Node(gwf.FK_Node);
            Work cWork = cNode.HisWork;
            cWork.OID = this.WorkID;
            msg += nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, nd, wk, null);
            if (cNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
                else
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='Do.aspx?ActionType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
            }
            else
            {
                // 更新是否显示。
                // DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + cNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
                else
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
            }
        }
        /// <summary>
        /// 撤消分流点
        /// </summary>
        /// <param name="gwf"></param>
        /// <returns></returns>
        private string DoUnSendFeiLiu_bak(GenerWorkFlow gwf)
        {
            //首先要检查，当前的处理人是否是分流节点的处理人？如果是，就要把，未走完的所有子线程都删除掉。
            GenerWorkerList gwl = new GenerWorkerList();

            string sql = "SELECT FK_Node FROM WF_GenerWorkerList WHERE WorkID=" + this.WorkID + " AND FK_Emp='" + BP.Web.WebUser.No + "' AND FK_Node=" + gwf.FK_Node ;
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                return "@您不能执行撤消发送，因为当前工作不是您发送的。";

            //处理事件.
            Node nd = new Node(gwf.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = gwf.WorkID;
            wk.RetrieveFromDBSources();

            string msg = nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, nd, wk, null);

            // 记录日志..
            WorkNode wn = new WorkNode(wk, nd);
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, gwf.FK_Node, gwf.NodeName, "");

            // 删除分合流记录。
            if (nd.IsStartNode)
            {
                DBAccess.RunSQL("DELETE FROM WF_GenerFH WHERE FID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE WorkID=" + this.WorkID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND FK_Node=" + nd.NodeID);
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist WHERE FID=" + this.WorkID);
            }

            //删除上一个节点的数据。
            foreach (Node ndNext in nd.HisToNodes)
            {
                int i = DBAccess.RunSQL("DELETE FROM WF_GenerWorkerList WHERE FID=" + this.WorkID + " AND FK_Node=" + ndNext.NodeID);
                if (i == 0)
                    continue;

                // 删除工作记录。
                Works wks = ndNext.HisWorks;
                if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                    wks.Delete(GenerWorkerListAttr.FID, this.WorkID);

                // 删除已经发起的流程。
                DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow WHERE FID=" + this.WorkID + " AND FK_Node=" + ndNext.NodeID);
            }

            //设置当前节点。
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node + " AND IsPass=1");
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerFH SET FK_Node=" + gwf.FK_Node + " WHERE FID=" + this.WorkID);

            // 设置当前节点的状态.
            Node cNode = new Node(gwf.FK_Node);
            Work cWork = cNode.HisWork;
            cWork.OID = this.WorkID;
            msg += nd.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, nd, wk, null);
            if (cNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
                else
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='Do.aspx?ActionType=DeleteFlow&WorkID=" + cWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
            }
            else
            {
                // 更新是否显示。
                // DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + cNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
                else
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=0&FK_Node=" + gwf.FK_Node + "' ><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
            }
        }
        /// <summary>
        /// 执行撤销发送
        /// </summary>
        /// <param name="gwf"></param>
        /// <returns></returns>
        public string DoUnSendHeiLiu_Main(GenerWorkFlow gwf)
        {
            Node currNode = new Node(gwf.FK_Node);
            Node priFLNode = currNode.HisPriFLNode;
            GenerWorkerList wl = new GenerWorkerList();
            int i = wl.Retrieve(GenerWorkerListAttr.FK_Node, priFLNode.NodeID, GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No);
            if (i == 0)
                return "@不是您把工作发送到当前节点上，所以您不能撤消。";

            WorkNode wn = this.GetCurrentWorkNode();
            WorkNode wnPri = new WorkNode(this.WorkID, priFLNode.NodeID);

            // 记录日志..
            wnPri.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, wnPri.HisNode.NodeID, wnPri.HisNode.Name, "无");

            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node.ToString());

            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            gwf.FK_Node = wnPri.HisNode.NodeID;
            gwf.NodeName = wnPri.HisNode.Name;
            gwf.Update();

            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerFH SET FK_Node=" + gwf.FK_Node + " WHERE FID=" + this.WorkID);

            ShiftWorks fws = new ShiftWorks();
            fws.Delete(ShiftWorkAttr.FK_Node, wn.HisNode.NodeID.ToString(),
                ShiftWorkAttr.WorkID, this.WorkID.ToString());

            #region 恢复工作轨迹，解决工作抢办。
            if (wnPri.HisNode.IsStartNode == false)
            {
                WorkNode ppPri = wnPri.GetPreviousWorkNode();
                wl = new GenerWorkerList();
                wl.Retrieve(GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID, GenerWorkerListAttr.WorkID, this.WorkID);
                // BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + backtoNodeID + " AND WorkID=" + this.WorkID);
                RememberMe rm = new RememberMe();
                rm.Retrieve(RememberMeAttr.FK_Node, wnPri.HisNode.NodeID, RememberMeAttr.FK_Emp, ppPri.HisWork.Rec);

                string[] empStrs = rm.Objs.Split('@');
                foreach (string s in empStrs)
                {
                    if (s == "" || s == null)
                        continue;

                    if (s == wl.FK_Emp)
                        continue;
                    GenerWorkerList wlN = new GenerWorkerList();
                    wlN.Copy(wl);
                    wlN.FK_Emp = s;

                    WF.Port.WFEmp myEmp = new Port.WFEmp(s);
                    wlN.FK_EmpText = myEmp.Name;

                    wlN.Insert();
                }
            }
            #endregion 恢复工作轨迹，解决工作抢办。

            // 删除以前的节点数据.
            wnPri.DeleteToNodesData(priFLNode.HisToNodes);
            if (wnPri.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    if (wnPri.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + this.VirPath + this.AppType + "/MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。";
                    else
                        return "@撤销成功.";
                }
                else
                {
                    if (wnPri.HisNode.HisFormType != NodeFormType.SDKForm)
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + this.VirPath + this.AppType + "/MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。";
                    else
                        return "@撤销成功.";
                }
            }
            else
            {
                // 更新是否显示。
#warning
                // DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。";
                }
                else
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。";
                }
            }
        }
        public string DoUnSendSubFlow(GenerWorkFlow gwf)
        {
            WorkNode wn = this.GetCurrentWorkNode();
            WorkNode wnPri = wn.GetPreviousWorkNode();

            GenerWorkerList wl = new GenerWorkerList();
            int num = wl.Retrieve(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No,
                GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID);
            if (num == 0)
                return "@您不能执行撤消发送，因为当前工作不是您发送的。";

            // 处理事件。
            string msg = wn.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneBefore, wn.HisNode, wn.HisWork, null);

            // 删除工作者。
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Delete(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, gwf.FK_Node.ToString());

            if (this.HisFlow.HisDataStoreModel == BP.WF.Template.DataStoreModel.ByCCFlow)
                wn.HisWork.Delete();

            gwf.FK_Node = wnPri.HisNode.NodeID;
            gwf.NodeName = wnPri.HisNode.Name;
            gwf.Update();

            BP.DA.DBAccess.RunSQL("UPDATE WF_GenerWorkerlist SET IsPass=0 WHERE WorkID=" + this.WorkID + " AND FK_Node=" + gwf.FK_Node);
            ShiftWorks fws = new ShiftWorks();
            fws.Delete(ShiftWorkAttr.FK_Node, wn.HisNode.NodeID.ToString(), ShiftWorkAttr.WorkID, this.WorkID.ToString());

            #region 判断撤消的百分比条件的临界点条件
            if (wn.HisNode.PassRate != 0)
            {
                decimal all = (decimal)BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) NUM FROM dbo.WF_GenerWorkerList WHERE FID=" + this.FID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                decimal ok = (decimal)BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(*) NUM FROM dbo.WF_GenerWorkerList WHERE FID=" + this.FID + " AND IsPass=1 AND FK_Node=" + wnPri.HisNode.NodeID);
                decimal rate = ok / all * 100;
                if (wn.HisNode.PassRate <= rate)
                    DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=0 WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.FID);
                else
                    DBAccess.RunSQL("UPDATE WF_GenerWorkerList SET IsPass=3 WHERE FK_Node=" + wn.HisNode.NodeID + " AND WorkID=" + this.FID);
            }
            #endregion

            // 处理事件。
            msg += wn.HisFlow.DoFlowEventEntity(EventListOfNode.UndoneAfter, wn.HisNode, wn.HisWork, null);

            // 记录日志..
            wn.AddToTrack(ActionType.UnSend, WebUser.No, WebUser.Name, wn.HisNode.NodeID, wn.HisNode.Name, "无");

            if (wnPri.HisNode.IsStartNode)
            {
                if (BP.Web.WebUser.IsWap)
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + VirPath + "WF/MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
                else
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + VirPath + "WF/MyFlowInfo.aspx?DoType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A> , <a href='" + VirPath + "WF/Do.aspx?ActionType=DeleteFlow&WorkID=" + wn.HisWork.OID + "&FK_Flow=" + this.FlowNo + "' /><img src='" + VirPath + "WF/Img/Btn/Delete.gif' border=0/>此流程已经完成(删除它)</a>。" + msg;
                }
            }
            else
            {
                // 更新是否显示。
                //  DBAccess.RunSQL("UPDATE WF_ForwardWork SET IsRead=1 WHERE WORKID=" + this.WorkID + " AND FK_Node=" + wnPri.HisNode.NodeID);
                if (BP.Web.WebUser.IsWap == false)
                {
                    if (this.HisFlow.FK_FlowSort != "00")
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                    else
                        return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
                else
                {
                    return "@撤消执行成功，您可以点这里<a href='" + this.VirPath + this.AppType + "/MyFlow.aspx?FK_Flow=" + this.FlowNo + "&WorkID=" + this.WorkID + "&FID=" + gwf.FID + "&FK_Node=" + gwf.FK_Node + "'><img src='" + VirPath + "WF/Img/Btn/Do.gif' border=0/>执行工作</A>。" + msg;
                }
            }
        }
    }
}
