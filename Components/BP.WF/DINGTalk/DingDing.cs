using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using BP.WF;
using BP.Tools;
using BP.WF.WXin;
using BP.WF.DINGTalk;
using BP.GPM;
using BP.Sys;
using BP.WF.Data;
using BP.En;
using System.Collections;

namespace BP.WF
{
    /// <summary>
    /// 钉钉主类
    /// </summary>
    public class DingDing
    {
        private string corpid = BP.Sys.SystemConfig.Ding_CorpID;
        private string corpsecret = BP.Sys.SystemConfig.Ding_CorpSecret;

        public string getAccessToken()
        {
            string accessToken = string.Empty;
            string url = "https://oapi.dingtalk.com/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                AccessToken AT = new AccessToken();
                AT = FormatToJson.ParseFromJson<AccessToken>(str);
                if (AT != null)
                {
                    accessToken = AT.access_token;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return accessToken;
        }

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="code"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public string GetUserID(string code)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/user/getuserinfo?access_token=" + access_token + "&code=" + code;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                CreateUser_PostVal user = new CreateUser_PostVal();
                user = FormatToJson.ParseFromJson<CreateUser_PostVal>(str);
                //BP.DA.Log.DefaultLogWriteLineError(access_token + "code:" + code + "1." + user.userid + "2." + user.errcode + "3." + user.errmsg);
                if (!string.IsNullOrEmpty(user.userid))
                    return user.userid;
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
                return ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 同步钉钉通讯录到CCGPM
        /// </summary>
        /// <returns></returns>
        public bool AnsyOrgToCCGPM()
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/department/list?access_token=" + access_token;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                DepartMent_List departMentList = FormatToJson.ParseFromJson<DepartMent_List>(str);
                //部门集合
                if (departMentList != null && departMentList.department != null && departMentList.department.Count > 0)
                {
                    //删除旧数据
                    ClearOrg_Old();
                    //获取根部门
                    DepartMentDetailInfo rootDepartMent = new DepartMentDetailInfo();
                    foreach (DepartMentDetailInfo deptMenInfo in departMentList.department)
                    {
                        if (deptMenInfo.id == "1")
                        {
                            rootDepartMent = deptMenInfo;
                            break;
                        }
                    }
                    //增加跟部门
                    int deptIdx = 0;
                    Dept rootDept = new Dept();
                    rootDept.No = rootDepartMent.id;
                    rootDept.Name = rootDepartMent.name;
                    rootDept.ParentNo = "0";
                    rootDept.Idx = deptIdx;
                    rootDept.DirectInsert();


                    //部门信息
                    foreach (DepartMentDetailInfo deptMentInfo in departMentList.department)
                    {
                        //增加部门,排除根目录
                        if (deptMentInfo.id != "1")
                        {
                            Dept dept = new Dept();
                            dept.No = deptMentInfo.id;
                            dept.Name = deptMentInfo.name;
                            dept.ParentNo = deptMentInfo.parentid;
                            dept.Idx = deptIdx++;
                            dept.DirectInsert();
                        }

                        //部门人员
                        DepartMentUser_List userList = GenerDeptUser_List(access_token, deptMentInfo.id);
                        if (userList != null)
                        {
                            foreach (DepartMentUserInfo userInfo in userList.userlist)
                            {
                                Emp emp = new Emp();
                                DeptEmp deptEmp = new DeptEmp();
                                //如果账户存在则人员信息不添加，添加关联表
                                if (emp.IsExit(EmpAttr.No, userInfo.userid) == true)
                                {
                                    deptEmp.MyPK = deptMentInfo.id + "_" + emp.No;
                                    deptEmp.FK_Dept = deptMentInfo.id;
                                    deptEmp.FK_Emp = emp.No;
                                    deptEmp.DirectInsert();
                                    continue;
                                }

                                //增加人员
                                emp.No = userInfo.userid;
                                emp.EmpNo = userInfo.jobnumber;
                                emp.Name = userInfo.name;
                                emp.FK_Dept = deptMentInfo.id;
                                emp.Tel = userInfo.mobile;
                                emp.Email = userInfo.email;
                                //emp.Idx = string.IsNullOrEmpty(userInfo.order) == true ? 0 : Int32.Parse(userInfo.order);
                                emp.DirectInsert();

                                //增加人员与部门对应表
                                deptEmp.MyPK = deptMentInfo.id + "_" + emp.No;
                                deptEmp.FK_Dept = deptMentInfo.id;
                                deptEmp.FK_Emp = emp.No;
                                deptEmp.DirectInsert();
                            }
                        }
                    }

                    #region 处理部门名称全程
                    BP.WF.DTS.OrgInit_NameOfPath nameOfPath = new DTS.OrgInit_NameOfPath();
                    if (nameOfPath.IsCanDo)
                        nameOfPath.Do();
                    #endregion

                    return true;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 获取部门下的人员
        /// </summary>
        /// <returns></returns>
        private DepartMentUser_List GenerDeptUser_List(string access_token, string department_id)
        {
            string url = "https://oapi.dingtalk.com/user/list?access_token=" + access_token + "&department_id=" + department_id;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                DepartMentUser_List departMentUserList = FormatToJson.ParseFromJson<DepartMentUser_List>(str);

                //部门人员集合
                if (departMentUserList != null && departMentUserList.userlist != null && departMentUserList.userlist.Count > 0)
                    return departMentUserList;
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }

        #region 组织结构同步
        /// <summary>
        /// 钉钉，新增部门同步钉钉
        /// </summary>
        /// <param name="dept">部门基本信息</param>
        /// <returns></returns>
        public CreateDepartMent_PostVal GPM_Ding_CreateDept(Dept dept)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/department/create?access_token=" + access_token;
            try
            {
                IDictionary<string, object> list = new Dictionary<string, object>();
                list.Add("name", dept.Name);
                list.Add("parentid", dept.ParentNo);
                //list.Add("order", "1");
                list.Add("createDeptGroup", "true");

                string str = BP.Tools.FormatToJson.ToJson_FromDictionary(list);
                str = new HttpWebResponseUtility().HttpResponsePost_Json(url, str);
                CreateDepartMent_PostVal postVal = FormatToJson.ParseFromJson<CreateDepartMent_PostVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    if (postVal.errcode != "0")
                        BP.DA.Log.DefaultLogWriteLineError("钉钉新增部门失败：" + postVal.errcode + "-" + postVal.errmsg);

                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 钉钉，编辑部门同步钉钉
        /// </summary>
        /// <param name="dept">部门基本信息</param>
        /// <returns></returns>
        public Ding_Post_ReturnVal GPM_Ding_EditDept(Dept dept)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/department/update?access_token=" + access_token;
            try
            {
                IDictionary<string, object> list = new Dictionary<string, object>();
                list.Add("id", dept.No);
                list.Add("name", dept.Name);
                //根目录不允许修改
                if (dept.No != "1")
                {
                    list.Add("parentid", dept.ParentNo);
                }
                //大于零才可以
                if (dept.Idx > 0)
                {
                    list.Add("order", dept.Idx);
                }
                string str = BP.Tools.FormatToJson.ToJson_FromDictionary(list);
                str = new HttpWebResponseUtility().HttpResponsePost_Json(url, str);
                Ding_Post_ReturnVal postVal = FormatToJson.ParseFromJson<Ding_Post_ReturnVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    if (postVal.errcode != "0")
                        BP.DA.Log.DefaultLogWriteLineError("钉钉修改部门失败：" + postVal.errcode + "-" + postVal.errmsg);

                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 钉钉，删除部门同步钉钉
        /// </summary>
        /// <param name="deptId">部门编号</param>
        /// <returns></returns>
        public Ding_Post_ReturnVal GPM_Ding_DeleteDept(string deptId)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/department/delete?access_token=" + access_token + "&id=" + deptId;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                Ding_Post_ReturnVal postVal = FormatToJson.ParseFromJson<Ding_Post_ReturnVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    if (postVal.errcode != "0")
                        BP.DA.Log.DefaultLogWriteLineError("钉钉删除部门失败：" + postVal.errcode + "-" + postVal.errmsg);

                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 钉钉，新增人员同步钉钉
        /// </summary>
        /// <param name="emp">部门基本信息</param>
        /// <returns></returns>
        public CreateUser_PostVal GPM_Ding_CreateEmp(Emp emp)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/user/create?access_token=" + access_token;
            try
            {
                IDictionary<string, object> list = new Dictionary<string, object>();
                //如果用户编号存在则按照此账号进行新建
                if (!(string.IsNullOrEmpty(emp.No) || string.IsNullOrWhiteSpace(emp.No)))
                {
                    list.Add("userid", emp.No);
                }
                list.Add("name", emp.Name);
                //部门数组
                List<string> listArrary = new List<string>();
                listArrary.Add(emp.FK_Dept);

                list.Add("department", listArrary);
                list.Add("mobile", emp.Tel);
                list.Add("email", emp.Email);
                list.Add("jobnumber", emp.EmpNo);
                list.Add("position", emp.FK_DutyText);

                string str = BP.Tools.FormatToJson.ToJson_FromDictionary(list);
                str = new HttpWebResponseUtility().HttpResponsePost_Json(url, str);
                CreateUser_PostVal postVal = FormatToJson.ParseFromJson<CreateUser_PostVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    if (postVal.errcode != "0")
                    {
                        //在钉钉通讯录已经存在
                        if (postVal.errcode == "60102") postVal.userid = emp.No;
                        BP.DA.Log.DefaultLogWriteLineError("钉钉新增人员失败：" + postVal.errcode + "-" + postVal.errmsg);
                    }
                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 钉钉，编辑人员同步钉钉
        /// </summary>
        /// <param name="emp">部门基本信息</param>
        /// <returns></returns>
        public Ding_Post_ReturnVal GPM_Ding_EditEmp(Emp emp, List<string> deptIds = null)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/user/update?access_token=" + access_token;
            try
            {
                IDictionary<string, object> list = new Dictionary<string, object>();
                list.Add("userid", emp.No);
                list.Add("name", emp.Name);
                list.Add("email", emp.Email);
                list.Add("jobnumber", emp.EmpNo);
                list.Add("mobile", emp.Tel);
                list.Add("position", emp.FK_DutyText);
                //钉钉根据此从其他部门删除或增加到其他部门
                if (deptIds != null && deptIds.Count > 0)
                {
                    list.Add("department", deptIds);
                }
                string str = BP.Tools.FormatToJson.ToJson_FromDictionary(list);
                str = new HttpWebResponseUtility().HttpResponsePost_Json(url, str);
                Ding_Post_ReturnVal postVal = FormatToJson.ParseFromJson<Ding_Post_ReturnVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    bool create_Ding_user = false;
                    //40022企业中的手机号码和登陆钉钉的手机号码不一致,暂时不支持修改用户信息,可以删除后重新添加
                    if (postVal.errcode == "40022" || postVal.errcode == "40021")
                    {
                        create_Ding_user = true;
                        postVal = GPM_Ding_DeleteEmp(emp.No);
                        //删除失败
                        if (postVal.errcode != "0")
                            create_Ding_user = false;
                    }
                    else if (postVal.errcode == "60121")//60121找不到该用户
                    {
                        create_Ding_user = true;
                    }

                    //需要新增人员
                    if (create_Ding_user == true)
                    {
                        CreateUser_PostVal postUserVal = GPM_Ding_CreateEmp(emp);
                        //消息传递
                        postVal.errcode = postUserVal.errcode;
                        postVal.errmsg = postUserVal.errmsg;
                    }

                    if (postVal.errcode != "0")
                    {
                        BP.DA.Log.DefaultLogWriteLineError("钉钉修改人员失败：" + postVal.errcode + "-" + postVal.errmsg);
                    }
                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 钉钉，删除人员同步钉钉
        /// </summary>
        /// <param name="userid">人员编号</param>
        /// <returns></returns>
        public Ding_Post_ReturnVal GPM_Ding_DeleteEmp(string userid)
        {
            string access_token = getAccessToken();
            string url = "https://oapi.dingtalk.com/user/delete?access_token=" + access_token + "&userid=" + userid;
            try
            {
                string str = new HttpWebResponseUtility().HttpResponseGet(url);
                Ding_Post_ReturnVal postVal = FormatToJson.ParseFromJson<Ding_Post_ReturnVal>(str);

                //请求返回信息
                if (postVal != null)
                {
                    if (postVal.errcode != "0")
                        BP.DA.Log.DefaultLogWriteLineError("钉钉删除人员失败：" + postVal.errcode + "-" + postVal.errmsg);

                    return postVal;
                }
            }
            catch (Exception ex)
            {
                BP.DA.Log.DefaultLogWriteLineError(ex.Message);
            }
            return null;
        }
        #endregion

        /// <summary>
        /// 清空组织结构
        /// </summary>
        private void ClearOrg_Old()
        {
            //人员
            BP.DA.DBAccess.RunSQL("DELETE FROM Port_Emp");
            //部门
            BP.DA.DBAccess.RunSQL("DELETE FROM Port_Dept");
            //部门人员
            BP.DA.DBAccess.RunSQL("DELETE FROM Port_DeptEmp");
            //部门人员岗位
            BP.DA.DBAccess.RunSQL("DELETE FROM Port_DeptEmpStation");
            //admin 是必须存在的
            Emp emp = new Emp();
            emp.No = "admin";
            emp.Pass = "pub";
            emp.Name = "管理员";
            emp.FK_Dept = "1";
            emp.DirectInsert();
            //部门人员关联表
            DeptEmp deptEmp = new DeptEmp();
            deptEmp.FK_Dept = "1";
            deptEmp.FK_Emp = "admin";
            deptEmp.DutyLevel = 0;
            deptEmp.DirectInsert();
        }

        public Ding_Post_ReturnVal Ding_SendWorkMessage(DingMsgType msgType, long WorkID, string sender)
        {
            //主业务表
            GenerWorkFlow workFlow = new GenerWorkFlow(WorkID);
            //结束不发送消息
            if (workFlow.WFState == WFState.Complete)
                return null;
            //判断节点类型，分合流等
            Node node = new Node(workFlow.FK_Node);

            Monitors empWorks = new Monitors();
            QueryObject obj = new QueryObject(empWorks);
            obj.AddWhere(MonitorAttr.WorkID, WorkID);
            obj.addOr();
            obj.AddWhere(MonitorAttr.FID, WorkID);
            obj.DoQuery();
            string toUsers = "";
            foreach (Monitor empWork in empWorks)
            {
                if (toUsers.Length > 0)
                    toUsers += "|";
                toUsers += empWork.FK_Emp;
            }
            if (toUsers.Length == 0)
                return null;

            switch (msgType)
            {
                case DingMsgType.text:
                    Ding_Msg_Text msgText = new Ding_Msg_Text();
                    msgText.Access_Token = getAccessToken();
                    msgText.agentid = SystemConfig.Ding_AgentID;
                    msgText.touser = toUsers;
                    msgText.content = workFlow.Title + "\n发送人：" + sender + "\n时间：" + BP.DA.DataType.CurrentDataTimeCNOfShort;
                    return DingTalk_Message.Msg_AgentText_Send(msgText);
                    break;
                case DingMsgType.link:
                    Ding_Msg_Link msgLink = new Ding_Msg_Link();
                    msgLink.Access_Token = getAccessToken();
                    msgLink.touser = toUsers;
                    msgLink.agentid = SystemConfig.Ding_AgentID;
                    msgLink.messageUrl = SystemConfig.Ding_MessageUrl + "/CCMobile/login.aspx";
                    msgLink.picUrl = "@lALOACZwe2Rk";
                    msgLink.title = workFlow.Title;
                    msgLink.text = "发送人：" + sender + "\n时间：" + BP.DA.DataType.CurrentDataTimeCNOfShort;
                    return DingTalk_Message.Msg_AgentLink_Send(msgLink);
                    break;
                case DingMsgType.OA:
                    string[] users = toUsers.Split('|');
                    string faildSend = "";
                    Ding_Post_ReturnVal postVal = null;
                    foreach (string user in users)
                    {
                        Ding_Msg_OA msgOA = new Ding_Msg_OA();
                        msgOA.Access_Token = getAccessToken();
                        msgOA.agentid = SystemConfig.Ding_AgentID;
                        msgOA.touser = user;
                        msgOA.messageUrl = SystemConfig.Ding_MessageUrl + "/CCMobile/DingAction.aspx?ActionFrom=message&UserID=" + user
                            + "&ActionType=ToDo&FK_Flow=" + workFlow.FK_Flow + "&FK_Node=" + workFlow.FK_Node
                            + "&WorkID=" + workFlow.WorkID + "&FID=" + workFlow.FID;
                        //00是完全透明，ff是完全不透明，比较适中的透明度值是 1e
                        msgOA.head_bgcolor = "FFBBBBBB";
                        msgOA.head_text = "审批";
                        msgOA.body_title = workFlow.Title;
                        Hashtable hs = new Hashtable();
                        hs.Add("流程名", workFlow.FlowName);
                        hs.Add("当前节点", workFlow.NodeName);
                        hs.Add("申请人", workFlow.StarterName);
                        hs.Add("申请时间", workFlow.RDT);
                        msgOA.body_form = hs;
                        msgOA.body_author = sender;
                        postVal = DingTalk_Message.Msg_OAText_Send(msgOA);
                        if (postVal.errcode != "0")
                        {
                            if (faildSend.Length > 0)
                                faildSend += ",";
                            faildSend += user;
                        }
                    }
                    //有失败消息
                    if (faildSend.Length > 0)
                    {
                        postVal.errcode = "500";
                        postVal.errmsg = faildSend + "消息发送失败";
                    }
                    return postVal;
                    break;
            }
            return null;
        }
    }
}
