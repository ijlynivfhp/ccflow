﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Collections;
using System.Text;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace BP.Tools
{
     
    public class Json
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string Hastable2Json(Hashtable ht)
        {
            return ToJson(ht, false);
        }
        /// <summary>
        /// 把一个json转化一个datatable
        /// </summary>
        /// <param name="json">一个json字符串</param>
        /// <returns>序列化的datatable</returns>
        public static DataTable ToDataTable(string strJson)
        {
            if (strJson.Trim().IndexOf('[') != 0)
            {
                strJson = "[" + strJson + "]";
            }
            DataTable dtt = (DataTable)JsonConvert.DeserializeObject<DataTable>(strJson);
            return dtt;

            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();

            //取出表名  
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名  
            try
            {
                strJson = strJson.Substring(strJson.IndexOf("[") + 1);
                strJson = strJson.Substring(0, strJson.IndexOf("]"));
            }
            catch (Exception ex)
            {

            }
            //获取数据  
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');
                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        /// <summary>
        /// 把一个json转化一个datatable 杨玉慧
        /// </summary>
        /// <param name="json">一个json字符串</param>
        /// <returns>序列化的datatable</returns>
        public static DataTable ToDataTableOneRow(string strJson)
        {
            ////杨玉慧  写  用这个写
            if (strJson.Trim().IndexOf('[') != 0)
            {
                strJson = "[" + strJson + "]";
            }
            DataTable dtt = (DataTable)JsonConvert.DeserializeObject<DataTable>(strJson);
            
            return dtt;

            //转换json格式
            
            //把 *  和# 先替换成别的符号
            string str1 = "@@@~~~+++";
            string str2 = "+++---$$$";
            strJson = strJson.Replace(str1, "");
            strJson = strJson.Replace(str2, "");

            //strJson = strJson.Replace("*", str1);
            //strJson = strJson.Replace("#", str2);

            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();


            //取出表名  
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名  
            try
            {
                strJson = strJson.Substring(strJson.IndexOf("[") + 1);
                strJson = strJson.Substring(0, strJson.IndexOf("]"));
            }
            catch (Exception ex)
            {

            }
            //获取数据  
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        if (str.Contains("#"))
                        {
                            var dc = new DataColumn();
                            string[] strCell = str.Split('#');
                            string columnName = string.Empty;
                            if (strCell[0].Substring(0, 1) == "\"")
                            {
                                int a = strCell[0].Length;
                                columnName = strCell[0].Substring(1, a - 2);
                            }
                            else
                            {
                                columnName = strCell[0];
                            }
                            columnName = columnName.Replace(str1, "*").Replace(str1, "#");
                            dc.ColumnName = columnName;

                            tb.Columns.Add(dc);
                        }
                        else
                        {
                            var dc = new DataColumn();
                            dc.ColumnName = "无" + i;
                            tb.Columns.Add(dc);
                        }
                    }
                    tb.AcceptChanges();
                }
                //增加内容  
                DataRow dr = tb.NewRow();
                string content = string.Empty;
                for (int r = 0; r < strRows.Length; r++)
                {
                    if (strRows[r].Contains("#"))
                    {
                        content = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                        content = content.Replace(str1, "*").Replace(str1, "#");

                        dr[r] = content;
                    }
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        /// <summary>
        /// 把一个json转化一个datatable
        /// </summary>
        /// <param name="json">一个json字符串</param>
        /// <returns>序列化的datatable</returns>
        public static DataSet ToDataSet(string json)
        {
            ////杨玉慧  写  用这个写
            DataSet ds2 = JsonConvert.DeserializeObject<DataSet>(json);
            return ds2;

            DataSet ds = new DataSet();
            return ds;
        }
        /// <summary>
        /// 对象转换为Json字符串
        /// </summary>
        /// <param name="jsonObject">对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(object jsonObject)
        {
            string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            return json;

            string jsonString = "{";
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                string value = string.Empty;
                if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                {
                    value = "'" + objectValue.ToString() + "'";
                }
                else if (objectValue is string)
                {
                    value = "'" + ToJson(objectValue.ToString()) + "'";
                }
                else if (objectValue is IEnumerable)
                {
                    value = ToJson((IEnumerable)objectValue);
                }
                else
                {
                    value = ToJson(objectValue.ToString());
                }
                jsonString += "\"" + ToJson(propertyInfo[i].Name) + "\":" + value + ",";
            }
            return Json.DeleteLast(jsonString) + "}";
        }
        /// <summary>
        /// 对象集合转换Json
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonStr = JsonConvert.SerializeObject(array);
            return jsonStr;

            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += Json.ToJson(item) + ",";
            }
            return Json.DeleteLast(jsonString) + "]";
        }
        
        /// <summary>
        /// 普通集合转换Json
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <returns>Json字符串</returns>
        public static string ToArrayString(IEnumerable array)
        {
            string jsonStr = JsonConvert.SerializeObject(array);
            return jsonStr;

            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            return Json.DeleteLast(jsonString) + "]";
        }
        /// <summary>
        /// 删除结尾字符
        /// </summary>
        /// <param name="str">需要删除的字符</param>
        /// <returns>完成后的字符串</returns>
        private static string DeleteLast(string str)
        {

            if (str.Length > 1)
            {
                return str.Substring(0, str.Length - 1);
            }
            return str;
        }
        /// <summary>
        /// 转化成Json.
        /// </summary>
        /// <param name="ht">Hashtable</param>
        /// <param name="isNoNameFormat">是否编号名称格式</param>
        /// <returns></returns>
        public static string ToJson(Hashtable ht, bool isNoNameFormat)
        {


            if (isNoNameFormat == true)
            {
                /*如果是datatable 模式. */
                DataTable dt = new DataTable();
                dt.TableName = "HT";  //此表名不能修改.
                dt.Columns.Add(new DataColumn("No", typeof(string)));
                dt.Columns.Add(new DataColumn("Name", typeof(string)));
                foreach (string key in ht.Keys)
                {
                    if (key == null || key == "")
                        continue;

                    DataRow dr = dt.NewRow();
                    dr["No"] = key;

                    var v = ht[key] as string;
                    if (v == null)
                        v = "";
                    dr["Name"] = v;
                    dt.Rows.Add(dr);
                }
                return ToJson(dt);
            }

            else {
                DataTable dt = new DataTable();
                foreach (string key in ht.Keys)
                {
                    dt.Columns.Add(key);
                }

                List<object> l=new List<object> ();
                foreach (string key in ht.Keys)
                {
                    l.Add(ht[key]);
                }
                dt.Rows.Add(l.ToArray());
                return ToJson(dt);
            }

            string strs = "{";
            foreach (string key in ht.Keys)
            {
                strs += "\"" + key + "\":\"" + ht[key] + "\",";
            }
            strs += "\"OutEnd\":\"无效参数请忽略\"";
            strs += "}";
            strs = TranJsonStr(strs);
            return strs;
        }
        
        /// <summary>
        /// Datatable转换为Json
        /// </summary>
        /// <param name="table">Datatable对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(DataTable table)
        {
            string jsonStr = JsonConvert.SerializeObject(table);
            return jsonStr;

            string jsonString = "[";
            DataRowCollection drc = table.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString += "{";
                foreach (DataColumn column in table.Columns)
                {
                    jsonString += "\"" + ToJson(column.ColumnName) + "\":";
                    if (column.DataType == typeof(DateTime)
                        || column.DataType == typeof(string)
                        || column.DataType == typeof(bool)
                        || column.DataType == typeof(Boolean))
                    {
                        jsonString += "\"" + ToJson(drc[i][column.ColumnName].ToString()) + "\",";
                    }
                    else
                    {
                        string val = ToJson(drc[i][column.ColumnName].ToString());
                        if (string.IsNullOrEmpty(val) == true)
                            val = "0";

                        jsonString += val + ",";
                    }
                }
                jsonString = DeleteLast(jsonString) + "},";
            }
            return DeleteLast(jsonString) + "]";
        }
        /// <summary>
        /// DataSet转换为Json
        /// </summary>
        /// <param name="dataSet">DataSet对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(DataSet dataSet)
        {
            string jsonStr = JsonConvert.SerializeObject(dataSet);
            return jsonStr;

            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + ToJson(table.TableName) + "\":" + ToJson(table) + ",";
            }
            return jsonString = DeleteLast(jsonString) + "}";
        }
        /// <summary>
        /// String转换为Json
        /// </summary>
        /// <param name="value">String对象</param>
        /// <returns>Json字符串</returns>
        public static string ToJson(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string temstr;
            temstr = value;
            temstr = temstr.Replace("{", "｛").Replace("}", "｝").Replace(":", "：").Replace(",", "，").Replace("[", "【").Replace("]", "】").Replace(";", "；").Replace("\n", "<br/>").Replace("\r", "");
            temstr = temstr.Replace("\t", "   ");
            temstr = temstr.Replace("'", "\'");
            temstr = temstr.Replace(@"\", @"\\");
            temstr = temstr.Replace("\"", "\"\"");
            return temstr;
        }
        /// <summary>
        /// JSON字符串的转义
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        private static string TranJsonStr(string jsonStr) {
            string strs = jsonStr;
            strs = strs.Replace("\\", "\\\\");
            strs = strs.Replace("\n", "\\n");
            strs = strs.Replace("\b", "\\b");
            strs = strs.Replace("\t", "\\t");
            strs = strs.Replace("\f", "\\f");
            strs = strs.Replace("/", "\\/");
            return strs;
        }
    }
}
