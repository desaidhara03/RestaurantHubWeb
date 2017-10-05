using System.Collections.Generic;

namespace RestHubWebApp
{
    /* ORDER INFORMATION DATA STRUCTURE: DATA SENT TO AUTHORIZE NET */
    public class orderInformation
    {
        public string invoiceNumber;
        public List<cartItem> cart;
        public cartTotals totals;
        public customerInfo billToCustomer;
    }

    public class cartItem
    {
        public string itemId;
        public string itemName;
        public string itemDescription;
        public decimal itemQuantity;
        public decimal itemPrice;
    }

    public class cartTotals
    {
        public double salesTax;
        public double totalAmount;
    }

    public class customerInfo
    {
        public string customerID;
        public string customerEmail;
        public customerAddress billToAddress;
    }

    public class customerAddress
    {
        public string firstName;
        public string lastName;
        public string streetAddress;
        public string city;
        public string state;
        public string country;
        public string zipcode;
        public string phone;
    }
}
