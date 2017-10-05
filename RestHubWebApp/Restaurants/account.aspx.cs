using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace RestHubWebApp.Restaurants
{
    public partial class RestaurantAccount : System.Web.UI.Page
    {
        /* THIS CLASS IS AVAILABLE TO EDIT EXISTING ACCOUNTS AND REGISTRATION: AUTHENTICATION NOT REQUIRED */
        protected void Page_Load(object sender, EventArgs e)
        {
            //variables declaration
            string sql;
            DBObject db = new DBObject();
            SqlDataReader rec;
            string address = "";    //variable that will hold the whole address
            double[] latLng = new double[2];        //variable that will hold the latitude and longitude
            double sales_tax_rate = 0.00;
            string fileName = ""; // will hold the file name
            bool duplicateEmail = false;

            /* CHECK IF THERE ARE POSTED DATA AND PROCESS */
            if ( Request["branch_manager_email"] !=null && Request["branch_manager_email"] !="")
            { //EMAIL IS REQUIRED!

                if (Request["sales_tax_rate"] != null && Request["sales_tax_rate"] != "")
                { //PROCESS POSTED TAX RATE
                    string taxRate;
                    taxRate = Request["sales_tax_rate"].Replace("%", ""); //REMOVE ANY PERCENT SIGN
                    bool isNumeric = double.TryParse(taxRate, out sales_tax_rate);
                    if (isNumeric && sales_tax_rate >= 0 && sales_tax_rate <= 100)
                    {
                        sales_tax_rate = sales_tax_rate / 100;
                    }
                    else
                    {
                        sales_tax_rate = 0.00;
                    }
                }
                
                if (Request.Files.Count > 0)
                { //PROCESS POSTED FILE (IF ANY)
                    HttpPostedFile file = Request.Files[0]; //get a handler to the file
                    if (file != null && file.ContentLength > 0)
                    {
                        fileName = Path.GetFileName(file.FileName);

                        //generate a new file name
                        Random rand = new Random();
                        fileName = ProjectTools.NowPSTime().ToString("yyyyMMddHHmmssfff") + "-" + rand.Next(1000, 9999) + "." + fileName.Substring((fileName.Length - 3), 3);
                        string path = Path.Combine(Server.MapPath("../images/restaurants/"), fileName);
                        file.SaveAs(path);
                    }
                }

                //UPDATE SESSION VARIABLES
                Session["restaurant_name"] = ProjectTools.RemQuot(Request["restaurant"]);
                Session["street_address"] = ProjectTools.RemQuot(Request["address"]);
                Session["city"] = ProjectTools.RemQuot(Request["city"]);
                Session["state"] = ProjectTools.RemQuot(Request["state"]);
                Session["zip_code"] = ProjectTools.RemQuot(Request["zipcode"]);
                Session["phone1"] = ProjectTools.RemQuot(Request["phone1"]);
                Session["phone2"] = ProjectTools.RemQuot(Request["phone2"]);
                Session["sales_tax_rate"] = sales_tax_rate;
                Session["branch_manager_name"] = ProjectTools.RemQuot(Request["contact_name"]);
                Session["branch_manager_email"] = ProjectTools.RemQuot(Request["branch_manager_email"]);
                Session["max_orders_queue"] = ProjectTools.RemQuot(Request["max_orders_queue"]);
                if (fileName != "")
                    Session["restaurant_photo"] = fileName;

                if (Request["address"] != null && Request["city"] != null && Request["state"] != null && Request["zipcode"] != null)
                { //IF USER PROVIDED THE FULL ADDRESS, CALCULATE THE LATITUDE AND LONGITUDE
                    address = Request["address"] + ", " + Request["city"] + ", " + Request["state"] + " " + Request["zipcode"];
                    latLng = ProjectTools.GetLatLng(address);   //Call method: pass an address to get an array with {lat, lng}
                }

                if (Request["restaurant_id"] != null && Request["restaurant_id"] != "")
                { //IF RESTAURANT ID ALREADY EXISTS...
                    if (Session["auth"] != null && Session["auth"].ToString() != "")
                    { //USER IS LOGGED IN: UPDATE THE DATABASE
                        sql = "UPDATE dbo.restaurant_branch SET " +
                                "restaurant_name        = '" + ProjectTools.RemQuot(Request["restaurant"]) + "', " +
                                "street_address         = '" + ProjectTools.RemQuot(Request["address"]) + "', " +
                                "city                   = '" + ProjectTools.RemQuot(Request["city"]) + "', " +
                                "state                  = '" + ProjectTools.RemQuot(Request["state"]) + "', " +
                                "zip_code               = '" + ProjectTools.RemQuot(Request["zipcode"]) + "', " +
                                "latitude               = '" + latLng[0] + "', " +
                                "longitude              = '" + latLng[1] + "', " +
                                "phone1                 = '" + ProjectTools.RemQuot(Request["phone1"]) + "', " +
                                "phone2                 = '" + ProjectTools.RemQuot(Request["phone2"]) + "', " +
                                "sales_tax_rate         = '" + sales_tax_rate + "', " +
                                "branch_manager_name    = '" + ProjectTools.RemQuot(Request["contact_name"]) + "', " +
                                "branch_manager_email   = '" + ProjectTools.RemQuot(Request["branch_manager_email"]) + "', " +
                                "max_orders_queue       = '" + ProjectTools.RemQuot(Request["max_orders_queue"]) + "', " +
                                "admin_approval_status  = '0'"; //if no password or photo, do not update those.

                        if (Request["password"] != "")
                        { //IF PASSWORD IS PROVIDED, UPDATE PASSWORD
                            sql += ", password = '" + ProjectTools.CalculateMD5Hash(Request["password"]) + "'";
                        }
                        if (fileName != "")
                        { //IF A PHOTO IS PROVIDED, UPDATE PHOTO
                            sql += ", restaurant_photo = '" + fileName + "'";
                        }
                        sql += " WHERE restaurant_branch_id = '" + ProjectTools.RemQuot(Request["restaurant_id"]) + "'";
                        db.ProcessData(sql);
                        feedbackDiv.InnerHtml = "Information updated. Thank you!";
                    }
                }
                else
                { //THE RESTAURANT ID WAS NOT PROVIDED: TRY TO INSERT RECORD
                  //CHECK IF THE EMAIL ADDRESS ALREADY EXISTS IN THE SYSTEM
                    sql = "SELECT * FROM dbo.restaurant_branch WHERE branch_manager_email = '" + ProjectTools.RemQuot(Request["branch_manager_email"]) + "'";
                    rec = db.ProcessData(sql);
                    if (rec.HasRows)
                        duplicateEmail = true;

                    if (!duplicateEmail)
                    { //IF EMAIL IS UNIQUE: INSERT
                        sql = "INSERT INTO dbo.restaurant_branch (" +
                                "restaurant_name, " +
                                "street_address, " +
                                "city, " +
                                "state, " +
                                "zip_code, " +
                                "latitude, " +
                                "longitude, " +
                                "phone1, " +
                                "phone2, " +
                                "sales_tax_rate, " +
                                "branch_manager_name, " +
                                "branch_manager_email, " +
                                "password, " +
                                "date_created, " +
                                "public_visibility_status, " +
                                "admin_approval_status, " +
                                "restaurant_photo, " +
                                "max_orders_queue" +
                            ") VALUES (" +
                                "'" + ProjectTools.RemQuot(Request["restaurant"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["address"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["city"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["state"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["zipcode"]) + "', " +
                                "'" + latLng[0] + "', " +         //latitude
                                "'" + latLng[1] + "', " +         //longitude
                                "'" + ProjectTools.RemQuot(Request["phone1"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["phone2"]) + "', " +
                                "'" + sales_tax_rate + "', " +
                                "'" + ProjectTools.RemQuot(Request["contact_name"]) + "', " +
                                "'" + ProjectTools.RemQuot(Request["branch_manager_email"]) + "', " +
                                "'" + ProjectTools.CalculateMD5Hash(Request["password"]) + "', " +
                                "'" + ProjectTools.NowPSTime().ToString() + "', " +
                                "'0', " +                       //visibility: not visible by default
                                "'0', " +                       //approval: not approved by default
                                "'" + fileName + "', " +        //photo
                                "'" + ProjectTools.RemQuot(Request["max_orders_queue"]) + "')";
                        db.ProcessData(sql);

                        //SEND USER AN EMAIL
                        string mailTo = Request["branch_manager_email"].ToString();
                        string subject = "Welcome to Restaurant Hub";
                        string message = "<html>\n" + "<head><title>Account Created</title></head>\n" +
                                    "<body>\n" + "<h1>Welcome to Restaurant Hub!</h1>\n" +
                                    "<p>Your restaurant account was successfully created and the system administrator has been notified.</p>\n" +
                                    "<p>Please, go to <a href='https://resthubwebapp.azurewebsites.net/restaurants/login.aspx'>resthubwebapp.azurewebsites.net/restaurants/login.aspx</a> to log into your account and start adding menu items.</p>\n" +
                                    "<p>Feel free to contact us at " + ProjectTools.mailFrom + " if you have any question about your new account at Restaurant Hub.</p>\n" +
                                    "</body>\n" + "</html>";
                        ProjectTools.SendEmail(mailTo, subject, message);

                        HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
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