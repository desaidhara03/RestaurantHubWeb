using System;
using System.Data.SqlClient;

namespace RestHubWebApp.Customers
{
    public partial class menu_item_details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;

            int menu_item_id = Convert.ToInt16(Request.QueryString["id"]);
            string output = "";
            string menu_item_photo;

            //LOAD DATA FROM DATABASE
            sql = "SELECT * FROM dbo.restaurant_menu_items WHERE menu_item_id='" + menu_item_id + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                //LOAD MENU ITEM PHOTO
                menu_item_photo = "nophoto.png";
                //string menuItemImageUrlPrefix = "http://resthub.azurewebsites.net/Restaurant_Menu_Items/Index/"; // TODO - Make sure this url is working.
                if (rec["menu_item_photo"].ToString() != "")
                    menu_item_photo = rec["menu_item_photo"].ToString();

                //LOAD OUTPUT
                output += "<div class=\"row\">\n" +
                           "    <div class=\"col-sm-6 col-md-4\">\n" +
                           "        <div class=\"thumbnail\">\n" +
                           "            <img src=\"../images/menu_items/" + menu_item_photo + "\" alt=\"\">\n" +
                           "            <div class=\"caption\">\n" +
                           "                <h3>" + rec["menu_item_name"] + "</h3>\n" +
                           "                <p>" + rec["menu_item_description"] + "</p>\n" +
                           "                <p class='lead'>$" + rec["menu_item_price"] + "</p>\n" +
                           "                <label for=\"txtQtty\">Qtty: </label>\n" +
                           "                <input type=\"text\" class=\"form-control\" name=\"txtQtty\" id=\"txtQtty\" value=\"1\" /><br clear='all' /><br />\n" +
                           "                <a href=\"restaurant_menu?id=" + rec["restaurant_branch_id"] + "\" class=\"btn btn-default\" role=\"button\">\n" +
                           "                    <span class=\"glyphicon glyphicon-arrow-left\"></span>  Back\n" +
                           "                </a>\n";
                if (Convert.ToBoolean(Session["ccOnFile"]))
                {
                    output += "                <a href=\"javascript:void(0)\" class=\"btn btn-success pull-right\" onclick=\"AddToCart('" + menu_item_id + "','add');\" role=\"button\">\n" +
                              "                    <span class=\"glyphicon glyphicon-shopping-cart\"></span> Add to Cart\n" +
                              "                </a>\n";
                }
                else
                {
                    output += "                <a href=\"account_cc.aspx\" class=\"btn btn-warning pull-right\" role=\"button\">\n" +
                              "                    <span class=\"glyphicon glyphicon-pencil\"></span> Add a Credit Card\n" +
                              "                </a>\n";
                }


                output +=  "            </div>\n" +
                           "        </div>\n" +
                           "    </div>\n" +
                           "</div>\n";
                menuItemDetails.InnerHtml = output;
            }
            else
            { //RESTAURANT HAS NO MENU ITEMS TO LIST
                feedbackDiv.InnerHtml = "This menu item is not available at this time. Please, try again later.";
            }
        }
    }
}