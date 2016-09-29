﻿using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Text;
using System.Configuration;
using System.Web.SessionState;
using BP.DA;
using BP.Web;
using BP.BPMN;
using BP.Sys;
using BP.En;
using BP.WF.Template;
using System.Collections.Generic;
using BP.WF;
using LitJson;
using CCFlow.ViewModels;
using Newtonsoft.Json.Utilities;

namespace CCFlow.WF.Admin.CCFormDesigner
{
    /// <summary>
    /// SFTableHandler 的摘要说明
    /// </summary>
    public class SFTableHandler : IHttpHandler
    {
        #region 属性.
        public string FK_SFTable
        {
            get
            {
                return context.Request["FK_SFTable"].ToString();
            }            
        }
        public string DoType
        {
            get
            {
                return context.Request["DoType"].ToString();
            }
        }
        #endregion 


        public HttpContext context=null;
        public void ProcessRequest(HttpContext _context)
        {
            context = _context;

            string json = "";

            switch (this.DoType)
            {
                case "CreateTableDataInit":
                    WriteInfo( CreateTableDataInit()); //输出数据.
                    break;
                case "CreateTableDataSave":
                    {
                        using (StreamReader reader = new System.IO.StreamReader(context.Request.InputStream))
                        {
                            json = reader.ReadToEnd();
                        }

                        WriteInfo( CreateTableDataSave(json)); //输出保存结果..
                        break;
                    }
                case "CreateTreeDataInit":
                    {
                        WriteInfo(CreateTreeDataInit()); //输出数据.
                        break;
                    }
                case "CreateTreeDataSave": 
                    {
                        using (StreamReader reader = new System.IO.StreamReader(context.Request.InputStream))
                        {
                            json = reader.ReadToEnd();
                        }
                        WriteInfo(CreateTreeDataSave(json));
                        break;
                    }
                default:
                    break;
            }
        }

