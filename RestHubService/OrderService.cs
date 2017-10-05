using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RestHubService
{
    public class OrderService
    {
        public String CheckOrderStatus(int orderId)
        {
            try
            {
                using (RestaurantHubEntities entities = new RestaurantHubEntities())
                {
                    String orderStatus = entities.restaurant_orders.Where(x => x.order_id == orderId).FirstOrDefault().order_status;

                    return orderStatus;
                }
            }
            catch (Exception ex) 
            {
                // TODO: Handle Exceptions properly. Setup email exception to developer service.
                return "OrderId is not valid.";
            }
        }

        public String PrepareOrderReadyEmailText(int orderId)
        {
            try
            {
                using (RestaurantHubEntities entities = new RestaurantHubEntities())
                {
                    int restBranchId = entities.restaurant_orders.Where(x => x.order_id == orderId).FirstOrDefault().restaurant_branch_id;

                    restaurant_branch restBranch = entities.restaurant_branch.Where(x => x.restaurant_branch_id == restBranchId).FirstOrDefault();

                    String output = "<p class='lead'>Your Order is ready for Pick up.</p>\n" +
                        "<h2>" + restBranch.restaurant_name + "</h2>\n" +
                        "<p class='lead'>For your reference, your Order ID is: " + orderId + "</p>\n" +
                        "<p class='lead'>Just as a reminder, restaurant's address is: </p>\n" +
                        "<p class='lead'>" + restBranch.street_address + "<br />\n" +
                        restBranch.city + ", " + restBranch.state + " " + restBranch.zip_code + "</p>\n" +
                        "<p class='lead'>Use the following phone number(s) if you need to contact the restaurant: <br />" + restBranch.phone1 + "</p>\n";

                    //GENERATE DIRECTIONS TO THE RESTAURANT BUTTON
                    //String address = restBranch.street_address + ", " + restBranch.city + ", " + restBranch.state + " " + restBranch.zip_code;
                    //address = address.Replace(" ", "+");
                    //if (latitude != null)
                    //{
                    //    output += "<a href='https://maps.google.com?saddr=" + latitude + "," + longitude + "&daddr=" + address + "&layer=t' class='btn btn-success pull-right' />Directions</a>";
                    //}
                    //else
                    //{
                    //    output += "<a href='https://www.google.com/maps/dir//" + address + "' class='btn btn-success pull-right' />Directions</a>";
                    //}

                    return output;
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle Exceptions properly. Setup email exception to developer service.
                return "";
            }
        }

        
    }
}
