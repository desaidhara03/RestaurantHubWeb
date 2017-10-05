<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="RestHubWebApp.SysAdmin.AdminDashboard" %>
<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Dashboard</title>
    </head>
    <body>
        <!-- #include file ="includes\top_navigation.aspx" -->
        <div class="container-fluid">
            <br />
            <h1 class="page-header">Dashboard</h1>
            <div class="row">
                <div class="col-md-6 main">
                    <p class="lead">Restaurants Pending Review</p>
                    <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                        <tr>
                            <th>#</th>
                            <th>Restaurant</th>
                            <th>Created</th>
                        </tr>
                        </thead>
                        <tbody id="tblRestaurants" runat="server"></tbody>
                    </table>
                    </div>
                </div>

                <div class="col-md-6 main">
                    <p class="lead">Menu Items Pending Review</p>
                    <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                        <tr>
                            <th>#</th>
                            <th>Restaurant Name</th>
                            <th>Menu Item</th>
                        </tr>
                        </thead>
                        <tbody id="tblMenuItems" runat="server"></tbody>
                    </table>
                    </div>
                </div>
            </div>
            </div>
        </div>
        <!-- Bootstrap core JavaScript
        ================================================== -->
        <!-- Placed at the end of the document so the pages load faster -->
        <script src="../assets/js/jquery.min.js"></script>
        <script>window.jQuery || document.write('<script src="../assets/js/jquery.min.js"><\/script>')</script>
        <script src="../assets/js/bootstrap.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function ($) {
                $("#tblRestaurants tr").click(function () {
                    window.location = $(this).data("href");
                });
                $("#tblMenuItems tr").click(function () {
                    window.location = $(this).data("href");
                });
            });
        </script>
    </body>
</html>
