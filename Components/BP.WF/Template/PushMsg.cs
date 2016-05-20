﻿using System;
using System.Collections;
using BP.DA;
using BP.Web;
using BP.En;
using BP.Port;
using BP.Sys;

namespace BP.WF.Template
{
    /// <summary>
    /// 推送的方式
    /// </summary>
    public enum PushWay
    {
        /// <summary>
        /// 当前节点的接受人
        /// </summary>
        CurrentWorkers,
        /// <summary>
        /// 指定节点的工作人员
        /// </summary>
        NodeWorker,
        /// <summary>
        /// 指定的工作人员s
        /// </summary>
        SpecEmps,
        /// <summary>
        /// 指定的工作岗位s
        /// </summary>
        SpecStations,
        /// <summary>
        /// 指定的部门人员
        /// </summary>
        SpecDepts,
        /// <summary>
        /// 指定的SQL
        /// </summary>
        SpecSQL,
        /// <summary>
        /// 按照系统指定的字段
        /// </summary>
        ByParas
    }
	/// <summary>
	/// 消息推送属性
	/// </summary>
    public class PushMsgAttr
    {
        /// <summary>
        /// 节点
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// 事件
        /// </summary>
        public const string FK_Event = "FK_Event";
        /// <summary>
        /// 推送方式
        /// </summary>
        public const string PushWay = "PushWay";
        /// <summary>
        /// 推送处理内容
        /// </summary>
        public const string PushDoc = "PushDoc";
        /// <summary>
        /// 推送处理内容 tag.
        /// </summary>
        public const string Tag = "Tag";

        #region 消息设置.
        /// <summary>
        /// 是否启用发送邮件
        /// </summary>
        public const string MailEnable = "MailEnable";
        /// <summary>
        /// 消息标题
        /// </summary>
        public const string MailTitle = "MailTitle";
        /// <summary>
        /// 消息内容模版
        /// </summary>
        public const string MailDoc = "MailDoc";
        /// <summary>
        /// 是否启用短信
        /// </summary>
        public const string SMSEnable = "SMSEnable";
        /// <summary>
        /// 短信内容模版
        /// </summary>
        public const string SMSDoc = "SMSDoc";
        /// <summary>
        /// 是否推送？
        /// </summary>
        public const string MobilePushEnable = "MobilePushEnable";
        #endregion 消息设置.

