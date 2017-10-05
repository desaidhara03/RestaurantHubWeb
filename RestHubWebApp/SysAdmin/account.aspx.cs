using System;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.SysAdmin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthSysAdmin();

            //variables declaration
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;

            if (Request["email"] != null && Request["password"] != null && Request["email"] != "" && Request["password"] != "")
            {
                if(Session["sys_admin_email"] != null && Session["sys_admin_email"].ToString() != "")
                { //user logged in: update database
                    sql = "UPDATE dbo.system_administrator SET " +
                        "email = '" + ProjectTools.RemQuot(Request["email"]) + "', " + 
                        "name = '" + ProjectTools.RemQuot(Request["name"]) + "'";

                    if (Request["password"]!="") {
                        sql += ", password = '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "'";
                    }
                    sql += " WHERE email LIKE '" + ProjectTools.RemQuot(Request["email"]) + "'";
                    db.ProcessData(sql);
                    //UPDATE SESSION
                    Session["sys_admin_name"] = ProjectTools.RemQuot(Request["name"]);
                    Session["sys_admin_email"] = ProjectTools.RemQuot(Request["email"]);
                    HttpContext.Current.Response.Redirect("dashboard.aspx"); //redirect user to the dashboard page
                }
                else
                { //new user: insert record
                    sql = "INSERT INTO dbo.system_administrator (email, name, password) VALUES ('" + ProjectTools.RemQuot(Request["email"]) + "', '" + ProjectTools.RemQuot(Request["name"]) + "', '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "')";
                    db.ProcessData(sql);
                    //UPDATE SESSION
                    Session["sys_admin_name"] = ProjectTools.RemQuot(Request["name"]);
                    Session["sys_admin_email"] = ProjectTools.RemQuot(Request["email"]);
                    HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
                }
            }
        }
    }
}