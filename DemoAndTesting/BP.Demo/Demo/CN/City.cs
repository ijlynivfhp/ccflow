using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Port;
namespace BP.CN
{
	/// <summary>
	/// 城市 
	/// </summary>
    public class CityAttr : EntityNoNameAttr
    {
        #region 基本属性
        public const string FK_PQ = "FK_PQ";
        public const string FK_SF = "FK_SF";
        public const string Grade = "Grade";
        public const string Names = "Names";
        #endregion
    }
	/// <summary>
    /// 城市
	/// </summary>
    public class City : EntityNoName
    {
        #region 基本属性
        public string Names
        {
            get
            {
                return this.GetValStrByKey(CityAttr.Names);
            }
        }
        public string FK_PQ
        {
            get
            {
                return this.GetValStrByKey(CityAttr.FK_PQ);
            }
        }
        public string FK_SF
        {
            get
            {
                return this.GetValStrByKey(CityAttr.FK_SF);
            }
        }
        #endregion

        #region 构造函数
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        /// <summary>
        /// 城市
        /// </summary>		
        public City() { }
        public City(string no)
            : base(no)
        {
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map();

                #region 基本属性
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN);
                map.PhysicsTable = "CN_City";
                map.AdjunctType = AdjunctType.AllType;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.None;
                map.IsCheckNoLength = false;
                map.EnDesc = "城市";
                map.EnType = EnType.App;
                map.CodeStruct = "4";
                #endregion

                #region 字段
                map.AddTBStringPK(CityAttr.No, null, "编号", true, false, 0, 50, 50);
                map.AddTBString(CityAttr.Name, null, "名称", true, false, 0, 50, 200);
                map.AddTBString(CityAttr.Names, null, "小名", true, false, 0, 50, 200);
                map.AddTBInt(CityAttr.Grade, 0, "Grade", false, false);

                map.AddDDLEntities(CityAttr.FK_SF, null, "省份", new SFs(), true);
                map.AddDDLEntities(CityAttr.FK_PQ, null, "片区", new PQs(), true);

                map.AddSearchAttr(CityAttr.FK_SF);
                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

    }
	/// <summary>
	/// 城市
	/// </summary>
	public class Citys : EntitiesNoName
	{
		#region 
		/// <summary>
		/// 得到它的 Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new City();
			}
		}	
		#endregion 

		#region 构造方法
		/// <summary>
		/// 城市s
		/// </summary>
		public Citys(){}

        /// <summary>
        /// 城市s
        /// </summary>
        /// <param name="sf">省份</param>
        public Citys(string sf)
        {
            this.Retrieve(CityAttr.FK_SF, sf);
        }
		#endregion
	}
	
}
