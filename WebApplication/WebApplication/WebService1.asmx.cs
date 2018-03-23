using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public void UploadFile()
        {
            Upload up = new Upload();
            up.UploadFile();
        }

        [WebMethod]
        public void DownloadFile()
        {
            string jsonwebtext = HttpContext.Current.Request.QueryString["JsontoLogic"];
            JObject jsonWeb = new JObject();
            jsonWeb = (JObject)JsonConvert.DeserializeObject(jsonwebtext);
            string str = jsonWeb["functionname"].ToString();
            string resultjson = null;
            switch (str)
            {
                case "downloadfile":
                    Download down = new Download();
                    resultjson = down.DownloadFile();
                    break;
                default:
                    break;
            }
            HttpContext.Current.Response.Write(resultjson);
        }
    }
}
