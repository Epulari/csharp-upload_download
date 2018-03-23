using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebApplication
{
    public class Upload
    {
        private static string updownfile = Common.project_path + Common.updown_path + "/";    //    E:\Projects\Others\Csharp_UploadDownload\UpDownFile
        
        /// <summary>
        /// 上传
        /// </summary>
        public void UploadFile()
        {
            HttpContext context = System.Web.HttpContext.Current;
            context.Response.ContentType = "text/plain";
            context.Response.Write("{\"chunked\" : true, \"hasError\" : false}");
            //如果进行了分片
            if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                //取得chunk和chunks
                int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                //根据GUID创建用该GUID命名的临时文件夹
                //string folder = context.Server.MapPath("~/1/" + context.Request["guid"] + "/");
                string folder = updownfile + context.Request["guid"] + "\\";
                string path = folder + chunk;

                //建立临时传输文件夹
                if (!Directory.Exists(Path.GetDirectoryName(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                BinaryWriter AddWriter = new BinaryWriter(addFile);
                //获得上传的分片数据流
                HttpPostedFile file = context.Request.Files[0];
                Stream stream = file.InputStream;

                BinaryReader TempReader = new BinaryReader(stream);
                //将上传的分片追加到临时文件末尾
                AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                //关闭BinaryReader文件阅读器
                TempReader.Close();
                stream.Close();
                AddWriter.Close();
                addFile.Close();

                TempReader.Dispose();
                stream.Dispose();
                AddWriter.Dispose();
                addFile.Dispose();
                if (chunk == chunks - 1)
                {
                    ProcessRequest(context.Request["guid"], Path.GetExtension(file.FileName));
                }
            }
            else//没有分片直接保存
            {
                //string targetPath = updownfile + "KPJDdata" + Path.GetExtension(context.Request.Files[0].FileName);
                string targetPath = updownfile + context.Request.Files[0].FileName;
                context.Request.Files[0].SaveAs(targetPath);
            }
        }

        private void ProcessRequest(string guid, string fileExt)
        {
            HttpContext context = System.Web.HttpContext.Current;
            context.Response.ContentType = "text/plain";
            string sourcePath = Path.Combine(updownfile, guid + "\\");//源数据文件夹
            string targetPath = Path.Combine(updownfile, Guid.NewGuid() + fileExt);//合并后的文件

            DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
            if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
            {
                FileInfo[] files = dicInfo.GetFiles();
                foreach (FileInfo file in files.OrderBy(f => int.Parse(f.Name)))
                {
                    FileStream addFile = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                    BinaryWriter AddWriter = new BinaryWriter(addFile);

                    //获得上传的分片数据流
                    Stream stream = file.Open(FileMode.Open);
                    BinaryReader TempReader = new BinaryReader(stream);
                    //将上传的分片追加到临时文件末尾
                    AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                    //关闭BinaryReader文件阅读器
                    TempReader.Close();
                    stream.Close();
                    AddWriter.Close();
                    addFile.Close();

                    TempReader.Dispose();
                    stream.Dispose();
                    AddWriter.Dispose();
                    addFile.Dispose();
                }
                Directory.Delete(sourcePath, true);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="strPath"></param>
        private static void DeleteFolder(string strPath)
        {
            //if (strPath != backuprecoveryfile)
            //{
            //    Directory.Delete(strPath, true);
            //}
            //else
            //{
                if (Directory.GetDirectories(strPath).Length > 0)
                {
                    foreach (string fl in Directory.GetDirectories(strPath))
                    {
                        Directory.Delete(fl, true);
                    }
                }
                //删除这个目录下的所有文件
                if (Directory.GetFiles(strPath).Length > 0)
                {
                    foreach (string f in Directory.GetFiles(strPath))
                    {
                        System.IO.File.Delete(f);
                    }
                }
            //}
        }
    }
}