        public string CreateTreeDataInit()
        {
            if (String.IsNullOrEmpty(this.FK_SFTable))
                return "";

            //verifyTable();

            SFTable sf = new SFTable(this.FK_SFTable);
            
            string sql = "SELECT * FROM " + sf.No;
            DataTable dt = sf.RunSQLReturnTable(sql);

            string json = "";
            //List<CodeItem> items = new List<CodeItem>();

            if (dt != null && dt.Rows.Count > 0)
            {
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        if (dt.Rows[i] != null)
            //        {
            //            items.Add(new CodeItem()
            //            {
            //                ID = dt.Rows[i]["No"].ToString(),
            //                Value = dt.Rows[i]["Name"].ToString(),
            //                Parent = dt.Columns.Contains("ParentNo") ? dt.Rows[i]["ParentNo"].ToString() : dt.Rows[i]["No"].ToString()
            //            });
            //        }
            //    }

                //return BP.Tools.Json.ToJson(models.ToArray());

                //CodeItem[] treeItems = items.ToArray();

                //treeItems = this.buildTreeItems(treeItems);

                //json = Newtonsoft.Json.JsonConvert.SerializeObject(treeItems);

                //return json;
            }
            else
            {
                dt = sf.GenerData();
                //return BP.Tools.Json.ToJson(dt);

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (dt.Rows[i] != null)
                //    {
                //        items.Add(new CodeItem()
                //        {
                //            ID = dt.Rows[i]["No"].ToString(),
                //            Value = dt.Rows[i]["Name"].ToString(),
                //            Parent = dt.Columns.Contains("ParentNo") ? dt.Rows[i]["ParentNo"].ToString() : dt.Rows[i]["No"].ToString()
                //        });
                //    }
                //}

                ////return BP.Tools.Json.ToJson(models.ToArray());
                //CodeItem[] treeItems = items.ToArray();

                //treeItems = this.buildTreeItems(treeItems);

                //json = Newtonsoft.Json.JsonConvert.SerializeObject(treeItems);

                //foreach (var item in items)
                //{
                //    sql = "INSERT INTO " + this.FK_SFTable + " (No,Name)Values('" + item.ID + "','" + item.Value + "')";
                //    sf.RunSQL(sql);
                //}
            }

            json = BP.Tools.Json.ToJson(dt);

            //json = "[{\"No\":\"1\",\"Name\":\"CN_City-Item-1\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"1\"},{\"No\":\"2\",\"Name\":\"CN_City-Item-2\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"1\"},{\"No\":\"3\",\"Name\":\"CN_City-Item-3\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"1\"},{\"No\":\"4\",\"Name\":\"CN_City-Item-4\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"2\"},{\"No\":\"5\",\"Name\":\"CN_City-Item-5\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"1\"},{\"No\":\"6\",\"Name\":\"CN_City-Item-6\",\"Names\":\"\",\"Grade\":0,\"FK_SF\":\"\",\"FK_PQ\":\"\", \"Parent\":\"2\"}]";

            //json = "[{id: \"1\", value: \"1\", parent: \"\", children:[{id: \"11\", value: \"11\", parent: \"1\", children:[{ id: \"1111\", value: \"1111\", parent: \"11\", children: [] },{ id: \"1112\", value: \"1112\", parent: \"11\", children: [] },{ id: \"1113\", value: \"1113\", parent: \"11\", children: [] }]},{id: \"12\", value: \"12\", parent: \"1\", children:[{ id: \"1211\", value: \"1211\", parent: \"12\", children: [] },{ id: \"1212\", value: \"1212\", parent: \"12\", children: [] },{ id: \"1213\", value: \"1213\", parent: \"12\", children: [] }]},{ id: \"13\", value: \"13\", parent: \"1\", children: [] }},{id: \"2\", value: \"2\", parent: \"\", children:[{ id: \"21\", value: \"21\", parent: \"2\", children: [] },{ id: \"22\", value: \"22\", parent: \"2\", children: [] },{ id: \"23\", value: \"23\", parent: \"2\", children: [] }]},{id: \"3\", value: \"3\", parent: \"\", children:[{ id: \"31\", value: \"31\", parent: \"3\", children: [] },{ id: \"32\", value: \"32\", parent: \"3\", children: [] },{ id: \"33\", value: \"33\", parent: \"3\", children: [] }]}]";

            return json;

            //string json = "";
            //List<CodeItem> items = new List<CodeItem>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i] != null)
            //    {
            //        items.Add(new CodeItem()
            //        {
            //            ID = dt.Rows[i]["No"].ToString(),
            //            Value = dt.Rows[i]["Name"].ToString(),
            //            Parent = dt.Columns.Contains("ParentNo") ? dt.Rows[i]["ParentNo"].ToString() : ""
            //        });
            //    }
            //}
            //CodeItem[] treeItems = this.buildTreeItems(items.ToArray());
            //json = Newtonsoft.Json.JsonConvert.SerializeObject(treeItems);
            //return json;
        }

