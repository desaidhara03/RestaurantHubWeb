using RestHubService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestHubWebApp.Account
{
    public partial class CreditCard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FinishRegistration_btn_Click(object sender, EventArgs e)
        {
            Guid guid = new Guid();
            int customerId = Convert.ToInt32(Application["customerid"].ToString());

            AuthenticationService authenticatorService = new AuthenticationService();
            bool result = authenticatorService.addCreditCard(nameOnCard_tb.Text, cardNumber_tb.Text, cvv_tb.Text, Convert.ToInt32(expirationMonth_tb.Text), Convert.ToInt32(ExpirationYear_tb.Text), customerId, AddrLine1_tb.Text, AddrLine2_tb.Text, City_tb.Text, State_tb.Text, Postcode_tb.Text, Country_tb.Text);

            if (result)
                IdentityHelper.RedirectToReturnUrl("Default.aspx", Response);
            else
                ErrorMessage.Text = "Something went wrong adding credit card. Please make sure entered credit card info is correct.";
        }

        protected void Skip_btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("LandingPage.aspx");
        }
    }
}