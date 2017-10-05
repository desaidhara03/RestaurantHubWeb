using Newtonsoft.Json;
using RestHubService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;

namespace RestHubWebApp.Customers
{
    public partial class cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;
            int menu_item_id;
            int menu_item_quantity;
            string action;
            string output = "";
            string menu_item_photo;
            double tax_rate = 0.00;
            double sales_tax = 0.00;
            double subtotal = 0.00;
            double total = 0.00;
            string[] cart_item = new string[11];
            List<string[]> cart = new List<string[]>();
            if (Session["cart"] == null)
                Session["cart"] = cart;
            else
                cart = (List<string[]>)Session["cart"];

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

            if ( Request.QueryString["id"] != null && Request.QueryString["id"] != "" && Request.QueryString["action"] != null)
            {
                menu_item_id = Convert.ToInt16(Request.QueryString["id"]);
                menu_item_quantity = Convert.ToInt16(Request.QueryString["qtty"] ?? "1");
                if((int)menu_item_quantity < 1) menu_item_quantity = 1; //ENSURE THAT QUANTITY IS AT LEAST ONE
                action = Request.QueryString["action"].ToString();
                switch (action)
                {
                    case "add":
                        sql = "SELECT * FROM dbo.restaurant_menu_items WHERE menu_item_id='" + menu_item_id + "'";
                        rec = db.ProcessData(sql);
                        if (rec.HasRows)
                        {
                            rec.Read();
                            //PRE-CALCULATE VALUES
                            menu_item_photo = "nophoto.png";
                            if (rec["menu_item_photo"].ToString() != "")
                                menu_item_photo = rec["menu_item_photo"].ToString();
                            tax_rate = ProjectTools.GetRestaurantSalesTax(Convert.ToInt16(rec["restaurant_branch_id"]));
                            subtotal = menu_item_quantity * Convert.ToDouble(rec["menu_item_price"]);
                            sales_tax = subtotal * tax_rate;
                            total = subtotal + sales_tax;

                            //LOAD ITEM TO SESSION
                            cart_item[0] = rec["menu_item_id"].ToString();
                            cart_item[1] = rec["menu_item_name"].ToString();
                            cart_item[2] = rec["menu_item_description"].ToString();
                            cart_item[3] = menu_item_photo;
                            cart_item[4] = rec["menu_item_price"].ToString();
                            cart_item[5] = menu_item_quantity.ToString();
                            cart_item[6] = rec["restaurant_branch_id"].ToString();
                            cart_item[7] = tax_rate.ToString();
                            cart_item[8] = sales_tax.ToString();
                            cart_item[9] = subtotal.ToString();
                            cart_item[10]= total.ToString();
                            cart.Add(cart_item);
                            Session["cart"] = cart;
                            Session["last_restaurant_id"] = Convert.ToInt16(rec["restaurant_branch_id"]);
                            Session["last_restaurant_name"] = ProjectTools.GetRestaurantNameFromID(rec["restaurant_branch_id"].ToString());
                        }
                        break;
                    case "delete":
                        for(int i = 0; i<cart.Count; i++)
                        {
                            if (cart[i][0] == menu_item_id.ToString())
                            {
                                cart.RemoveAt(i);
                                break;
                            }
                        }
                        break;
                }
            }

            //LOAD/DISPLAY CART
            output = "";
            if( cart != null && cart.Count > 0)
            {
                //RESET VARIABLES
                subtotal = 0.00;
                sales_tax = 0.00;
                total = 0.00;

                foreach (string[] item in cart)
                {
                    output += "<div class='media'>\n";
                    output += "<div class='media-left'>\n" +
                                    "<a href='#' class='thumbnail'>\n" +
                                        "<img class='media-object' src='../images/menu_items/" + item[3] + "' alt='" + item[1] + "' style='max-width:100px; max-height:100px;'>\n" +
                                    "</a>\n" +
                                "</div>\n";
                    output += "<div class='media-body'>\n" +
                                "   <h4 class='media-heading'>" + item[1] + "</h4>\n" +
                                "   <p class='lead pull-right'>" + item[5] + " x " + String.Format("{0:C2}", item[4]) + " = " + String.Format("{0:C2}", item[9]) + "</p>\n" +
                                "   <div class=\"btn-group-vertical\" role=\"group\">\n" +
                                "       <a href='cart.aspx?action=delete&id=" + item[0] + "' class='btn btn-danger' onclick='return confirm(\"Are you sure you want to remove this item?\");'><span class='glyphicon glyphicon-remove'></span> Delete</a>\n" +
                                "       <a href='menu_item_details.aspx?id=" + item[0] + "' class='btn btn-default'><span class='glyphicon glyphicon-zoom-in'></span> Details</a>\n" +
                                "   </div>\n" +
                                "</div><hr />\n";

                    subtotal    += Convert.ToDouble(item[9]);
                    sales_tax   += Convert.ToDouble(item[8]);
                    total       += Convert.ToDouble(item[10]);
                }
                output += "<table class='table pull-right' style='width:auto;'>\n" +
                            "   <tr><th style='text-align:right;'>Subtotal:</th><td style='text-align:right;'>" + String.Format("{0:C2}", subtotal) + "</td></tr>\n" +
                            "   <tr><th style='text-align:right;'>Sales Tax:</th><td style='text-align:right;'>" + String.Format("{0:C2}", sales_tax) + "</td></tr>\n" +
                            "   <tr><th style='text-align:right;'>Total:</th><td style='text-align:right;'>" + String.Format("{0:C2}", total) + "</td></tr>\n" +
                            "</table><br />\n";
            }
            
            if(output != "")
            {
                output += "<div class='row pull-right'>\n" +
                            "   <div class='col-sm-12 pull-right'>\n" + 
                            "       <a href='order_confirmation.aspx' class=\"btn btn-success pull-right\">Confirm Order</a>\n" +
                            "       <a href='restaurant_menu.aspx?id=" + Session["last_restaurant_id"] + "' class='btn btn-success' style='margin-right:10px'><span class='glyphicon glyphicon-arrow-left'></span> Back to the Menu</a>\n" +
                            "   </div>\n" +
                            "</div>";
                shoppingCart.InnerHtml = output;
            }else
            {
                output += "<div class='row'>\n" +
                            "   <div class='col-sm-12'>\n" +
                            "       <p class='lead'>Your shopping cart is empty!</p>\n" +
                            "       <p class='lead'>Please, use the button below to find a restaurant near you.</p>\n" +
                            "       <a href='find_restaurant.aspx' class='btn btn-success'><span class='glyphicon glyphicon-map-marker'></span> Restaurants</a>\n" +
                            "   </div>\n" +
                            "</div>";
                shoppingCart.InnerHtml = output;
            }
        }
    }

    
}