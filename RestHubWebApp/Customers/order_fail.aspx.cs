using System;

namespace RestHubWebApp.Customers
{
    public partial class order_fail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();
        }
    }
}