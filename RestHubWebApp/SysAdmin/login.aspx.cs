﻿using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        int login_counter = 0;
        int last_login_attempt = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            /* If the user is logged in, redirect to the dashboard */
            Ini init = new Ini();
            bool isLoggedIn = false;
            if (Session["sys_admin_email"] != null && Session["auth"] != null)
            {
                isLoggedIn = init.admin_authenticated(Session["sys_admin_email"].ToString());
            }

            if (isLoggedIn)
            {
                HttpContext.Current.Response.Redirect("dashboard.aspx");
            }
            /* end: redirect to the dashboard */

            //ENFORCE SSL
            //if (!Request.IsLocal && !Request.IsSecureConnection)
            //{
            //    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            //    Response.Redirect(redirectUrl, false);
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}
            //END ENFORCE SSL

            string sql = "";
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
                    DBObject db = new DBObject();   //Object containing method to access database
                    SqlDataReader rec;              //SQL data reader

                    sql = "SELECT * FROM dbo.system_administrator WHERE email LIKE '" + ProjectTools.RemQuot(Request["email"]) + "' AND password LIKE '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "'";
                    rec = db.ProcessData(sql);
                    if (rec.HasRows)
                    {
                        //user authenticated: reset login counters
                        login_counter = 0;
                        Session["login_counter"] = 0;
                        //authenticate user for the session
                        rec.Read(); //Load the customer's record
                        Session["sys_admin_name"] = Convert.ToString(rec["name"]);
                        Session["sys_admin_email"] = Convert.ToString(rec["email"]);
                        Session["auth"] = ProjectTools.CalculateMD5Hash( Convert.ToString(rec["email"]) ); //load session with a hash of the customer's email
                        HttpContext.Current.Response.Redirect("dashboard.aspx"); //redirect user to the dashboard page
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
    }
}