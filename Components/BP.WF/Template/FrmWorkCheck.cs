using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.WF;
namespace BP.WF.Template
{
    /// <summary>
    /// 附件类型
    /// </summary>
    public enum FWCAth
    {
        /// <summary>
        /// 使用附件
        /// </summary>
        None,
        /// <summary>
        /// 多附件
        /// </summary>
        MinAth,
        /// <summary>
        /// 单附件
        /// </summary>
        SingerAth,
        /// <summary>
        /// 图片附件
        /// </summary>
        ImgAth
    }
    /// <summary>
    /// 类型
    /// </summary>
    public enum FWCType
    {
        /// <summary>
        /// 审核组件
        /// </summary>
        Check,
        /// <summary>
        /// 日志组件
        /// </summary>
        DailyLog,
        /// <summary>
        /// 周报
        /// </summary>
        WeekLog,
        /// <summary>
        /// 月报
        /// </summary>
        MonthLog
    }
    /// <summary>
    /// 显示格式
    /// </summary>
    public enum FrmWorkShowModel
    {
        /// <summary>
        /// 表格
        /// </summary>
        Table,
        /// <summary>
        /// 自由显示
        /// </summary>
        Free
    }
    /// <summary>
    /// 审核组件状态
    /// </summary>
    public enum FrmWorkCheckSta
    {
        /// <summary>
        /// 不可用
        /// </summary>
        Disable,
        /// <summary>
        /// 可用
        /// </summary>
        Enable,
        /// <summary>
        /// 只读
        /// </summary>
        Readonly
    }
    /// <summary>
    /// 审核组件
    /// </summary>
    public class FrmWorkCheckAttr : EntityNoAttr
    {
        /// <summary>
        /// 是否可以审批
        /// </summary>
        public const string FWCSta = "FWCSta";
        /// <summary>
        /// X
        /// </summary>
        public const string FWC_X = "FWC_X";
        /// <summary>
        /// Y
        /// </summary>
        public const string FWC_Y = "FWC_Y";
        /// <summary>
        /// H
        /// </summary>
        public const string FWC_H = "FWC_H";
        /// <summary>
        /// W
        /// </summary>
        public const string FWC_W = "FWC_W";
        /// <summary>
        /// 应用类型
        /// </summary>
        public const string FWCType = "FWCType";
        /// <summary>
        /// 附件
        /// </summary>
        public const string FWCAth = "FWCAth";
        /// <summary>
        /// 显示方式.
        /// </summary>
        public const string FWCShowModel = "FWCShowModel";
        /// <summary>
        /// 轨迹图是否显示?
        /// </summary>
        public const string FWCTrackEnable = "FWCTrackEnable";
        /// <summary>
        /// 历史审核信息是否显示?
        /// </summary>
        public const string FWCListEnable = "FWCListEnable";
        /// <summary>
        /// 是否显示所有的步骤？
        /// </summary>
        public const string FWCIsShowAllStep = "FWCIsShowAllStep";
        /// <summary>
        /// 默认审核信息
        /// </summary>
        public const string FWCDefInfo = "FWCDefInfo";
        /// <summary>
        /// 节点意见名称
        /// </summary>
        public const string FWCNodeName = "FWCNodeName";

