using System.Web;

namespace RestHubWebApp
{
    public class Ini : System.Web.UI.Page
    {
        public bool admin_authenticated(string user_id)
        {
            if( Session["auth"] != null )
            {
                if(ProjectTools.CalculateMD5Hash(user_id) == Session["auth"].ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public void isAuthSysAdmin()
        {
            if( Session["auth"]==null || Session["auth"].ToString()=="" )
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page

            //Validate Administrator Authentication
            Ini init = new Ini();
            bool isLoggedIn = false;
            if (Session["sys_admin_email"] != null)
            {
                isLoggedIn = init.admin_authenticated(Session["sys_admin_email"].ToString());
            }

            if (!isLoggedIn)
            {
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
            }
        }

        public void isAuthRestaurant()
        {
            if (Session["auth"] == null || Session["auth"].ToString() == "")
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page

            //Validate Restaurant Authentication
            Ini init = new Ini();
            bool isLoggedIn = false;
            if (Session["branch_manager_email"] != null)
            {
                isLoggedIn = init.admin_authenticated(Session["branch_manager_email"].ToString());
            }

            if (!isLoggedIn)
            {
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
            }
        }

        public void isAuthUser()
        {
            if (Session["auth"] == null || Session["auth"].ToString() == "")
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page

            //Validate Restaurant Authentication
            Ini init = new Ini();
            bool isLoggedIn = false;
            if (Session["email"] != null)
            {
                isLoggedIn = init.admin_authenticated(Session["email"].ToString());
            }

            if (!isLoggedIn)
            {
                HttpContext.Current.Response.Redirect("login.aspx"); //redirect user to the login page
            }
        }
    }
}