using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class Account : System.Web.UI.Page
    {
        /* THIS CLASS IS AVAILABLE TO EDIT EXISTING ACCOUNTS AND REGISTRATION: AUTHENTICATION NOT REQUIRED */
        protected void Page_Load(object sender, EventArgs e)
        {
            //VARIABLES DECLARATION
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;

            /* CHECK IF THE CUSTOMER IS LOGGED IN */
            bool loggedIn = false;
            if ( Session["email"]!=null && Session["auth"]!=null && Session["customer_id"].ToString() != "" )
            {
                loggedIn = true;
            }

            /* CHECK IF THERE ARE POSTED DATA AND PROCESS */
            if ( Request["email"] !=null && Request["email"].ToString() != "" )
            { //EMAIL IS REQUIRED!
                //UPDATE SESSION VARIABLES
                Session["name"] = ProjectTools.RemQuot(Request["name"]);
                Session["email"] = ProjectTools.RemQuot(Request["email"]);
                Session["phone"] = ProjectTools.RemQuot(Request["phone"]);

                if (loggedIn)
                { //IF CUSTOMER ID ALREADY EXISTS...
                    if (Session["auth"] != null && Session["auth"].ToString() != "")
                    { //USER IS LOGGED IN: UPDATE THE DATABASE
                        sql = "UPDATE dbo.customers SET " +
                                "name   = '" + ProjectTools.RemQuot(Request["name"]) + "', " +
                                "email  = '" + ProjectTools.RemQuot(Request["email"]) + "', " +
                                "phone  = '" + ProjectTools.RemQuot(Request["phone"]) + "'";
                        if (Request["password"].ToString() != "")
                        { //IF PASSWORD IS PROVIDED, UPDATE PASSWORD
                            sql += ", password = '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "'";
                        }
                        sql += " WHERE customer_id = '" + ProjectTools.RemQuot(Session["customer_id"].ToString()) + "'";
                        db.ProcessData(sql);
                        HttpContext.Current.Response.Redirect("account_address.aspx"); //REDIRECT USER TO THE NEXT PAGE
                    }
                }
                else
                { //THE CUSTOMER ID WAS NOT PROVIDED: TRY TO INSERT RECORD
                  //CHECK IF THE EMAIL ADDRESS ALREADY EXISTS IN THE SYSTEM
                    bool duplicateEmail = false;
                    sql = "SELECT * FROM dbo.customers WHERE email = '" + ProjectTools.RemQuot(Request["email"]) + "'";
                    rec = db.ProcessData(sql);
                    if (rec.HasRows)
                        duplicateEmail = true;

                    if (!duplicateEmail)
                    { //IF EMAIL IS UNIQUE
                        sql = "INSERT INTO dbo.customers (" +
                                "name, " +
                                "email, " +
                                "phone, " +
                                "password, " +
                                "account_creation_date" +
                            ") VALUES (" +
                                "'" + ProjectTools.RemQuot(Request["name"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["email"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["phone"]) + "', " +
                                "'" + ProjectTools.CalculateMD5Hash(Request["password"]) + "', " +
                                "'" + ProjectTools.NowPSTime().ToString() + "')";
                        db.ProcessData(sql);
                        //GET THE NEW CUSTOMER ID
                        sql = "SELECT SCOPE_IDENTITY() AS id";
                        rec = db.ProcessData(sql);
                        string newID = rec["id"].ToString() ?? "";
                        if (newID != "")
                        {
                            //AUTHENTICATE USER
                            Session["customer_id"] = newID;
                            Session["auth"] = ProjectTools.CalculateMD5Hash(Convert.ToString(Request["email"])); //load session with a hash of the customer's email
                            HttpContext.Current.Response.Redirect("account_address.aspx"); //REDIRECT USER TO THE NEXT PAGE
                        }else
                        {
                            feedbackDiv.InnerHtml = "We were not able to process your request. Please, try again.";
                        }
                    }
                    else
                    { //EMAIL IS DUPLICATE: INFORM USER BUT DO NOT CHANGE THE DATABASE
                        feedbackDiv.InnerHtml = "The e-mail you are trying to use is already in the system. Please, try using another e-mail address.";
                    }
                }
            }
        }
    }
}