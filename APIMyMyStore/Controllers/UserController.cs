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
    public class UserController : CommonController
    {

        protected override string TableName => "users";

        protected override string ViewName => "users";

        protected override string OrderByGrid => "id ASC";

        protected override string FieldSelect => Variables.FieldSelectUser;

        protected override List<string> FieldInsert => new List<string> { "name", "fileid", "phone", "email", "address" };

        protected override List<string> FieldUpdate => new List<string> { "name", "address" };

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

        [Route("updateavatar")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(IFormFile file)
        {
            APIResult res = new APIResult();
            DBLibrary.TemplateDAL dal = GetTemplateDAL(ViewName);

            try
            {
                dal.BeginTransaction();
                long pId = GetTokenInfo().id;
                var dtset = dal.GetAllById(pId);
                long fileid = 0;
                string name = "";
                if (dtset.Tables[0].Rows.Count > 0)
                {
                    fileid = CommonMethods.ConvertToInt64(dtset.Tables[0].Rows[0]["fileid"]);
                    name = CommonMethods.ConvertToString(dtset.Tables[0].Rows[0]["name"]);

                }
                if (fileid <= 0)
                {
                    fileid = GetTemplateDAL("files").Insert(new string[] { "name" }, new object[] { name });
                }
                var url = await CommonFileStore.Upload(file, fileid).ConfigureAwait(false);
                if (url != null)
                GetTemplateDAL(ViewName).Update(pId, new string[] { "fileid" }, new object[] { fileid });
                res.SetIntResult(fileid);
                dal.CommitTransaction();

            }
            catch (Exception ex)
            {
                dal.RollbackTransaction();
                res.SetException(ex);
            }
            return Ok(res);
        }
        
    }
}
