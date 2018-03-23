using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using System.IO;

namespace WebApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            XmlDocument configXml = new XmlDocument();
            string strPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Config.xml");// "KPJD_Logic_config.xml";
            if (File.Exists(strPath))
            {
                configXml.Load(strPath);
                XmlNode root = configXml.SelectSingleNode("config");
                //project_path
                XmlNodeList project_path = root.SelectNodes("project_path");
                Common.project_path = ((XmlElement)project_path.Item(0)).ChildNodes.Item(0).InnerText;
                //updown_path
                XmlNodeList updown_path = root.SelectNodes("updown_path");
                Common.updown_path = ((XmlElement)updown_path.Item(0)).ChildNodes.Item(0).InnerText;
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}