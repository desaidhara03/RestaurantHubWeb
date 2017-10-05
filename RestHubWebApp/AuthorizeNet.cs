using System;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Data.SqlClient;

namespace RestHubWebApp
{
    public class ChargeCreditCard
    {
        /* THIS METHOD CALLS AUTHORIZE NET; SEND DATA AND REQUESTS RESPONSE */

        public static string ChargeCard(string customerID, string restaurantID, double subTotal, string ccNumber, string ccCvv, string expDate, orderInformation orderInfo, DateTime customerETA)
        {
            DBObject db = new DBObject();   //Object containing method to access database
            SqlDataReader rec;              //SQL data reader
            string orderID = "";            //TO HOLD ORDER ID AFTER INSERT
            string sql = "";
            int i = 0;                      //GENERIC COUNTER
            ProjectTools.initVars();        //INITIALIZE CONFIGURATION VARIABLES
            String ApiLoginID = ProjectTools.authorizeNetApiLoginID;
            String ApiTransactionKey = ProjectTools.authorizeNetApiTransactionKey;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            //ORDER INFORMATION
            var authNetOrder = new orderExType
            {
                invoiceNumber       = orderInfo.invoiceNumber,
                purchaseOrderNumber = "",
                description         = "New order from Restaurant Hub"
            };

            // CREDIT CARD INFORMATION
            var creditCard = new creditCardType
            {
                cardNumber = ccNumber,
                expirationDate = expDate,
                cardCode = ccCvv
            };
            var authNetPayment = new paymentType {
                Item = creditCard
            };

            //LOAD SHOPPING CART INTO VARIABLE
            int cartSize = orderInfo.cart.Count;    //GET NUMBER OF ITEMS IN CART
            lineItemType[] authNetCart = new lineItemType[cartSize];    //INITIALIZE ARRAY BASED ON CART SIZE
            i = 0;  //CART ARRAY'S INDEX
            foreach(cartItem item in orderInfo.cart)
            {
                lineItemType authNetItem = new lineItemType();
                authNetItem.itemId      = item.itemId;
                authNetItem.name        = item.itemName;
                authNetItem.description = item.itemDescription;
                authNetItem.quantity    = item.itemQuantity;
                authNetItem.unitPrice   = item.itemPrice;
                authNetCart.SetValue(authNetItem, i); //ADD TO CART
                i++; //INCREMENT CART INDEX
            }

            //LOAD BILL TO NAME, ADDRESS, PHONE
            var authNetBilling = new customerAddressType
            {
                firstName   = orderInfo.billToCustomer.billToAddress.firstName,
                lastName    = orderInfo.billToCustomer.billToAddress.lastName,
                address     = orderInfo.billToCustomer.billToAddress.streetAddress,
                city        = orderInfo.billToCustomer.billToAddress.city,
                state       = orderInfo.billToCustomer.billToAddress.state,
                country     = orderInfo.billToCustomer.billToAddress.country,
                zip         = orderInfo.billToCustomer.billToAddress.zipcode,
                phoneNumber = orderInfo.billToCustomer.billToAddress.phone
            };

            //LOAD CUSTOMER ID, EMAIL
            var authNetCustomer = new customerDataType
            {
                id      = orderInfo.billToCustomer.customerID,
                email   = orderInfo.billToCustomer.customerEmail
            };

            /* * * BOF: SEND DATA TO AUTHORIZENET * * */
            var transactionRequest = new transactionRequestType
            {
                transactionType     = transactionTypeEnum.authCaptureTransaction.ToString(),
                amount              = Convert.ToDecimal(orderInfo.totals.totalAmount),
                payment             = authNetPayment,
                order               = authNetOrder,
                lineItems           = authNetCart,
                billTo              = authNetBilling,
                customer            = authNetCustomer
            };
            var request = new createTransactionRequest { transactionRequest = transactionRequest };
            var controller = new createTransactionController(request);
            controller.Execute();
            /* * * EOF: SEND DATA TO AUTHORIZENET * * */

            /* RECEIVE AND PROCESS AUTHORIZE NET'S RESPONSE */
            var response = controller.GetApiResponse();
            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                if (response.transactionResponse != null)
                {
                    if(Convert.ToInt16(response.transactionResponse.responseCode) == 1)
                    { //IF RESPONSE CODE IS 1 (APPROVED)
                      // ADD TO ORDERS TABLE
                        sql = "INSERT INTO dbo.restaurant_orders " +
                                "(customer_id, restaurant_branch_id, order_date_time, pickup_date_time, order_status, subtotal, tax, discount, total_charged, customer_eta) " +
                                "VALUES " +
                                "('" + customerID + "','" + restaurantID + "','" + ProjectTools.NowPSTime().ToString() + "','" + customerETA.ToString() + "','new','" + subTotal + "','" + orderInfo.totals.salesTax + "', '0.00', '" + orderInfo.totals.totalAmount + "','" + customerETA.ToString() + "'); ";
                        //GET LAST INSERT ID
                        sql += "SELECT SCOPE_IDENTITY() AS oID FROM dbo.restaurant_orders;";
                        rec = db.ProcessData(sql);
                        if (rec.HasRows)
                        {
                            rec.Read();
                            orderID = rec["oID"].ToString();
                        }

                        // ADD TO AUTHORIZE NET TABLE
                        sql = "INSERT INTO dbo.authorizenet " +
                                "(customer_id, order_id, transaction_id, response_code, authorization_code, request_time) " +
                                "VALUES " +
                                "('" + customerID + "', '" + orderID + "', '" + response.transactionResponse.transId + "', '" + response.transactionResponse.responseCode + "', '" + response.transactionResponse.authCode + "', '" + ProjectTools.NowPSTime().ToString() + "')";
                        db.ProcessData(sql);

                        //ADD INFORMATION ON ITEMS TO THE ORDER ITEMS TABLE
                        if (orderInfo != null)
                        {
                            sql = "INSERT INTO dbo.restaurant_order_items " +
                                "(order_id, menu_item_id, menu_item_quantity, menu_item_final_price) " +
                                "VALUES ";
                            i = 1;  //CART ITEMS COUNTER
                            foreach (cartItem item in orderInfo.cart)
                            {
                                sql += "('" + orderID + "', '" + item.itemId + "', '" + item.itemQuantity + "', '" + item.itemPrice + "')";
                                if( i < orderInfo.cart.Count)
                                {
                                    sql += ",\n";
                                }else
                                {
                                    sql += ";";
                                }
                                i++;
                            }
                            db.ProcessData(sql);
                        }

                        return orderID;
                    }
                    else
                    { //RESPONSE CODE IS NOT 1: CREDIT CARD DECLINED
                        // ADD TO AUTHORIZE NET TABLE WITH ORDER ID OF ZERO
                        sql = "INSERT INTO dbo.authorizenet " +
                                "(customer_id, order_id, transaction_id, response_code, authorization_code, request_time) " +
                                "VALUES " +
                                "('" + customerID + "', '0', '" + response.transactionResponse.transId + "', '" + response.transactionResponse.responseCode + "', '" + response.transactionResponse.authCode + "', '" + ProjectTools.NowPSTime().ToString() + "')";
                        db.ProcessData(sql);
                        return ""; //ORDER ID NOT RETURNED
                    }
                }else
                { //AUTHORIZE NET RESPONSE: NULL
                    return ""; //ORDER ID NOT RETURNED
                }
            }
            else
            { //AUTHORIZE NET DID NOT RETURN OK
                return ""; //ORDER ID NOT RETURNED
            }
            /* EOF: RECEIVE AND PROCESS AUTHORIZE NET'S RESPONSE */
        } //End Function: ChargeCard
    } //End Class: ChargeCreditCard
} //End namespace