using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class order_process : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            if (Session["cart"] == null)
            {   //IF CART IS EMPTY, GO BACK TO THE CART INFORMATION PAGE
                HttpContext.Current.Response.Redirect("cart.aspx");
            }

            //VARIABLES DECLARATION
            string ccCustomerID = Session["customer_id"].ToString();
            double ccSalesTax = 0.00;
            double ccSubTotal = 0.00;
            double ccTotal = 0.00;
            string ccNumber = "";
            string ccCVV = "";
            string ccExpiration = "";
            string[] ccName;
            string ccFirstName = "";
            string ccLastName = "";
            string ccAddress = "";
            string ccCity = "";
            string ccState = "";
            string ccZipCode = "";
            string ccCountry = "";
            string ccPhone = "";
            string ccEmail = "";
            string ccRestaurantID = Session["last_restaurant_id"].ToString();
            DateTime ccCustomerETA;
            string ccApproved_OrderID = "";
            orderInformation orderInfo;
            string sql;
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            List<cartItem> shoppingCart = new List<cartItem>();

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

            //LOAD DATA TO SEND TO AUTHORIZE NET
            if (cart != null)
            {
                //RESET VARIABLES
                ccSubTotal = 0.00;
                ccSalesTax = 0.00;
                ccTotal = 0.00;

                foreach (string[] item in cart)
                {
                    //LOAD TOTALS
                    ccSubTotal += Convert.ToDouble(item[9]);
                    ccSalesTax += Convert.ToDouble(item[8]);
                    ccTotal += Convert.ToDouble(item[10]);

                    //LOAD CART OBJECT
                    cartItem orderItem = new cartItem
                    {
                        itemId = item[0],
                        itemName = item[1],
                        itemDescription = item[2],
                        itemQuantity = Convert.ToInt16(item[5]),
                        itemPrice = Convert.ToDecimal(item[4])
                    };
                    shoppingCart.Add(orderItem);            //ADD ITEM DETAILS TO THE CART LIST
                }

            }

            /* PREPARE DATA TO SEND TO AUTHORIZE NET */
            //GET CREDIT CARD INFORMATION
            sql = "SELECT c.cc_name, c.billing_address_id, c.cc_type, c.cc_number, c.cc_expiration, c.cvv_number, e.encryption_key " +
                    "FROM dbo.customer_credit_card AS c " +
                    "LEFT JOIN dbo.cc_encryption AS e ON c.credit_card_id=e.credit_card_id " +
                    "WHERE customer_id='" + ccCustomerID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                ccNumber = ProjectTools.DecryptCC(rec["cc_number"].ToString(), rec["encryption_key"].ToString());  //CARD NUMBER
                ccCVV = rec["cvv_number"].ToString();                            //CVV NUMBER
                DateTime dt = new DateTime();
                dt = Convert.ToDateTime(rec["cc_expiration"]);
                ccExpiration = dt.Month.ToString().PadLeft(2, '0') + dt.Year.ToString().Substring(2, 2);    //EXPIRATION: MMYY (e.g. 0819)
                ccName = rec["cc_name"].ToString().Split(' ');
                ccFirstName = ccName[0];                                            //FIRST NAME
                if (ccName.Length > 1)
                {
                    ccLastName = ccName[ccName.Length - 1];                         //LAST NAME
                }
                //GET BILLING ADDRESS INFORMATION
                sql = "SELECT a.street_address, a.city, a.state, a.zip_code, a.country, c.phone, c.email " +
                        "FROM dbo.customer_addresses AS a " +
                        "LEFT JOIN dbo.customers AS c ON a.customer_id=c.customer_id " +
                        "WHERE a.address_id = '" + rec["billing_address_id"] + "' AND " +
                        "a.customer_id='" + ccCustomerID + "'";

                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    ccAddress = rec["street_address"].ToString();             //BILLING ADDRESS
                    ccCity = rec["city"].ToString();                       //BILLING CITY
                    ccState = rec["state"].ToString();                      //BILLING STATE
                    ccZipCode = rec["zip_code"].ToString();                   //BILLING POSTAL CODE
                    if (rec["country"] == null)
                        ccCountry = "USA";                                     //BILLING COUNTRY (DEFAULT = USA)
                    else
                        ccCountry = rec["country"].ToString();
                    ccPhone = rec["phone"].ToString();                         //CUSTOMER'S PHONE NUMBER
                    ccEmail = rec["email"].ToString();
                }

            }

            //LOAD OBJECT CUSTOMER ADDRESS THAT WILL BE SENT TO AUTHORIZE NET
            //NOTE: INFORMATION IS LOADED FROM THE BOTTOM UP
            customerAddress billAddress = new customerAddress
            {
                firstName = ccFirstName,
                lastName = ccLastName,
                streetAddress = ccAddress,
                city = ccCity,
                state = ccState,
                zipcode = ccZipCode,
                country = ccCountry,
                phone = ccPhone
            };

            customerInfo custInfo = new customerInfo
            {
                customerID = ccCustomerID,
                customerEmail = ccEmail,
                billToAddress = billAddress
            };

            cartTotals orderTotals = new cartTotals
            {
                salesTax = ccSalesTax,
                totalAmount = ccTotal
            };

            orderInfo = new orderInformation
            {
                invoiceNumber = "",             //NOT DEFINED YET: WILL BE ADDED TO ORDERS TABLE AFTER PAYMENT CONFIRMATION
                cart = shoppingCart,
                totals = orderTotals,
                billToCustomer = custInfo
            };

            /* EOF */

            /* BOF: FIND CUSTOMER ETA */
            //LOAD RESTAURANTS LIST. INDEXES: 0 = restaurant_branch_id, 9 = distance (miles), 10 = ETA (min)
            List<string[]> restaurants = new List<string[]>();
            if (Session["restaurant_list"] == null)
                Session["restaurant_list"] = restaurants;
            else
                restaurants = (List<string[]>)Session["restaurant_list"];
            ccCustomerETA = ProjectTools.NowPSTime().AddMinutes(20);    //DEFAULT: ETA = 20 MINUTES
            if (restaurants != null)
            {
                foreach(string[] restaurant in restaurants)
                {
                    if ( restaurant[0].ToString() == ccRestaurantID.ToString() )
                    {
                        string etaMinutes = restaurant[10].ToString();
                        etaMinutes = Regex.Replace(etaMinutes, @"[^\d]", "");
                        ccCustomerETA = ProjectTools.NowPSTime().AddMinutes( Convert.ToDouble(etaMinutes) ); //UPDATE ETA (TIMEZONE CORRECTED)
                    }
                }
            }
            /* EOF: FIND CUSTOMER ETA */

            /* BOF: CHARGE CC, ONCE */
            if (Session["cart"] != null)
            {
                ccApproved_OrderID = ChargeCreditCard.ChargeCard(ccCustomerID, ccRestaurantID, ccSubTotal, ccNumber, ccCVV, ccExpiration, orderInfo, ccCustomerETA);
                Session["cart"] = null;
            }
            /* EOF: CHARGE CC, ONCE */

            if ( ccApproved_OrderID != "" )
            {
                //PAYMENT APPROVED: REDIRECT USER TO THE ORDER SUCCESS PAGE
                HttpContext.Current.Response.Redirect("order_success.aspx?restaurantID=" + ccRestaurantID + "&oID=" + ccApproved_OrderID);
            }
            else
            {
                //PAYMENT APPROVED: REDIRECT USER TO THE ORDER FAIL PAGE
                HttpContext.Current.Response.Redirect("order_fail.aspx");

                //PAYMENT APPROVED: REDIRECT USER TO THE ORDER SUCCESS PAGE
                HttpContext.Current.Response.Redirect("order_success.aspx?restaurantID=" + ccRestaurantID);
            }
        }

    }
}