<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_details.aspx.cs" Inherits="RestHubWebApp.Restaurants.order_details" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Order Details</title>
        </head>
    <body>
        <!-- #include file ="includes\top_navigation.aspx" -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 main">
                    <h1 class="page-header">Order Details</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-8 main">
                    <div class="panel panel-default">
                        <!-- Default panel contents -->
                        <div class="panel-heading">Order# <% Response.Write(Request.QueryString["oID"]); %></div>
                        <div class="panel-body">
                            <!-- Table -->
                            <table class="table table-responsive" Width="100%">
                                <tr>
                                    <th>Item Name</th>
                                    <th>Qtty</th>
                                    <th>Price</th>
                                    <th>Subtotal</th>
                                </tr>
                                <tr>
                                    <td id="tblItemNames" runat="server"></td>
                                    <td id="tblItemQttys" runat="server"></td>
                                    <td id="tblItemPrices" runat="server"></td>
                                    <td id="tblItemSubtotals" runat="server"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td id="tblTotalsTitles" runat="server"></td>
                                    <td id="tblTotalsValues" runat="server"></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 main">
                    <h4>Order Status:</h4>
                    <div class="btn-group-vertical" role="group" runat="server" id="oStatus"></div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6" id="etaDetails" runat="server"></div>
                <div class="col-md-6">
                    <a href="orders.aspx" class="btn btn-lg btn-success">Return</a>
                </div>
            </div>
            <div id="feedbackDiv" runat="server"></div>
            <!-- #include file ="includes\copyright.aspx" -->
        </div>

        <!-- Bootstrap core JavaScript
        ================================================== -->
        <!-- Placed at the end of the document so the pages load faster -->
        <script src="../assets/js/jquery.min.js"></script>
        <script>window.jQuery || document.write('<script src="../assets/js/jquery.min.js"><\/script>')</script>
        <script src="../assets/js/bootstrap.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function ($) {
                $(".menuItem").click(function () {
                    window.location = $(this).data("href");
                });
            });
        </script>
    </body>
</html>