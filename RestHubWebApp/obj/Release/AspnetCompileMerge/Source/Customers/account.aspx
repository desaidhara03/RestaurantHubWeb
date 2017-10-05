<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="RestHubWebApp.Customers.Account" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: Account</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container">
        <h1 class="page-header"><% if (Session["customer_id"] != null) Response.Write("My Account"); else Response.Write("Create an Account"); %></h1>
        <div class="row">
            <div class="col-sm-2">
            </div>
            <div class="col-sm-8">
                <form method="post" action="Account" class="form-signin">
                    <h3>Contact Information</h3>
                    <label for="inputName">Name: </label>
                    <input name="name" id="inputName" class="form-control" placeholder="Joe Doe" autocomplete="off" value="<% if(Session["name"]!=null) Response.Write(Session["name"]); %>" required autofocus />
                    <br />
                    <label for="inputEmail">E-Mail: </label>
                    <input name="email" id="inputEmail" class="form-control" placeholder="Joe@example.com" autocomplete="off" value="<% if(Session["email"]!=null) Response.Write(Session["email"]); %>" required />
                    <br />
                    <label for="inputPhone">Phone: </label>
                    <input name="phone" id="inputPhone" class="form-control" placeholder="(888)123-4567" autocomplete="off" value="<% if(Session["phone"]!=null) Response.Write(Session["phone"]); %>" required />
                    <br />
                    <label for="inputPassword">Password: </label>
                    <input name="password" type="password" id="inputPassword" class="form-control" placeholder="*******" autocomplete="off" value="" />
                    <br />
                    <% if (Session["customer_id"] != null) { %>
                    <a href="account_address" class="btn btn-lg btn-default pull-right" style="margin-left:10px;">Next <span class="glyphicon glyphicon-arrow-right"></span></a>
                    <% } %>
                    <button class="btn btn-lg btn-primary pull-right" type="submit">Save</button>
                    
                </form>
                <div id="feedbackDiv" runat="server"></div>
                <% if (Session["customer_id"] == null || Session["customer_id"].ToString() == "")
                    {
                %>
                <p class="lead">If you already have an account with us, please, login:</p>
                <a href="login.aspx" class="bnt btn-lg btn-success">Login</a>
                <br />
                <%
                    }
                %>
            </div>
            <div class="col-sm-2">
            </div>
        </div>
        <!-- #include file ="includes\footer.aspx" -->
    </div>
</body>
</html>
