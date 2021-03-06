using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	/// <summary>
	/// sss
	/// </summary>
    public class SysEnumMainAttr : EntityNoNameAttr
	{
        public const string CfgVal = "CfgVal";

        public const string Lang = "Lang";

	}
	/// <summary>
	/// SysEnumMain
	/// </summary>
    public class SysEnumMain : EntityNoName
    {
        #region 实现基本的方方法
        public string CfgVal
        {
            get
            {
                return this.GetValStrByKey(SysEnumMainAttr.CfgVal);
            }
            set
            {
                this.SetValByKey(SysEnumMainAttr.CfgVal, value);
            }
        }
        public string Lang
        {
            get
            {
                return this.GetValStrByKey(SysEnumMainAttr.Lang);
            }
            set
            {
                this.SetValByKey(SysEnumMainAttr.Lang, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// SysEnumMain
        /// </summary>
        public SysEnumMain() { }
        /// <summary>
        /// SysEnumMain
        /// </summary>
        /// <param name="no"></param>
        public SysEnumMain(string no)
        {
            try
            {
                this.No = no;
                this.Retrieve();
            }
            catch (Exception ex)
            {
                SysEnums ses = new SysEnums(no);
                if (ses.Count == 0)
                    throw ex;

                this.No = no;
                this.Name = "未命名";
                string cfgVal = "";
                foreach (SysEnum item in ses)
                {
                    cfgVal += "@" + item.IntKey + "=" + item.Lab;
                }
                this.CfgVal = cfgVal;
                this.Insert();
            }
        }
        private void InitUnRegEnum()
        {
            //   DataTable dt = BP.DA.DBAccess.RunSQL("SELECT DISTINCT EnumKey FROM SYS_Enum WHERE EnumKey Not IN (SELECT No FROM SYS_EnumMain )");
            //stringSELECT DISTINCT EnumKey FROM SYS_ENUM
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null) return this._enMap;
                Map map = new Map("Sys_EnumMain");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = "枚举";
                map.EnType = EnType.Sys;

                map.AddTBStringPK(SysEnumMainAttr.No, null, "编号", true, false, 1, 40, 8);
                map.AddTBString(SysEnumMainAttr.Name, null, "名称", true, false, 0, 40, 8);
                map.AddTBString(SysEnumMainAttr.CfgVal, null, "配置信息", true, false, 0, 1500, 8);
                map.AddTBString(SysEnumMainAttr.Lang, "CH", "语言", true, false, 0, 10, 8);
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	/// 纳税人集合 
	/// </summary>
	public class SysEnumMains : EntitiesNoName
	{
		/// <summary>
		/// SysEnumMains
		/// </summary>
		public SysEnumMains(){}
		/// <summary>
		/// 得到它的 Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new SysEnumMain();
			}
		}

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<SysEnumMain> ToJavaList()
        {
            return (System.Collections.Generic.IList<SysEnumMain>)this;
        }

        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<SysEnumMain> Tolist()
        {
            System.Collections.Generic.List<SysEnumMain> list = new System.Collections.Generic.List<SysEnumMain>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((SysEnumMain)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
	}
}
