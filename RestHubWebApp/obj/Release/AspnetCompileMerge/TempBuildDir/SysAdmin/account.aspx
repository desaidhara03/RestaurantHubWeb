<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="RestHubWebApp.SysAdmin.login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: My Account</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container main">
        <br />
        <h1 class="page-header">My Account</h1>
        <div class="row">
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
                <form method="post" action="account" class="form-signin">
                    <label for="inputName">Name</label>
                    <input name="name" id="inputName" class="form-control" placeholder="Full Name" autocomplete="off" value="<% if(Session["sys_admin_name"]!=null) Response.Write(Session["sys_admin_name"]); %>" required autofocus />
                    <br />
                    <label for="inputEmail">Email</label>
                    <input name="email" type="email" id="inputEmail" class="form-control" placeholder="Email address" autocomplete="off" value="<% if(Session["sys_admin_email"]!=null) Response.Write(Session["sys_admin_email"]); %>" required />
                    <br />
                    <label for="inputPassword">Password</label>
                    <input name="password" type="password" id="inputPassword" class="form-control" placeholder="Password" autocomplete="off" value="" <% if (Session["sys_admin_email"] == null) Response.Write("required"); %> />
                    <br />
                    <button class="btn btn-lg btn-primary btn-block" type="submit">Save</button>
                </form>
                <div id="feedbackDiv" runat="server"></div>
            </div>
            <div class="col-sm-3">
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