        /// <summary>
        /// 短信字段
        /// </summary>
        public const string SMSField = "SMSField";
        /// <summary>
        /// 推送方式
        /// </summary>
        public const string SMSPushWay = "SMSPushWay";
        /// <summary>
        /// 邮件字段
        /// </summary>
        public const string MailAddress = "MailAddress";
        /// <summary>
        /// 邮件推送方式
        /// </summary>
        public const string MailPushWay = "MailPushWay";
    }
	/// <summary>
	/// 消息推送
	/// </summary>
    public class PushMsg : EntityMyPK
    {
        #region 基本属性
        /// <summary>
        /// 事件
        /// </summary>
        public string FK_Event
        {
            get
            {
                return this.GetValStringByKey(PushMsgAttr.FK_Event);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.FK_Event, value);
            }
        }
        public int PushWay
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.PushWay);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.PushWay, value);
            }
        }
        /// <summary>
        ///节点
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.FK_Node, value);
            }
        }
        public string PushDoc
        {
            get
            {
                string s = this.GetValStringByKey(PushMsgAttr.PushDoc);
                if (string.IsNullOrEmpty(s) == true)
                    s = "";
                return s;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.PushDoc, value);
            }
        }
        public string Tag
        {
            get
            {
                string s = this.GetValStringByKey(PushMsgAttr.Tag);
                if (string.IsNullOrEmpty(s) == true)
                    s = "";
                return s;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.Tag, value);
            }
        }
        #endregion

        #region 事件消息.
        /// <summary>
        /// 邮件推送方式
        /// </summary>
        public int MailPushWay
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.MailPushWay);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailPushWay, value);
            }
        }
        public string MailPushWayText
        {
            get
            {
                if (this.FK_Event == EventListOfNode.WorkArrive)
                {
                    if (this.MailPushWay == 0)
                        return "不发送";

                    if (this.MailPushWay == 1)
                        return "发送给当前节点的所有处理人";

                    if (this.MailPushWay == 2)
                        return "向指定的字段发送";
                }

                if (this.FK_Event == EventListOfNode.SendSuccess)
                {
                    if (this.MailPushWay == 0)
                        return "不发送";

                    if (this.MailPushWay == 1)
                        return "发送给下一个节点的所有接受人";

                    if (this.MailPushWay == 2)
                        return "向指定的字段发送";
                }

                if (this.FK_Event == EventListOfNode.ReturnAfter)
                {
                    if (this.MailPushWay == 0)
                        return "不发送";

                    if (this.MailPushWay == 1)
                        return "发送给被退回的节点处理人";

                    if (this.MailPushWay == 2)
                        return "向指定的字段发送";
                }

                return "未知";
            }
        }
        /// <summary>
        /// 邮件地址
        /// </summary>
        public string MailAddress
        {
            get
            {
                return this.GetValStringByKey(PushMsgAttr.MailAddress);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailAddress, value);
            }
        }
        /// <summary>
        /// 邮件标题.
        /// </summary>
        public string MailTitle
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.MailTitle);
                if (string.IsNullOrEmpty(str) == false)
                    return str;
                switch (this.FK_Event)
                {
                    case EventListOfNode.WorkArrive:
                        return "新工作{{Title}},发送人@WebUser.No,@WebUser.Name";
                    case EventListOfNode.SendSuccess:
                        return "新工作{{Title}},发送人@WebUser.No,@WebUser.Name";
                    case EventListOfNode.ShitAfter:
                        return "移交来的新工作{{Title}},移交人@WebUser.No,@WebUser.Name";
                    case EventListOfNode.ReturnAfter:
                        return "被退回来{{Title}},退回人@WebUser.No,@WebUser.Name";
                    case EventListOfNode.UndoneAfter:
                        return "工作被撤销{{Title}},发送人@WebUser.No,@WebUser.Name";
                    case EventListOfNode.AskerReAfter:
                        return "加签新工作{{Title}},发送人@WebUser.No,@WebUser.Name";
                        break;
                    default:
                        throw new Exception("@该事件类型没有定义默认的消息模版:" + this.FK_Event);
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string MailTitle_Real
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.MailTitle);
                return str;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailTitle, value);
            }
        }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string MailDoc_Real
        {
            get
            {
                return this.GetValStrByKey(PushMsgAttr.MailDoc);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.MailDoc, value);
            }
        }
        public string MailDoc
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.MailDoc);
                if (string.IsNullOrEmpty(str) == false)
                    return str;
                switch (this.FK_Event)
                {
                    case EventListOfNode.WorkArrive:
                        str += "\t\n您好:";
                        str += "\t\n    有新工作{{Title}}需要您处理, 点击这里打开工作{Url} .";
                        str += "\t\n致! ";
                        str += "\t\n    @WebUser.No, @WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.SendSuccess:
                        str += "\t\n您好:";
                        str += "\t\n    有新工作{{Title}}需要您处理, 点击这里打开工作{Url} .";
                        str += "\t\n致! ";
                        str += "\t\n    @WebUser.No, @WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ReturnAfter:
                        str += "\t\n您好:";
                        str += "\t\n    工作{{Title}}被退回来了, 点击这里打开工作{Url} .";
                        str += "\t\n    退回意见: \t\n ";
                        str += "\t\n    {  @ReturnMsg }";
                        str += "\t\n 致! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.ShitAfter:
                        str += "\t\n您好:";
                        str += "\t\n    移交给您的工作{{Title}}, 点击这里打开工作{Url} .";
                        str += "\t\n 致! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.UndoneAfter:
                        str += "\t\n您好:";
                        str += "\t\n    移交给您的工作{{Title}}, 点击这里打开工作{Url} .";
                        str += "\t\n 致! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    case EventListOfNode.AskerReAfter: //加签.
                        str += "\t\n您好:";
                        str += "\t\n    移交给您的工作{{Title}}, 点击这里打开工作{Url} .";
                        str += "\t\n 致! ";
                        str += "\t\n    @WebUser.No,@WebUser.Name";
                        str += "\t\n    @RDT";
                        break;
                    default:
                        throw new Exception("@该事件类型没有定义默认的消息模版:" + this.FK_Event);
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 短信接收人字段
        /// </summary>
        public string SMSField
        {
            get
            {
                return this.GetValStringByKey(PushMsgAttr.SMSField);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSField, value);
            }
        }
        /// <summary>
        /// 短信提醒方式
        /// </summary>
        public int SMSPushWay
        {
            get
            {
                return this.GetValIntByKey(PushMsgAttr.SMSPushWay);
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSPushWay, value);
            }
        }
        /// <summary>
        /// 发送消息标签
        /// </summary>
        public string SMSPushWayText
        {
            get
            {
                if (this.FK_Event == EventListOfNode.WorkArrive)
                {
                    if (this.SMSPushWay == 0)
                        return "不发送";

                    if (this.SMSPushWay == 1)
                        return "发送给当前节点的所有处理人";

                    if (this.SMSPushWay == 2)
                        return "向指定的字段发送";
                }

                if (this.FK_Event == EventListOfNode.SendSuccess)
                {
                    if (this.SMSPushWay == 0)
                        return "不发送";

                    if (this.SMSPushWay == 1)
                        return "发送给下一个节点的所有接受人";

                    if (this.SMSPushWay == 2)
                        return "向指定的字段发送";
                }

                if (this.FK_Event == EventListOfNode.ReturnAfter)
                {
                    if (this.SMSPushWay == 0)
                        return "不发送";

                    if (this.SMSPushWay == 1)
                        return "发送给被退回的节点处理人";

                    if (this.SMSPushWay == 2)
                        return "向指定的字段发送";
                }

                return "未知";
            }
        }
        /// <summary>
        /// 短信模版内容
        /// </summary>
        public string SMSDoc_Real
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.SMSDoc);
                return str;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSDoc, value);
            }
        }
        /// <summary>
        /// 短信模版内容
        /// </summary>
        public string SMSDoc
        {
            get
            {
                string str = this.GetValStrByKey(PushMsgAttr.SMSDoc);
                if (string.IsNullOrEmpty(str) == false)
                    return str;

                switch (this.FK_Event)
                {
                    case EventListOfNode.WorkArrive:
                    case EventListOfNode.SendSuccess:
                        str = "有新工作{{Title}}需要您处理, 发送人:@WebUser.No, @WebUser.Name,打开{Url} .";
                        break;
                    case EventListOfNode.ReturnAfter:
                        str = "工作{{Title}}被退回,退回人:@WebUser.No, @WebUser.Name,打开{Url} .";
                        break;
                    case EventListOfNode.ShitAfter:
                        str = "移交工作{{Title}},移交人:@WebUser.No, @WebUser.Name,打开{Url} .";
                        break;
                    case EventListOfNode.UndoneAfter:
                        str = "工作撤销{{Title}},撤销人:@WebUser.No, @WebUser.Name,打开{Url}.";
                        break;
                    case EventListOfNode.AskerReAfter: //加签.
                        str = "工作加签{{Title}},加签人:@WebUser.No, @WebUser.Name,打开{Url}.";
                        break;
                    default:
                        throw new Exception("@该事件类型没有定义默认的消息模版:" + this.FK_Event);
                        break;
                }
                return str;
            }
            set
            {
                this.SetValByKey(PushMsgAttr.SMSDoc, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 消息推送
        /// </summary>
        public PushMsg()
        {
        }
        /// <summary>
        /// 重写基类方法
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_PushMsg", "消息推送");

                map.AddMyPK();

                map.AddTBInt(PushMsgAttr.FK_Node, 0, "节点", true, false);
                map.AddTBString(PushMsgAttr.FK_Event, null, "事件类型", true, false, 0, 15, 10);

                #region 将要删除.
                map.AddDDLSysEnum(PushMsgAttr.PushWay, 0, "推送方式", true, false, PushMsgAttr.PushWay,
                    "@0=按照指定节点的工作人员@1=按照指定的工作人员@2=按照指定的工作岗位@3=按照指定的部门@4=按照指定的SQL@5=按照系统指定的字段");
                //设置内容.
                map.AddTBString(PushMsgAttr.PushDoc, null, "推送保存内容", true, false, 0, 3500, 10);
                map.AddTBString(PushMsgAttr.Tag, null, "Tag", true, false, 0, 500, 10);
                #endregion 将要删除.

                #region 短信.
                map.AddTBInt(PushMsgAttr.SMSPushWay, 0, "短信发送方式", true, true);
                map.AddTBString(PushMsgAttr.SMSField, null, "短信字段", true, false, 0, 100, 10);
                map.AddTBStringDoc(PushMsgAttr.SMSDoc, null, "短信内容模版", true, false, true);
                #endregion 短信.

                #region 邮件.
                map.AddTBInt(PushMsgAttr.MailPushWay, 0, "邮件发送方式",true, true);
                map.AddTBString(PushMsgAttr.MailAddress, null, "邮件字段", true, false, 0, 100, 10);
                map.AddTBString(PushMsgAttr.MailTitle, null, "邮件标题模版", true, false, 0, 200, 20, true);
                map.AddTBStringDoc(PushMsgAttr.MailDoc, null, "邮件内容模版", true, false, true);
                #endregion 邮件.


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        /// <summary>
        /// 生成提示信息.
        /// </summary>
        /// <returns></returns>
        private string generAlertMessage=null;
       
        /// <summary>
        /// 执行消息发送
        /// </summary>
        /// <param name="currNode">当前节点</param>
        /// <param name="en">数据实体</param>
        /// <param name="atPara">参数</param>
        /// <param name="objs">发送返回对象</param>
        /// <param name="jumpToNode">跳转到的节点</param>
        /// <param name="jumpToEmps">跳转到的人员</param>
        /// <returns>执行成功的消息</returns>
        public string DoSendMessage(Node currNode, Entity en, string atPara, SendReturnObjs objs, Node jumpToNode = null, string jumpToEmps = null)
        {
            if (en == null)
                return "";

            #region 处理参数.
            Row r = en.Row;
            try
            {
                //系统参数.
                r.Add("FK_MapData", en.ClassID);
            }
            catch
            {
                r["FK_MapData"] = en.ClassID;
            }

            if (atPara != null)
            {
                AtPara ap = new AtPara(atPara);
                foreach (string s in ap.HisHT.Keys)
                {
                    try
                    {
                        r.Add(s, ap.GetValStrByKey(s));
                    }
                    catch
                    {
                        r[s] = ap.GetValStrByKey(s);
                    }
                }
            }

            Int64 workid = Int64.Parse(en.PKVal.ToString());

            string title = "标题";
            if (en.Row.ContainsKey("Title") == true)
                title = en.GetValStringByKey("Title"); // 获得工作标题.
            else
                title = BP.DA.DBAccess.RunSQLReturnStringIsNull("SELECT Title FROM WF_GenerWorkFlow WHERE WorkID=" + en.PKVal, "标题");

            string hostUrl = Glo.HostURL;
            string sid = "{EmpStr}_" + workid + "_" + currNode.NodeID + "_" + DataType.CurrentDataTime;
            string openWorkURl = hostUrl + "WF/Do.aspx?DoType=OF&SID=" + sid;
            openWorkURl = openWorkURl.Replace("//", "/");
            openWorkURl = openWorkURl.Replace("//", "/");
#endregion 

            #region 处理发送邮件.
            // 发送邮件.
            string mailTitleTmp = "";
            string mailDocTmp = "";
            if (this.MailPushWay != 0)
            {
                // 标题.
                mailTitleTmp = this.MailTitle;
                mailTitleTmp = mailTitleTmp.Replace("{Title}", title);
                mailTitleTmp = mailTitleTmp.Replace("@WebUser.No", WebUser.No);
                mailTitleTmp = mailTitleTmp.Replace("@WebUser.Name", WebUser.Name);

                // 内容.
                mailDocTmp = this.MailDoc;
                mailDocTmp = mailDocTmp.Replace("{Url}", openWorkURl);
                mailDocTmp = mailDocTmp.Replace("{Title}", title);

                mailDocTmp = mailDocTmp.Replace("@WebUser.No", WebUser.No);
                mailDocTmp = mailDocTmp.Replace("@WebUser.Name", WebUser.Name);

                /*如果仍然有没有替换下来的变量.*/
                if (mailDocTmp.Contains("@"))
                    mailDocTmp = Glo.DealExp(mailDocTmp, en, null);

                //求发送给的人员ID.
                string toEmpIDs = "";

                #region WorkArrive-工作到达. - 邮件处理.
                if (this.FK_Event == BP.Sys.EventListOfNode.WorkArrive || this.FK_Event == BP.Sys.EventListOfNode.ReturnAfter)
                {
                    /*工作到达.*/
                    if (this.MailPushWay == 1)
                    {
                        /*如果向接受人发送邮件.*/
                        toEmpIDs = jumpToEmps;
                        string[] emps = toEmpIDs.Split(',');
                        foreach (string emp in emps)
                        {
                            if (string.IsNullOrEmpty(emp))
                                continue;

                            // 因为要发给不同的人，所有需要clone 一下，然后替换发送.
                            string mailDocReal = mailDocTmp.Clone() as string;
                            mailDocReal = mailDocReal.Replace("{EmpStr}", emp);

                            //获得当前人的邮件.
                            BP.WF.Port.WFEmp empEn = new Port.WFEmp(emp);

                            //发送邮件.
                                BP.WF.Dev2Interface.Port_SendEmail(empEn.Email, mailTitleTmp, mailDocReal, "ToDo", "WKAlt" + currNode.NodeID + "_" + workid);
                        }
                        generAlertMessage += "@已向:{" + toEmpIDs + "}发送提醒邮件，由 " + this.FK_Event + " 发出.";
                    }

                    if (this.MailPushWay == 2)
                    {
                        /*如果向指定的字段作为发送邮件的对象, 从字段里取数据. */
                        string emailAddress = r[this.MailAddress] as string;

                        //发送邮件
                            BP.WF.Dev2Interface.Port_SendEmail(emailAddress, mailTitleTmp, mailDocTmp, "ToDo", "WKAlt" + currNode.NodeID + "_" + workid);
                            generAlertMessage += "@已向:{" + emailAddress + "}发送提醒邮件，由 "+this.FK_Event+" 发出.";
                    }
                }
                #endregion 发送成功事件.

                #region SendSuccess - 发送成功事件. - 邮件处理.
                if (this.FK_Event == BP.Sys.EventListOfNode.SendSuccess)
                {
                    /*发送成功事件.*/
                    if (this.MailPushWay == 1  && objs.VarAcceptersID!=null )
                    {
                        /*如果向接受人发送邮件.*/
                        toEmpIDs = objs.VarAcceptersID;
                        string[] emps = toEmpIDs.Split(',');
                        foreach (string emp in emps)
                        {
                            if (string.IsNullOrEmpty(emp))
                                continue;

                            // 因为要发给不同的人，所有需要clone 一下，然后替换发送.
                            string mailDocReal = mailDocTmp.Clone() as string;
                            mailDocReal = mailDocReal.Replace("{EmpStr}", emp);

                            //获得当前人的邮件.
                            BP.WF.Port.WFEmp empEn = new Port.WFEmp(emp);

                            //发送邮件.
                            BP.WF.Dev2Interface.Port_SendEmail(empEn.Email, mailTitleTmp, mailDocReal, "ToDo", "WKAlt" + objs.VarToNodeID + "_" + workid);
                        }
                        generAlertMessage += "@已向:{" + toEmpIDs + "}发送提醒邮件，由 SendSuccess 发出.";
                    }

                    if (this.MailPushWay == 2)
                    {
                        /*如果向指定的字段作为发送邮件的对象, 从字段里取数据. */
                        string emailAddress = r[this.MailAddress] as string;

                        //发送邮件
                        BP.WF.Dev2Interface.Port_SendEmail(emailAddress, mailTitleTmp, mailDocTmp, "ToDo", "WKAlt" + objs.VarToNodeID + "_" + workid);

                        generAlertMessage += "@已向:{" + emailAddress + "}发送提醒邮件，由 SendSuccess 发出.";
                    }
                }
                #endregion 发送成功事件.

            }
            #endregion 处理发送邮件.

            #region 处理短信..
            //定义短信内容.
            string smsDocTmp = "";           
            if (this.SMSPushWay != 0)
            {

                #region  生成短信内容  
                smsDocTmp = this.SMSDoc.Clone() as string;
                    smsDocTmp = smsDocTmp.Replace("{Title}", title);
                    smsDocTmp = smsDocTmp.Replace("{Url}", openWorkURl);
                    smsDocTmp = smsDocTmp.Replace("@WebUser.No", WebUser.No);
                    smsDocTmp = smsDocTmp.Replace("@WebUser.Name", WebUser.Name);

                    /*如果仍然有没有替换下来的变量.*/
                    if (smsDocTmp.Contains("@") == true)
                        smsDocTmp = Glo.DealExp(smsDocTmp, en, null);
              
                /*如果仍然有没有替换下来的变量.*/
                    if (smsDocTmp.Contains("@"))
                        smsDocTmp = Glo.DealExp(smsDocTmp, en, null);

                #endregion 处理当前的内容.

                //求发送给的人员ID.
                string toEmpIDs = "";

                #region WorkArrive - 发送成功事件
                if (this.FK_Event == BP.Sys.EventListOfNode.WorkArrive 
                    || this.FK_Event == BP.Sys.EventListOfNode.ReturnAfter)
                {
                    string msgType = "ToDo";
                    if (this.FK_Event == BP.Sys.EventListOfNode.ReturnAfter)
                        msgType = "Return";
                    
                    /*发送成功事件.*/
                    if (this.SMSPushWay == 1)
                    {
                        /*如果向接受人发送短信.*/
                        toEmpIDs = jumpToEmps;
                        string[] emps = toEmpIDs.Split(',');
                        foreach (string emp in emps)
                        {
                            if (string.IsNullOrEmpty(emp))
                                continue;

                            string smsDocTmpReal = smsDocTmp.Clone() as string;
                            smsDocTmpReal = smsDocTmpReal.Replace("{EmpStr}", emp);
                            BP.WF.Port.WFEmp empEn = new Port.WFEmp(emp);

                            //发送短信.
                            Dev2Interface.Port_SendSMS(empEn.Tel, smsDocTmpReal, msgType, "WKAlt" + currNode.NodeID + "_" + workid, BP.Web.WebUser.No, null, emp, null);
                        }
                        generAlertMessage += "@已向:{" + toEmpIDs + "}发送提醒手机短信，由 " + this.FK_Event + " 发出.";
                    }

                    if (this.MailPushWay == 2)
                    {
                        /*如果向指定的字段作为发送邮件的对象, 从字段里取数据. */
                        string tel = r[this.SMSField] as string;
                        //发送短信.

                        BP.WF.Dev2Interface.Port_SendSMS(tel, smsDocTmp, msgType, "WKAlt" + currNode.NodeID + "_" + workid);
                        generAlertMessage += "@已向:{" + tel + "}发送提醒手机短信，由 " + this.FK_Event + " 发出.";
                    }
                }
                #endregion WorkArrive - 工作到达事件


                #region SendSuccess - 发送成功事件
                if (this.FK_Event == BP.Sys.EventListOfNode.SendSuccess)
                {
                    /*发送成功事件.*/
                    if (this.SMSPushWay == 1 && objs.VarAcceptersID != null )
                    {
                        /*如果向接受人发送短信.*/
                        toEmpIDs = objs.VarAcceptersID;
                        string[] emps = toEmpIDs.Split(',');
                        foreach (string emp in emps)
                        {
                            if (string.IsNullOrEmpty(emp))
                                continue;

                            string smsDocTmpReal = smsDocTmp.Clone() as string;
                            smsDocTmpReal = smsDocTmpReal.Replace("{EmpStr}", emp);

                            BP.WF.Port.WFEmp empEn = new Port.WFEmp(emp);

                            //发送短信.
                            Dev2Interface.Port_SendSMS(empEn.Tel, smsDocTmpReal, "ToDo", "WKAlt" + objs.VarToNodeID + "_" + workid, BP.Web.WebUser.No, null, emp, null);
                        }
                        generAlertMessage += "@已向:{" + toEmpIDs + "}发送提醒手机短信，由 SendSuccess 发出.";
                    }

                    if (this.MailPushWay == 2)
                    {
                        /*如果向指定的字段作为发送邮件的对象, 从字段里取数据. */
                        string tel = r[this.SMSField] as string;
                        if (tel != null ||  tel.Length > 6)
                        {
                            //发送短信.
                            BP.WF.Dev2Interface.Port_SendSMS(tel, smsDocTmp, "ToDo", "WKAlt" + objs.VarToNodeID + "_" + workid);
                            generAlertMessage += "@已向:{" + tel + "}发送提醒手机短信，由 SendSuccess 发出.";
                        }
                    }
                }
                #endregion SendSuccess - 发送成功事件

            }
            #endregion 处理短信.

            return generAlertMessage;
        }

        protected override bool beforeUpdateInsertAction()
        {
           //  this.MyPK = this.FK_Event + "_" + this.FK_Node + "_" + this.PushWay;
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	/// 消息推送
	/// </summary>
    public class PushMsgs : EntitiesMyPK
    {
        /// <summary>
        /// 消息推送
        /// </summary>
        public PushMsgs() { }
        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="fk_flow"></param>
        public PushMsgs(string fk_flow)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereInSQL(PushMsgAttr.FK_Node, "SELECT NodeID FROM WF_Node WHERE FK_Flow='" + fk_flow + "'");
            qo.DoQuery();
        }
        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="nodeid">节点ID</param>
        public PushMsgs(int nodeid)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(PushMsgAttr.FK_Node, nodeid);
            qo.DoQuery();
        }
        /// <summary>
        /// 得到它的 Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new PushMsg();
            }
        }
        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<PushMsg> ToJavaList()
        {
            return (System.Collections.Generic.IList<PushMsg>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<PushMsg> Tolist()
        {
            System.Collections.Generic.List<PushMsg> list = new System.Collections.Generic.List<PushMsg>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((PushMsg)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
    }
}
