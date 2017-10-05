using System;
using System.Web;

namespace RestHubWebApp.Restaurants
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Redirect("orders.aspx"); //redirect user to the dashboard
        }
    }
}