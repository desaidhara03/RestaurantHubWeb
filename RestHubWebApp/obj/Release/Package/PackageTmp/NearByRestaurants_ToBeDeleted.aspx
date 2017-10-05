<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NearByRestaurants_ToBeDeleted.aspx.cs" Inherits="RestHubWebApp.NearByRestaurants" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="" />
    <link rel="icon" href="favicon.ico" />
    <title>Restaurant List Module</title>
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container-fluid">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="#">Restaurant Hub Application</a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
            <ul class="nav navbar-nav navbar-right">
            <li><a href="AdminDashboard.aspx">Home</a></li>
            <li><a href="#">My Account</a></li>
            <li><a href="#">Recomendations</a></li>
                <li><a href="#">Shopping Cart</a></li>
            </ul>
        </div>
        </div>
    </nav>

    <div class="container-fluid">
        <div class="row">
        <div class="col-sm-12 main">
            <h1 class="page-header">Restaurant List</h1>
            <div id="feedbackDiv" runat="server"></div>
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