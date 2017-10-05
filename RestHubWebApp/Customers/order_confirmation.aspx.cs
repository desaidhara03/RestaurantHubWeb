using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestHubWebApp.Customers
{
    public partial class order_confirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            //VARIABLES DECLARATION
            string output = "";
            double sales_tax = 0.00;
            double subtotal = 0.00;
            double total = 0.00;

            string[] cart_item = new string[11];
            List<string[]> cart = new List<string[]>();
            if (Session["cart"] == null)
                Session["cart"] = cart;
            else
                cart = (List<string[]>)Session["cart"];

            //CHECK IF CART HAS ANY ITEMS
            if(cart.Count < 1)
            {
                HttpContext.Current.Response.Redirect("find_restaurant.aspx");
            }

            /* cart_item will contain the following: 
             * 0.  menu item id                 *
             * 1.  menu item name               *
             * 2.  menu item description        *
             * 3.  menu item photo              *
             * 4.  menu item price              *
             * 5.  menu item quantity ordered   *
             * 6.  restaurant id                *
             * 7.  restaurant tax rate          *
             * 8.  item's sales tax             *
             * 9.  subtotal (price x quantity)  *
             * 10. total (subtotal + tax)       */

            //LOAD/DISPLAY CART
            if (cart != null)
            {
                //RESET VARIABLES
                subtotal = 0.00;
                sales_tax = 0.00;
                total = 0.00;

                output = "<div class=\"panel panel-default\">\n";
                output += "<div class=\"panel-heading\">Order Confirmation</div>\n" +
                          "<div class=\"panel-body\">\n" +
                            "<p>Please, check your order to confirm that we have everything you need.</p>" +
                          "</div>\n" +
                          "<table class=\"table\">\n" + 
                          "     <tr>" +
                          "         <th>Name</th>" +
                          "         <th>Price</th>" +
                          "         <th>Quantity</th>" +
                          "         <th>Subtotal</th>" +
                          "     </tr>\n";

                foreach (string[] item in cart)
                {
                    output += "   <tr>\n" +
                                "       <td>" + item[1] + "</td>\n" +
                                "       <td>" + String.Format("{0:C2}", item[4]) + "</td>\n" +
                                "       <td>" + item[5] + "</td>\n" +
                                "       <td>" + String.Format("{0:C2}", item[9]) + "</td>\n" +
                                "   </tr>\n";

                    subtotal += Convert.ToDouble(item[9]);
                    sales_tax += Convert.ToDouble(item[8]);
                    total += Convert.ToDouble(item[10]);
                }
                output += "</table>\n" +
                                "</div><hr />\n";

                output += "<table class='table pull-right' style='width:auto;'>\n" +
                            "   <tr><th style='text-align:right;'>Subtotal:</th><td style='text-align:right;'>" + String.Format("{0:C2}", subtotal) + "</td></tr>\n" +
                            "   <tr><th style='text-align:right;'>Sales Tax:</th><td style='text-align:right;'>" + String.Format("{0:C2}", sales_tax) + "</td></tr>\n" +
                            "   <tr><th style='text-align:right;'>Total:</th><td style='text-align:right;'>" + String.Format("{0:C2}", total) + "</td></tr>\n" +
                            "</table>\n" +
                            "</div>";
            }

            if (output != "")
            {
                output += "<div class='row pull-right'>\n" +
                            "   <div class='col-sm-12 pull-right'>\n" +
                            "       <a href='order_process.aspx' class=\"btn btn-success pull-right\">Process Payment</a>\n" +
                            "       <a href='cart.aspx' class='btn btn-success' style='margin-right:10px'><span class='glyphicon glyphicon-arrow-left'></span> Back to the Shopping Cart</a>\n" +
                            "   </div>\n"+
                            "</div>";
                shoppingCart.InnerHtml = output;
            }
            else
            {
                output = "Your shopping cart is empty.<br />\n" +
                    "<a href='restaurant_menu.aspx?id=" + Session["last_restaurant_id"] + "' class='btn btn-success'><span class='glyphicon glyphicon-arrow-left'></span> Back to the Menu</a><br />\n";
                feedbackDiv.InnerHtml = "Your shopping cart is empty!";
            }

        }
    }
}