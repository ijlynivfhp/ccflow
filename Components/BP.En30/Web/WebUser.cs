using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using System.Web;
using System.Data;
using BP.En;
using BP.DA;
using System.Configuration;
using BP.Port;
using BP.Sys;


namespace BP.Web
{
    /// <summary>
    /// 用户工作设备
    /// </summary>
    public enum UserWorkDev
    {
        PC,
        Mobile,
        TablePC
    }
    /// <summary>
    /// User 的摘要说明。
    /// </summary>
    public class WebUser
    {
        /// <summary>
        /// 密码解密
        /// </summary>
        /// <param name="pass">用户输入密码</param>
        /// <returns>解密后的密码</returns>
        public static string ParsePass(string pass)
        {
            if (pass == "")
                return "";

            string str = "";
            char[] mychars = pass.ToCharArray();
            int i = 0;
            foreach (char c in mychars)
            {
                i++;

                //step 1 
                long A = Convert.ToInt64(c) * 2;

                // step 2
                long B = A - i * i;

                // step 3 
                long C = 0;
                if (B > 196)
                    C = 196;
                else
                    C = B;

                str = str + Convert.ToChar(C).ToString();
            }
            return str;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="em"></param>
        public static void SignInOfGener(Emp em)
        {
            SignInOfGener(em, "CH", null, true, false);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="em"></param>
        /// <param name="isRememberMe"></param>
        public static void SignInOfGener(Emp em, bool isRememberMe)
        {
            SignInOfGener(em, "CH", null, isRememberMe, false);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="em"></param>
        /// <param name="auth"></param>
        public static void SignInOfGenerAuth(Emp em, string auth)
        {
            SignInOfGener(em, "CH", auth, true, false);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="em"></param>
        /// <param name="lang"></param>
        public static void SignInOfGenerLang(Emp em, string lang, bool isRememberMe)
        {
            SignInOfGener(em, lang, null, isRememberMe, false);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="em"></param>
        /// <param name="lang"></param>
        public static void SignInOfGenerLang(Emp em, string lang)
        {
            SignInOfGener(em, lang, null, true, false);
        }
        public static void SignInOfGener(Emp em, string lang)
        {
            SignInOfGener(em, lang, em.No, true, false);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="em">登录人</param>
        /// <param name="lang">语言</param>
        /// <param name="auth">被授权登录人</param>
        /// <param name="isRememberMe">是否记忆我</param>
        public static void SignInOfGener(Emp em, string lang, string auth, bool isRememberMe)
        {
            SignInOfGener(em, lang, auth, isRememberMe, false);
        }
        /// <summary>
        /// 通用的登陆
        /// </summary>
        /// <param name="em">人员</param>
        /// <param name="lang">语言</param>
        /// <param name="auth">授权人</param>
        /// <param name="isRememberMe">是否记录cookies</param>
        /// <param name="IsRecSID">是否记录SID</param>
        public static void SignInOfGener(Emp em, string lang, string auth, bool isRememberMe, bool IsRecSID)
        {
            if (System.Web.HttpContext.Current == null)
                SystemConfig.IsBSsystem = false;
            else
                SystemConfig.IsBSsystem = true;

            if (SystemConfig.IsBSsystem)
                BP.Sys.Glo.WriteUserLog("SignIn", em.No, "登录");

            WebUser.No = em.No;
            WebUser.Name = em.Name;
            WebUser.Auth = auth;

            //登录模式？
            BP.Web.WebUser.UserWorkDev = Web.UserWorkDev.PC;

            #region 解决部门的问题.
            if (BP.Sys.SystemConfig.OSDBSrc == OSDBSrc.Database)
            {
                if (string.IsNullOrEmpty(em.FK_Dept) == true)
                {
                    string sql = "SELECT FK_Dept FROM Port_EmpDept WHERE FK_Emp='" + em.No + "'";
                    string deptNo = BP.DA.DBAccess.RunSQLReturnString(sql);
                    if (string.IsNullOrEmpty(deptNo) == true)
                    {
                        throw new Exception("@登录人员(" + em.No + "," + em.Name + ")没有维护部门...");
                    }
                    else
                    {
                        Dept mydept = new Dept(deptNo);
                        em.FK_Dept = mydept.No;
                        em.Update();
                    }
                }

                BP.Port.Dept dept = new Dept();
                dept.No = em.FK_Dept;
                if (dept.RetrieveFromDBSources() == 0)
                    throw new Exception("@登录人员(" + em.No + "," + em.Name + ")没有维护部门.");
            }


            if (BP.Sys.SystemConfig.OSDBSrc == OSDBSrc.WebServices)
            {
                var ws = DataType.GetPortalInterfaceSoapClientInstance();
                DataTable dt = ws.GetEmpHisDepts(em.No);
                string strs = BP.DA.DBAccess.GenerWhereInPKsString(dt);
                Paras ps = new Paras();
                ps.SQL = "UPDATE WF_Emp SET Depts=" + SystemConfig.AppCenterDBVarStr + "Depts WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
                ps.Add("Depts", strs);
                ps.Add("No", em.No);
                BP.DA.DBAccess.RunSQL(ps);
                WebUser.HisDeptsStr= strs;

                dt = ws.GetEmpHisStations(em.No);
                strs = BP.DA.DBAccess.GenerWhereInPKsString(dt);
                ps = new Paras();
                ps.SQL = "UPDATE WF_Emp SET Stas=" + SystemConfig.AppCenterDBVarStr + "Stas WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
                ps.Add("Stas", strs);
                ps.Add("No", em.No);
                BP.DA.DBAccess.RunSQL(ps);
                WebUser.HisStationsStr = strs;
            }

            #endregion 解决部门的问题.

            WebUser.FK_Dept = em.FK_Dept;
            WebUser.FK_DeptName = em.FK_DeptText;
            WebUser.HisDepts = null;
            WebUser.HisStations = null;
            if (IsRecSID)
            {
                /*如果记录sid*/
                string sid1 = DateTime.Now.ToString("MMddHHmmss");
                DBAccess.RunSQL("UPDATE Port_Emp SET SID='" + sid1 + "' WHERE No='" + WebUser.No + "'");
                WebUser.SID = sid1;
            }
             

            WebUser.SysLang = lang;
            if (BP.Sys.SystemConfig.IsBSsystem)
            {
                //System.Web.HttpContext.Current.Response.Cookies.Clear();

                HttpCookie hc = BP.Sys.Glo.Request.Cookies["CCS"];
                if (hc != null)
                    BP.Sys.Glo.Request.Cookies.Remove("CCS");

                HttpCookie cookie = new HttpCookie("CCS");
                cookie.Expires = DateTime.Now.AddDays(2);
                cookie.Values.Add("No", em.No);
                cookie.Values.Add("Name", HttpUtility.UrlEncode(em.Name));


                if (isRememberMe)
                    cookie.Values.Add("IsRememberMe", "1");
                else
                    cookie.Values.Add("IsRememberMe", "0");

                cookie.Values.Add("FK_Dept", em.FK_Dept);
                cookie.Values.Add("FK_DeptName", HttpUtility.UrlEncode(em.FK_DeptText));

                if (System.Web.HttpContext.Current.Session != null)
                {
                    cookie.Values.Add("Token", System.Web.HttpContext.Current.Session.SessionID);
                    cookie.Values.Add("SID", System.Web.HttpContext.Current.Session.SessionID);
                }

                cookie.Values.Add("Lang", lang);

                //string isEnableStyle = SystemConfig.AppSettings["IsEnableStyle"];
                //if (isEnableStyle == "1")
                //{
                //    try
                //    {
                //        string sql = "SELECT Style FROM WF_Emp WHERE No='" + em.No + "' ";
                //        int val = DBAccess.RunSQLReturnValInt(sql, 0);
                //        cookie.Values.Add("Style", val.ToString());
                //        WebUser.Style = val.ToString();
                //    }
                //    catch
                //    {
                //    }
                //}
                if (auth == null)
                    auth = "";
                cookie.Values.Add("Auth", auth); //授权人.
                System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        #region 静态方法
        /// <summary>
        /// 通过key,取出session.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="isNullAsVal">如果是Null, 返回的值.</param>
        /// <returns></returns>
        public static string GetSessionByKey(string key, string isNullAsVal)
        {
            if (IsBSMode && System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                string str = System.Web.HttpContext.Current.Session[key] as string;
                if (string.IsNullOrEmpty(str))
                    str = isNullAsVal;
                return str;
            }
            else
            {
                if (BP.Port.Current.Session[key] == null || BP.Port.Current.Session[key].ToString() == "")
                {
                    BP.Port.Current.Session[key] = isNullAsVal;
                    return isNullAsVal;
                }
                else
                    return (string)BP.Port.Current.Session[key];
            }
        }
        public static object GetObjByKey(string key)
        {
            if (IsBSMode)
            {
                return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                return BP.Port.Current.Session[key];
            }
        }
        #endregion

        /// <summary>
        /// 是不是b/s 工作模式。
        /// </summary>
        protected static bool IsBSMode
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                    return false;
                else
                    return true;
            }
        }
        public static object GetSessionByKey(string key, Object defaultObjVal)
        {
            if (IsBSMode == true
                && System.Web.HttpContext.Current != null
                && System.Web.HttpContext.Current.Session != null)
            {
                if (System.Web.HttpContext.Current.Session[key] == null)
                    return defaultObjVal;
                else
                    return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                if (BP.Port.Current.Session[key] == null)
                    return defaultObjVal;
                else
                    return BP.Port.Current.Session[key];
            }
        }
        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        public static void SetSessionByKey(string key, object val)
        {
            if (val == null)
                return;

            if (IsBSMode == true
                && System.Web.HttpContext.Current != null
                && System.Web.HttpContext.Current.Session != null)
                System.Web.HttpContext.Current.Session[key] = val;
            else
                BP.Port.Current.SetSession(key, val);
        }
        /// <summary>
        /// 退回
        /// </summary>
        public static void Exit()
        {
            if (IsBSMode == false)
            {
                try
                {
                    string token = WebUser.Token;
                    System.Web.HttpContext.Current.Response.Cookies.Clear();
                    BP.Sys.Glo.Request.Cookies.Clear();
                    HttpCookie cookie = new HttpCookie("CCS", string.Empty);
                    cookie.Expires = DateTime.Now.AddDays(2);
                    cookie.Values.Add("No", string.Empty);
                    cookie.Values.Add("Name", string.Empty);
                    // 2013.06.07 H
                    cookie.Values.Add("Pass", string.Empty);
                    cookie.Values.Add("IsRememberMe", "0");
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    WebUser.Token = token;
                    BP.Port.Current.Session.Clear();
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    string token = WebUser.Token;
                    System.Web.HttpContext.Current.Response.Cookies.Clear();
                    BP.Sys.Glo.Request.Cookies.Clear();


                    System.Web.HttpContext.Current.Session.Clear();

                    HttpCookie cookie = new HttpCookie("CCS", string.Empty);
                    cookie.Expires = DateTime.Now.AddDays(2);
                    cookie.Values.Add("No", string.Empty);
                    cookie.Values.Add("Name", string.Empty);
                    // 2013.06.07 H
                    cookie.Values.Add("Pass", string.Empty);
                    cookie.Values.Add("IsRememberMe", "0");
                    cookie.Values.Add("Auth", string.Empty); //授权人.
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    WebUser.Token = token;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 授权人
        /// </summary>
        public static string Auth
        {
            get
            {
                string val = GetValFromCookie("Auth", null, false);
                if (val == null)
                    val = GetSessionByKey("Auth", null);
                return val;
            }
            set
            {
                if (value == "")
                    SetSessionByKey("Auth", null);
                else
                    SetSessionByKey("Auth", value);
            }
        }
        public static string FK_DeptName
        {
            get
            {
                try
                {
                    string val = GetValFromCookie("FK_DeptName", null, true);
                    return val;
                }
                catch
                {
                    return "无";
                }
            }
            set
            {
                SetSessionByKey("FK_DeptName", value);
            }
        }
        /// <summary>
        /// 部门全称
        /// </summary>
        public static string FK_DeptNameOfFull
        {
            get
            {
                string val = GetValFromCookie("FK_DeptNameOfFull", null, true);
                if (string.IsNullOrEmpty(val))
                {
                    try
                    {
                        val = DBAccess.RunSQLReturnStringIsNull("SELECT NameOfFull FROM Port_Dept WHERE No='" + WebUser.FK_Dept + "'", null);
                        return val;
                    }
                    catch
                    {
                        val = WebUser.FK_DeptName;
                    }

                    //给它赋值.
                    FK_DeptNameOfFull = val;
                }
                return val;
            }
            set
            {
                SetSessionByKey("FK_DeptNameOfFull", value);
            }
        }
        public static string SysLang
        {
            get
            {
                return "CH";
                string no = GetSessionByKey("Lang", null);
                if (no == null || no == "")
                {
                    if (IsBSMode)
                    {
                        HttpCookie hc1 = BP.Sys.Glo.Request.Cookies["CCS"];
                        if (hc1 == null)
                            return "CH";
                        SetSessionByKey("Lang", hc1.Values["Lang"]);
                    }
                    else
                    {
                        return "CH";
                    }
                    return GetSessionByKey("Lang", "CH");
                }
                else
                {
                    return no;
                }
            }
            set
            {
                SetSessionByKey("Lang", value);
            }
        }
        /// <summary>
        /// sessionID
        /// </summary>
        public static string NoOfSessionID
        {
            get
            {
                string s = GetSessionByKey("No", null);
                if (s == null)
                    return System.Web.HttpContext.Current.Session.SessionID;
                return s;
            }
        }
        /// <summary>
        /// FK_Dept
        /// </summary>
        public static string FK_Dept
        {
            get
            {
                string val = GetValFromCookie("FK_Dept", null, false);
                if (val == null)
                {
                    if (WebUser.No == null)
                        throw new Exception("@登录信息丢失，请你确认是否启用了cookie? ");

                    string sql = "SELECT FK_Dept FROM Port_Emp WHERE No='"+WebUser.No+"'";
                    string dept = BP.DA.DBAccess.RunSQLReturnStringIsNull(sql, null);
                    if (dept == null)
                    {
                        sql = "SELECT FK_Dept FROM Port_EmpDept WHERE FK_Emp='" + WebUser.No + "'";
                        dept = BP.DA.DBAccess.RunSQLReturnStringIsNull(sql, null);
                    }

                    if (dept == null)
                        throw new Exception("@err-003 FK_Dept，当前登录人员("+WebUser.No+")，没有设置部门。");

                    SetSessionByKey("FK_Dept", dept);
                    return dept;
                }
                return val;
            }
            set
            {
                SetSessionByKey("FK_Dept", value);
            }
        }
        /// <summary>
        /// 当前登录人员的父节点编号
        /// </summary>
        public static string DeptParentNo
        {
            get
            {
                string val = GetValFromCookie("DeptParentNo", null, false);
                if (val == null)
                    throw new Exception("@err-001 DeptParentNo 登陆信息丢失。");
                return val;
            }
            set
            {
                SetSessionByKey("DeptParentNo", value);
            }
        }
        /// <summary>
        /// 检查权限控制
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool CheckSID(string userNo, string sid)
        {
            if (BP.Sys.SystemConfig.OSDBSrc == OSDBSrc.WebServices)
                return true;

            string mysid = DBAccess.RunSQLReturnStringIsNull("SELECT SID FROM Port_Emp WHERE No='" + userNo + "'", null);
            if (sid == mysid)
                return true;
            else
                return false;
        }
        public static string NoOfRel
        {
            get
            {
                return GetSessionByKey("No", null);
            }
        }
        public static string GetValFromCookie(string valKey, string isNullAsVal, bool isChinese)
        {
            if (IsBSMode == false)
                return BP.Port.Current.GetSessionStr(valKey, isNullAsVal);

            try
            {
                //先从session里面取.
                string v = System.Web.HttpContext.Current.Session[valKey] as string;
                if (string.IsNullOrEmpty(v) == false)
                    return v;
            }
            catch
            {
            }

            string key = "CCS";
            HttpCookie hc = BP.Sys.Glo.Request.Cookies[key];
            if (hc == null)
                return null;

            try
            {
                string val = null;
                if (isChinese)
                {
                    val = HttpUtility.UrlDecode(hc[valKey]);
                    if (val == null)
                        val = hc.Values[valKey];
                }
                else
                    val = hc.Values[valKey];

                if (string.IsNullOrEmpty(val))
                    return isNullAsVal;
                return val;
            }
            catch
            {
                return isNullAsVal;
            }
            throw new Exception("@err-001 (" + valKey + ")登陆信息丢失。");
        }
        /// <summary>
        /// 编号
        /// </summary>
        public static string No
        {
            get
            {
                string val = GetValFromCookie("No", null, true);
                return val;
                   
                string no = null; // GetSessionByKey("No", null);
                if (no == null || no == "")
                {
                    if (IsBSMode == false)
                        return "admin";
                    //return "admin";
                    //string key = "CCS";
                    string key = "CCS";


                    HttpCookie hc = BP.Sys.Glo.Request.Cookies[key];
                    if (hc == null)
                        return null;

                    if (hc.Values["No"] != null)
                    {
                        WebUser.No = hc["No"];
                        WebUser.FK_Dept = hc["FK_Dept"];
                        WebUser.Auth = hc["Auth"];
                        WebUser.FK_DeptName = HttpUtility.UrlDecode(hc["FK_DeptName"]);
                        WebUser.Name = HttpUtility.UrlDecode(hc["Name"]);

                        //if (BP.Sys.SystemConfig.IsUnit)
                        //{
                        //    WebUser.FK_Unit = HttpUtility.UrlDecode(hc["FK_Unit"]);
                        //    WebUser.FK_UnitName = HttpUtility.UrlDecode(hc["FK_UnitName"]);
                        //}

                        return hc.Values["No"];
                    }
                    throw new Exception("@err-001 No 登陆信息丢失。");
                }
                return no;
            }
            set
            {
                SetSessionByKey("No", value);
            }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public static string Name
        {
            get
            {
                string val = GetValFromCookie("Name", null, true);
                if (val == null)
                    throw new Exception("@err-002 Name 登陆信息丢失。");
                return val;
            }
            set
            {
                SetSessionByKey("Name", value);
            }
        }
        /// <summary>
        /// 域
        /// </summary>
        public static string Domain
        {
            get
            {
                string val = GetValFromCookie("Domain", null, true);
                if (val == null)
                    throw new Exception("@err-003 Domain 登陆信息丢失。");
                return val;
            }
            set
            {
                SetSessionByKey("Domain", value);
            }
        }
        /// <summary>
        /// 令牌
        /// </summary>
        public static string Token
        {
            get
            {

                return GetSessionByKey("token", "null");
            }
            set
            {
                SetSessionByKey("token", value);
            }
        }
        public static string Style
        {
            get
            {
                return GetSessionByKey("Style", "0");
            }
            set
            {
                SetSessionByKey("Style", value);
            }
        }
        /// <summary>
        /// 当前工作人员实体
        /// </summary>
        public static Emp HisEmp
        {
            get
            {
                return new Emp(WebUser.No);
            }
        }
        public static Stations HisStations
        {
            get
            {
                object obj = null;
                obj = GetSessionByKey("HisSts", obj);
                if (obj == null)
                {
                    Stations sts = new Stations();
                    QueryObject qo = new QueryObject(sts);
                    qo.AddWhereInSQL("No", "SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + WebUser.No + "'");
                    qo.DoQuery();
                    SetSessionByKey("HisSts", sts);
                    return sts;
                }
                return obj as Stations;
            }
            set
            {
                SetSessionByKey("HisSts", value);
            }
        }
        public static Depts HisDepts
        {
            get
            {
                object obj = null;
                obj = GetSessionByKey("HisDepts", obj);
                if (obj == null)
                {
                    Depts sts = WebUser.HisEmp.HisDepts;
                    SetSessionByKey("HisDepts", sts);
                    return sts;
                }
                return obj as Depts;
            }
            set
            {
                SetSessionByKey("HisDepts", value);
            }
        }
        /// <summary>
        /// 部门s
        /// </summary>
        public static string HisDeptsStr
        {
            get
            {
                string val = GetValFromCookie("HisDeptsStr", "", true);
                if (val == null)
                {
                    val = BP.DA.DBAccess.RunSQLReturnVal("SELECT Depts FROM WF_Emp WHERE No='" + WebUser.No + "'") as string;
                    if (val == null)
                        val = "";
                    SetSessionByKey("HisDeptsStr", val);
                }
                return val;
            }
            set
            {
                SetSessionByKey("HisDeptsStr", value);

                //Paras ps = new Paras();
                //ps.SQL = "UPDATE WF_Emp SET Depts=" + SystemConfig.AppCenterDBVarStr + "Depts WHERE No=" + SystemConfig.AppCenterDBVarStr + "No ";
                //ps.Add("Depts", value);
                //ps.Add("No", WebUser.NoOfRel);
                //BP.DA.DBAccess.RunSQL(ps);
            }
        }
        /// <summary>
        /// 岗位s
        /// </summary>
        public static string HisStationsStr
        {
            get
            {
                string val = GetValFromCookie("HisStationsStr", null, true);
                if (val == null)
                {
                    val= BP.DA.DBAccess.RunSQLReturnVal("SELECT Stas FROM WF_Emp WHERE No='" + WebUser.No + "'") as string;

                    if (val == null)
                        val = "";
                    SetSessionByKey("HisStationsStr", val);
                }
                return val;
            }
            set
            {
                SetSessionByKey("HisStationsStr", value);
              
            }
        }
        /// <summary>
        /// SID
        /// </summary>
        public static string SID
        {
            get
            {
                string val = GetValFromCookie("SID", null, true);
                if (val == null)
                    return "";
                return val;
            }
            set
            {
                SetSessionByKey("SID", value);
            }
        }
        /// <summary>
        /// 设置SID
        /// </summary>
        /// <param name="sid"></param>
        public static void SetSID(string sid)
        {
            Paras ps = new Paras();
            ps.SQL = "UPDATE Port_Emp SET SID=" + SystemConfig.AppCenterDBVarStr + "SID WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("SID", sid);
            ps.Add("No", WebUser.No);
            BP.DA.DBAccess.RunSQL(ps);
            WebUser.SID = sid;
        }
        /// <summary>
        /// 是否是授权状态
        /// </summary> 
        public static bool IsAuthorize
        {
            get
            {
                if (Auth == null || Auth == "")
                    return false;
                return true;
            }
        }
        /// <summary>
        /// 使用授权人ID
        /// </summary>
        public static string AuthorizerEmpID
        {
            get
            {
                return (string)GetSessionByKey("AuthorizerEmpID", null);

            }
            set
            {
                SetSessionByKey("AuthorizerEmpID", value);
            }
        }
        /// <summary>
        /// 用户工作方式.
        /// </summary>
        public static UserWorkDev UserWorkDev
        {
            get
            {
                if (BP.Sys.SystemConfig.IsBSsystem == false)
                    return UserWorkDev.PC;

                int s = (int)GetSessionByKey("UserWorkDev", 0);
                BP.Web.UserWorkDev wd = (BP.Web.UserWorkDev)s;
                return wd;
            }
            set
            {
                SetSessionByKey("UserWorkDev", (int)value);
            }
        }
        /// <summary>
        /// IsWap
        /// </summary>
        public static bool IsWap
        {
            get
            {
                if (BP.Sys.SystemConfig.IsBSsystem == false)
                    return false;
                int s = (int)GetSessionByKey("IsWap", 9);
                if (s == 9)
                {
                    bool b = BP.Sys.Glo.Request.RawUrl.ToLower().Contains("wap");
                    IsWap = b;
                    if (b)
                    {
                        SetSessionByKey("IsWap", 1);
                    }
                    else
                    {
                        SetSessionByKey("IsWap", 0);
                    }
                    return b;
                }
                if (s == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    SetSessionByKey("IsWap", 1);
                else
                    SetSessionByKey("IsWap", 0);
            }
        }

        

        #region 当前人员操作方法.
        public static void DeleteTempFileOfMy()
        {
            HttpCookie hc = BP.Sys.Glo.Request.Cookies["CCS"];
            if (hc == null)
                return;
            string usr = hc.Values["No"];
            string[] strs = System.IO.Directory.GetFileSystemEntries(SystemConfig.PathOfTemp);
            foreach (string str in strs)
            {
                if (str.IndexOf(usr) == -1)
                    continue;

                try
                {
                    System.IO.File.Delete(str);
                }
                catch
                {
                }
            }
        }
        public static void DeleteTempFileOfAll()
        {
            string[] strs = System.IO.Directory.GetFileSystemEntries(SystemConfig.PathOfTemp);
            foreach (string str in strs)
            {
                try
                {
                    System.IO.File.Delete(str);
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}
