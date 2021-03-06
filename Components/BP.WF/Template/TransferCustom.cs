using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.WF.Template
{
	/// <summary>
	/// 自定义运行路径 属性
	/// </summary>
    public class TransferCustomAttr : EntityMyPKAttr
    {
        #region 基本属性
        /// <summary>
        /// 工作ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        /// 节点ID
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        /// 处理人
        /// </summary>
        public const string Worker = "Worker";
        /// <summary>
        /// 顺序
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        /// 发起时间
        /// </summary>
        public const string StartDT = "StartDT";
        /// <summary>
        /// 插入日期
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        /// 要启用的子流程编号
        /// </summary>
        public const string SubFlowNo = "SubFlowNo";
        /// <summary>
        /// 是否通过了
        /// </summary>
        public const string TodolistModel = "TodolistModel";
        #endregion
    }
	/// <summary>
	/// 自定义运行路径
	/// </summary>
    public class TransferCustom : EntityMyPK
    {
        #region 属性
        /// <summary>
        /// 节点ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(TransferCustomAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.FK_Node, value);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(TransferCustomAttr.WorkID);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.WorkID, value);
            }
        }
        /// <summary>
        /// 处理人
        /// </summary>
        public string Worker
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.Worker);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.Worker, value);
            }
        }
        /// <summary>
        /// 要启用的子流程编号
        /// </summary>
        public string SubFlowNo
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.SubFlowNo);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.SubFlowNo, value);
            }
        }
        /// <summary>
        /// 顺序
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(TransferCustomAttr.Idx);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.Idx, value);
            }
        }
        /// <summary>
        /// 多人处理工作模式
        /// </summary>
        public TodolistModel TodolistModel
        {
            get
            {
                return (TodolistModel)this.GetValIntByKey(TransferCustomAttr.TodolistModel);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.TodolistModel, (int)value);
            }
        }
        /// <summary>
        /// 发起时间（可以为空）
        /// </summary>
        public string StartDT
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.StartDT);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.StartDT, value);
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// TransferCustom
        /// </summary>
        public TransferCustom()
        {
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
                Map map = new Map("WF_TransferCustom", "自定义运行路径");
                map.Java_SetEnType(EnType.Admin);

                map.AddMyPK(); //唯一的主键.

                //主键.
                map.AddTBInt(TransferCustomAttr.WorkID, 0, "WorkID", true, false);
                map.AddTBInt(TransferCustomAttr.FK_Node, 0, "节点ID", true, false);
                map.AddTBString(TransferCustomAttr.Worker, null, "处理人", true, false, 0, 200, 10);
                map.AddTBString(TransferCustomAttr.SubFlowNo, null, "要经过的子流程编号", true, false, 0, 3, 10);
                map.AddTBDateTime(TransferCustomAttr.RDT, null, "日期时间", true, false);
                map.AddTBInt(TransferCustomAttr.Idx, 0, "顺序号", true, false);

                map.AddTBInt(TransferCustomAttr.TodolistModel, 0, "多人工作处理模式", true, false);

              
                //map.AddTBString(TransferCustomAttr.StartDT, null, "发起时间", true, false, 0, 20, 10);
                
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        /// 获取下一个要到达的定义路径.
        /// 要分析如下几种情况:
        /// 1, 当前节点不存在队列里面，就返回第一个.
        /// 2, 如果当前队列为空,就认为需要结束掉, 返回null.
        /// 3, 如果当前节点是最后一个,就返回null,表示要结束流程.
        /// </summary>
        /// <param name="workid">当前工作ID</param>
        /// <param name="currNodeID">当前节点ID</param>
        /// <returns>获取下一个要到达的定义路径,如果没有就返回空.</returns>
        public static TransferCustom GetNextTransferCustom(Int64 workid, int currNodeID)
        {
            TransferCustoms ens = new TransferCustoms();
            ens.Retrieve(TransferCustomAttr.WorkID, workid, TransferCustomAttr.Idx);
            if (ens.Count == 0)
                return null;

            return (TransferCustom)ens[0];

                ///*获取最后一个*/
                //TransferCustom tEnd = ens[ens.Count-1] as TransferCustom;
                //if (tEnd.FK_Node == currNodeID)
                //{
                //    //if (tEnd.TodolistModel == true)
                //    //    return null; //表示要结束，因为这是最后一个环节.
                //    return tEnd;
                //}

            // 开始找, 找到当前节点的下一个.
            bool isRec = false;
            foreach (TransferCustom en in ens)
            {
                if (en.FK_Node == currNodeID && en.Worker != BP.Web.WebUser.No)
                {
                    isRec = true;
                    continue;
                }

                if (isRec)
                {
                    /*是否出现*/
                  // en.TodolistModel = true;
                    return en;
                }
            }

            //如果没有找到，就返回最后一个.
            return (TransferCustom)ens[0];
        }
    }
	/// <summary>
	/// 自定义运行路径
	/// </summary>
	public class TransferCustoms: EntitiesMyPK
	{
		#region 方法
		/// <summary>
		/// 得到它的 Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new TransferCustom();
			}
		}
		/// <summary>
        /// 自定义运行路径
		/// </summary>
		public TransferCustoms(){}
		#endregion

        #region 为了适应自动翻译成java的需要,把实体转换成List.
        /// <summary>
        /// 转化成 java list,C#不能调用.
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.IList<TransferCustom> ToJavaList()
        {
            return (System.Collections.Generic.IList<TransferCustom>)this;
        }
        /// <summary>
        /// 转化成list
        /// </summary>
        /// <returns>List</returns>
        public System.Collections.Generic.List<TransferCustom> Tolist()
        {
            System.Collections.Generic.List<TransferCustom> list = new System.Collections.Generic.List<TransferCustom>();
            for (int i = 0; i < this.Count; i++)
            {
                list.Add((TransferCustom)this[i]);
            }
            return list;
        }
        #endregion 为了适应自动翻译成java的需要,把实体转换成List.
	}
}
