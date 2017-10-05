using System;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* CLEAR PERSISTENT LOGIN COOKIE */
            HttpCookie authCookie = new HttpCookie("authCookie");
            authCookie = Request.Cookies["authCookie"];
            Response.Cookies["authCookie"]["customer_id"]   = "";//ADD VALUES TO THE COOKIE
            Response.Cookies["authCookie"]["name"]          = "";
            Response.Cookies["authCookie"]["email"]         = "";
            Response.Cookies["authCookie"]["phone"]         = "";
            Response.Cookies["authCookie"]["auth"]          = "";
            Response.Cookies["authCookie"].Expires          = ProjectTools.NowPSTime().AddDays(-1); //SET EXPIRATION = YESTERDAY
            Response.Cookies.Set(Response.Cookies["authCookie"]); //WRITE COOKIE TO CLIENT

            /* REMOVE ALL VALUES FROM THE SESSION */
            Session["restaurant_branch_id"] = "";
            Session["restaurant_name"]      = ""; ;
            Session["street_address"]       = "";
            Session["city"]                 = "";
            Session["state"]                = "";
            Session["zip_code"]             = "";
            Session["phone1"]               = "";
            Session["phone2"]               = "";
            Session["branch_manager_name"]  = "";
            Session["branch_manager_email"] = "";
            Session["restaurant_photo"]     = "";
            Session["auth"]                 = "";
            HttpContext.Current.Session.Abandon();
        }
    }
}