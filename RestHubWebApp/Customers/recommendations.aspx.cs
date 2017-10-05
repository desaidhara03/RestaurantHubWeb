using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RestHubWebApp.Customers
{
    public partial class recommendations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;
            string restaurantIDs = "";
            int restaurant_id = 0;
            string output = "";
            string menu_item_photo;

            //TRY TO GET A RESTAURANT ID
            if (Session["last_restaurant_id"] != null)
                restaurant_id = Convert.ToInt16(Session["last_restaurant_id"]);
            if(restaurant_id == 0)
            {   //GET ANY RESTAURANT NEARBY THAT IS IN USER'S ORDER HISTORY
                var list = Session["SessionList"] as List<string[]>;
                if ( list != null)
                {
                    foreach(string[] restaurantInfo in list)
                    {
                        restaurantIDs += restaurantInfo[0] + ",";
                    }
                    restaurantIDs = restaurantIDs.Substring(0, restaurantIDs.Length - 1); //REMOVE LAST COMMA
                }

                sql = "SELECT TOP 1 restaurant_branch_id, COUNT(*) AS repeats FROM dbo.restaurant_orders WHERE customer_id = '" + Session["customer_id"] + "' " + (restaurantIDs!=""?"AND restaurant_branch_id IN (" + restaurantIDs + ") ":"") + " GROUP BY restaurant_branch_id ORDER BY repeats DESC";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    restaurant_id = Convert.ToInt16(rec["restaurant_branch_id"]);
                }
            }

            //LOAD DATA FROM DATABASE
            sql = "SELECT * FROM dbo.restaurant_menu_items WHERE menu_item_id IN " +
                    "("+
                        "SELECT TOP 10 mi.menu_item_id "+
                        "FROM dbo.restaurant_menu_items AS mi " +
                        "    JOIN dbo.restaurant_order_items AS ri " +
                        "       ON mi.menu_item_id = ri.menu_item_id " +
                        "WHERE mi.restaurant_branch_id = '" + restaurant_id + "' " +
                        "GROUP BY mi.menu_item_id " +
                        "ORDER BY SUM(ri.menu_item_quantity) DESC " +
                    ")";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                while (rec.Read())
                {
                    //LOAD MENU ITEM PHOTO
                    menu_item_photo = "nophoto.png";
                    if (rec["menu_item_photo"].ToString() != "")
                        menu_item_photo = rec["menu_item_photo"].ToString();

                    //LOAD OUTPUT
                    output += "<div class='media'>\n";
                    output += "<div class='media-left'>\n" +
                                    "<a href='#' class='thumbnail'>\n" +
                                        "<img class='media-object' src='../images/menu_items/" + menu_item_photo + "' alt='" + rec["menu_item_name"] + "' style='max-width:100px; max-height:100px;'>\n" +
                                    "</a>\n" +
                                "</div>\n";
                    output += "<div class='media-body'>\n" +
                                "   <div class=\"btn-group-vertical pull-right\" role=\"group\">\n" +
                                "       <a href='menu_item_details.aspx?id=" + rec["menu_item_id"] + "' class='btn btn-default pull-right'><span class='glyphicon glyphicon-folder-open'></span> Details</a><br />\n";

                    if (Convert.ToBoolean(Session["ccOnFile"]))
                    {
                        output += "       <a href='cart?action=add&qtty=1&id=" + rec["menu_item_id"] + "' class='btn btn-success pull-right'><span class='glyphicon glyphicon-shopping-cart'></span> Add</a>\n";
                    }
                    else
                    {
                        output += "       <a href='account_cc.aspx' class='btn btn-warning pull-right'><span class='glyphicon glyphicon-pencil'></span> Add a Credit Card</a>\n";
                    }

                    output += "   </div>\n" +
                                "   <h4 class='media-heading'>" + rec["menu_item_name"] + "</h4>\n" +
                                "   <p>" + rec["menu_item_description"] + ".</p>\n" +
                                "   <p class='lead'>$" + rec["menu_item_price"] + "</p>\n" +
                                "</div><hr />\n";
                }

                menuRecommendations.InnerHtml = output;
            }
            else
            { //RESTAURANT HAS NO MENU ITEMS TO LIST
                feedbackDiv.InnerHtml  = "We do not have any menu item recommendation yet. <br />\nPlease, try picking a restaurant first:<br />\n";
                feedbackDiv.InnerHtml += "<a href=\"find_restaurant.aspx\" class=\"btn btn-success\">Restaurants</a>\n";
                feedbackDiv.InnerHtml += "<a href=\"locationPage.aspx\" class=\"btn btn-default\">Update Location</a>\n";
            }

        }
    }
}