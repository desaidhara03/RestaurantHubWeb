using RestHubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestHubWebApp
{
    public partial class RestaurantDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int restaurantId = Convert.ToInt32(Request["id"]);

            loadRestaurantMenu(restaurantId);
        }

        private void loadRestaurantMenu(int restaurantId)
        {
            List<restaurant_menu_items> menuItems = RestaurantService.getRestaurantMenuItem(restaurantId);

            string menuItemImageUrlPrefix = "http://resthub.azurewebsites.net/Restaurant_Menu_Items/Index/";
            string output = "";

            if (menuItems.Count > 0)
            { //There are restaurants in the list
                foreach (restaurant_menu_items menuItem in menuItems)
                {
                    output += "<div class='media'>\n";

                    output += "<div class='media-left'>\n" +
                                        //"<a href='RestaurantDetails.aspx?id=" + restaurant[0] + "' class='thumbnail'>\n" +   TODO: We can expand this to zoom into the image on image touch.
                                        "<img class='media-object' src='" + menuItemImageUrlPrefix + menuItem.menu_item_photo + "' style='max-width:100px; max-height:100px;'>\n" +
                                //"</a>\n" +
                                "</div>\n";

                    output += "<div class='media-body'>\n" +
                                "<h3 class='media-heading'>" + menuItem.menu_item_name + "</h3>\n" +
                                "<p>" + menuItem.menu_item_description + "\n</p><br />" +
                                "<h3 class='media-heading'>Price: $" + menuItem.menu_item_price + "</h3><br />" +
                                "<button type='button' class='btn btn-primary'>Add to Cart</input>" +
                            "</div>\n" +
                        "</div><hr />\n";
                }
            }
            else
            {
                output = "No Menu Item found. We do apologize for the inconvenience.";
            }

            feedbackDiv.InnerHtml = output;
        }
    }
}