        private void verifyTable() 
        {
            string connectionString = ConfigurationManager.AppSettings.Get("AppCenterDSN");
            string dbType = ConfigurationManager.AppSettings.Get("AppCenterDBType");

            if (dbType.ToLower() == "mssql")
            {
                string sqlCmdTxtCreateTB = String.Format("CREATE TABLE {0} (No nvarchar(30) NOT NULL, Name nvarchar(60) NULL, ParentNo nvarchar(30) NULL CONSTRAINT {1}pk PRIMARY KEY CLUSTERED (No ASC))", this.FK_SFTable, this.FK_SFTable);

                using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string sqlCmdTxtGetTB = String.Format("select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_CATALOG = '{0}' and TABLE_NAME = '{1}'", sqlConn.Database, this.FK_SFTable);

                    System.Data.SqlClient.SqlCommand sqlCmd = new System.Data.SqlClient.SqlCommand(sqlCmdTxtGetTB, sqlConn);

                    if (sqlConn.State != ConnectionState.Open)
                    {
                        sqlConn.Open();
                    }

                    string tableName = "";

                    using (System.Data.SqlClient.SqlDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                    {
                        while (reader.Read())
                        {
                            tableName = reader.GetString(0);
                        }
                    }

                    if (String.IsNullOrEmpty(tableName))
                    {
                        sqlCmd = new System.Data.SqlClient.SqlCommand(sqlCmdTxtCreateTB, sqlConn);
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private CodeItem[] buildTreeItems(CodeItem[] codeItems) 
        {
            List<CodeItem> nodes = new List<CodeItem>(codeItems);

            List<CodeItem> parentNodes = new List<CodeItem>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ID.ToLower() == nodes[i].Parent.ToLower())
                {
                    parentNodes.Add(nodes[i]);
                    nodes.RemoveAt(i);
                    i--;
                }
            }

            Dictionary<string, List<CodeItem>> childNodes = new Dictionary<string, List<CodeItem>>();

            for (int i = 0; i < parentNodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (nodes[j].Parent.ToLower() == parentNodes[i].ID.ToLower())
                    {
                        if (childNodes.ContainsKey(parentNodes[i].ID.ToLower()))
                        {
                            childNodes.Add(parentNodes[i].ID.ToLower(), new List<CodeItem>(new CodeItem[] { nodes[j] }));
                            nodes.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            childNodes[parentNodes[i].ID.ToLower()].Add(nodes[j]);
                            j--;
                        }
                    }
                }
            }

            for (int i = 0; i < parentNodes.Count; i++)
            {
                if (childNodes.ContainsKey(parentNodes[i].ID.ToLower()) && childNodes[parentNodes[i].ID.ToLower()] != null)
                {
                    parentNodes[i].Children = childNodes[parentNodes[i].ID.ToLower()].ToArray();
                }
            }

            childNodes = new Dictionary<string, List<CodeItem>>();

            foreach (var parentNode in parentNodes)
            {
                if (parentNode.Children != null)
                {
                    for (int i = 0; i < parentNode.Children.Length; i++)
                    {
                        for (int j = 0; j < nodes.Count; j++)
                        {
                            if (nodes[j].Parent.ToLower() == parentNode.Children[i].ID.ToLower())
                            {
                                if (childNodes.ContainsKey(parentNode.Children[i].ID.ToLower()))
                                {
                                    childNodes.Add(parentNode.Children[i].ID.ToLower(), new List<CodeItem>(new CodeItem[] { nodes[j] }));
                                    nodes.RemoveAt(j);
                                    j--;
                                }
                                else
                                {
                                    childNodes[parentNode.Children[i].ID.ToLower()].Add(nodes[j]);
                                    j--;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var parentNode in parentNodes)
            {
                if (parentNode.Children != null)
                {
                    foreach (var childNode in parentNode.Children)
                    {
                        if (childNodes.ContainsKey(childNode.ID.ToLower()) && childNodes[childNode.ID.ToLower()] != null)
                        {
                            childNode.Children = childNodes[parentNode.ID.ToLower()].ToArray();
                        }
                    }
                }
            }

            for (int i = 0; i < parentNodes.Count; i++)
            {
                if (parentNodes[i].Children != null)
                {
                    for (int j = 0; j < parentNodes[i].Children.Length; j++)
                    {
                        if (childNodes.ContainsKey(parentNodes[i].Children[j].ID.ToLower()) && childNodes[parentNodes[i].Children[j].ID.ToLower()] != null)
                        {
                            parentNodes[i].Children[j].Children = childNodes[parentNodes[i].ID.ToLower()].ToArray();
                        }
                    }
                }
            }

            return parentNodes.ToArray();
        }
        /// <summary>
        /// 保存树
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string CreateTreeDataSave(string json)
        {
            DataTable dt = BP.Tools.Json.ToDataTable(json);
            if (dt.Rows.Count == 0)
                return "err@数据错误,保存的值为空.";

            SFTable sf = new SFTable(this.FK_SFTable);

            //原来的数据.
            DataTable dtSrc = sf.GenerData();
            string pks = ",";
            foreach (DataRow dr in dt.Rows)
                pks += dr["GUID"] + ",";

            string sql = "";
            int insertNum = 0;
            int updateNum = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string no = dr["No"].ToString();
                string name = dr["Name"].ToString();
                string parentNo = dr["ParentNo"].ToString();
                string guid = dr["GUID"].ToString();

                if (string.IsNullOrEmpty(guid))
                {
                    sql = "INSERT INTO " + sf.SrcTable + " (No,Name,ParentNo,GUID)VALUES('" + no + "','" + name + "','" + parentNo + "','" + BP.DA.DBAccess.GenerGUID() + "')";
                    sf.RunSQL(sql);
                    insertNum++;
                }
                else
                {
                    pks = pks.Replace(guid + ",", ""); //替换下来.
                    sql = "UPDATE " + sf.SrcTable + " SET No='" + no + "', Name='" + name + "', ParentNo='" + parentNo + "' WHERE GUID='" + guid + "'";
                    sf.RunSQL(sql);
                    updateNum++;
                }
            }

            //删除没有替换的.
            string[] strs = pks.Split(',');
            int deleteNum = 0;
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                sql = "DELETE FROM " + sf.SrcTable + " WHERE GUID='" + str + "'";
                deleteNum++;
                sf.RunSQL(sql);
            }

            return "保存成功, 更新[" + updateNum + "]条,新建[" + insertNum + "]条,修改[" + updateNum + "]条.";
        }

        public string CreateTableDataInit()
        {
            if (String.IsNullOrEmpty(this.FK_SFTable))
            {
                return "";
            }

            SFTable sf = new SFTable(this.FK_SFTable);
            string sql = "SELECT * FROM "+ sf.No;
            DataTable dt = sf.RunSQLReturnTable(sql);

            string json = "";
            List<CodeItem> items = new List<CodeItem>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i] != null)
                    {
                        items.Add(new CodeItem()
                        {
                            ID = dt.Rows[i]["No"].ToString(),
                            Value = dt.Rows[i]["Name"].ToString(),
                            Parent = dt.Columns.Contains("ParentNo") ? dt.Rows[i]["ParentNo"].ToString() : ""
                        });
                    }
                }

                //return BP.Tools.Json.ToJson(models.ToArray());

                json = Newtonsoft.Json.JsonConvert.SerializeObject(items.ToArray());

                //return json;
            }
            else//如果表中没有数据，则自动向该表插入3条默认数据
            {
                //items.AddRange(new CodeItem[] 
                //{
                //    new CodeItem()
                //    { 
                //        ID = String.Format("{0}-Item-001", this.FK_SFTable), 
                //        Value = String.Format("{0}-Item-001", this.FK_SFTable)
                //    },
                //    new CodeItem()
                //    {
                //        ID = String.Format("{0}-Item-002", this.FK_SFTable), 
                //        Value = String.Format("{0}-Item-002", this.FK_SFTable)
                //    },
                //    new CodeItem()
                //    {
                //        ID = String.Format("{0}-Item-003", this.FK_SFTable), 
                //        Value = String.Format("{0}-Item-003", this.FK_SFTable)
                //    }
                //});

                for (int i = 0; i < 3; i++)
                {
                    items.Add(new CodeItem()
                    { 
                        ID = String.Format("{0}", (i + 1)),
                        Value = String.Format("{0}-Item-{1}", this.FK_SFTable, (i + 1))
                    });
                }

                json = Newtonsoft.Json.JsonConvert.SerializeObject(items.ToArray());

                foreach (var item in items)
                {
                    sql = "INSERT INTO " + this.FK_SFTable + " (No,Name)Values('" + item.ID + "','" + item.Value + "')";
                    sf.RunSQL(sql);
                }
            }

            return json;

            //return BP.Tools.Json.ToJson(dt);
        }

