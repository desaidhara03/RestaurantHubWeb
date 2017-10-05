using System;
using System.Data.SqlClient;

namespace RestHubWebApp.Customers
{
    public partial class restaurant_menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;

            int restaurant_id = Convert.ToInt16(Request.QueryString["id"]);
            string output = "";
            string menu_item_photo;

            //UPDATE LAST RESTAURANT ID IN SESSION
            if(Request.QueryString["id"] != "")
            {
                Session["last_restaurant_id"] = restaurant_id;
                Session["last_restaurant_name"] = ProjectTools.GetRestaurantNameFromID( restaurant_id.ToString() );
            }

            //LOAD DATA FROM DATABASE
            sql = "SELECT * FROM dbo.restaurant_menu_items " +
                    "WHERE restaurant_branch_id='" + restaurant_id + "' " +
                    "AND admin_approval_status = 1 " +
                    "AND deleted = 0";
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
                    }else
                    {
                        output += "       <a href='account_cc.aspx' class='btn btn-warning pull-right'><span class='glyphicon glyphicon-pencil'></span> Add a Credit Card</a>\n";
                    }
                    
                    output += "   </div>\n" +
                                "   <h4 class='media-heading'>" + rec["menu_item_name"] + "</h4>\n" +
                                "   <p>" + rec["menu_item_description"] + ".</p>\n" +
                                "   <p class='lead'>$" + rec["menu_item_price"] + "</p>\n" +
                                "</div><hr />\n";
                }

                menuList.InnerHtml = output;
            }else
            { //RESTAURANT HAS NO MENU ITEMS TO LIST
                feedbackDiv.InnerHtml = "This restaurant does not have any menu item to list at this time. Please, try again later.";
            }
        }
    }
}