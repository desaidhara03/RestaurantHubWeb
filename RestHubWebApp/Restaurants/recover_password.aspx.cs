using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace RestHubWebApp.Restaurants
{
    public partial class recover_password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mailTo = "";
            string subject = "";
            string message = "";

            if (Request["email"] != null && Request["email"] != "") {
                DBObject db = new DBObject();   //Object containing method to access database
                SqlDataReader rec;              //SQL data reader
                string sql;

                sql = "SELECT * FROM dbo.restaurant_branch " +
                        "WHERE branch_manager_email LIKE '" + Request["email"] + "' " +
                        "AND deleted = 0";
                rec = db.ProcessData(sql);
                if (rec.HasRows) { //email is in the database
                    rec.Read();
                    //GENERATE A NEW PASSWORD
                    string newPassword = System.Web.Security.Membership.GeneratePassword(100, 0);
                    newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => "");
                    newPassword = newPassword.Substring(0, 10);

                    //SAVE THAT NEW PASSWORD IN THE DATABASE
                    sql = "UPDATE dbo.restaurant_branch SET password = '" + ProjectTools.CalculateMD5Hash(newPassword) + "' WHERE restaurant_branch_id = '" + rec["restaurant_branch_id"] + "'";
                    db.ProcessData(sql);

                    feedbackDiv.InnerHtml = "You password has been successfully updated. Please, check your email for the new password. Please, click the login button to login. Feel free to change your password after you log in at any time using your Account page.";

                    //Send user the new password
                    mailTo = Request["email"].ToString();
                    subject = "Password reset from Restaurant Hub";
                    message = "<html>\n" + "<head><title>Password Recovery</title></head>\n" +
                                "<body>\n" + "<h1>Password Recovery from Restaurant Hub</h1>\n" +
                                "<p>As per your request, we have changed your password. Your new password is: </p>\n" +
                                "<p><b>" + newPassword + "</b></p>\n" +
                                "<p>Please, go to <a href='https://resthubwebapp.azurewebsites.net/restaurants/login.aspx'>resthubwebapp.azurewebsites.net/restaurants/login.aspx</a> to login.</p>\n" +
                                "<p>If you did not request to change your password, please contact the website administrator imediatelly at " + ProjectTools.mailFrom + "</p>\n" +
                                "</body>\n" + "</html>";
                    ProjectTools.SendEmail(mailTo, subject, message);
                }
                else
                { //email is not in the database
                    feedbackDiv.InnerHtml = "We do not have any record of that email address. Please, try another one. If you do not have an account with us, please create one.";
                }
            }
        }
    }
}