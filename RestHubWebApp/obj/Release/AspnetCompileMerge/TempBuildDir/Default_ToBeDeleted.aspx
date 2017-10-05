<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default_ToBeDeleted.aspx.cs" Inherits="RestHubWebApp._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <meta name="description" content="">
        <title>Restaurant Hub: Sign In</title>
        <link href="assets/css/signin.css" rel="stylesheet">
    </head>
    <body>
        <h2 class="form-signin-heading">Please sign in</h2>
        <label for="inputEmail" class="sr-only">Email address</label>
        <asp:TextBox ID="Email_tb" class="form-control" placeholder="Email address" runat="server"></asp:TextBox>
        <label for="inputPassword" class="sr-only">Password</label>
        <asp:TextBox type="password" ID="Password_tb" class="form-control" placeholder="Password" runat="server"></asp:TextBox>
        <div class="checkbox">
            <label>
                <input type="checkbox" value="remember-me">
                Remember me
            </label>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="login_btn_Click" ID="signin_btn" Text="Sign In" class="btn btn-primary" />
            </div>
        </div>
        <%--<asp:Button class="btn btn-lg btn-primary btn-block" ID="signin_btn" runat="server" Text="Sign In" OnClick="login_btn_Click" />--%>

        <div class="login-register">
            <a href="Account/Register">Create new account</a>
        </div>
        
        <asp:Label ID="ErrorMsg_lbl" runat="server" ForeColor="Red" Text=""></asp:Label>
    </body>
</asp:Content>
