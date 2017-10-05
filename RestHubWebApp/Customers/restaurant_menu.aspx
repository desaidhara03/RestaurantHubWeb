<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="restaurant_menu.aspx.cs" Inherits="RestHubWebApp.Customers.restaurant_menu" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Menu</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
        <div class="col-sm-12 main">
            <h1 class="page-header">Menu: <% Response.Write(Session["last_restaurant_name"]); %></h1>
            <div id="menuList" runat="server"></div>
            <br clear="all" />
            <div id="feedbackDiv" runat="server"></div>
        </div>
        </div>
    </div>
    <!-- #include file ="includes\footer.aspx" -->
</body>
</html>
