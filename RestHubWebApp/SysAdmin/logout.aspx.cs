using System;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["sys_admin_name"] = "";
            Session["sys_admin_email"] = "";
            Session["auth"] = "";
            HttpContext.Current.Session.Abandon();
        }
    }
}