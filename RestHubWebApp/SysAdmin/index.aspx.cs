using System;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
        }
    }
}