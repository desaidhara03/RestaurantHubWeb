using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class AccountCC : System.Web.UI.Page
    {
        /* THIS CLASS IS AVAILABLE TO EDIT EXISTING ACCOUNTS AND REGISTRATION: AUTHENTICATION NOT REQUIRED */
        protected void Page_Load(object sender, EventArgs e)
        {
            //PAGE IS ONLY AVAILABLE TO AUTHENTICATED USERS: CONFIRM AUTHENTICATION
            Ini init = new Ini();
            init.isAuthUser();

            //VARIABLES DECLARATION
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;
            string expiry = "";
            string cryptoKey = "";
            string ccEncrypted = "";

            /* CHECK IF THERE ARE POSTED DATA AND PROCESS */
            if (Request["cc_number"] != null && Request["cc_number"].ToString() != "")
            { //NEW CREDIT CARD INFORMATION POSTED
                expiry = Request["expiry-year"] + "-" + Request["expiry-month"] + "-01";
                //ENCRYPT CREDIT CARD
                cryptoKey = System.Web.Security.Membership.GeneratePassword(64, 0);
                ccEncrypted = ProjectTools.EncryptCC(Request["cc_number"], cryptoKey);

                //CHECK IF CUSTOMER ALREADY HAS A CC ON FILE
                sql = "SELECT * FROM dbo.customer_credit_card WHERE customer_id='" + Session["customer_id"] + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                { //CUSTOMER ALREADY HAS A CREDIT CARD NUMBER ON FILE
                    //UPDATE CREDIT CARD TABLE
                    sql = "UPDATE dbo.customer_credit_card SET " +
                            "cc_name = '" + ProjectTools.RemQuot(Request["cc_name"]) + "', " +
                            "billing_address_id = '" + ProjectTools.RemQuot(Request["billing_address_id"]) + "', " +
                            "cc_type            = '" + ProjectTools.RemQuot(Request["cc_type"]) + "', " +
                            "cc_number          = '" + ccEncrypted + "', " +
                            "cc_expiration      = '" + expiry + "', " +
                            "cvv_number         = '" + ProjectTools.RemQuot(Request["cvv_number"]) + "' " +
                            " WHERE customer_id = '" + Session["customer_id"] + "'";
                    db.ProcessData(sql);
                    //UPDATE CRYPTO KEY ON ENCRYPTION TABLE
                    rec.Read();
                    sql = "UPDATE dbo.cc_encryption SET " +
                            "encryption_key = '" + cryptoKey + "' " +
                            "WHERE credit_card_id = '" + rec["credit_card_id"] + "'";
                    db.ProcessData(sql);

                    feedbackDiv.InnerHtml = "Credit card information updated. Thank you!";
                }
                else
                { //NO CREDIT CARD ON FILE
                    //ADD CREDIT CARD
                    sql = "INSERT INTO dbo.customer_credit_card (" +
                            "cc_name, " +
                            "customer_id, " +
                            "billing_address_id, " +
                            "cc_type, " +
                            "cc_number, " +
                            "cc_expiration, " +
                            "cvv_number" +
                        ") VALUES (" +
                            "'" + ProjectTools.RemQuot(Request["cc_name"]) + "', " +
                            "'" + Session["customer_id"] + "', " +
                            "'" + ProjectTools.RemQuot(Request["billing_address_id"]) + "', " +
                            "'" + ProjectTools.RemQuot(Request["cc_type"]) + "', " +
                            "'" + ccEncrypted + "', " +
                            "'" + expiry + "', " +
                            "'" + ProjectTools.RemQuot(Request["cvv_number"]) + "'); ";

                    ////GET LAST INSERT ID
                    sql += "SELECT SCOPE_IDENTITY() AS ccid FROM dbo.customer_credit_card;";
                    rec = db.ProcessData(sql);
                    string newID = "";
                    if (rec.HasRows)
                    {
                        rec.Read();
                        newID = rec["ccid"].ToString();
                    }
                    

                    //ADD CRYPTO KEY TO THE CC ENCRYPTION TABLE
                    if(newID != "")
                    {
                        sql = "INSERT INTO dbo.cc_encryption VALUES ('" + newID +"', '" + cryptoKey +"')";
                        db.ProcessData(sql);
                        Session["ccOnFile"] = true;
                        feedbackDiv.InnerHtml = "Credit card added. Thank you!";
                    }
                    else
                    {
                        feedbackDiv.InnerHtml = "We were not able to add your credit card number at this time. Please, try again later or contact the system administrator for support.";
                    }
                }
            }

            //NO REQUEST TO ADD/UPDATE CREDIT CARD: SHOW WHAT IS ON FILE
            string output = "";

            output = "<div class=\"panel panel-info\">\n" +
                   "    <div class=\"panel-heading\">\n" +
                   "        <h3 class=\"panel-title\">Credit Card on File</h3>\n" +
                   "    </div>\n" +
                   "    <div class=\"panel-body\">\n";

            sql = "SELECT cc.credit_card_id, cc.cc_name, cc.customer_id, cc.billing_address_id, cc.cc_type, cc.cc_number, " +
                        "cc.cc_expiration, cc.cvv_number, ca.street_address, ca.city, ca.state, ca.zip_code " +
                    "FROM dbo.customer_credit_card AS cc " +
                    "LEFT JOIN customer_addresses AS ca " +
                    "ON cc.billing_address_id=ca.address_id " +
                    "WHERE cc.customer_id = '" + Session["customer_id"] + "'";

            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                string ccName = "";
                if (rec["cc_name"] != null)
                {   //NAME ON THE CREDIT CARD
                    ccName = rec["cc_name"].ToString();
                    Session["cc_name"] = ccName;
                }

                string ccType = "";
                if (rec["cc_type"] != null)
                {   //CREDIT CARD TYPE
                    ccType = rec["cc_type"].ToString();
                    Session["cc_type"] = ccType;
                }

                string ccNumber = "";
                if (rec["cc_number"] != null)
                {   // DECRYPT CREDIT CARD NUMBER
                    sql = "SELECT encryption_key FROM dbo.cc_encryption WHERE credit_card_id = '" + rec["credit_card_id"] + "'";
                    SqlDataReader ccCrypt = db.ProcessData(sql);
                    if (ccCrypt.HasRows)
                    {
                        ccCrypt.Read();
                        cryptoKey = ccCrypt["encryption_key"].ToString();
                        if (cryptoKey != "")
                        {
                            ccNumber = ProjectTools.DecryptCC(rec["cc_number"].ToString(), cryptoKey);
                            // LAST 4 DIGITS OF CREDIT CARD
                            ccNumber = "***********" + ccNumber.Substring(ccNumber.Length - 4, 4);
                        }
                    }
                }

                string ccExpiration = "";
                if (rec["cc_expiration"] != null && rec["cc_expiration"].ToString() != "")
                {   //CREDIT CARD EXPIRATION MONTH/YEAR
                    DateTime dt = Convert.ToDateTime(rec["cc_expiration"]);
                    ccExpiration = dt.Month + "/" + dt.Year;
                    Session["ccExpMonth"] = dt.Month;
                    Session["ccExpYear"] = dt.Year;
                }

                string ccAddress = "";
                if (rec["street_address"] != null && rec["street_address"].ToString() != "")
                {
                    ccAddress = rec["street_address"] + ", " + rec["city"] + ", " + rec["state"] + " " + rec["zip_code"];
                    Session["billing_address"] = ccAddress;
                    Session["billing_address_id"] = rec["billing_address_id"];
                }

                output = "<div class=\"panel panel-default\">\n" +
                    "   <div class=\"panel-heading\">Credit Card on File</div>\n" +
                    "   <div class=\"panel-body\">\n" +
                    "       <p>The following credit card will be used for all your orders:</p>\n" +
                    "   </div>\n" +
                    "   <table class=\"table\">\n" +
                    "       <tr><th>Name:</th><td>" + ccName + "</td></tr>\n" +
                    "       <tr><th>Type:</th><td>" + ccType + "</td></tr>\n" +
                    "       <tr><th>Number:</th><td>" + ccNumber + "</td></tr>\n" +
                    "       <tr><th>Expiration:</th><td>" + ccExpiration + "</td></tr>\n" +
                    "       <tr><th>Address:</th><td>" + ccAddress + "</td></tr>\n" +
                    "   </table>\n" +
                    "</div>\n";
            }
            else
            {
                output += "You do not have any credit card on file. Use the Add Card button to record a payment method into your account.";
            }

            output += "    </div>\n" +
                    "</ div>\n";
            ccOnFile.InnerHtml = output;

            /* LOAD A LIST WITH ALL ADDRESSES AVAILABLE TO THE CUSTOMER INTO THE DROP DOWN */
            string address_list = "";
            sql = "SELECT * FROM customer_addresses WHERE customer_id = '" + Session["customer_id"] + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                string addr = "";
                while (rec.Read())
                {
                    addr = rec["street_address"] + ", " + rec["city"] + ", " + rec["state"] + " " + rec["zip_code"];
                    address_list += "<li><a href=\"javascript:void(0)\" onclick=\"updateAddress('" + rec["address_id"] + "','" + ProjectTools.RemQuot(addr) + "');\">" + addr + "</a></li>\n";
                }
                billAddressOptions.InnerHtml = address_list;
            }
        }
    }
}