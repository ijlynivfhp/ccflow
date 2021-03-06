using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.GPM
{
    /// <summary>
    /// 操作员属性
    /// </summary>
    public class EmpAttr : BP.En.EntityNoNameAttr
    {
        #region 基本属性
        /// <summary>
        /// 员工编号
        /// </summary>
        public const string EmpNo = "EmpNo";
        /// <summary>
        /// 部门
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        /// 职务
        /// </summary>
        public const string FK_Duty = "FK_Duty";
        /// <summary>
        /// FK_Unit
        /// </summary>
        public const string FK_Unit = "FK_Unit";
        /// <summary>
        /// 密码
        /// </summary>
        public const string Pass = "Pass";
        /// <summary>
        /// sid
        /// </summary>
        public const string SID = "SID";
        /// <summary>
        /// 电话
        /// </summary>
        public const string Tel = "Tel";
        /// <summary>
        /// 邮箱
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        /// 部门数量
        /// </summary>
        public const string NumOfDept = "NumOfDept";
        /// <summary>
        /// 序号
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        /// 领导
        /// </summary>
        public const string Leader = "Leader";
        #endregion
    }
    /// <summary>
    /// 操作员 的摘要说明。
    /// </summary>
    public class Emp : EntityNoName
    {
        #region 扩展属性
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmpNo
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.EmpNo);
            }
            set
            {
                this.SetValByKey(EmpAttr.EmpNo, value);
            }
        }
        /// <summary>
        /// 主要的部门。
        /// </summary>
        public Dept HisDept
        {
            get
            {
                try
                {
                    return new Dept(this.FK_Dept);
                }
                catch (Exception ex)
                {
                    throw new Exception("@获取操作员" + this.No + "部门[" + this.FK_Dept + "]出现错误,可能是系统管理员没有给他维护部门.@" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(EmpAttr.FK_Dept, value);
            }
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string FK_Duty
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.FK_Duty);
            }
            set
            {
                this.SetValByKey(EmpAttr.FK_Duty, value);
            }
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string FK_DutyText
        {
            get
            {
                string fk_Duty = this.GetValStrByKey(EmpAttr.FK_Duty);
                if (string.IsNullOrEmpty(fk_Duty))
                    return "";
                Duty duty = new Duty();
                duty.RetrieveByAttr(DutyAttr.No, fk_Duty);
                return duty.Name;
            }
        }
        public string FK_DeptText
        {
            get
            {
                return this.GetValRefTextByKey(EmpAttr.FK_Dept);
            }
        }
        public string Tel
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.Tel);
            }
            set
            {
                this.SetValByKey(EmpAttr.Tel, value);
            }
        }
        public string Email
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.Email);
            }
            set
            {
                this.SetValByKey(EmpAttr.Email, value);
            }
        }

        public string Leader
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.Leader);
            }
            set
            {
                this.SetValByKey(EmpAttr.Leader, value);
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pass
        {
            get
            {
                return this.GetValStrByKey(EmpAttr.Pass);
            }
            set
            {
                this.SetValByKey(EmpAttr.Pass, value);
            }
        }
        /// <summary>
        /// 顺序号
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(EmpAttr.Idx);
            }
            set
            {
                this.SetValByKey(EmpAttr.Idx, value);
            }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 检查密码(可以重写此方法)
        /// </summary>
        /// <param name="pass">密码</param>
        /// <returns>是否匹配成功</returns>
        public bool CheckPass(string pass)
        {
            if (this.Pass == pass)
                return true;
            return false;
        }
        #endregion 公共方法

        #region 构造函数
        /// <summary>
        /// 操作员
        /// </summary>
        public Emp()
        {
        }
        /// <summary>
        /// 操作员
        /// </summary>
        /// <param name="no">编号</param>
        public Emp(string no)
        {
            this.No = no.Trim();
            if (this.No.Length == 0)
                throw new Exception("@要查询的操作员编号为空。");
            try
            {
                this.Retrieve();
            }
            catch (Exception ex)
            {
                int i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception("@用户或者密码错误：[" + no + "]，或者帐号被停用。@技术信息(从内存中查询出现错误)：ex1=" + ex.Message);
            }
        }
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForAppAdmin();
                return uac;
            }
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

                Map map = new Map();

                #region 基本属性
                map.EnDBUrl =
                    new DBUrl(DBUrlType.AppCenterDSN); //要连接的数据源（表示要连接到的那个系统数据库）。
                map.PhysicsTable = "Port_Emp"; // 要物理表。
                map.DepositaryOfMap = Depositary.Application;    //实体map的存放位置.
                map.DepositaryOfEntity = Depositary.Application; //实体存放位置
                map.EnDesc = "用户"; // "用户"; // 实体的描述.
                map.EnType = EnType.App;   //实体类型。
                #endregion

                #region 字段
                /*关于字段属性的增加 */
                map.AddTBStringPK(EmpAttr.No, null, "编号", true, false, 1, 20, 30);
                map.AddTBString(EmpAttr.EmpNo, null, "职工编号", true, false, 0, 20, 30);
                map.AddTBString(EmpAttr.Name, null, "名称", true, false, 0, 100, 30);
                map.AddTBString(EmpAttr.Pass, "123", "密码", false, false, 0, 100, 10);

                //map.AddDDLEntities(EmpAttr.FK_Dept, null, "部门", new Port.Depts(), true);

                map.AddTBString(EmpAttr.FK_Dept, null, "当前部门", false, false, 0, 20, 10);
                map.AddTBString(EmpAttr.FK_Duty, null, "当前职务", false, false, 0, 20, 10);
                map.AddTBString(EmpAttr.Leader, null, "当前领导", false, false, 0, 50, 1);

                map.AddTBString(EmpAttr.SID, null, "安全校验码", false, false, 0, 36, 36);
                map.AddTBString(EmpAttr.Tel, null, "电话", true, false, 0, 20, 130);
                map.AddTBString(EmpAttr.Email, null, "邮箱", true, false, 0, 100, 132);
                map.AddTBInt(EmpAttr.NumOfDept, 0, "部门数量", true, false);

                map.AddTBInt(EmpAttr.Idx, 0, "序号", true, false);
                #endregion 字段

                 map.AddSearchAttr(EmpAttr.FK_Dept);

                ////#region 增加点对多属性
                ////他的部门权限
                //map.AttrsOfOneVSM.Add(new EmpDepts(), new Depts(), EmpDeptAttr.FK_Emp, EmpDeptAttr.FK_Dept,
                //    DeptAttr.Name, DeptAttr.No, "部门权限");

                //map.AttrsOfOneVSM.Add(new EmpStations(), new Stations(), EmpStationAttr.FK_Emp, EmpStationAttr.FK_Station,
                //    DeptAttr.Name, DeptAttr.No, "岗位权限");
                ////#endregion

                RefMethod rm = new RefMethod();
                rm.Title = "修改密码";
                rm.ClassMethodName = this.ToString() + ".DoResetpassword";
                rm.HisAttrs.AddTBString("pass1", null, "输入密码", true, false, 0, 100, 100);
                rm.HisAttrs.AddTBString("pass2", null, "再次输入", true, false, 0, 100, 100);
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        public void DoUp()
        {
            this.DoOrderUp(EmpAttr.FK_Dept, this.FK_Dept, EmpAttr.Idx);
        }
        /// <summary>
        /// 向下移动
        /// </summary>
        public void DoDown()
        {
            this.DoOrderDown(EmpAttr.FK_Dept, this.FK_Dept, EmpAttr.Idx);
        }

        public string DoResetpassword(string pass1, string pass2)
        {
            if (pass1.Equals(pass2) == false)
                return "两次密码不一致";

            this.Pass = pass1;
            this.Update();
            return "密码设置成功";
        }
        /// <summary>
        /// 获取集合
        /// </summary>
        public override Entities GetNewEntities
        {
            get { return new Emps(); }
        }
        #endregion 构造函数
    }
    /// <summary>
    /// 操作员s
    // </summary>
    public class Emps : EntitiesNoName
    {
        #region 构造方法
        /// <summary>
        /// 得到它的 Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Emp();
            }
        }
        /// <summary>
        /// 操作员s
        /// </summary>
        public Emps()
        {
        }
        public override int RetrieveAll()
        {
            return base.RetrieveAll("Name");
        }
        #endregion 构造方法

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<Emp> ToJavaList()
        {
            return (System.Collections.Generic.IList<Emp>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<Emp> Tolist()
        {
            System.Collections.Generic.List<Emp> list = new System.Collections.Generic.List<Emp>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((Emp)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
    }
}