        /// <summary>
        /// 如果用户未审核是否按照默认意见填充？
        /// </summary>
        public const string FWCIsFullInfo = "FWCIsFullInfo";
        /// <summary>
        /// 操作名词(审核，审定，审阅，批示)
        /// </summary>
        public const string FWCOpLabel = "FWCOpLabel";
        /// <summary>
        /// 操作人是否显示数字签名
        /// </summary>
        public const string SigantureEnabel = "SigantureEnabel";
        /// <summary>
        /// 操作字段
        /// </summary>
        public const string FWCFields = "FWCFields";
    }
    /// <summary>
    /// 审核组件
    /// </summary>
    public class FrmWorkCheck : Entity
    {
        #region 属性
        public string No
        {
            get
            {
                return "ND" + this.NodeID;
            }
            set
            {
                string nodeID = value.Replace("ND", "");
                this.NodeID = int.Parse(nodeID);
            }
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.NodeID);
            }
            set
            {
                this.SetValByKey(NodeAttr.NodeID, value);
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public FrmWorkCheckSta HisFrmWorkCheckSta
        {
            get
            {
                return (FrmWorkCheckSta)this.GetValIntByKey(FrmWorkCheckAttr.FWCSta);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCSta, (int)value);
            }
        }
        /// <summary>
        /// 显示格式(0=表格,1=自由.)
        /// </summary>
        public FrmWorkShowModel HisFrmWorkShowModel
        {
            get
            {
                return (FrmWorkShowModel)this.GetValIntByKey(FrmWorkCheckAttr.FWCShowModel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCShowModel, (int)value);
            }
        }
        /// <summary>
        /// 附件类型
        /// </summary>
        public FWCAth FWCAth
        {
            get
            {
                return (FWCAth)this.GetValIntByKey(FrmWorkCheckAttr.FWCAth);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCAth, (int)value);
            }
        }
        /// <summary>
        /// 组件类型
        /// </summary>
        public FWCType HisFrmWorkCheckType
        {
            get
            {
                return (FWCType)this.GetValIntByKey(FrmWorkCheckAttr.FWCType);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCType, (int)value);
            }
        }
        /// <summary>
        /// 组件类型名称
        /// </summary>
        public string FWCTypeT
        {
            get
            {
                return this.GetValRefTextByKey(FrmWorkCheckAttr.FWCType);
            }
        }
        /// <summary>
        /// Y
        /// </summary>
        public float FWC_Y
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_Y);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_Y, value);
            }
        }
        /// <summary>
        /// X
        /// </summary>
        public float FWC_X
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_X);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_X, value);
            }
        }
        /// <summary>
        /// W
        /// </summary>
        public float FWC_W
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_W);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_W, value);
            }
        }
        public string FWC_Wstr
        {
            get
            {
                if (this.FWC_W == 0)
                    return "100%";
                return this.FWC_W + "px";
            }
        }
        /// <summary>
        /// H
        /// </summary>
        public float FWC_H
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_H);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_H, value);
            }
        }
        public string FWC_Hstr
        {
            get
            {
                if (this.FWC_H == 0)
                    return "100%";
                return this.FWC_H + "px";
            }
        }
        /// <summary>
        /// 轨迹图是否显示?
        /// </summary>
        public bool FWCTrackEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCTrackEnable);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCTrackEnable, value);
            }
        }
        /// <summary>
        /// 历史审核信息是否显示?
        /// </summary>
        public bool FWCListEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCListEnable);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCListEnable, value);
            }
        }
        /// <summary>
        /// 在轨迹表里是否显示所有的步骤？
        /// </summary>
        public bool FWCIsShowAllStep
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCIsShowAllStep);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCIsShowAllStep, value);
            }
        }
        /// <summary>
        /// 如果用户未审核是否按照默认意见填充?
        /// </summary>
        public bool FWCIsFullInfo
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCIsFullInfo);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCIsFullInfo, value);
            }
        }
        /// <summary>
        /// 默认审核信息
        /// </summary>
        public string FWCDefInfo
        {
            get
            {
                return this.GetValStringByKey(FrmWorkCheckAttr.FWCDefInfo);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCDefInfo, value);
            }
        }
        /// <summary>
        /// 节点名称.
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey("Name");
            }
        }
        /// <summary>
        /// 节点意见名称，如果为空则取节点名称.
        /// </summary>
        public string FWCNodeName
        {
            get
            {
                string str = this.GetValStringByKey(FrmWorkCheckAttr.FWCNodeName);
                if (string.IsNullOrEmpty(str))
                    return this.Name;
                return str;
            }
        }
        /// <summary>
        /// 操作名词(审核，审定，审阅，批示)
        /// </summary>
        public string FWCOpLabel
        {
            get
            {
                return this.GetValStringByKey(FrmWorkCheckAttr.FWCOpLabel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCOpLabel, value);
            }
        }
        /// <summary>
        /// 操作字段
        /// </summary>
        public string FWCFields
        {
            get
            {
                return this.GetValStringByKey(FrmWorkCheckAttr.FWCFields);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCFields, value);
            }
        }
        /// <summary>
        /// 是否显示数字签名？
        /// </summary>
        public bool SigantureEnabel
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.SigantureEnabel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.SigantureEnabel, value);
            }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 控制
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                uac.IsDelete = false;
                uac.IsInsert = false;
                return uac;
            }
        }
        /// <summary>
        /// 重写主键
        /// </summary>
        public override string PK
        {
            get
            {
                return "NodeID";
            }
        }
        /// <summary>
        /// 审核组件
        /// </summary>
        public FrmWorkCheck()
        {
        }
        /// <summary>
        /// 审核组件
        /// </summary>
        /// <param name="no"></param>
        public FrmWorkCheck(string mapData)
        {
            if (mapData.Contains("ND") == false)
            {
                this.HisFrmWorkCheckSta = FrmWorkCheckSta.Disable;
                return;
            }

            string mapdata = mapData.Replace("ND", "");
            if (DataType.IsNumStr(mapdata) == false)
            {
                this.HisFrmWorkCheckSta = FrmWorkCheckSta.Disable;
                return;
            }

            try
            {
                this.NodeID = int.Parse(mapdata);
            }
            catch
            {
                return;
            }
            this.Retrieve();
        }
        /// <summary>
        /// 审核组件
        /// </summary>
        /// <param name="no"></param>
        public FrmWorkCheck(int nodeID)
        {
            this.NodeID = nodeID;
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

                Map map = new Map("WF_Node", "审核组件");
                map.Java_SetEnType(EnType.Sys);

                map.AddTBIntPK(NodeAttr.NodeID, 0, "节点ID", true, true);
                map.AddTBString(NodeAttr.Name, null, "节点名称", true, true, 0, 100, 10);

                #region 此处变更了 NodeSheet类中的，map 描述该部分也要变更.
                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCSta, (int)FrmWorkCheckSta.Disable, "审核组件状态",
                   true, true, FrmWorkCheckAttr.FWCSta, "@0=禁用@1=启用@2=只读");
                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCShowModel, (int)FrmWorkShowModel.Free, "显示方式",
                    true, true, FrmWorkCheckAttr.FWCShowModel, "@0=表格方式@1=自由模式"); //此属性暂时没有用.

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCType, (int)FWCType.Check, "审核组件", true, true,
                    FrmWorkCheckAttr.FWCType, "@0=审核组件@1=日志组件@2=周报组件@3=月报组件");

                map.AddTBString(FrmWorkCheckAttr.FWCNodeName, null, "节点意见名称", true, false, 0, 100, 10);

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCAth, (int)FWCAth.None, "附件上传", true, true,
                   FrmWorkCheckAttr.FWCAth, "@0=不启用@1=多附件@2=单附件(暂不支持)@3=图片附件(暂不支持)");
                map.SetHelperAlert(FrmWorkCheckAttr.FWCAth,
                    "在审核期间，是否启用上传附件？启用什么样的附件？注意：附件的属性在节点表单里配置。"); //使用alert的方式显示帮助信息.

                map.AddBoolean(FrmWorkCheckAttr.FWCTrackEnable, true, "轨迹图是否显示？", true, true, false);

                map.AddBoolean(FrmWorkCheckAttr.FWCListEnable, true, "历史审核信息是否显示？(否,仅出现意见框)", true, true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCIsShowAllStep, false, "在轨迹表里是否显示所有的步骤？", true, true);

                map.AddTBString(FrmWorkCheckAttr.FWCOpLabel, "审核", "操作名词(审核/审阅/批示)", true, false, 0, 50, 10);
                map.AddTBString(FrmWorkCheckAttr.FWCDefInfo, "同意", "默认审核信息", true, false, 0, 50, 10);
                map.AddBoolean(FrmWorkCheckAttr.SigantureEnabel, false, "操作人是否显示为图片签名？", true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCIsFullInfo, true, "如果用户未审核是否按照默认意见填充？", true, true, true);


                map.AddTBFloat(FrmWorkCheckAttr.FWC_X, 5, "位置X", false, false);
                map.AddTBFloat(FrmWorkCheckAttr.FWC_Y, 5, "位置Y", false, false);

                map.AddTBFloat(FrmWorkCheckAttr.FWC_H, 300, "高度(0=100%)", true, false);
                map.AddTBFloat(FrmWorkCheckAttr.FWC_W, 400, "宽度(0=100%)", true, false);

                map.AddTBString(FrmWorkCheckAttr.FWCFields, null, "审批格式字段", true, false, 0, 1000, 10, true);

                #endregion 此处变更了 NodeSheet类中的，map 描述该部分也要变更.

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        protected override bool beforeUpdateInsertAction()
        {
            FrmAttachment workCheckAth = new FrmAttachment();
            bool isHave = workCheckAth.RetrieveByAttr(FrmAttachmentAttr.MyPK, this.NodeID + "_FrmWorkCheck");
            //不包含审核组件
            if (isHave == false)
            {
                workCheckAth = new FrmAttachment();
                /*如果没有查询到它,就有可能是没有创建.*/
                workCheckAth.MyPK = this.NodeID + "_FrmWorkCheck";
                workCheckAth.FK_MapData = this.NodeID.ToString();
                workCheckAth.NoOfObj = this.NodeID + "_FrmWorkCheck";
                workCheckAth.Exts = "*.*";

                //存储路径.
                workCheckAth.SaveTo = "/DataUser/UploadFile/";
                workCheckAth.IsNote = false; //不显示note字段.
                workCheckAth.IsVisable = false; // 让其在form 上不可见.

                //位置.
                workCheckAth.X = (float)94.09;
                workCheckAth.Y = (float)333.18;
                workCheckAth.W = (float)626.36;
                workCheckAth.H = (float)150;

                //多附件.
                workCheckAth.UploadType = AttachmentUploadType.Multi;
                workCheckAth.Name = "审核组件";
                workCheckAth.SetValByKey("AtPara", "@IsWoEnablePageset=1@IsWoEnablePrint=1@IsWoEnableViewModel=1@IsWoEnableReadonly=0@IsWoEnableSave=1@IsWoEnableWF=1@IsWoEnableProperty=1@IsWoEnableRevise=1@IsWoEnableIntoKeepMarkModel=1@FastKeyIsEnable=0@IsWoEnableViewKeepMark=1@FastKeyGenerRole=@IsWoEnableTemplete=1");
                workCheckAth.Insert();
            }   
            return base.beforeUpdateInsertAction();
        }
    }
    /// <summary>
    /// 审核组件s
    /// </summary>
    public class FrmWorkChecks : Entities
    {
        #region 构造
        /// <summary>
        /// 审核组件s
        /// </summary>
        public FrmWorkChecks()
        {
        }
        /// <summary>
        /// 审核组件s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmWorkChecks(string fk_mapdata)
        {
            if (SystemConfig.IsDebug)
                this.Retrieve("No", fk_mapdata);
            else
                this.RetrieveFromCash("No", (object)fk_mapdata);
        }
        /// <summary>
        /// 得到它的 Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmWorkCheck();
            }
        }
        #endregion

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<FrmWorkCheck> ToJavaList()
        {
            return (System.Collections.Generic.IList<FrmWorkCheck>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<FrmWorkCheck> Tolist()
        {
            System.Collections.Generic.List<FrmWorkCheck> list = new System.Collections.Generic.List<FrmWorkCheck>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((FrmWorkCheck)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
    }
}
