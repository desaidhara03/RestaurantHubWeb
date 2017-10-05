<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="find_restaurant.aspx.cs" Inherits="RestHubWebApp.Customers.find_restaurant" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurants</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
        <div class="col-sm-12 main">
            <h1 class="page-header">Restaurant List</h1>
            <div id="restaurantList" runat="server"></div>
            <br clear="all" />
            <div id="feedbackDiv" runat="server"></div>
        </div>
        </div>
    </div>
    <!-- #include file ="includes\footer.aspx" -->
    <script type="text/javascript">
        $(document).ready(function ($) {
            $("#restaurantList div").click(function () {
                window.location = $(this).data("href");
            });
        });
    </script>
</body>
</html>
