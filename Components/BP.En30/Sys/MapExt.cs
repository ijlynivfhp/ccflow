using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    /// 扩展
    /// </summary>
    public class MapExtAttr : EntityNoNameAttr
    {
        /// <summary>
        /// 主表
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// ExtType
        /// </summary>
        public const string ExtType = "ExtType";
        /// <summary>
        /// 插入表单的位置
        /// </summary>
        public const string RowIdx = "RowIdx";
        /// <summary>
        /// GroupID
        /// </summary>
        public const string GroupID = "GroupID";
        /// <summary>
        /// 高度
        /// </summary>
        public const string H = "H";
        /// <summary>
        /// 宽度
        /// </summary>
        public const string W = "W";
        /// <summary>
        /// 是否可以自适应大小
        /// </summary>
        public const string IsAutoSize = "IsAutoSize";
        /// <summary>
        /// 设置的属性
        /// </summary>
        public const string AttrOfOper = "AttrOfOper";
        /// <summary>
        /// 激活的属性
        /// </summary>
        public const string AttrsOfActive = "AttrsOfActive";
        /// <summary>
        /// 执行方式
        /// </summary>
        public const string DoWay = "DoWay";
        /// <summary>
        /// Tag
        /// </summary>
        public const string Tag = "Tag";
        /// <summary>
        /// Tag1
        /// </summary>
        public const string Tag1 = "Tag1";
        /// <summary>
        /// Tag2
        /// </summary>
        public const string Tag2 = "Tag2";
        /// <summary>
        /// Tag3
        /// </summary>
        public const string Tag3 = "Tag3";
        /// <summary>
        /// 数据源
        /// </summary>
        public const string DBSrc = "DBSrc";
        /// <summary>
        /// Doc
        /// </summary>
        public const string Doc = "Doc";
        /// <summary>
        /// 参数
        /// </summary>
        public const string AtPara = "AtPara";
        /// <summary>
        /// 计算的优先级
        /// </summary>
        public const string PRI = "PRI";
        /// <summary>
        /// 数据源
        /// </summary>
        public const string FK_DBSrc = "FK_DBSrc";
    }
    /// <summary>
    /// 扩展
    /// </summary>
    public class MapExt : EntityMyPK
    {
        #region 关于 at 参数
        /// <summary>
        /// Pop参数.
        /// </summary>
        public int PopValFormat
        {
            get
            {
                return this.GetParaInt("PopValFormat");
            }
            set
            {
                this.SetPara("PopValFormat", value);
            }
        }
        /// <summary>
        /// pop 选择方式
        /// 0,多选,1=单选.
        /// </summary>
        public int PopValSelectModel
        {
            get
            {
                return this.GetParaInt("PopValSelectModel");
            }
            set
            {
                this.SetPara("PopValSelectModel", value);
            }
        }

        /// <summary>
        /// 工作模式
        /// 0=url, 1=内置.
        /// </summary>
        public int PopValWorkModel
        {
            get
            {
                return this.GetParaInt("PopValWorkModel");
            }
            set
            {
                this.SetPara("PopValWorkModel", value);
            }
        }


        /// <summary>
        /// pop 呈现方式
        /// 0,表格,1=目录.
        /// </summary>
        public int PopValShowModel
        {
            get
            {
                return this.GetParaInt("PopValShowModel");
            }
            set
            {
                this.SetPara("PopValShowModel", value);
            }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        public string FK_DBSrc
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.FK_DBSrc);
            }
            set
            {
                this.SetValByKey(MapExtAttr.FK_DBSrc, value);
            }
        }
        #endregion

        #region 属性
        public string ExtDesc
        {
            get
            {
                string dec = "";
                switch (this.ExtType)
                {
                    case MapExtXmlList.ActiveDDL:
                        dec += "字段" + this.AttrOfOper;
                        break;
                    case MapExtXmlList.TBFullCtrl:
                        dec += this.AttrOfOper;
                        break;
                    case MapExtXmlList.DDLFullCtrl:
                        dec += "" + this.AttrOfOper;
                        break;
                    case MapExtXmlList.InputCheck:
                        dec += "字段：" + this.AttrOfOper + " 检查内容：" + this.Tag1;
                        break;
                    case MapExtXmlList.PopVal:
                        dec += "字段：" + this.AttrOfOper + " Url：" + this.Tag;
                        break;
                    default:
                        break;
                }
                return dec;
            }
        }
        /// <summary>
        /// 是否自适应大小
        /// </summary>
        public bool IsAutoSize
        {
            get
            {
                return this.GetValBooleanByKey(MapExtAttr.IsAutoSize);
            }
            set
            {
                this.SetValByKey(MapExtAttr.IsAutoSize, value);
            }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        public string DBSrc111
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.DBSrc);
            }
            set
            {
                this.SetValByKey(MapExtAttr.DBSrc, value);
            }
        }
        public string AtPara
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.AtPara);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AtPara, value);
            }
        }
      
        public string ExtType
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.ExtType);
            }
            set
            {
                this.SetValByKey(MapExtAttr.ExtType, value);
            }
        }
        public int DoWay
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.DoWay);
            }
            set
            {
                this.SetValByKey(MapExtAttr.DoWay, value);
            }
        }
        /// <summary>
        /// 操作的attrs
        /// </summary>
        public string AttrOfOper
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.AttrOfOper);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AttrOfOper, value);
            }
        }
        /// <summary>
        /// 激活的attrs
        /// </summary>
        public string AttrsOfActive
        {
            get
            {
              //  return this.GetValStrByKey(MapExtAttr.AttrsOfActive).Replace("~", "'");
                return this.GetValStrByKey(MapExtAttr.AttrsOfActive);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AttrsOfActive, value);
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(MapExtAttr.FK_MapData, value);
            }
        }
        /// <summary>
        /// Doc
        /// </summary>
        public string Doc
        {
            get
            {
                return  this.GetValStrByKey("Doc").Replace("~","'");
            }
            set
            {
                this.SetValByKey("Doc", value);
            }
        }
        public string TagOfSQL_autoFullTB
        {
            get
            {
                if (string.IsNullOrEmpty(this.Tag))
                    return this.DocOfSQLDeal;
                

                string sql = this.Tag;
                sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                return sql;
            }
        }

        public string DocOfSQLDeal
        {
            get
            {
                string sql = this.Doc;
                sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                return sql;
            }
        }
        public string Tag
        {
            get
            {
                string s= this.GetValStrByKey("Tag").Replace("~", "'");

                s = s.Replace("\\\\", "\\");
                s = s.Replace("\\\\", "\\");

                s = s.Replace(@"CCFlow\Data\", @"CCFlow\WF\Data\");

                return s;
            }
            set
            {
                this.SetValByKey("Tag", value);
            }
        }
        public string Tag1
        {
            get
            {
                return this.GetValStrByKey("Tag1").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag1", value);
            }
        }
        public string Tag2
        {
            get
            {
                return this.GetValStrByKey("Tag2").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag2", value);
            }
        }
        public string Tag3
        {
            get
            {
                return this.GetValStrByKey("Tag3").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag3", value);
            }
        }
        public int H
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.H);
            }
            set
            {
                this.SetValByKey(MapExtAttr.H, value);
            }
        }
        public int W
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.W);
            }
            set
            {
                this.SetValByKey(MapExtAttr.W, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 扩展
        /// </summary>
        public MapExt()
        {
        }
        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="no"></param>
        public MapExt(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("Sys_MapExt");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = "扩展";
                map.EnType = EnType.Sys;

                map.AddMyPK();

                map.AddTBString(MapExtAttr.FK_MapData, null, "主表", true, false, 0, 100, 20);
                map.AddTBString(MapExtAttr.ExtType, null, "类型", true, false, 0, 30, 20);
                map.AddTBInt(MapExtAttr.DoWay, 0, "执行方式", true, false);

                map.AddTBString(MapExtAttr.AttrOfOper, null, "操作的Attr", true, false, 0, 30, 20);
                map.AddTBString(MapExtAttr.AttrsOfActive, null, "激活的字段", true, false, 0, 900, 20);

                map.AddTBString(MapExtAttr.FK_DBSrc, null, "数据源", true, false, 0, 100, 20);
                map.AddTBStringDoc();
                map.AddTBString(MapExtAttr.Tag, null, "Tag", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag1, null, "Tag1", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag2, null, "Tag2", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag3, null, "Tag3", true, false, 0, 2000, 20);

                map.AddTBString(MapExtAttr.AtPara, null, "参数", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.DBSrc, null, "数据源", true, false, 0, 20, 20);


                map.AddTBInt(MapExtAttr.H, 500, "高度", false, false);
                map.AddTBInt(MapExtAttr.W, 400, "宽度", false, false);

                // add by stone 2013-12-21 计算的优先级,用于js的计算.
                map.AddTBInt(MapExtAttr.PRI, 0, "PRI", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion


        #region 其他方法.
        /// <summary>
        /// 统一生成主键的规则.
        /// </summary>
        public void InitPK()
        {
            switch (this.ExtType)
            {
                case MapExtXmlList.ActiveDDL:
                    this.MyPK =  MapExtXmlList.ActiveDDL +"_"+this.FK_MapData + "_" + this.AttrsOfActive;
                    break;
                case MapExtXmlList.DDLFullCtrl:
                    this.MyPK = MapExtXmlList.DDLFullCtrl + "_" + this.FK_MapData + "_" + this.AttrOfOper;
                    break;
                case MapExtXmlList.PopVal:
                    this.MyPK = MapExtXmlList.PopVal + "_" + this.FK_MapData + "_" + this.AttrOfOper;
                    break;
                default:
                    break;
            }
        }
        #endregion 
    }
    /// <summary>
    /// 扩展s
    /// </summary>
    public class MapExts : Entities
    {
        #region 构造
        /// <summary>
        /// 扩展s
        /// </summary>
        public MapExts()
        {
        }
        /// <summary>
        /// 扩展s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public MapExts(string fk_mapdata)
        {
            this.Retrieve(MapExtAttr.FK_MapData, fk_mapdata, MapExtAttr.PRI);
        }
        /// <summary>
        /// 得到它的 Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapExt();
            }
        }
        #endregion

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<MapExt> ToJavaList()
        {
            return (System.Collections.Generic.IList<MapExt>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<MapExt> Tolist()
        {
            System.Collections.Generic.List<MapExt> list = new System.Collections.Generic.List<MapExt>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((MapExt)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
    }
}
