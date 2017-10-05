using System;
using System.Data.SqlClient;

namespace RestHubWebApp.SysAdmin
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthSysAdmin();

            //Declare Variables
            string table_rows = "";
            DBObject db = new DBObject();
            string sql;
            SqlDataReader rec;

            //Load Restaurants List
            sql = "SELECT TOP 100 restaurant_branch_id, restaurant_name, date_created FROM dbo.restaurant_branch WHERE admin_approval_status = 0";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                int i = 1;
                table_rows = "";
                while (rec.Read())
                {
                    table_rows += "<tr data-href='restaurant?id=" + rec["restaurant_branch_id"] + "'>\n";
                    table_rows += "<td>" + i + "</td>\n";
                    table_rows += "<td>" + rec["restaurant_name"] + "</td>\n";
                    table_rows += "<td>" + rec["date_created"] + "</td>\n";
                    table_rows += "</tr>\n";
                    i++;
                }
                tblRestaurants.InnerHtml = table_rows;
            }
            else
            {
                tblRestaurants.InnerHtml = "<tr><td colspan=3>There are no restaurants pending review at this time.</td></tr>";
            }

            //Load Menu Items List
            sql = "SELECT TOP 100 m.menu_item_id, r.restaurant_name, m.menu_item_name FROM dbo.restaurant_menu_items AS m LEFT JOIN dbo.restaurant_branch AS r ON m.restaurant_branch_id=r.restaurant_branch_id WHERE m.admin_approval_status = 0";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                int i = 1;
                table_rows = "";
                while (rec.Read())
                {
                    table_rows += "<tr data-href='MenuItem?id=" + rec["menu_item_id"] + "'>\n";
                    table_rows += "<td>" + i + "</td>\n";
                    table_rows += "<td>" + rec["restaurant_name"] + "</td>\n";
                    table_rows += "<td>" + rec["menu_item_name"] + "</td>\n";
                    table_rows += "</tr>\n";
                    i++;
                }
                tblMenuItems.InnerHtml = table_rows;
            }else
            {
                tblMenuItems.InnerHtml = "<tr><td colspan=3>There are no menu items pending review at this time.</td></tr>";
            }
        }
    }
}