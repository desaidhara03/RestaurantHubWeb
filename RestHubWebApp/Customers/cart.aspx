<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="RestHubWebApp.Customers.cart" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Shopping Cart</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12 main">
                <h1 class="page-header">Cart</h1>
                <div id="shoppingCart" runat="server"></div>
                <br clear="all" />
                <div id="feedbackDiv" runat="server"></div>
            </div>
        </div>
    </div>
    <!-- #include file ="includes\footer.aspx" -->
</body>
</html>
