<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="RestHubWebApp.Customers.login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: Login</title>
    <link href="../assets/css/signin.css" rel="stylesheet">
</head>
<body>
    <div class="container">
        <div class="media">
            <div class="media-left">
                <a href="#">
                    <img class="media-object" src="../images/logo.png" alt="Restaurant Hub" width="100" />
                </a>
            </div>
            <div class="media-body">
                <h2 class="media-heading">Restaurant Hub</h2>
                <h4>Customers</h4>
            </div>
        </div>
        <form class="form-signin" method="post" action="Login">
            <h2 class="form-signin-heading">Please, sign in here:</h2>
            <label for="inputEmail" class="sr-only">Email address</label>
            <input name="email" type="email" id="inputEmail" class="form-control" placeholder="Email address" required autofocus>
            <label for="inputPassword" class="sr-only">Password</label>
            <input name="password" type="password" id="inputPassword" class="form-control" placeholder="Password" required>
            <div class="checkbox">
                <label>
                    <input type="checkbox" name="cbxRememberMe" value="remember-me">
                    Remember me
                </label>
            </div>
            <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
            <div id="feedbackDiv" runat="server"></div>
            <a href="recover_password.aspx">I forgot my password...</a>
        </form>
        <br />
        <div class="row">
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
                <p>If you do not have an account yet, please use the following button to create an account:</p>
                <a href="account.aspx" class="bnt btn-lg btn-success pull-right">Registration</a>
            </div>
            <div class="col-sm-4"></div>
        </div>
        <!-- #include file ="includes\footer.aspx" -->
    </div>
    <!-- /container -->
</body>
</html>
