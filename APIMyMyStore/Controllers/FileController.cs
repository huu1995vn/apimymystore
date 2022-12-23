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
    [Route("[controller]")]
    [ApiController]
    public class FileController : CommonController
    {

        protected override string TableName => "files";

        protected override string ViewName => "files";

        protected override string OrderByGrid => "id ASC";

        protected override string FieldSelect => Variables.FieldSelectUser;

        protected override List<string> FieldInsert => new List<string> { "name", "userid" };

        protected override List<string> FieldUpdate => new List<string> { "name", "userid" };

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

        [Route("upload")]
        [HttpPost]
        [Authorize]
        public IActionResult Upload(IFormFile file)
        {
            return Ok(() =>
            {
                long pUserId = GetTokenInfo().id;
                String name = Request.Query["name"];
                long fileid = GetTemplateDAL("files").Insert(new string[] { "name", "userid" }, new object[] { name, pUserId });
                if (fileid > 0)
                {
                    CommonFileStore.Upload(file, fileid).ConfigureAwait(false);
                }
                else
                {
                    throw new Exception(CommonConstants.MESSAGE_DATA_NOT_VALID);
                }
                return fileid;
            });

        }

        [Route("{slug}-{pFileId:long}j.{pExtension}")]
        [HttpGet]
        public async Task<IActionResult> ShowFileAsync(long pFileId, string pExtension)
        {
            string contentType = CommonMethods.GetContentType(pExtension);
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = await client.GetAsync($"https://firebasestorage.googleapis.com/v0/b/modern-optics-234509.appspot.com/o/image%2F{pFileId}?alt=media&token=bb0f0a4c-8237-44aa-a059-64c45ca6eeed");
                Stream streamToReadFrom = await httpResponse.Content.ReadAsStreamAsync();
                if (streamToReadFrom.Length <= 65)
                {
                    throw new Exception("Unknown");
                }
                return File(streamToReadFrom, contentType);

            }
            catch (System.Exception)
            {

                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = await client.GetAsync($"https://firebasestorage.googleapis.com/v0/b/modern-optics-234509.appspot.com/o/noimage.jpg?alt=media&token=a07cea58-6460-49ac-b183-676ccf1522c3");
                Stream streamToReadFrom = await httpResponse.Content.ReadAsStreamAsync();
                return File(streamToReadFrom, contentType);
            }


        }



        // private bool Is304(long pFileId)
        // {
        //     if (!string.IsNullOrEmpty(pPathFile))
        //     {
        //         string since = Request.Headers["If-Modified-Since"];
        //         if (!string.IsNullOrEmpty(since))
        //         {
        //             DateTime ModifyDate;
        //             System.IO.FileInfo ObjFile = new System.IO.FileInfo(pPathFile);
        //             if (DateTime.TryParse(since, out ModifyDate) && ObjFile.LastWriteTime < ModifyDate)
        //             {
        //                 return true;
        //             }
        //         }
        //     }
        //     return false;
        // }

    }
}
