﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web;
using BP.WF;
using BP.Web;
using BP.Sys;
using BP.DA;
using BP.En;
using System.Reflection;

namespace BP.WF.HttpHandler
{
    abstract public class WebContralBase
    {
        #region 属性.
        /// <summary>
        /// 编号
        /// </summary>
        public string No
        {
            get
            {
                string str = context.Request.QueryString["No"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        /// <summary>
        /// 执行类型
        /// </summary>
        public string DoType
        {
            get
            {
                string str = context.Request.QueryString["DoType"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        public string MyPK
        {
            get
            {
                string str = context.Request.QueryString["MyPK"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        /// <summary>
        /// 字典表
        /// </summary>
        public string FK_SFTable
        {
            get
            {
                string str = context.Request.QueryString["FK_SFTable"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;

            }
        }
        public string EnumKey
        {
            get
            {
                string str = context.Request.QueryString["EnumKey"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;

            }
        }
        public string KeyOfEn
        {
            get
            {
                string str = context.Request.QueryString["KeyOfEn"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        /// <summary>
        /// FK_MapData
        /// </summary>
        public string FK_MapData
        {
            get
            {
                string str = context.Request.QueryString["FK_MapData"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        /// <summary>
        /// 流程编号
        /// </summary>
        public string FK_Flow
        {
            get
            {
                string str = context.Request.QueryString["FK_Flow"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        public int GroupField
        {
            get
            {
                string str = context.Request.QueryString["GroupField"];
                if (str == null || str == "" || str == "null")
                    return 0;
                return int.Parse(str);
            }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                string str = context.Request.QueryString["FK_Node"];
                if (str == null || str == "" || str == "null")
                    return 0;
                return int.Parse(str);
            }
        }
        /// <summary>
        /// 框架ID
        /// </summary>
        public string FK_MapFrame
        {
            get
            {
                string str = context.Request.QueryString["FK_MapFrame"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }
        /// <summary>
        ///   RefOID
        /// </summary>
        public int RefOID
        {
            get
            {
                string str = context.Request.QueryString["RefOID"];
                if (str == null || str == "" || str == "null")
                    return 0;
                return int.Parse(str);
            }
        }
        /// <summary>
        /// 明细表
        /// </summary>
        public string FK_MapDtl
        {
            get
            {
                string str = context.Request.QueryString["FK_MapDtl"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }

        /// <summary>
        /// 字段属性编号
        /// </summary>
        public string Ath
        {
            get
            {
                string str = context.Request.QueryString["Ath"];
                if (str == null || str == "" || str == "null")
                    return null;
                return str;
            }
        }

        public HttpContext context = null;
        /// <summary>
        /// 获得表单的属性.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValFromFrmByKey(string key)
        {
            string val = context.Request.Form[key];
            if (val == null)
                return null;
            val = val.Replace("'", "~");
            return val;
        }
        public int GetValIntFromFrmByKey(string key)
        {
            string str = this.GetValFromFrmByKey(key);
            if (str == null || str == "")
                throw new Exception("@参数:" + key + " 没有取到值.");

            return int.Parse(str);
        }
        public bool GetValBoolenFromFrmByKey(string key)
        {
            string val = this.GetValFromFrmByKey(key);
            if (val == null || val == "")
                return false;
            return true;
        }
        /// <summary>
        /// 公共方法获取值
        /// </summary>
        /// <param name="param">参数名</param>
        /// <returns></returns>
        public string GetRequestVal(string param)
        {
            return HttpUtility.UrlDecode(context.Request[param], System.Text.Encoding.UTF8);
        }
        /// <summary>
        /// 公共方法获取值
        /// </summary>
        /// <param name="param">参数名</param>
        /// <returns></returns>
        public int GetRequestValInt(string param)
        {
            string str = GetRequestVal(param);
            if (str == null || str == "")
                return 0;
            return int.Parse(str);
        }
        #endregion 属性.

        public string DoMethod(object myEn,  string methodName)
        {
            
            Type tp = myEn.GetType();
            MethodInfo mp = tp.GetMethod(methodName);
            if (mp == null)
            {
                /* 没有找到方法名字，就执行默认的方法. */
                try
                {
                    return this.DoDefaultMethod();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.InnerException.Message);
                }
            }

            //执行该方法.
            object[] paras = null;
            try
            {
                return mp.Invoke(this, paras) as string;  //调用由此 MethodInfo 实例反射的方法或构造函数。
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }
        /// <summary>
        /// 执行默认的方法名称
        /// </summary>
        /// <returns>返回执行的结果</returns>
        protected virtual string DoDefaultMethod()
        {
            return null;
        }
    }
}
