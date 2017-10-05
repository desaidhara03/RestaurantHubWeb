using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace RestHubWebApp.Restaurants
{
    public partial class RestaurantMenuItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate Administrator's Authentication
            Ini init = new Ini();
            init.isAuthRestaurant();

            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql = "";

            //Process menu item image file upload
            string fileName = ""; // will hold the file name
            if (Request.Files.Count > 0)
            { //if a file has been posted
                HttpPostedFile file = Request.Files[0]; //get a handler to the file
                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);

                    //generate a new file name
                    Random rand = new Random();
                    fileName = ProjectTools.NowPSTime().ToString("yyyyMMddHHmmssfff") + "-" + rand.Next(1000,9999) + "." + fileName.Substring((fileName.Length - 3), 3);
                    string path = Path.Combine(Server.MapPath("../images/menu_items/"), fileName);
                    file.SaveAs(path);
                }
            }
            
            if (Request["menu_item_name"] != null && Request["menu_item_name"] != "")
            { //If request to add or change menu item
                if (Request["menu_item_id"] != null && Request["menu_item_id"] != "")
                {
                    sql = "UPDATE dbo.restaurant_menu_items SET restaurant_branch_id = '" + Session["restaurant_branch_id"] + "', menu_item_name = '" + Request["menu_item_name"] + "', menu_item_description = '" + Request["menu_item_description"] + "', menu_item_price = '" + Request["menu_item_price"] + "', admin_approval_status = '0'";
                    if (fileName != "")
                    {
                        sql += ", menu_item_photo = '" + fileName + "'";
                    }
                    sql += " WHERE menu_item_id='" + Request["menu_item_id"] + "'";
                }
                else
                {
                    feedbackDiv.InnerHtml += "Insert";
                    sql = "INSERT INTO dbo.restaurant_menu_items (restaurant_branch_id, menu_item_name, menu_item_description, menu_item_photo, menu_item_price, admin_approval_status) VALUES ('" + Session["restaurant_branch_id"] + "', '" + Request["menu_item_name"] + "', '" + Request["menu_item_description"] + "', '" + fileName + "', '" + Request["menu_item_price"] + "', '0')";
                }
                db.ProcessData(sql);
                HttpContext.Current.Response.Redirect("menu.aspx");
            }

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                //Load Restaurants List
                sql = "SELECT m.*, r.restaurant_name FROM dbo.restaurant_menu_items AS m LEFT JOIN dbo.restaurant_branch AS r ON m.restaurant_branch_id=r.restaurant_branch_id WHERE menu_item_id = '" + Request.QueryString["id"] + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    Session["menu_item_name"]       = rec["menu_item_name"].ToString();
                    Session["menu_item_description"]= rec["menu_item_description"].ToString();
                    Session["menu_item_price"]      = rec["menu_item_price"].ToString();
                    Session["menu_item_photo"]      = rec["menu_item_photo"].ToString();
                    Session["admin_approval_status"] = rec["admin_approval_status"].ToString();
                }
            }
        }
    }
}