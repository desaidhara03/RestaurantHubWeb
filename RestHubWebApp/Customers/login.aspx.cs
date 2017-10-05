using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class login : System.Web.UI.Page
    {
        int login_counter = 0;
        int last_login_attempt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //VARIABLE DECLARATION
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            HttpCookie authCookie;
            string sql;

            /* PROCESS PERSISTENT LOGIN IF AVAILABLE */
            authCookie = new HttpCookie("authCookie");
            authCookie = Request.Cookies["authCookie"];
            
            if (authCookie != null && authCookie.Values["auth"] != null && authCookie.Values["auth"] != "")
            {
                Session["customer_id"]  = authCookie.Values["customer_id"].ToString();
                Session["name"]         = authCookie.Values["name"].ToString();
                Session["email"]        = authCookie.Values["email"].ToString();
                Session["phone"]        = authCookie.Values["phone"].ToString();
                Session["auth"]         = authCookie.Values["auth"].ToString();
                Session["ccOnFile"]     = GetIsCCOnFile( authCookie.Values["customer_id"].ToString() );
            }

            /* If the user is logged in, redirect to the find restaurant page */
            Ini init = new Ini();
            bool isLoggedIn = false;

            if (Session["email"] != null && Session["auth"] != null)
            {
                isLoggedIn = init.admin_authenticated(Session["email"].ToString());
            }

            if (isLoggedIn)
            {
                HttpContext.Current.Response.Redirect("locationPage.aspx");
            }
            /* end: redirect to the find restaurant page */

            //ENFORCE SSL TO PROTECT PASSWORD
            if (!Request.IsLocal && !Request.IsSecureConnection)
            {
                string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
                Response.Redirect(redirectUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            //END ENFORCE SSL TO PROTECT PASSWORD

            //recall persistent counters from session
            if (Session["login_counter"] != null)
                login_counter = Convert.ToInt16(Session["login_counter"].ToString());
            if (Session["last_login_attempt"] != null)
                last_login_attempt = Convert.ToInt16(Session["last_login_attempt"].ToString());
            if (Request["email"] != null && Request["email"] != "" && Request["password"] != "")
            {
                if (login_counter < 5)
                {
                    //CHECK IF THERE IS A MATCH IN THE DATABASE
                    sql = "SELECT * FROM dbo.customers WHERE email LIKE '" + Request["email"] + "' AND password LIKE '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "'";
                    rec = db.ProcessData(sql);
                    if (rec.HasRows)
                    {
                        //user authenticated: reset login counters
                        login_counter = 0;
                        Session["login_counter"] = 0;
                        //authenticate user for the session
                        rec.Read(); //Load the customer's record
                        Session["customer_id"]  = rec["customer_id"];
                        Session["name"]         = Convert.ToString(rec["name"]);
                        Session["email"]        = Convert.ToString(rec["email"]);
                        Session["phone"]        = Convert.ToString(rec["phone"]);
                        Session["auth"]         = ProjectTools.CalculateMD5Hash( Convert.ToString(rec["email"]) ); //load session with a hash of the customer's email
                        Session["ccOnFile"]     = GetIsCCOnFile( rec["customer_id"].ToString() );

                        //if user checked the Remember Me checkbox
                        if (Request["cbxRememberMe"]!=null && Request["cbxRememberMe"] == "remember-me")
                        { //Activate perpetual login
                            Response.Cookies["authCookie"]["customer_id"]   = Session["customer_id"].ToString();//ADD VALUES TO THE COOKIE
                            Response.Cookies["authCookie"]["name"]          = Session["name"].ToString();
                            Response.Cookies["authCookie"]["email"]         = Session["email"].ToString();
                            Response.Cookies["authCookie"]["phone"]         = Session["phone"].ToString();
                            Response.Cookies["authCookie"]["auth"]          = Session["auth"].ToString();
                            Response.Cookies["authCookie"].Expires          = ProjectTools.NowPSTime().AddDays(365);        //SET EXPIRATION = 1 YEAR
                            Response.Cookies.Set(Response.Cookies["authCookie"]);       //WRITE COOKIE TO CLIENT
                        }
                        HttpContext.Current.Response.Redirect("locationPage.aspx"); //redirect user to the restaurant listing page
                    }
                    else
                    {
                        last_login_attempt = (ProjectTools.NowPSTime().Hour * 60) + ProjectTools.NowPSTime().Minute;
                        Session["last_login_attempt"] = last_login_attempt;
                        login_counter++;
                        Session["login_counter"] = login_counter;
                        feedbackDiv.InnerHtml = "The email address and password you entered do not match! Please, try again.";
                    }
                }else if((ProjectTools.NowPSTime().Hour * 60) + ProjectTools.NowPSTime().Minute - last_login_attempt >= 5)
                { //if number of minutes since last failed login attemt is greater than 5 minutes
                    Session["login_counter"] = 0; //reset the login counter for the session
                }else
                {
                    feedbackDiv.InnerHtml = "You have reached your limit of consecutive login attempts. Please, try again in 5 minutes.";
                }
            }
        }

        private bool GetIsCCOnFile(string customerID)
        {
            //VARIABLE DECLARATION
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            bool isCCOnFile = false;
            string sql;

            //CHECK IF USER HAS A CREDIT CARD ON FILE
            sql = "SELECT * FROM customer_credit_card WHERE customer_id = '" + customerID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                if( rec["cc_number"].ToString()!="" && rec["cc_expiration"].ToString() != "")
                {
                    isCCOnFile = true;
                }
            }

            return isCCOnFile;
        }
    }
}