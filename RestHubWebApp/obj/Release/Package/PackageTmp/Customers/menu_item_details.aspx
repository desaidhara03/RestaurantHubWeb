<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_item_details.aspx.cs" Inherits="RestHubWebApp.Customers.menu_item_details" %>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Menu Item Details</title>
    <style type="text/css">
        #txtQtty{
            width:50px;
            display:inline-block;
        }
    </style>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
        <div class="col-sm-12 main">
            <h1 class="page-header" id="menuItemTitle" runat="server"></h1>
            <div id="menuItemDetails" runat="server"></div>
            <br clear="all" />
            <div id="feedbackDiv" runat="server"></div>
        </div>
        </div>
    </div>
    <!-- #include file ="includes\footer.aspx" -->
    <script>
        function AddToCart(mid, act) {
            var qtty = $("#txtQtty").val();
            window.location = "cart?id=" + mid + "&qtty=" + qtty + "&action=" + act;
        }
    </script>
</body>
</html>
