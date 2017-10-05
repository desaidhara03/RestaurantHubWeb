<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_success.aspx.cs" Inherits="RestHubWebApp.Customers.order_success" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Order Confirmed</title>

    <%--Notification--%>

    <script type="text/javascript">

        function ExecutePageMethod(page, fn, paramArray, successFn, errorFn) {
            var paramList = '';
            if (paramArray.length > 0) {
                for (var i = 0; i < paramArray.length; i += 2) {
                    if (paramList.length > 0) paramList += ',';
                    paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
                }
            }
            paramList = '{' + paramList + '}';

            $.ajax({
                type: "POST",
                url: page + "/" + fn,
                contentType: "application/json; charset=utf-8",
                data: paramList,
                dataType: "json",
                success: successFn,
                error: errorFn
            });
        }

        function OnSuccess(result) {
            //var parsedResult = jQuery.parseJSON(result);
            // var parsedResult = result.d;
            //var parsedResult = JSON.parse(result)
            //alert("order status : " + result.d);

            document.getElementById('OrderStatus_lbl').innerHTML = "Order Status : " + result.d;
            
            <%--// var orderStatus_lbl = document.getElementById('<%= OrderStatus_lbl.ClientID %>');--%>
           // orderStatus_lbl.Text = "order status : " + result.d;
        }

        function OnFailure(result) {
            // alert("Failed to receive the response.");
            document.getElementById('OrderStatus_lbl').innerHTML = "Waiting for order status to be updated.";
        }

        var ajax_call = function () {
            ExecutePageMethod("order_success.aspx", "CheckOrderStatus", [], OnSuccess, OnFailure);
        };

        var interval = 1000 * 5; // Every 5 seconds

        setInterval(ajax_call, interval);

    </script>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <form runat="server">
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12 main">
                    <h1 class="page-header">Order Request</h1>
                    <div style="margin-left: auto; margin-right: auto; text-align: center;">
                        <asp:Label ID="OrderStatus_lbl" class="label label-info" Text="" BackColor="red" runat="server" Font-Bold="True" Font-Size="Medium" />
                    </div>
                    <div id="orderSuccessInformation" runat="server"></div>
                    <%--<asp:HiddenField ID="OrderId_hf" runat="server" />--%>
                </div>
            </div>
        </div>
        <!-- #include file ="includes\footer.aspx" -->
    </form>
</body>
</html>
