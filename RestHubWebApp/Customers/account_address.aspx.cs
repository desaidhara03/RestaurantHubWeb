using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

namespace RestHubWebApp.Customers
{
    public partial class AccountAddress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //PAGE IS ONLY AVAILABLE TO AUTHENTICATED USERS: CONFIRM AUTHENTICATION
            Ini init = new Ini();
            init.isAuthUser();

            //VARIABLES DECLARATION
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;
            SqlDataReader addr;
            /* CHECK IF THERE IS ANY REQUEST TO REMOVE AN ADDRESS */
            if (Request.QueryString["action"] != null && Request.QueryString["action"] == "delete" && Request.QueryString["aID"] != "")
            {
                //CHECK IF ADDRESS IS NOT A BILLING ADDRESS
                sql = "SELECT * FROM dbo.customer_credit_card WHERE billing_address_id='" + Request.QueryString["aID"] + "'";
                addr = db.ProcessData(sql);
                if (!addr.HasRows)
                {
                    sql = "DELETE FROM dbo.customer_addresses WHERE customer_id='" + Session["customer_id"] + "' AND address_id='" + Request.QueryString["aID"] + "'";
                    db.ProcessData(sql);
                    feedbackDiv.InnerHtml = "Address successfully removed!";
                }else
                {
                    feedbackDiv.InnerHtml = "We were not able to remove that address because it is a billing address. If you want to delete that address, please change your billing address first.";
                }
            }

            /* CHECK IF THERE IS ANY POSTED DATA AND PROCESS */
            if (Request["street_address"] != null && Request["street_address"].ToString() != "")
            { //STREET ADDRESS IS REQUIRED!
                //UPDATE SESSION VARIABLES

                //CHECK IF REQUEST IS TO INSERT OR UPDATE AN ADDRESS
                if (Session["address_id"] != null && Session["address_id"].ToString() != "")
                {   //ADDRESS ID IS PROVIDED: UPDATE THE DATABASE
                    sql = "UPDATE dbo.customer_addresses SET " +
                                "street_address   = '" + ProjectTools.RemQuot(Request["steet_address"]) + "', " +
                                "city  = '" + ProjectTools.RemQuot(Request["city"]) + "', " +
                                "state  = '" + ProjectTools.RemQuot(Request["state"]) + "', " +
                                "zip_code  = '" + ProjectTools.RemQuot(Request["zip_code"]) + "' " +
                            "WHERE customer_id = '" + Session["customer_id"] + "' " +
                            "AND address_id = " + Session["address_id"];
                    db.ProcessData(sql);
                    HttpContext.Current.Response.Redirect("account_cc.aspx"); //REDIRECT USER TO THE NEXT PAGE
                }
                else
                { //THE ADDRESS ID WAS NOT PROVIDED: INSERT RECORD
                    sql = "INSERT INTO dbo.customer_addresses (" +
                            "customer_id, " +
                            "street_address, " +
                            "city, " +
                            "state, " +
                            "zip_code" +
                        ") VALUES (" +
                            "'" + Session["customer_id"] + "', " +
                            "'" + ProjectTools.RemQuot(Request["street_address"]) + "', " +
                            "'" + ProjectTools.RemQuot(Request["city"]) + "', " +
                            "'" + ProjectTools.RemQuot(Request["state"]) + "', " +
                            "'" + ProjectTools.RemQuot(Request["zip_code"]) + "')";
                    db.ProcessData(sql);
                }
            }

            /* LOAD ADDRESS LIST */
            sql = "SELECT * FROM dbo.customer_addresses WHERE customer_id='" + Session["customer_id"] + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                string addresses;
                addresses = "<div class=\"panel panel-info\">\n" +
                    "<div class=\"panel-heading\">\n" +
                    "<h3 class=\"panel-title\">Address Book</h3>\n" +
                    "</div>\n" +
                    "<div class=\"panel-body\">\n" +
                    "  <ul class=\"list-group\">\n";
                while (rec.Read())
                {
                    addresses += "<li class=\"list-group-item\">" + rec["street_address"] + ", " + rec["city"] + ", " + rec["state"] + " " + rec["zip_code"] + "\n";
                    //IF ADDRESS IS NOT A BILLING ADDRESS, SHOW DELETE BUTTON
                    sql = "SELECT * from customer_credit_card WHERE billing_address_id='" + rec["address_id"] + "'";
                    addr = db.ProcessData(sql);
                    if (!addr.HasRows)
                    {
                        addresses += "<a href=\"account_address?action=delete&aID=" + rec["address_id"] + "\" onclick=\"return confirm('Are you sure you want to delete that address?');\" class=\"btn btn-xs btn-danger pull-right\"><span class=\"glyphicon glyphicon-remove\"></span> Delete</a>\n";
                    }else
                    {
                        addresses += "<span class='pull-right' style='color:red;'>Billing*</span>\n";
                    }

                    addresses += "</li>\n";
                }
                addresses += "  </ul>\n" +
                    "</div>\n" +
                    "</div>\n";

                addressList.InnerHtml = addresses;
            }
        }
    }
}