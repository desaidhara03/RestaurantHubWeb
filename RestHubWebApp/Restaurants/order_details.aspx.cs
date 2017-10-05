using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace RestHubWebApp.Restaurants
{
    public partial class order_details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthRestaurant();

            //Declare Variables
            DBObject db = new DBObject();
            string sql;
            SqlDataReader orders;
            SqlDataReader orderItems;
            string itemNames    = "";
            string itemQttys    = "";
            string itemPrices   = "";
            string itemSubtotal = "";
            string totalsTitles = "";
            string totalsValues = "";
            string details      = "";
            string statusButtons= "";
            string[] statusOptions = { "new", "preparing", "ready", "delivered" };
            DateTime dt = new DateTime();
            DateTime dtNow = ProjectTools.NowPSTime();
            TimeSpan tSpan = new TimeSpan();
            string eta = "";

            //UPDATE STATUS IF REQUESTED
            if (Request.QueryString["status"] != null && statusOptions.Contains(Request.QueryString["status"]))
            {
                sql = "UPDATE dbo.restaurant_orders SET order_status='" + Request.QueryString["status"] + "' WHERE order_id = '" + Request.QueryString["oID"] + "'";
                db.ProcessData(sql);
            }

            //Load Order Details
            sql = "SELECT o.order_id, o.order_date_time, o.customer_eta, o.order_status, o.subtotal, o.tax, o.discount, o.total_charged, c.name, rb.sales_tax_rate, c.email " +
                    "FROM dbo.restaurant_orders AS o LEFT JOIN dbo.customers AS c ON o.customer_id=c.customer_id " +
                    "LEFT JOIN dbo.restaurant_branch AS rb ON o.restaurant_branch_id=rb.restaurant_branch_id " +
                    "WHERE o.restaurant_branch_id = '" + Session["restaurant_branch_id"] + "' " +
                    "AND o.order_id = '" + Request.QueryString["oID"].ToString() + "'";
            orders = db.ProcessData(sql);

            

            if (orders.HasRows)
            {
                orders.Read();
                //Load the list of items ordered
                sql = "SELECT mi.menu_item_name, oi.menu_item_final_price, oi.menu_item_quantity " +
                        "FROM dbo.restaurant_order_items AS oi LEFT JOIN dbo.restaurant_menu_items AS mi ON oi.menu_item_id=mi.menu_item_id " + 
                        "WHERE oi.order_id = '" + orders["order_id"] + "'";
                orderItems = db.ProcessData(sql);
                if (orderItems.HasRows)
                {
                    while (orderItems.Read())
                    {
                        itemNames   += orderItems["menu_item_name"] + "<br />\n";
                        itemQttys   += orderItems["menu_item_quantity"] + "<br />\n";
                        itemPrices  += orderItems["menu_item_final_price"] + "<br />\n";
                        itemSubtotal += (Convert.ToDouble(orderItems["menu_item_final_price"]) * Convert.ToDouble(orderItems["menu_item_quantity"])).ToString("#,##0.00") + "<br />\n";
                    }
                }

                //Add row to the table
                tblItemNames.InnerHtml = itemNames;
                tblItemQttys.InnerHtml = itemQttys;
                tblItemPrices.InnerHtml = itemPrices;
                tblItemSubtotals.InnerHtml = itemSubtotal;

                //Add Totals
                totalsTitles = "Subtotal: <br />";
                totalsValues = orders["subtotal"].ToString() + "<br />\n";
                if (Convert.ToDouble(orders["tax"]) > 0)
                {
                    totalsTitles += "Tax: <br />";
                    totalsValues += orders["tax"].ToString() + "<br />\n";
                }
                if (Math.Abs(Convert.ToDouble(orders["discount"])) > 0)
                {
                    totalsTitles += "Discount: <br />";
                    totalsValues += orders["discount"].ToString() + "<br />\n";
                }
                totalsTitles += "Total:";
                totalsValues += orders["total_charged"].ToString() + "\n";

                tblTotalsTitles.InnerHtml = totalsTitles;
                tblTotalsValues.InnerHtml = totalsValues;
            }
            //Load the order's details
            //customer name
            details = "<table class=\"table table-responsive\">";
            details += "<tr><th>Customer Name:</th><td>" + orders["name"] + "</td></tr>\n";
            //order date/time
            dt = Convert.ToDateTime(orders["order_date_time"]);
            details += "<tr><th>Order Date/Time:</th><td>" + dt.ToString("MMM dd, h:m tt") + "</td></tr>\n";
            //Customer's ETA
            dt = Convert.ToDateTime(orders["customer_eta"]);
            tSpan = dt.Subtract(dtNow);
            eta = " ";
            if (Math.Abs(tSpan.Days) > 0)
            {
                eta += Math.Abs(tSpan.Days).ToString() + " days, ";
            }
            if (Math.Abs(tSpan.Hours) > 0)
            {
                eta += Math.Abs(tSpan.Hours).ToString() + " hours, ";
            }
            eta += Math.Abs(tSpan.Minutes).ToString() + " minutes";
            //INDICATE IF TIME IS IN THE PAST
            if(tSpan.Hours < 0)
            {
                eta = "<span style='color:red;'>" + eta + " ago.</span>";
            }
            details += "<tr><th>ETA:</th><td>" + eta + "</td></tr>\n";
            details += "</table>\n";
            etaDetails.InnerHtml = details;

            // Sending order ready email to customer.
            if (Request.QueryString["status"] != null && statusOptions.Contains(Request.QueryString["status"]))
            {
                if (Request.QueryString["status"] == "ready")
                {
                    RestHubService.OrderService orderService = new RestHubService.OrderService();
                    String output = orderService.PrepareOrderReadyEmailText(Convert.ToInt32(Request.QueryString["oID"]));
                    if (output != "")
                    {
                        ProjectTools.SendEmail(orders["email"].ToString(), "Your Order is Ready for Pick up from Restaurant Hub", output);
                    }
                }
            }

            //Load the order's status
            foreach (string status in statusOptions)
            {
                statusButtons += "<a href=\"order_details.aspx?oID=" + Request.QueryString["oID"] + "&status=" + status + 
                    "\" class='btn btn-" + (orders["order_status"].ToString() == status ? "warning" : "default") + "'>" + status + "</a>\n";
            }
            oStatus.InnerHtml = statusButtons;
        }
    }
}