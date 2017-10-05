using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class Restaurant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthSysAdmin();

            //VARIABLE DECLARATION, CLASS INSTANTIATION
            DBObject db = new DBObject();
            string sql = "";
            string photo = "nophoto.png";

            //If request to change status, change status
            if (Request["rdStatus"] != null && Request.QueryString["id"] != null)
            {
                if (Request["rdStatus"] == "approved")
                {
                    sql = "UPDATE dbo.restaurant_branch SET admin_approval_status='1' WHERE restaurant_branch_id='" + Request.QueryString["id"] + "'";
                }else if (Request["rdStatus"] == "pending")
                {
                    sql = "UPDATE dbo.restaurant_branch SET admin_approval_status='0' WHERE restaurant_branch_id='" + Request.QueryString["id"] + "'";
                }
                db.ProcessData(sql);
                HttpContext.Current.Response.Redirect("dashboard.aspx");
            }

            //Show values
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                //Declare Variables
                string table_rows = "";
                SqlDataReader rec;

                //Load Restaurants List
                sql = "SELECT * FROM dbo.restaurant_branch WHERE restaurant_branch_id = '" + Request.QueryString["id"] + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    if (rec["restaurant_photo"].ToString() != "")
                        photo = rec["restaurant_photo"].ToString();
                    table_rows = "<table class='table table-condensed'>";
                    table_rows += "<tr><th>Restaurant Name: </th><td>" + rec["restaurant_name"] + "</td></tr>\n";
                    table_rows += "<tr><th>Street Address: </th><td>" + rec["street_address"] + "</td></tr>\n";
                    table_rows += "<tr><th>City: </th><td>" + rec["city"] + "</td></tr>\n";
                    table_rows += "<tr><th>State: </th><td>" + rec["state"] + "</td></tr>\n";
                    table_rows += "<tr><th>Zip Code</th><td>" + rec["zip_code"] + "</td></tr>\n";
                    table_rows += "<tr><th>Phone: </th><td>" + rec["phone1"] + "</td></tr>\n";
                    table_rows += "<tr><th>Phone: </th><td>" + rec["phone2"] + "</td></tr>\n";
                    table_rows += "<tr><th>Contact: </th><td>" + rec["branch_manager_name"] + "</td></tr>\n";
                    table_rows += "<tr><th>Email: </th><td>" + rec["branch_manager_email"] + "</td></tr>\n";
                    table_rows += "<tr><th>Created</th><td>" + rec["date_created"] + "</td></tr>\n";
                    table_rows += "<tr><th>Location:</th><td>Lat: " + rec["latitude"] + ", Lng: " + rec["longitude"] + "</td></tr>\n";
                    table_rows += "<tr><th>Restaurant Photo: </th><td><img src='../images/restaurants/" + rec["restaurant_photo"] + "' style='max-width:500px;' /></td></tr>\n";
                    table_rows += "<tr><th>Status: </th><td>\n";
                    table_rows += "<input type='radio' name='rdStatus' value='approved'" + (rec["admin_approval_status"].ToString() == "True" ? " checked" : "") + " /> Approved<br />\n";
                    table_rows += "<input type='radio' name='rdStatus' value='pending'" + (rec["admin_approval_status"].ToString() == "False" ? " checked" : "") + " /> Pending<br />\n";
                    table_rows += "<td></tr>\n";
                    table_rows += "</table>\n";

                    tblRestaurant.InnerHtml = table_rows;
                }
                else
                {
                    tblRestaurant.InnerHtml = "<p class='lead'>There are no data to show.</p>\n";
                }
            }else
            {
                HttpContext.Current.Response.Redirect("dashboard.aspx");
            }
        }
    }
}