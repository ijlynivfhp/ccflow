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
    public class CCMobile_MyFlow : DirectoryPageBase
    {
        /// <summary>
        /// 页面功能实体
        /// </summary>
        /// <param name="mycontext"></param>
        public CCMobile_MyFlow(HttpContext mycontext)
        {
            this.context = mycontext;
        }
        /// <summary>
        /// 获得工作节点
        /// </summary>
        /// <returns></returns>
        public string GenerWorkNode()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.GenerWorkNode();
        }
        /// <summary>
        /// 获得toolbar
        /// </summary>
        /// <returns></returns>
        public string InitToolBar()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.InitToolBar();
        }
        public string MyFlow_Init()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.MyFlow_Init();
        }

        public string MyFlow_StopFlow()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.MyFlow_StopFlow();
        }
        public string Save()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.Save();
        }
        public string Send()
        {
            WF_MyFlow en = new WF_MyFlow(this.context);
            return en.Send();
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
            throw new Exception("@标记[" + this.DoType + "]，没有找到. @RowURL:" + context.Request.RawUrl);
        }
        #endregion 执行父类的重写方法.

        #region xxx 界面 .
        #endregion xxx 界面方法.

    }
}