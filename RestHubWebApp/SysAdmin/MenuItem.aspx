<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuItem.aspx.cs" Inherits="RestHubWebApp.SysAdmin.MenuItem" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Menu Item Details</title>
    </head>
    <body>
        <!-- #include file ="includes\top_navigation.aspx" -->
        <br />
        <div class="container-fluid">
            <h1 class="page-header">Menu Item Details</h1>
            <div class="row">
                <div class="col-md-12 main">
                    <form name="frmMenu" action="MenuItem?id=<% Response.Write(Request.QueryString["id"]); %>" method="post">
                        <div id="tblMenuItem" class="table-responsive main" runat="server"></div>
                        <button name="BtnReturn" class="btn btn-lg btn-success" onclick="window.location=dashboard" /><span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span> Return</button>
                        <input type="submit" name="btnSend" value="Update" class="btn btn-lg btn-warning" />
                    </form>
                </div>
            </div>
        </div>
        <!-- Bootstrap core JavaScript
        ================================================== -->
        <!-- Placed at the end of the document so the pages load faster -->
        <script src="../assets/js/jquery.min.js"></script>
        <script>window.jQuery || document.write('<script src="../assets/js/jquery.min.js"><\/script>')</script>
        <script src="../assets/js/bootstrap.min.js"></script>
    </body>
</html>
