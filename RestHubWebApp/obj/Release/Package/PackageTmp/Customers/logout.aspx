<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="RestHubWebApp.Customers.LogOut" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Log Out</title>
        <link href="../assets/css/signin.css" rel="stylesheet">
    </head>
    <body>
        <div class="container">
            <div class="form-signin">
                <h2 class="form-signin-heading">Log Out</h2>
                <p class="lead">You have successfully logged out of the system.</p>
                <p class="lead">If you want to log back in, use the login button to return to the login page.</p>
                <a href="login.aspx" class="btn btn-lg btn-success pull-right">Login</a>
            </div>
        </div> <!-- /container -->
    </body>
</html>