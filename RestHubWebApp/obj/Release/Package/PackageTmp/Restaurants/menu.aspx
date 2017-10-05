<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="RestHubWebApp.Restaurants.RestaurantMenu" %>
<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Menu</title>
        </head>
    <body>
        <!-- #include file ="includes\top_navigation.aspx" -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 main">
                    <h1 class="page-header">Menu</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 main">
                    <p class="lead">Menu Items</p>
                    <div class="table-responsive" id="menuItemsList" runat="server"></div>
                    <hr />
                    <a class="btn btn-lg btn-success" href="menu_item.aspx">Add Menu Item</a>
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
            $(document).ready(function ($) {
                $(".menuItem").click(function () {
                    window.location = $(this).data("href");
                });
            });
        </script>
    </body>
</html>