        public void WriteInfo(string info)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(info);
        }
        /// <summary>
        /// 执行保存.
        /// </summary>
        /// <returns></returns>
        public string CreateTableDataSave(string json= "@001=xxxx@002=xxxxx")
        {
            CodeItem[] items = Newtonsoft.Json.JsonConvert.DeserializeObject<CodeItem[]>(json);
            if (items.Length <= 0)
            {
                return "err@数据错误,保存的值为空.";
            }

            if (String.IsNullOrEmpty(this.FK_SFTable))
            {
                return "err@参数错误.";
            }

            //删除原来的数据.
            BP.Sys.SFTable sf = new BP.Sys.SFTable(this.FK_SFTable);
            sf.RunSQL("DELETE FROM " + sf.No);

            string sql = "";
            foreach (var item in items)
            {
                if (!String.IsNullOrEmpty(sf.ParentValue))
                {
                    sql = String.Format("INSERT INTO {0} (No, Name, {1}) Values ('{2}', '{3}', '{4}')", this.FK_SFTable, sf.ParentValue, item.ID, item.Value, item.Parent);
                }
                else
                {
                    sql = String.Format("INSERT INTO {0} (No, Name) Values ('{1}', '{2}')", this.FK_SFTable, item.ID, item.Value);
                }
                sf.RunSQL(sql);
            }

            return "保存成功";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}


namespace CCFlow.ViewModels
{
    public class CodeItem
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "parent")]
        public string Parent { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "children")]
        public CodeItem[] Children { get; set; }
    }
}