using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestHubWebApp.Customers
{
    public partial class locationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();
        }

        protected void FindNearByRest_Click(object sender, EventArgs e)
        {
            Application["longitude"] = Longitude_hf.Value;
            Application["latitude"] = Latitude_hf.Value;

            Response.Redirect("find_restaurant.aspx");
        }
    }
}