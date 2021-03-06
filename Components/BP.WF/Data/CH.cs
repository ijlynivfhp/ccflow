using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.En;
using BP.Port;
using BP.Web;
using BP.Sys;

namespace BP.WF.Data
{
    /// <summary>
    /// 完成状态
    /// </summary>
    public enum CHSta
    {
        /// <summary>
        /// 及时完成
        /// </summary>
        JiShi,
        /// <summary>
        /// 按期完成
        /// </summary>
        AnQi,
        /// <summary>
        /// 预期完成
        /// </summary>
        YuQi,
        /// <summary>
        /// 超期完成
        /// </summary>
        ChaoQi
    }
	/// <summary>
	/// 时效考核属性
	/// </summary>
    public class CHAttr
    {
        #region 属性
        public const string MyPK = "MyPK";
        /// <summary>
        /// 工作ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        /// 流程编号
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// 流程编号
        /// </summary>
        public const string FK_FlowT = "FK_FlowT";

        /// <summary>
        /// 节点
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// 节点编号
        /// </summary>
        public const string FK_NodeT = "FK_NodeT";

        /// <summary>
        /// 部门编号
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        /// 部门编号
        /// </summary>
        public const string FK_DeptT = "FK_DeptT";
        /// <summary>
        /// 送达否
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        public const string FK_EmpT = "FK_EmpT";
        /// <summary>
        /// 限期
        /// </summary>
        public const string TSpan = "TSpan";
        /// <summary>
        /// 实际期限
        /// </summary>
        public const string UseMinutes = "UseMinutes";
        /// <summary>
        /// 使用时间
        /// </summary>
        public const string UseTime = "UseTime";
        /// <summary>
        /// 逾期
        /// </summary>
        public const string OverMinutes = "OverMinutes";
        /// <summary>
        /// 预期
        /// </summary>
        public const string OverTime = "OverTime";
        /// <summary>
        /// 状态
        /// </summary>
        public const string CHSta = "CHSta";
        /// <summary>
        /// 年月
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        /// 周
        /// </summary>
        public const string WeekNum = "WeekNum";
        /// <summary>
        /// FID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// 标题
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        /// 时间从
        /// </summary>
        public const string DTFrom = "DTFrom";
        /// <summary>
        /// 时间到
        /// </summary>
        public const string DTTo = "DTTo";
        /// <summary>
        /// 应完成日期
        /// </summary>
        public const string SDT = "SDT";
        #endregion
    }
	/// <summary>
	/// 时效考核
	/// </summary> 
    public class CH : EntityMyPK
    {
        #region 基本属性
        /// <summary>
        /// 考核状态
        /// </summary>
        public CHSta CHSta
        {
            get
            {
                return (CHSta)this.GetValIntByKey(CHAttr.CHSta);
            }
            set
            {
                this.SetValByKey(CHAttr.CHSta, (int)value);
            }
        }
        /// <summary>
        /// 时间到
        /// </summary>
        public string DTTo
        {
            get
            {
                return this.GetValStringByKey(CHAttr.DTTo);
            }
            set
            {
                this.SetValByKey(CHAttr.DTTo, value);
            }
        }
        /// <summary>
        /// 时间从
        /// </summary>
        public string DTFrom
        {
            get
            {
                return this.GetValStringByKey(CHAttr.DTFrom);
            }
            set
            {
                this.SetValByKey(CHAttr.DTFrom, value);
            }
        }
        /// <summary>
        /// 应完成日期
        /// </summary>
        public string SDT
        {
            get
            {
                return this.GetValStringByKey(CHAttr.SDT);
            }
            set
            {
                this.SetValByKey(CHAttr.SDT, value);
            }
        }
        /// <summary>
        /// 流程标题
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(CHAttr.Title);
            }
            set
            {
                this.SetValByKey(CHAttr.Title, value);
            }
        }
        /// <summary>
        /// 流程编号
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(CHAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_Flow, value);
            }
        }
        /// <summary>
        /// 流程
        /// </summary>
        public string FK_FlowT
        {
            get
            {
                return this.GetValStringByKey(CHAttr.FK_FlowT);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_FlowT, value);
            }
        }
        /// <summary>
        /// 限期
        /// </summary>
        public string TSpan
        {
            get
            {
                return this.GetValStringByKey(CHAttr.TSpan);
            }
            set
            {
                this.SetValByKey(CHAttr.TSpan, value);
            }
        }
        /// <summary>
        /// 实际完成用时.
        /// </summary>
        public int UseMinutes
        {
            get
            {
                return this.GetValIntByKey(CHAttr.UseMinutes);
            }
            set
            {
                this.SetValByKey(CHAttr.UseMinutes, value);
            }
        }
        public string UseTime
        {
            get
            {
                return this.GetValStringByKey(CHAttr.UseTime);
            }
            set
            {
                this.SetValByKey(CHAttr.UseTime, value);
            }
        }
        /// <summary>
        /// 超过时限
        /// </summary>
        public int OverMinutes
        {
            get
            {
                return this.GetValIntByKey(CHAttr.OverMinutes);
            }
            set
            {
                this.SetValByKey(CHAttr.OverMinutes, value);
            }
        }
        /// <summary>
        /// 预期
        /// </summary>
        public string OverTime
        {
            get
            {
                return this.GetValStringByKey(CHAttr.OverTime);
            }
            set
            {
                this.SetValByKey(CHAttr.OverTime, value);
            }
        }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(CHAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_Emp, value);
            }
        }
        /// <summary>
        /// 人员
        /// </summary>
        public string FK_EmpT
        {
            get
            {
                return this.GetValStringByKey(CHAttr.FK_EmpT);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_EmpT, value);
            }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStrByKey(CHAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_Dept, value);
            }
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string FK_DeptT
        {
            get
            {
                return this.GetValStrByKey(CHAttr.FK_DeptT);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_DeptT, value);
            }
        }
        /// <summary>
        /// 年月
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStrByKey(CHAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_NY, value);
            }
        }
        /// <summary>
        /// 周
        /// </summary>
        public int WeekNum
        {
            get
            {
                return this.GetValIntByKey(CHAttr.WeekNum);
            }
            set
            {
                this.SetValByKey(CHAttr.WeekNum, value);
            }
        }
        /// <summary>
        /// 工作ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(CHAttr.WorkID);
            }
            set
            {
                this.SetValByKey(CHAttr.WorkID, value);
            }
        }
        /// <summary>
        /// 流程ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(CHAttr.FID);
            }
            set
            {
                this.SetValByKey(CHAttr.FID, value);
            }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(CHAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_Node, value);
            }
        }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string FK_NodeT
        {
            get
            {
                return this.GetValStrByKey(CHAttr.FK_NodeT);
            }
            set
            {
                this.SetValByKey(CHAttr.FK_NodeT, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// UI界面上的访问控制
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = false;
                uac.IsView = true;
                return uac;
            }
        }
        /// <summary>
        /// 时效考核
        /// </summary>
        public CH() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pk"></param>
        public CH(string pk)
            : base(pk)
        {
        }
        #endregion

        #region Map
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_CH", "时效考核");

                map.AddMyPK();

                map.AddTBInt(CHAttr.WorkID, 0, "工作ID", false, true);
                map.AddTBInt(CHAttr.FID, 0, "FID", false, true);
                
                map.AddTBString(CHAttr.Title, null, "标题", false, false, 0, 900, 5);

                map.AddTBString(CHAttr.FK_Flow, null, "流程", false, false, 3, 3, 3);
                map.AddTBString(CHAttr.FK_FlowT, null, "流程名称", true, true, 0, 50, 5);

                map.AddTBInt(CHAttr.FK_Node, 0, "节点", false, false);
                map.AddTBString(CHAttr.FK_NodeT, null, "节点名称", true, true, 0, 50, 5);

                map.AddTBString(CHAttr.DTFrom, null, "时间从", true, true, 0, 50, 5);
                map.AddTBString(CHAttr.DTTo, null, "到", true, true, 0, 50, 5);
                map.AddTBString(CHAttr.SDT, null, "应完成日期", true, true, 0, 50, 5);

                map.AddTBString(CHAttr.TSpan, null, "规定限期", true, true, 0, 50, 5);

                map.AddTBInt(CHAttr.UseMinutes, 0, "实际使用分钟", false, true);
                map.AddTBString(CHAttr.UseTime, null, "实际使用时间", true, true, 0, 50, 5);

                map.AddTBInt(CHAttr.OverMinutes, 0, "逾期分钟", false, true);
                map.AddTBString(CHAttr.OverTime, null, "逾期", true, true, 0, 50, 5);

                map.AddTBString(CHAttr.FK_Dept, null, "隶属部门", true, true, 0, 50, 5);
                map.AddTBString(CHAttr.FK_DeptT, null, "部门名称", true, true, 0, 50, 5);

                map.AddTBString(CHAttr.FK_Emp, null, "当事人", true, true, 0, 30, 3);
                map.AddTBString(CHAttr.FK_EmpT, null, "当事人名称", true, true, 0, 50, 5);

                map.AddTBString(CHAttr.FK_NY, null, "隶属月份", true, true, 0, 10, 10);
                map.AddTBInt(CHAttr.WeekNum, 0, "第几周", false, true);

                map.AddTBInt(CHAttr.FID, 0, "FID", false, true);
                map.AddTBInt(CHAttr.CHSta, 0, "状态", true, true);
                map.AddTBIntMyNum();

                //map.AddSearchAttr(CHAttr.FK_Dept);
                //map.AddSearchAttr(CHAttr.FK_NY);
                //map.AddSearchAttr(CHAttr.FK_Emp);

                //RefMethod rm = new RefMethod();
                //rm.Title = "打开";
                //rm.ClassMethodName = this.ToString() + ".DoOpen";
                //rm.Icon = "/WF/Img/FileType/doc.gif";
                //map.AddRefMethod(rm);

                //rm = new RefMethod();
                //rm.Title = "打开";
                //rm.ClassMethodName = this.ToString() + ".DoOpenPDF";
                //rm.Icon = "/WF/Img/FileType/pdf.gif";
                //map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            return base.beforeUpdateInsertAction();
        }
    }
	/// <summary>
	/// 时效考核s
	/// </summary>
	public class CHs :Entities
	{
		#region 构造方法属性
		/// <summary>
        /// 时效考核s
		/// </summary>
		public CHs(){}
		#endregion 

		#region 属性
		/// <summary>
        /// 时效考核
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new CH();
			}
		}
		#endregion

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<CH> ToJavaList()
        {
            return (System.Collections.Generic.IList<CH>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<CH> Tolist()
        {
            System.Collections.Generic.List<CH> list = new System.Collections.Generic.List<CH>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((CH)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
	}
}
