using RestHubService;
using System;
using System.Data.SqlClient;
using System.Web.Script.Services;
using System.Web.Services;

namespace RestHubWebApp.Customers
{
    public partial class order_success : System.Web.UI.Page
    {
        public static int orderId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            //VARIABLES DECLARATION
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            string sql;
            string output = "";
            string address = "";

            //LOAD ORDER ID, RESTAURANT ADDRESS // TODO: The query seems problamatic - if a customer puts second order before first order is ready, then the app will still show the old order since TOP 1 will return old order with NEW status.
            sql = "SELECT TOP 1 o.order_id, r.restaurant_name, r.street_address, r.city, r.state, r.zip_code, r.phone1, r.phone2, r.restaurant_photo " +
                    "FROM dbo.restaurant_branch AS r " +
                    "LEFT JOIN dbo.restaurant_orders AS o " +
                    "ON r.restaurant_branch_id=o.restaurant_branch_id " +
                    "WHERE o.customer_id = '" + Session["customer_id"] + "' " +
                    "AND o.order_status='New' " +
                    "AND r.restaurant_branch_id = '" + Request.QueryString["restaurantID"] + "' " +
                    "ORDER BY o.order_id DESC";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                orderId = Convert.ToInt32(rec["order_id"]);

                output = "<p class='lead'>You have successfully placed an order with</p>\n" +
                        "<h2>" + rec["restaurant_name"] + "</h2>\n" +
                        "<p class='lead'>For your reference, your Order ID is: " + orderId + "</p>\n" +
                        "<p class='lead'>Just as a reminder, restaurant's address is: </p>\n" +
                        "<p class='lead'>" + rec["street_address"] + "<br />\n" +
                        rec["city"] + ", " + rec["state"] + " " + rec["zip_code"] + "</p>\n" +
                        "<p class='lead'>Use the following phone number(s) if you need to contact the restaurant: <br />" + rec["phone1"] + (rec["phone2"] != "" ? " or " + rec["phone2"] : "") + "</p>\n";

                //GENERATE DIRECTIONS TO THE RESTAURANT BUTTON
                address = rec["street_address"] + ", " + rec["city"] + ", " + rec["state"] + " " + rec["zip_code"];
                address = address.Replace(" ", "+");
                if (Session["latitude"] != null)
                {
                    output += "<a href='https://maps.google.com?saddr=" + Session["latitude"] + "," + Session["longitude"] + "&daddr=" + address + "&layer=t' class='btn btn-success pull-right' />Directions</a>";
                }
                else
                {
                    output += "<a href='https://www.google.com/maps/dir//" + address + "' class='btn btn-success pull-right' />Directions</a>";
                }
            } //end if rec has rows

            orderSuccessInformation.InnerHtml = output;

            //DELETE SHOPPING CART FROM SESSION
            Session["cart"] = null;
            
            if(Session["order_success_token"] == null || Session["order_success_id"].ToString() != orderId.ToString() )
            {   //ENSURE ORDER CONFIRMATION EMAIL WILL BE SENT ONLY ONCE. SEND EMAIL.
                ProjectTools.SendEmail(Session["email"].ToString(), "Your Order Confirmation from Restaurant Hub", output);
            }
            Session["order_success_token"] = orderId;
        }

        //Check order status
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string CheckOrderStatus()
        {
             //Page page = (Page)HttpContext.Current.Handler;
             //HiddenField OrderId_hf = (HiddenField)page.FindControl("OrderId_hf");
            //int orderId = Convert.ToInt32(OrderId_hf.Value);// TODO: error  as order is not saved in db - updated with relative order id from order_success page.
            // int orderId = Convert.ToInt32(Application["orderId"].ToString()); //error  as order is not saved in db

            //int orderId = 9;
            OrderService orderService = new OrderService();

            string orderStatus = orderService.CheckOrderStatus(orderId);

            return orderStatus;
        }

    }
}