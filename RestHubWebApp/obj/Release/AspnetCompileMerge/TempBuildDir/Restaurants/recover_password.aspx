<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recover_password.aspx.cs" Inherits="RestHubWebApp.Restaurants.recover_password" %>

<!DOCTYPE html>
<html lang="en">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Recover Password</title>
        <link href="../assets/css/signin.css" rel="stylesheet">
    </head>
    <body>
        <div class="container">
            <form class="form-signin" method="post" action="recover_password">
                <h2 class="form-signin-heading">Please, enter your email address below:</h2>
                <label for="inputEmail" class="sr-only">Email address</label>
                <input name="email" type="email" id="inputEmail" class="form-control" placeholder="Email address" required autofocus>
                <button class="btn btn-lg btn-primary btn-block" type="submit">Submit</button>
                <div id="feedbackDiv" runat="server"></div>
            </form>
            <br />
            <div class="row">
                <div class="col-sm-4"></div>
                <div class="col-sm-4">
                    <p class="lead">Click the button to return to the loging page:</p>
                    <a href="login.aspx" class="bnt btn-lg btn-success pull-right">Login</a>
                </div>
                <div class="col-sm-4"></div>
            </div>
            <!-- #include file ="includes\copyright.aspx" -->
        </div> <!-- /container -->
</body>
</html>
