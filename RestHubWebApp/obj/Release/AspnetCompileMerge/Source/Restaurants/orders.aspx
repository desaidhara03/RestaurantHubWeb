<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="RestHubWebApp.Restaurants.Orders" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: Orders Manager</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 main">
                <h1 class="page-header">Orders Manager: <% Response.Write(Session["restaurant_name"]); %></h1>
                <a href="#" id="statusUpdate" runat="server"></a>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 main">
                <div class="panel panel-default">
                    <!-- Default panel contents -->
                    <div class="panel-heading">Orders Queue:</div>
                    <div class="panel-body">
                        Please, select an order to see the details and change the order's status.
                    </div>
                    <!-- Tables -->
                    <h3>New Orders</h3>
                    <table class="table table-responsive" id="tblNew">
                        <tr>
                            <th>Item Name</th>
                            <th>Qtty</th>
                            <th>Details</th>
                            <th>Action</th>
                        </tr>
                        <tr>
                            <td colspan="4"><center><img src="../images/loading-circle.gif" /></center></td>
                        </tr>
                    </table>

                    <h3>Preparing</h3>
                    <table class="table table-responsive" id="tblPreparing">
                        <tr>
                            <th>Item Name</th>
                            <th>Qtty</th>
                            <th>Details</th>
                            <th>Action</th>
                        </tr>
                        <tr>
                            <td colspan="4"><center><img src="../images/loading-circle.gif" /></center></td>
                        </tr>
                    </table>
                    <hr />

                    <h3>Ready</h3>
                    <table class="table table-responsive" id="tblReady">
                        <tr>
                            <th>Item Name</th>
                            <th>Qtty</th>
                            <th>Details</th>
                            <th>Action</th>
                        </tr>
                        <tr>
                            <td colspan="4"><center><img src="../images/loading-circle.gif" /></center></td>
                        </tr>
                    </table>
                    <hr />

                    <h3>Delivered</h3>
                    <table class="table table-responsive" id="tblDelivered">
                        <tr>
                            <th>Item Name</th>
                            <th>Qtty</th>
                            <th>Details</th>
                            <th>Action</th>
                        </tr>
                        <tr>
                            <td colspan="4"><center><img src="../images/loading-circle.gif" /></center></td>
                        </tr>
                    </table>

                    <div id="feedbackDiv" runat="server"></div>
                    <input type="hidden" id="restaurantID" value="<% Response.Write(Session["restaurant_branch_id"]); %>" />
                </div>
            </div>
        </div>
        <!-- #include file ="includes\copyright.aspx" -->
    </div>

    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="../assets/js/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="../assets/js/jquery.min.js"><\/script>')</script>
    <script src="../assets/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        $(document).ready(refreshStatus());

        var tid = setInterval(refreshStatus, 5000); //REFRESH STATUS EVERY 10 SECONDS
        function refreshStatus() {
            /* CHECK IF SESSION IS STILL ACTIVE, UPDATE IF IT IS NOT */
            $.ajax({
                type: "POST",
                url: "orders.aspx/StatusHandler",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    UpdateStatusBtn(response.d);
                    $.ajax({
                        type: "POST",
                        url: "orders.aspx/GetOrdersData",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            RefreshTables(response.d); //Orders Data receive: call refresh tables function to update page
                        },
                        failure: function (response) {
                            console.log("Fail to GetOrdersData: " + response.d);
                        }
                    });
                },
                failure: function (response) {
                    console.log("Fail: " + response.d);
                }
            });
        }
        
        function UpdateStatusBtn(status) {
            if (status == "online") {
                $("#statusUpdate").html("<span class='glyphicon glyphicon-ok'></span> Restaurant Online");
                $("#statusUpdate").attr("class","btn btn-lg btn-success pull-right");
                $("#statusUpdate").attr("href", "orders.aspx?status=offline");
            } else if (status == "offline") {
                $("#statusUpdate").html("<span class='glyphicon glyphicon-off'></span> Restaurant Offline");
                $("#statusUpdate").attr("class","btn btn-lg btn-warning pull-right");
                $("#statusUpdate").attr("href", "orders.aspx?status=online");
            } else {
                $("#statusUpdate").html("<span class='glyphicon glyphicon-time'></span> Pending Approval");
                $("#statusUpdate").attr("class", "btn btn-lg btn-danger pull-right");
                $("#statusUpdate").attr("href", "#");
            }
        }
    
        function RefreshTables(data) {
            //CLEAN UP TABLES: DELETE ALL ROWS EXCEPT THE HEADER
            $("#tblNew").find("tr:gt(0)").remove();
            $("#tblPreparing").find("tr:gt(0)").remove();
            $("#tblReady").find("tr:gt(0)").remove();
            $("#tblDelivered").find("tr:gt(0)").remove();

            if (data != "") {
                var orders = $.parseJSON(data);
                var itemNameList;
                var itemQttyList;
                var tblID;

                for (var i = 0; i < orders.length; i++) {
                    itemNameList = "";  //RESET NAME LIST
                    itemQttyList = "";  //RESET QUANTITIES LIST
                    for (var j = 0; j < orders[i].orderCart.length; j++) { //LOAD ITEMS NAMES AND QUANTITIES LISTS
                        itemNameList += orders[i].orderCart[j].itemName + "<br />\n";
                        itemQttyList += orders[i].orderCart[j].itemQuantity + "<br />\n";
                    }

                    switch (orders[i].orderStatus) { //FIND TABLE ID
                        case 'new':
                            tblID = "tblNew";
                            break;
                        case 'preparing':
                            tblID = "tblPreparing";
                            break;
                        case 'ready':
                            tblID = "tblReady";
                            break;
                        case 'delivered':
                            tblID = "tblDelivered";
                            break;
                    }

                    //LOAD ROWS INTO TABLE
                    $('#' + tblID + ' tr:last').after(
                        '<tr><td>' +
                            itemNameList +
                        '</td><td>' +
                            itemQttyList +
                        '</td><td>' +
                            '<b>Customer:</b> ' + orders[i].customerName + '<br />' +
                            '<b>Order Time:</b> ' + orders[i].orderDateTime + '<br />' +
                            '<b>ETA:</b> ' + orders[i].orderETA +
                        '</td><td>' +
                            '<a href="order_details.aspx?oID=' + orders[i].orderID + '" class="btn btn-lg btn-success">Details</a>' +
                         '</td></tr>');
                }
            } else {
                //REMOVE LOADING GIF IF TABLE IS EMPTY
                $("#tblNew tr:last").after('<tr><td colspan="4" style="text-align:center;">There are no NEW orders available.</td></tr>');
                $("#tblPreparing tr:last").after('<tr><td colspan="4" style="text-align:center;">There are no PREPARING orders available.</td></tr>');
                $("#tblReady tr:last").after('<tr><td colspan="4" style="text-align:center;">There are no READY orders available.</td></tr>');
                $("#tblDelivered tr:last").after('<tr><td colspan="4" style="text-align:center;">There are no DELIVERED orders available.</td></tr>');
            }
        }
    </script>

</body>
</html>
