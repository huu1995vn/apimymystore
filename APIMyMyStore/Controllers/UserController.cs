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

        protected override List<string> FieldInsert => new List<string> { "name", "image", "phone", "email", "address" };

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
        public IActionResult UpdateAvatar([FromBody] JObject pData)
        {
            return Ok(() =>
             {
                string image = CommonMethods.ConvertToString(pData.GetValue("image").ToString()).Trim();
                if(!image.IsUrl())
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_VALID);
                }
                long pId = GetTokenInfo().id;
                return GetTemplateDAL(ViewName).Update(pId, new string[]{"image"}, new object[]{image});

             });
        }

        [Route("checkapi")]

        public async Task<string> checkapiAsync()
        {
            var stream = System.IO.File.Open(@"D:\hinh.jpg", FileMode.Open);
            var ProjectId = FirebaseAdmin.FirebaseApp.DefaultInstance.Options.ProjectId;
            //authentication
            string customToken =  await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync("1");

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(
                 ProjectId+".appspot.com",
                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(customToken),
                     ThrowOnCancel = true,
                     
                 })
                .Child("image")
                .Child("1")
                .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;
            return downloadUrl;
        }
    
    }
}
