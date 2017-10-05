using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using RestHubWebApp.Models;
using RestHubService;

namespace RestHubWebApp.Account
{
    public partial class Register : Page
    {
        protected void nextbtn_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = email_tb.Text, Email = email_tb.Text };
            IdentityResult result = manager.Create(user, password_tb.Text);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);


                //Dhara's Code

                String email = email_tb.Text;
                String password = password_tb.Text;
                String name = name_tb.Text;
                String phone = phone_tb.Text;

                AuthenticationService authenticatorService = new AuthenticationService();
                int customerId = authenticatorService.registerUser(email, password, name, phone);  // Needed customer id to store credit card info in next page.
                if (customerId != 0)
                {
                    Application["customerid"] = customerId;
                    Response.Redirect("CreditCard.aspx");
                }
                else
                    ErrorMessage.Text = "Something went wrong creating new user. So Sorry for that! We are actively working to fix the issue.";
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}