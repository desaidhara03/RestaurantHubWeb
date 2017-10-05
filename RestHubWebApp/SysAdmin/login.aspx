<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RestHubWebApp.SysAdmin.Login" %>
<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Login</title>
        <link href="../assets/css/signin.css" rel="stylesheet">
    </head>
    <body>
        <div class="container">
            <div class="container">
            <div class="media">
                <div class="media-left">
                    <a href="#">
                        <img class="media-object" src="../images/logo.png" alt="Restaurant Hub" width="100" />
                    </a>
                </div>
                <div class="media-body">
                    <h2 class="media-heading">Restaurant Hub</h2>
                    <h4>System Administrator</h4>
                </div>
            </div>
            <form class="form-signin" method="post" action="login">
                <h2 class="form-signin-heading">Please sign in</h2>
                <label for="inputEmail" class="sr-only">Email address</label>
                <input name="email" type="email" id="inputEmail" class="form-control" placeholder="Email address" required autofocus>
                <label for="inputPassword" class="sr-only">Password</label>
                <input name="password" type="password" id="inputPassword" class="form-control" placeholder="Password" required>
                <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
                <div id="feedbackDiv" runat="server"></div>
            </form>
        </div> <!-- /container -->
    </body>
</html>
