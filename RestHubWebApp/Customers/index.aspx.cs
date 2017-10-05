using System;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login screen
        }
    }
}