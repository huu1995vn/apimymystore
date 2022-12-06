using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using APIMyMyStore;
using APIMyMyStore.Entites;
using APIMyMyStore.Helpers;
using APIMyMyStore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RaoXeAPI.Controllers
{
    [ApiController]
    public class CommonController : ControllerBase
    {
        #region "Variables"
        protected virtual string OrderByGrid => "Id DESC";
        protected virtual string FieldSelect => "*";
        protected virtual List<string> FieldInsert => null;
        protected virtual List<string> FieldUpdate => null;

        protected virtual string[] FieldSearch => new string[] { "KeywordSearch" };

        protected virtual bool IsAddUserIdColumn => false;
        protected virtual string ConditionSearch { get; set; }
        protected virtual string ConditionGetInfo { get; set; }
        protected virtual string ConditionUpdate { get; set; }
        protected virtual string ConditionDelete { get; set; }

        public Action CustomCheckValidSaveData = null;
        public Action<long> CustomCheckValidDeleteData = null;

        protected virtual string TableName => null;
        protected virtual string ViewName => null;

        private DBLibrary.TemplateDAL _dal = null;
        public Action<long> OnAfterSaveData = null;
        public Action<List<long>> OnAfterDeleteData = null;

        public Action<System.Data.DataSet> ConvertDataSet = null;

        #endregion

        #region "Common Methods"

        protected DBLibrary.TemplateDAL GetTemplateDAL()
        {
            if (this._dal == null)
            {
                this._dal = new DBLibrary.TemplateDAL(Variables.ConnectionSQL);
            }
            return this._dal;
        }

        protected DBLibrary.TemplateDAL GetTemplateDAL(string pTableName)
        {
            if (this._dal == null)
            {
                this._dal = new DBLibrary.TemplateDAL(Variables.ConnectionSQL, pTableName);
            }
            else
            {
                this._dal.TableName = pTableName;
            }
            return this._dal;
        }


        [Route("OK")]
        public OkObjectResult Ok(Func<object> pMethod)
        {
            APIResult res = new APIResult();
            try
            {
                res.SetResult(pMethod());
            }
            catch (Exception ex)
            {
                res.SetException(ex);
            }
            return base.Ok(res);
        }

        protected List<object> GetDataListToSave(JObject pData, List<string> pFields)
        {
            string fieldName;
            List<object> lstDatas = new List<object>();
            for (int i = 0; i < pFields.Count; i++)
            {
                fieldName = pFields[i];

                if (pData.ContainsKey(fieldName))
                {
                    lstDatas.Add(pData[fieldName] == null ? DBNull.Value : pData[fieldName]);
                }
                else
                {
                    lstDatas.Add(DBNull.Value);
                }
            }
            return lstDatas;
        }
        protected long SaveDataTable(JObject pData, string pTableName, List<string> pSaveFields, bool pIsAddUserId, string pConditionUpdate)
        {
            long res;
            long pId = CommonMethods.ConvertToInt64(pData["Id"]);
            if (pId > 0)
            {
                if (pIsAddUserId)
                {
                    if (!pSaveFields.Contains("UpdateUserId"))
                    {
                        pSaveFields.Add("UpdateUserId");
                    }
                    if (!pSaveFields.Contains("UpdateDate"))
                    {
                        pSaveFields.Add("UpdateDate");
                    }
                }
                DBLibrary.TemplateDAL dal = GetTemplateDAL(pTableName);
                dal.ID = "Id";
                if (!string.IsNullOrEmpty(pConditionUpdate))
                {
                    res = dal.Update(pId, pSaveFields, GetDataListToSave(pData, pSaveFields), pConditionUpdate);
                }
                else
                {
                    res = dal.Update(pId, pSaveFields, GetDataListToSave(pData, pSaveFields));
                }
            }
            else
            {
                if (pIsAddUserId)
                {
                    if (!pSaveFields.Contains("CreateUserId"))
                    {
                        pSaveFields.Add("CreateUserId");
                    }
                }

                res = GetTemplateDAL(pTableName).Insert(pSaveFields, GetDataListToSave(pData, pSaveFields));
            }
            return res;
        }
        protected virtual string GetCondition(JObject pFilters)
        {
            List<string> lstCondition = new List<string>();
            if (!string.IsNullOrEmpty(ConditionSearch))
            {
                lstCondition.Add(ConditionSearch);
            }
            if (pFilters != null)
            {
                foreach (var x in pFilters)
                {
                    string key = x.Key;
                    long value = CommonMethods.ConvertToInt64(x.Value);
                    if (value > -1)
                    {
                        lstCondition.Add(string.Format("{0}={1}", key, value));
                    }
                }
            }
            return string.Join(" AND ", lstCondition);
        }
        protected virtual string GetConditionParameters(JObject pParameters, ref List<string> pFieldCondition, ref List<object> pDataCondition)
        {
            List<string> lstCondition = new List<string>();
            foreach (var x in pParameters)
            {
                string key = x.Key;
                string value = CommonMethods.ConvertToString(x.Value);
                if (!string.IsNullOrEmpty(value))
                {


                    lstCondition.Add(string.Format("{0}={1}", key, value));
                    pFieldCondition.Add(key);
                    pDataCondition.Add(value);

                }
            }
            return string.Join(" AND ", lstCondition);
        }
        public static List<Dictionary<string, object>> DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (System.Data.DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }

            return list;
        }
        #endregion

        #region "Basic Methods"

        [Route("loaddata")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult LoadData([FromBody] JObject pData)
        {
            return Ok(() =>
             {
                 List<string> lstFieldSearch = null;
                 List<object> lstDataSearch = null;
                 List<string> lstFieldCondition = new List<string>();
                 List<object> lstDataCondition = new List<object>();
                 string textSearch = CommonMethods.ConvertToString(pData["textsearch"]).Trim();
                 string condition = GetCondition(pData["filter"] as JObject);
                 string conditionParameters = string.Empty;
                 int from = CommonMethods.ConvertToInt32(pData["from"]);
                 int to = CommonMethods.ConvertToInt32(pData["to"]);
                 bool isAsc = CommonMethods.ConvertToBoolean(pData["ordersort"]);
                 string fieldSort = CommonMethods.ConvertToString(pData["fieldsort"]);
                 string orderBy = string.Empty;
                 if (pData.ContainsKey("parameter"))
                 {
                     conditionParameters = GetConditionParameters(pData["parameter"] as JObject, ref lstFieldCondition, ref lstDataCondition);
                 }


                 if (string.IsNullOrEmpty(fieldSort))
                 {
                     orderBy = this.OrderByGrid;
                 }
                 else
                 {
                     orderBy = fieldSort + " " + (isAsc ? "ASC" : "DESC");
                 }

                 if (!string.IsNullOrEmpty(conditionParameters))
                 {
                     condition += string.IsNullOrEmpty(condition) ? conditionParameters : " AND " + conditionParameters;
                 }

                 if (!string.IsNullOrEmpty(textSearch))
                 {
                     textSearch = CommonMethods.ConvertUnicodeToASCII(textSearch);
                     lstFieldSearch = this.FieldSearch.ToList<string>();
                     lstDataSearch = new List<object> { textSearch };
                 }
                 var dataset = GetTemplateDAL(ViewName).SearchData(from, to, orderBy, FieldSelect, condition, lstFieldSearch, lstDataSearch, lstFieldCondition, lstDataCondition);
                 return DataTableToJSON(dataset.Tables[0]);
             });
        }

        [Route("getdata")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult GetData([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                long id = CommonMethods.ConvertToInt64(pData["id"]);
                if (id > 0)
                {
                    var dataset = GetTemplateDAL(ViewName ?? TableName).GetDataById(id, FieldSelect, ConditionGetInfo);
                    return DataTableToJSON(dataset.Tables[0])[0];
                }
                return null;
            });
        }

        [Route("updatestatus")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult UpdateStatus([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                int res = -1;
                List<long> lstIds = CommonMethods.ConvertToListInt64(pData["ids"].ToString());
                int status = CommonMethods.ConvertToInt32(pData["status"]);

                if (lstIds != null && lstIds.Count > 0 && status > 0)
                {
                    string[] lstFieldUpdate = new string[] { "Status" };
                    res = GetTemplateDAL(TableName).UpdateMulti(lstIds, string.Empty, lstFieldUpdate, new object[] { status, false });

                }
                return res;
            });
        }

        [Route("updatelock")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult UpdateLock([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                int res = -1;
                List<long> lstIds = CommonMethods.ConvertToListInt64(pData["ids"].ToString());
                bool IsLock = CommonMethods.ConvertToInt32(pData["lock"]) == 1;
                if (lstIds != null && lstIds.Count > 0)
                {
                    res = GetTemplateDAL(TableName).UpdateMulti(lstIds, string.Empty, new string[] { "IsLock" }, new object[] { IsLock });

                }
                return res;
            });
        }

        [Route("deletedata")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult DeleteData([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                int res = -1;
                List<long> lstIds = CommonMethods.ConvertToListInt64(pData["ids"].ToString());

                if (lstIds != null && lstIds.Count > 0)
                {
                    DBLibrary.TemplateDAL dal = GetTemplateDAL(this.TableName);

                    if (CustomCheckValidDeleteData == null)
                    {
                        res = dal.DeleteMulti(lstIds, this.ConditionDelete);
                    }
                    else
                    {
                        // Tích hợp luôn CustomCheckValidDeleteData cho dễ dùng với những màn hình tương tự
                        // TrongHuu - [Updated 20211118]
                        List<long> lstIdDeleteSearch = new List<long>();
                        foreach (long Id in lstIds)
                        {
                            try
                            {
                                // Check delete
                                CustomCheckValidDeleteData(Id);
                                this._dal.TableName = this.TableName;
                                // Delete data
                                res += dal.Delete(Id, this.ConditionDelete);
                                lstIdDeleteSearch.Add(Id);
                            }
                            catch (Exception ex)
                            {
                                this._dal.TableName = this.TableName;
                                if (lstIds.Count == 1)
                                {
                                    throw new Exception(ex.Message);
                                }
                            }
                        }
                    }
                    if (res > 0)
                    {
                        if (OnAfterDeleteData != null)
                        {
                            OnAfterDeleteData(lstIds.ToList());
                        }
                    }
                }
                return res;
            });
        }

        [Route("savelist")]
        [HttpPost]
        [Authorize]

        public virtual IActionResult SaveDataList([FromBody] List<JObject> pListData)
        {
            return Ok(() =>
            {
                long res = 0;

                if (pListData != null && pListData.Count > 0)
                {

                    List<long> lstIds = new List<long>();
                    foreach (JObject pData in pListData)
                    {
                        long id = CommonMethods.ConvertToInt64(pData["Id"]);
                        List<string> lstSaveFields = id > 0 ? this.FieldUpdate : this.FieldInsert;
                        if (CustomCheckValidSaveData != null)
                        {
                            CustomCheckValidSaveData();
                        }
                        lstIds.Add(id);
                        res += SaveDataTable(pData, TableName, lstSaveFields, IsAddUserIdColumn, ConditionUpdate);
                    }
                }
                return res;
            });
        }

        [Route("savedata")]
        [HttpPost]
        [Authorize]
        public virtual IActionResult SaveData([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                long res = 0;
                long id = CommonMethods.ConvertToInt64(pData["Id"]);
                List<string> lstSaveFields = id > 0 ? this.FieldUpdate : this.FieldInsert;
                if (CustomCheckValidSaveData != null)
                {
                    CustomCheckValidSaveData();
                }
                res = SaveDataTable(pData, TableName, lstSaveFields, IsAddUserIdColumn, ConditionUpdate);
                if (id < 1)
                {
                    id = res;
                }

                if (OnAfterSaveData != null)
                {
                    OnAfterSaveData(id);
                }
                return res;
            });
        }

        #endregion
        private string _token = string.Empty;  // Backing store

        public string token
        {
            get => _token;
            set
            {
                string strtoken = CommonMethods.ConvertToString(Request.Headers[CommonConstants.TOKEN_HEADER_NAME]);
                _token = strtoken.Replace("Bearer ", "").Replace("bearer ", "");
            }

        }

        protected int RemoveToken()
        {
            return new TokenService().RemoveToken(token);

        }
        protected string RefreshToken()
        {
            return new TokenService().RefreshToken(token);

        }

        protected string CreateToken(string username, string password)
        {
            password = CommonMethods.GetEncryptMD5(password);
            return new TokenService().CreateToken(username, password);

        }

        protected User GetTokenInfo()
        {

            return new TokenService().GetTokenInfo(token);

        }



    }
}