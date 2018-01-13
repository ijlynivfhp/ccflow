﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
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
    public class WF_CommEntity : DirectoryPageBase
    {
        /// <summary>
        /// 页面功能实体
        /// </summary>
        /// <param name="mycontext"></param>
        public WF_CommEntity(HttpContext mycontext)
        {
            this.context = mycontext;
        }

        #region 实体的操作.
        public string Entity_Init()
        {
            try
            {
                //初始化entity.
                string enName = this.EnName;
                Entity en = null;
                if (DataType.IsNullOrEmpty(enName) == true)
                {
                    if (DataType.IsNullOrEmpty(this.EnsName) == true)
                        return "err@类名没有传递过来";
                    Entities ens = ClassFactory.GetEns(this.EnsName);
                    en = ens.GetNewEntity;
                }
                else
                {
                    en = ClassFactory.GetEn(this.EnName);
                }

                if (en == null)
                    return "err@参数类名不正确.";

                //获得描述.
                Map map = en.EnMap;
                string pkVal = this.PKVal;
                if (DataType.IsNullOrEmpty(pkVal) == false)
                {
                    en.PKVal = pkVal;
                    en.RetrieveFromDBSources();
                }
                else
                {
                    foreach (Attr attr in en.EnMap.Attrs)
                        en.SetValByKey(attr.Key, attr.DefaultVal);
                    //设置默认的数据.
                    en.ResetDefaultVal();
                }

                //定义容器.
                DataSet ds = new DataSet();

                //定义Sys_MapData.
                MapData md = new MapData();
                md.No = this.EnName;
                md.Name = map.EnDesc;

                #region 加入权限信息.
                //把权限加入参数里面.
                if (en.HisUAC.IsInsert)
                    md.SetPara("IsInsert", "1");
                if (en.HisUAC.IsUpdate)
                    md.SetPara("IsUpdate", "1");
                if (en.HisUAC.IsDelete)
                    md.SetPara("IsDelete", "1");
                #endregion 加入权限信息.

                #region 增加 上方法.
                DataTable dtM = new DataTable("dtM");
                dtM.Columns.Add("No");
                dtM.Columns.Add("Title");
                dtM.Columns.Add("Tip");
                dtM.Columns.Add("Visable");

                dtM.Columns.Add("Url");
                dtM.Columns.Add("Target");
                dtM.Columns.Add("Warning");
                dtM.Columns.Add("RefMethodType");
                dtM.Columns.Add("GroupName");
                dtM.Columns.Add("W");
                dtM.Columns.Add("H");
                dtM.Columns.Add("Icon");
                dtM.Columns.Add("IsCanBatch");
                dtM.Columns.Add("RefAttrKey");

                RefMethods rms = map.HisRefMethods;
                foreach (RefMethod item in rms)
                {
                    string myurl = "";
                    if (item.RefMethodType != RefMethodType.Func)
                    {
                        myurl = item.Do(null) as string;
                        if (myurl == null)
                            continue;
                    }
                    else
                    {
                        myurl = "../RefMethod.htm?Index=" + item.Index + "&EnsName=" + en.GetNewEntities.ToString() + "&PK=" + this.PKVal;
                    }

                    DataRow dr = dtM.NewRow();

                    dr["No"] = item.Index;
                    dr["Title"] = item.Title;
                    dr["Tip"] = item.ToolTip;
                    dr["Visable"] = item.Visable;
                    dr["Warning"] = item.Warning;
                    dr["RefMethodType"] = (int)item.RefMethodType;
                    dr["RefAttrKey"] = item.RefAttrKey;
                    dr["URL"] = myurl;
                    dr["W"] = item.Width;
                    dr["H"] = item.Height;
                    dr["Icon"] = item.Icon;
                    dr["IsCanBatch"] = item.IsCanBatch;
                    dr["GroupName"] = item.GroupName;

                    dtM.Rows.Add(dr); //增加到rows.
                }
                #endregion 增加 上方法.

                #region 加入一对多的实体编辑
                AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
                string sql = "";
                int i = 0;
                if (oneVsM.Count > 0)
                {
                    foreach (AttrOfOneVSM vsM in oneVsM)
                    {
                        //判断该dot2dot是否显示？
                        Entity enMM = vsM.EnsOfMM.GetNewEntity;
                        enMM.SetValByKey(vsM.AttrOfOneInMM, this.PKVal);
                        if (enMM.HisUAC.IsView == false)
                            continue;
                        DataRow dr = dtM.NewRow();
                        dr["No"] = enMM.ToString();
                       // dr["GroupName"] = vsM.GroupName;
                        if (en.PKVal != null)
                        {
                            //判断模式.
                            string url = "";
                            if (vsM.Dot2DotModel == Dot2DotModel.TreeDept)
                                url = "Dot2DotTreeDeptModel.htm?EnsName=" + en.GetNewEntities.ToString() + "&EnName=" + this.EnName + "&AttrKey=" + vsM.EnsOfMM.ToString();
                            else if (vsM.Dot2DotModel == Dot2DotModel.TreeDeptEmp)
                                url = "Dot2DotTreeDeptEmpModel.htm?EnsName=" + en.GetNewEntities.ToString() + "&EnName=" + this.EnName + "&AttrKey=" + vsM.EnsOfMM.ToString();
                            else
                            {
                                // url = "Dot2Dot.aspx?EnsName=" + en.GetNewEntities.ToString() + "&EnName=" + this.EnName + "&AttrKey=" + vsM.EnsOfMM.ToString();
                                url = "Dot2Dot.htm?EnName=" + this.EnName + "&Dot2DotEnsName=" + vsM.EnsOfMM.ToString();
                                url += "&AttrOfOneInMM=" + vsM.AttrOfOneInMM; //存储表那个与主表关联. 比如: FK_Node
                                url += "&AttrOfMInMM=" + vsM.AttrOfMInMM; //dot2dot存储表那个与实体表.  比如:FK_Station.
                                url += "&EnsOfM=" + vsM.EnsOfM.ToString(); //默认的B实体分组依据.  比如:FK_Station.
                                url += "&DefaultGroupAttrKey=" + vsM.DefaultGroupAttrKey; //默认的B实体分组依据.  比如:FK_Station.

                                //+"&RefAttrEnsName=" + vsM.EnsOfM.ToString();
                                //url += "&RefAttrKey=" + vsM.AttrOfOneInMM + "&RefAttrEnsName=" + vsM.EnsOfM.ToString();
                            }

                            dr["URL"] = url + "&" + en.PK + "=" + en.PKVal + "&PKVal=" + en.PKVal;
                            dr["Icon"] = "../Img/M2M.png"; 
                            
                        }

                        dr["W"] = "900";
                        dr["H"] = "500";
                        dr["RefMethodType"] = (int)RefMethodType.RightFrameOpen;


                        // 获得选择的数量.
                        try
                        {
                            sql = "SELECT COUNT(*) as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "='" + en.PKVal + "'";
                            i = DBAccess.RunSQLReturnValInt(sql);
                        }
                        catch
                        {
                            sql = "SELECT COUNT(*) as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "=" + en.PKVal;
                            try
                            {
                                i = DBAccess.RunSQLReturnValInt(sql);
                            }
                            catch
                            {
                                vsM.EnsOfMM.GetNewEntity.CheckPhysicsTable();
                            }
                        }
                        dr["Title"] = vsM.Desc + "(" + i + ")";
                        dtM.Rows.Add(dr);
                    }
                }
                #endregion 增加 一对多.

                #region 从表
                EnDtls enDtls = en.EnMap.Dtls;
                foreach (EnDtl enDtl in enDtls)
                {
                    //判断该dtl是否要显示?
                    Entity myEnDtl = enDtl.Ens.GetNewEntity; //获取他的en
                    myEnDtl.SetValByKey(enDtl.RefKey, this.PKVal);  //给refpk赋值
                    if (myEnDtl.HisUAC.IsView == false)
                        continue;

                    DataRow dr = dtM.NewRow();
                    string url = "Dtl.aspx?EnName=" + this.EnName + "&PK=" + this.PKVal + "&EnsName=" + enDtl.EnsName + "&RefKey=" + enDtl.RefKey + "&RefVal=" + en.PKVal.ToString() + "&MainEnsName=" + en.ToString() ;
                    try
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "='" + en.PKVal + "'");
                    }
                    catch
                    {
                        try
                        {
                            i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "=" + en.PKVal);
                        }
                        catch
                        {
                            enDtl.Ens.GetNewEntity.CheckPhysicsTable();
                        }
                    }
                    dr["No"] = enDtl.EnsName;
                    dr["Title"] = enDtl.Desc+"("+i+")";
                    dr["Url"] = url;
                    dr["GroupName"] = enDtl.GroupName;
                    dtM.Rows.Add(dr);
                }
                ds.Tables.Add(dtM); //
                #endregion 增加 从表.


                ds.Tables.Add(md.ToDataTableField("Sys_MapData"));

                //把主数据放入里面去.
                DataTable dtMain = en.ToDataTableField("MainTable");
                ds.Tables.Add(dtMain);

                #region 增加上分组信息.
                EnCfg ec = new EnCfg(this.EnName);
                string groupTitle = ec.GroupTitle;
                if (DataType.IsNullOrEmpty(groupTitle) == true)
                    groupTitle = "@" + en.PK + ",基本信息," + map.EnDesc + "";

                //增加上.
                DataTable dtGroups = new DataTable("Sys_GroupField");
                dtGroups.Columns.Add("OID");
                dtGroups.Columns.Add("Lab");
                dtGroups.Columns.Add("Tip");
                dtGroups.Columns.Add("CtrlType");
                dtGroups.Columns.Add("CtrlID");

                string[] strs = groupTitle.Split('@');
                foreach (string str in strs)
                {
                    if (DataType.IsNullOrEmpty(str))
                        continue;

                    string[] vals = str.Split('=');
                    if (vals.Length == 1)
                        vals = str.Split(',');

                    if (vals.Length == 0)
                        continue;

                    DataRow dr = dtGroups.NewRow();
                    dr["OID"] = vals[0];
                    dr["Lab"] = vals[1];
                    if (vals.Length == 3)
                        dr["Tip"] = vals[2];
                    dtGroups.Rows.Add(dr);
                }
                ds.Tables.Add(dtGroups);

                #endregion 增加上分组信息.

                #region 字段属性.
                MapAttrs attrs = en.EnMap.Attrs.ToMapAttrs;
                DataTable sys_MapAttrs = attrs.ToDataTableField("Sys_MapAttr");
                sys_MapAttrs.Columns.Remove(MapAttrAttr.GroupID);
                sys_MapAttrs.Columns.Add("GroupID");

                //sys_MapAttrs.Columns[MapAttrAttr.GroupID].DataType = typeof(string); //改变列类型.

                //给字段增加分组.
                string currGroupID = "";
                foreach (DataRow drAttr in sys_MapAttrs.Rows)
                {
                    if (currGroupID.Equals("") == true)
                        currGroupID = dtGroups.Rows[0]["OID"].ToString();

                    string keyOfEn = drAttr[MapAttrAttr.KeyOfEn].ToString();
                    foreach (DataRow drGroup in dtGroups.Rows)
                    {
                        string field = drGroup["OID"].ToString();
                        if (keyOfEn.Equals(field))
                        {
                            currGroupID = field;
                        }
                    }
                    drAttr[MapAttrAttr.GroupID] = currGroupID;
                }
                ds.Tables.Add(sys_MapAttrs);
                #endregion 字段属性.

                #region 把外键与枚举放入里面去.
                foreach (DataRow dr in sys_MapAttrs.Rows)
                {
                    string uiBindKey = dr["UIBindKey"].ToString();
                    string lgType = dr["LGType"].ToString();
                    if (lgType != "2")
                        continue;

                    string UIIsEnable = dr["UIVisible"].ToString();
                    if (UIIsEnable == "0")
                        continue;

                    if (string.IsNullOrEmpty(uiBindKey) == true)
                    {
                        string myPK = dr["MyPK"].ToString();
                        /*如果是空的*/
                        //   throw new Exception("@属性字段数据不完整，流程:" + fl.No + fl.Name + ",节点:" + nd.NodeID + nd.Name + ",属性:" + myPK + ",的UIBindKey IsNull ");
                    }

                    // 检查是否有下拉框自动填充。
                    string keyOfEn = dr["KeyOfEn"].ToString();
                    string fk_mapData = dr["FK_MapData"].ToString();


                    // 判断是否存在.
                    if (ds.Tables.Contains(uiBindKey) == true)
                        continue;

                    ds.Tables.Add(BP.Sys.PubClass.GetDataTableByUIBineKey(uiBindKey));
                }

                string enumKeys = "";
                foreach (Attr attr in map.Attrs)
                {
                    if (attr.MyFieldType == FieldType.Enum)
                    {
                        enumKeys += "'" + attr.UIBindKey + "',";
                    }
                }

                if (enumKeys.Length > 2)
                {
                    enumKeys = enumKeys.Substring(0, enumKeys.Length - 1);
                    // Sys_Enum
                    string sqlEnum = "SELECT * FROM Sys_Enum WHERE EnumKey IN (" + enumKeys + ")";
                    DataTable dtEnum = DBAccess.RunSQLReturnTable(sqlEnum);
                    dtEnum.TableName = "Sys_Enum";
                    ds.Tables.Add(dtEnum);
                }

                #endregion 把外键与枚举放入里面去.

                return BP.Tools.Json.ToJson(ds);
            }
            catch (Exception ex)
            {
                return "err@" + ex.Message;
            }
        }
        #endregion 实体的操作.


        /// <summary>
        /// 执行保存
        /// </summary>
        /// <returns></returns>
        public string Dot2Dot_Save()
        {

            try
            {
                string eles = this.GetRequestVal("Eles");

                //实体集合.
                string dot2DotEnsName = this.GetRequestVal("Dot2DotEnsName");
                string attrOfOneInMM = this.GetRequestVal("AttrOfOneInMM");
                string attrOfMInMM = this.GetRequestVal("AttrOfMInMM");

                //获得点对点的实体.
                Entity en = ClassFactory.GetEns(dot2DotEnsName).GetNewEntity;
                en.Delete(attrOfOneInMM, this.PKVal); //首先删除.

                string[] strs = eles.Split(',');
                foreach (string str in strs)
                {
                    if (DataType.IsNullOrEmpty(str) == true)
                        continue;

                    en.SetValByKey(attrOfOneInMM, this.PKVal);
                    en.SetValByKey(attrOfMInMM, str);
                    en.Insert();
                }
                return "数据保存成功.";
            }
            catch (Exception ex)
            {
                return "err@"+ex.Message;
            }
        }
        /// <summary>
        /// 获得分组的数据源
        /// </summary>
        /// <returns></returns>
        public string Dot2Dot_GenerGroupEntitis()
        {
            string key = this.GetRequestVal("DefaultGroupAttrKey");

            //实体集合.
            string ensName = this.GetRequestVal("EnsOfM");
            Entities ens = ClassFactory.GetEns(ensName);
            Entity en = ens.GetNewEntity;

            Attrs attrs = en.EnMap.Attrs;
            Attr attr = attrs.GetAttrByKey(key);

            if (attr == null)
                return "err@设置的分组外键错误[" + key + "],不存在[" + ensName + "]或者已经被删除.";

            if (attr.MyFieldType == FieldType.Normal)
                return "err@设置的默认分组["+key+"]不能是普通字段.";

            if (attr.MyFieldType == FieldType.FK)
            {
                Entities ensFK = attr.HisFKEns;
                ensFK.Clear();
                ensFK.RetrieveAll();
                return ensFK.ToJson();
            }

            if (attr.MyFieldType == FieldType.Enum)
            {
                /* 如果是枚举 */
                SysEnums ses = new SysEnums();
                ses.Retrieve(SysEnumAttr.IntKey, attr.UIBindKey);

                //ses.ToStringOfSQLModelByKey

                BP.Pub.NYs nys = new Pub.NYs();
                foreach (SysEnum item in ses)
                {
                    BP.Pub.NY ny =new Pub.NY();
                    ny.No = item.IntKey.ToString();
                    ny.Name = item.Lab;
                    nys.AddEntity(ny);
                }
                return nys.ToJson();
            }

            return "err@设置的默认分组[" + key + "]不能是普通字段.";
        }
    }
}
