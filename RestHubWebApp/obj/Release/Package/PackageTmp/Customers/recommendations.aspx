<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recommendations.aspx.cs" Inherits="RestHubWebApp.Customers.recommendations" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Menu Items Recommendations</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container-fluid">
        <div class="row">
        <div class="col-sm-12 main">
            <h1 class="page-header">Menu Items Recommendations</h1>
            <div id="menuRecommendations" runat="server"></div>
            <br clear="all" />
            <div id="feedbackDiv" runat="server"></div>
        </div>
        </div>
    </div>
    <!-- #include file ="includes\footer.aspx" -->
</body>
</html>
