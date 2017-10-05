using System;
using System.Web;

namespace RestHubWebApp.Restaurants
{
    public partial class LogOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ENSURE THAT THE RESTAURANT WILL GO OFFLINE
            if (Session["restaurant_branch_id"] != null)
            {
                DBObject db = new DBObject();
                string sql;
                sql = "UPDATE dbo.restaurant_branch SET public_visibility_status = 0 WHERE restaurant_branch_id = '" + Session["restaurant_branch_id"] + "'";
                db.ProcessData(sql);
            }
            
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