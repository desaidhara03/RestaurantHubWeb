using System;
using System.Data.SqlClient;

namespace RestHubWebApp.Restaurants
{
    public partial class RestaurantMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthRestaurant();

            //Declare Variables
            string menu_list = "";
            DBObject db = new DBObject();
            string sql;
            SqlDataReader rec;

            //PROCESS DELETE MENU ITEM WHEN REQUESTED
            if (Request["delID"] != null && Request["delID"] != "")
            {
                //CHECK IF MENU ITEM BELONGS TO RESTAURANT
                sql = "SELECT * FROM dbo.restaurant_menu_items " +
                    "WHERE restaurant_branch_id = '" + Session["restaurant_branch_id"] + "' " +
                    "AND menu_item_id = '" + Request["delID"] + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    sql = "UPDATE dbo.restaurant_menu_items SET deleted = 1 WHERE menu_item_id = '" + Request["delID"] + "'";
                    db.ProcessData(sql);
                }
            }

            //Load Menu Items
            sql = "SELECT menu_item_id, menu_item_name, menu_item_description, menu_item_photo, menu_item_price, admin_approval_status " +
                    "FROM dbo.restaurant_menu_items " +
                    "WHERE restaurant_branch_id = '" + Session["restaurant_branch_id"] + "' " + 
                    "AND deleted = 0";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                menu_list = "";
                while (rec.Read())
                {
                    menu_list += "<div class='media menuItem' data-href='menu_item.aspx?id=" + rec["menu_item_id"] + "'>\n";
                        menu_list += " <div class='media-left'>\n";
                        menu_list += "     <a href='menu_item.aspx?id=" + rec["menu_item_id"] + "'>\n";
                    if (rec["menu_item_photo"].ToString() != "")
                    {
                        menu_list += "         <img class='media-object' src='../images/menu_items/" + rec["menu_item_photo"] + "' alt='" + rec["menu_item_name"] + "'/>\n";
                    }else
                    {
                        menu_list += "         <img class='media-object' src='../images/menu_items/nophoto.png' alt='" + rec["menu_item_name"] + "'/>\n";
                    }
                        menu_list += "     </a>\n";
                        menu_list += " </div>\n";
                    menu_list += " <div class='media-body'>\n";
                    menu_list += "     <h4 class='media-heading'>" + rec["menu_item_name"] + "</h4>\n";
                    menu_list += "    " + rec["menu_item_description"] + "<br />\n";
                    menu_list += "    " + rec["menu_item_price"] + "\n";
                    menu_list += " </div>\n";
                    menu_list += "</div>\n";
                }
                menuItemsList.InnerHtml = menu_list;
            }
            else
            {
                menuItemsList.InnerHtml = "<p class='lead'>There are no items recorded in your menu.</p>";
            }
        }
    }
}