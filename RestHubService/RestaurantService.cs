using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestHubService
{
    public class RestaurantService
    {
        public static List<restaurant_menu_items> getRestaurantMenuItem(int restaurantId)
        {
            List<restaurant_menu_items> menuItems = new List<restaurant_menu_items>();

            RestaurantHubEntities entities = new RestaurantHubEntities();

            menuItems = entities.restaurant_menu_items.Where(p => p.restaurant_branch_id == restaurantId).ToList();

            return menuItems;
        }
    }
}
