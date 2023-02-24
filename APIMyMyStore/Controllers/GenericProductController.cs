using System.Text.Json;
using APIMyMyStore.Helpers;
using APIMyMyStore.Services;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RaoXeAPI.Controllers;

namespace APIMyMyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericProductController : CommonController
    {

        protected override string TableName => "genericproducts";

        protected override string ViewName => "vwgenericproducts";

        protected override string OrderByGrid => "name ASC";

        protected override string FieldSelect => "";

        protected override List<string> FieldInsert => new List<string> { "name", "fileid", "typeid", "materialid", "description", "video" };

        protected override List<string> FieldUpdate => new List<string> { "name", "fileid", "typeid", "materialid", "description", "video" };

        [Route("loaddata")]
        [HttpPost]
        [Authorize]
        public override IActionResult LoadData([FromBody] JObject pData)
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

        [Route("savedata")]
        [HttpPost]
        [Authorize]
        public override IActionResult SaveData([FromBody] JObject pData)
        {
            return Ok(() =>
            {
                long res = 0;
                long id = CommonMethods.ConvertToInt64(pData["id"]);
                JArray products = (JArray)pData["products"];
                List<string> FieldProduct = new List<string> { "name", "fileid", "description", "video" };
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

    }
}
