using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class MenuItem : System.Web.UI.Page
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
                    sql = "UPDATE dbo.restaurant_menu_items SET admin_approval_status='1' WHERE menu_item_id='" + Request.QueryString["id"] + "'";
                }
                else if (Request["rdStatus"] == "pending")
                {
                    sql = "UPDATE dbo.restaurant_menu_items SET admin_approval_status='0' WHERE menu_item_id='" + Request.QueryString["id"] + "'";
                }
                db.ProcessData(sql);
                HttpContext.Current.Response.Redirect("dashboard.aspx");
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                //Declare Variables
                string table_rows = "";
                SqlDataReader rec;

                //Load Restaurants List
                sql = "SELECT m.*, r.restaurant_name FROM dbo.restaurant_menu_items AS m LEFT JOIN dbo.restaurant_branch AS r ON m.restaurant_branch_id=r.restaurant_branch_id WHERE menu_item_id = '" + Request.QueryString["id"] + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    if (rec["menu_item_photo"].ToString() != "")
                        photo = rec["menu_item_photo"].ToString();
                    table_rows = "<table class='table table-condensed'>";
                    table_rows += "<tr><th>Restaurant: </th><td>" + rec["restaurant_name"] + "</td></tr>\n";
                    table_rows += "<tr><th>Menu Item: </th><td>" + rec["menu_item_name"] + "</td></tr>\n";
                    table_rows += "<tr><th>Description: </th><td>" + rec["menu_item_description"] + "</td></tr>\n";
                    table_rows += "<tr><th>Price: </th><td>" + rec["menu_item_price"] + "</td></tr>\n";
                    table_rows += "<tr><th>Menu Item Photo: </th><td><img src='../images/menu_items/" + photo + "' style='max-width:500px;' /></td></tr>\n";
                    table_rows += "<tr><th>Status: </th><td>\n";
                    table_rows += "<input type='radio' name='rdStatus' value='approved'" + (rec["admin_approval_status"].ToString()=="True"?" checked":"") + " /> Approved<br />\n";
                    table_rows += "<input type='radio' name='rdStatus' value='pending'" + (rec["admin_approval_status"].ToString() == "False" ? " checked" : "") + " /> Pending<br />\n";
                    table_rows += "<td></tr>\n";
                    table_rows += "</table>\n";

                    tblMenuItem.InnerHtml = table_rows;
                }
                else
                {
                    tblMenuItem.InnerHtml = "<p class='lead'>There are no data to show.</p>";
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("dashboard.aspx");
            }
        }
    }
}