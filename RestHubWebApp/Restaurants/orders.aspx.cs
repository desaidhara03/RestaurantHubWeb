using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace RestHubWebApp.Restaurants
{
    public partial class Orders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthRestaurant();

            //Declare Variables
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            string sql;
            DateTime dtNow = ProjectTools.NowPSTime();
            string rID = Session["restaurant_branch_id"].ToString();
            bool restaurantIsOnline = false;
            bool restaurantApproved = false;

            //UPDATE ONLINE/OFFLINE STATUS
            if (Request.QueryString["status"] != null)
            {
                if (Request.QueryString["status"] == "online")
                {
                    sql = "UPDATE dbo.restaurant_branch SET public_visibility_status = 1 WHERE restaurant_branch_id = '" + rID + "'";
                    db.ProcessData(sql);
                }
                else if (Request.QueryString["status"] == "offline")
                {
                    sql = "UPDATE dbo.restaurant_branch SET public_visibility_status = 0 WHERE restaurant_branch_id = '" + rID + "'";
                    db.ProcessData(sql);
                }
            }
            
            //LoadOrders( rID );           //UPDATE ORDERS QUEUE TABLES

            //CHECK IF RESTAURANT IS ONLINE/OFFLINE, UPDATE STATUS
            sql = "SELECT public_visibility_status, admin_approval_status FROM dbo.restaurant_branch WHERE restaurant_branch_id = '" + rID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                restaurantIsOnline = Convert.ToBoolean(rec["public_visibility_status"]);
                restaurantApproved = Convert.ToBoolean(rec["admin_approval_status"]);
                if (!restaurantApproved)
                {
                    statusUpdate.InnerHtml = "<span class='glyphicon glyphicon-time'></span> Pending Approval";
                    statusUpdate.Attributes["class"] = "btn btn-lg btn-danger pull-right";
                    statusUpdate.Attributes["href"] = "#";
                }else if (restaurantIsOnline)
                {
                    statusUpdate.InnerHtml = "<span class='glyphicon glyphicon-ok'></span> Restaurant Online!";
                    statusUpdate.Attributes["class"] = "btn btn-lg btn-success pull-right";
                    statusUpdate.Attributes["href"] = "orders.aspx?status=offline";
                }
                else
                {
                    statusUpdate.InnerHtml = "<span class='glyphicon glyphicon-off'></span> Restaurant Offline";
                    statusUpdate.Attributes["class"] = "btn btn-lg btn-warning pull-right";
                    statusUpdate.Attributes["href"] = "orders.aspx?status=online";
                }
                //UPDATE LAST SEEN FIELD
                sql = "UPDATE dbo.restaurant_branch SET last_seen='" + ProjectTools.NowPSTime() + "' WHERE restaurant_branch_id = '" + rID + "'";
                db.ProcessData(sql);
            }

        } //END LOAD PAGE FUNCTION

        /***************************************************************************/
        [WebMethod]
        public static string StatusHandler()
        {
            //DECLARE VARIABLES
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;
            string status = "online";
            string rID = "";
            DateTime dtNow = ProjectTools.NowPSTime();


            //dtNow = TimeZoneInfo.ConvertTimeFromUtc(ProjectTools.NowPSTime(), pacificTimeZone);

            if (HttpContext.Current.Session["restaurant_branch_id"] != null)
            { //SESSION IS ACTIVE: GET RESTAURANT ID
                rID = HttpContext.Current.Session["restaurant_branch_id"].ToString();
                //UPDATE LAST SEEN DATE/TIME
                sql = "UPDATE restaurant_branch SET last_seen = '" + dtNow + "' " +
                        "WHERE restaurant_branch_id = '" + rID + "'";
                db.ProcessData(sql);
            }
            else
            { //IF SESSION EXPIRED, UPDATE DB AND REDIRECT USER TO LOGIN
                sql = "UPDATE dbo.restaurant_branch " +
                        "SET public_visibility_status = 0 " +
                        "WHERE restaurant_branch_id = '" + rID + "'";
                db.ProcessData(sql);
                HttpContext.Current.Response.Redirect("login.aspx"); //EXIT
            }

            //LOGIN ACTIVE: CHECK IF VISIBILITY STATUS IS ONLINE
            sql = "SELECT admin_approval_status, public_visibility_status FROM dbo.restaurant_branch WHERE restaurant_branch_id = '" + rID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                if (Convert.ToBoolean(rec["public_visibility_status"]) == false)
                {
                    status = "offline";
                }
                else
                {
                    status = "online";
                }
                if (Convert.ToBoolean(rec["admin_approval_status"]) == false)
                {
                    status = "pending";
                }
            }

            db.DestroyConnection(); //CLEAR DB CONNECTIONS

            return status;
        } //END OF STATUS HANDLER FUNCTION

        /***************************************************************************/

        [WebMethod]
        public static string GetOrdersData()
        {
            //Declare Variables
            DBObject db = new DBObject();
            SqlDataReader orders;
            SqlDataReader orderItems;
            string sql;
            DateTime dt = new DateTime();
            DateTime dtNow = ProjectTools.NowPSTime();
            TimeSpan tSpan = new TimeSpan();
            string eta = "";
            string rID = "";
            string output = "";
            List<Cart> orderItemsList = new List<Cart>(); ;
            Cart orderItem;
            List<Order> ordersList = new List<Order>();     //LIST WITH ALL ORDERS
            Order orderInfo;


            if (HttpContext.Current.Session["restaurant_branch_id"] != null)
            { //GET RESTAURANT BRANCH ID
                rID = HttpContext.Current.Session["restaurant_branch_id"].ToString();
            }else
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
            
            //LOAD ALL ORDERS SINCE YESTERDAY, ORDERED BY CUSTOMER ETA
            sql = "SELECT o.order_id, o.order_date_time, customer_eta, o.order_status, c.name " +
                    "FROM dbo.restaurant_orders AS o LEFT JOIN dbo.customers AS c ON o.customer_id=c.customer_id " +
                    "WHERE restaurant_branch_id = '" + rID + "' " +
                    "AND order_date_time >= '" + dtNow.AddDays(-1) + "' " +
                    "ORDER BY customer_eta DESC";
            orders = db.ProcessData(sql);
            if (orders.HasRows)
            {
                while (orders.Read())
                {
                    //Load the list of items ordered
                    sql = "SELECT mi.menu_item_name, oi.menu_item_quantity " +
                            "FROM dbo.restaurant_order_items AS oi " +
                            "LEFT JOIN dbo.restaurant_menu_items AS mi " +
                            "ON oi.menu_item_id=mi.menu_item_id " +
                            "WHERE oi.order_id = '" + orders["order_id"] + "'";

                    orderItems = db.ProcessData(sql);
                    if (orderItems.HasRows)
                    {
                        orderItemsList = new List<Cart>();
                        while (orderItems.Read())
                        { //LOAD ITEMS LIST
                            orderItem = new Cart();
                            orderItem.itemName = orderItems["menu_item_name"].ToString();
                            orderItem.itemQuantity = orderItems["menu_item_quantity"].ToString();
                            orderItemsList.Add(orderItem);
                        }
                    }

                    //LOAD ORDER INFO
                    orderInfo = new Order();
                    orderInfo.orderID = orders["order_id"].ToString();          //ORDER ID
                    orderInfo.customerName = orders["name"].ToString();         //CUSTOMER NAME
                    orderInfo.orderStatus = orders["order_status"].ToString();  //ORDER STATUS
                    dt = Convert.ToDateTime(orders["order_date_time"]);
                    orderInfo.orderDateTime = dt.ToString("h:m tt");            //ORDER DATE/TIME (FORMATTED)
                    
                    eta = "";
                    dt = Convert.ToDateTime(orders["customer_eta"]);
                    dtNow = ProjectTools.NowPSTime(); //GET CURRENT PACIFIC STANDARD TIME

                    tSpan = dt.Subtract(dtNow);
                    if (Math.Abs(tSpan.Days) > 0)
                    {
                        eta = Math.Abs(tSpan.Days).ToString() + " days, ";
                    }
                    if (Math.Abs(tSpan.Hours) > 0)
                    {
                        eta = Math.Abs(tSpan.Hours).ToString() + " hours, ";
                    }
                    eta += Math.Abs(tSpan.Minutes).ToString() + " minutes";
                    if (tSpan.TotalMinutes < 0)
                    { //INDICATE IF TIME IS IN THE PAST
                        eta = " <span style='color:red;'> " + eta + " ago.</span>";
                    }
                    else
                    { //TIME IS IN THE FUTURE
                        eta = " <span style='color:green;'> within " + eta + ".</span>";
                    }
                    orderInfo.orderETA = eta;                           //ORDER'S ESTIMATED TIME OF ARRIVAL
                    orderInfo.orderCart = orderItemsList;               //ORDER'S SHOPPING CART

                    ordersList.Add(orderInfo);                          //ADD ORDER TO THE LIST
                } //END WHILE

                output = JsonConvert.SerializeObject(ordersList);       //CONVERT OBJECT INTO A JSON FORMATTED STRING

            } //END IF ANY ORDER FROM THE DATABASE MATCHES THE CRITERIA

            db.DestroyConnection();

            return output;
        }
    } //END CLASS Orders

    /* JSON FILE'S DATA STRUCTURE */
    public class Order
    {
        public string orderID;
        public string orderDateTime;
        public string orderETA;
        public string orderStatus;
        public string customerName;
        public List<Cart> orderCart;
    }

    public class Cart
    {
        public string itemName;
        public string itemQuantity;
    }
} //END NAMESPACE