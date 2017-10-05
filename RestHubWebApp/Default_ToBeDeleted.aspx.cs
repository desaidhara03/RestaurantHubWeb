using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RestHubService;

namespace RestHubWebApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void login_btn_Click(object sender, EventArgs e)
        {
            ErrorMsg_lbl.Text = "";
            AuthenticationService authenticationService = new AuthenticationService();

            String username = Email_tb.Text;
            String password = Password_tb.Text;

            if (authenticationService.loginUser(username, password))
                Response.Redirect("LandingPage.aspx");
            else
                ErrorMsg_lbl.Text = "Please enter a valid username and password.";
        }
    }
}