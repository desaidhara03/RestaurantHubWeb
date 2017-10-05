<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
        <meta name="description" content="Restaurant Hub Android Application">
        <meta name="author" content="Will Davies Vasconcelos, Meet Mistry, Dhara Desai, Sampath Kumar Akshitha Karupakula" />
        <link rel="icon" href="favicon.ico" />
        <title>Restaurant Hub</title>
        <!-- Bootstrap core CSS -->
        <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
        <link href="assets/css/dashboard.css" rel="stylesheet" />
        <link href="assets/css/style.css" rel="stylesheet" />
        <link href="assets/css/jumbotron.css" rel="stylesheet">
        <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
        <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
        <![endif]-->
        <style type="text/css">
            .jumbotron{
                background:#FFF url('images/bg.jpg') no-repeat fixed top; 
                background-size:100% auto;
            }
            .jumbotron h1{
                font-weight:bold;
                color:yellow;
                text-shadow:5px 5px 5px black;
                margin:20px auto 0 auto;
            }
            .jumbotron p{
                color:white;
                text-shadow:2px 2px black;
                margin:0 auto;
            }
        </style>
    </head>
    <body>
    <nav class="navbar navbar-default navbar-fixed-top">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="index.aspx"><img src="images/logo.png" alt="Restaurant Hub" /></a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
          <ul class="nav navbar-nav navbar-right">
            <li><a href="Customers/index.aspx" target="_blank">Customer</a></li>
            <li><a href="Restaurants/index.aspx" target="_blank">Restaurant</a></li>
            <li><a href="SysAdmin/index.aspx" target="_blank">Admin</a></li>
          </ul>
        </div><!--/.navbar-collapse -->
      </div>
    </nav>

    <!-- Main jumbotron for a primary marketing message or call to action -->
    <div class="jumbotron">
      <div class="container">
        <h1>Restaurant Hub</h1>
        <p>Welcome to Restaurant Hub! Please select one of the following options to get started.</p>
      </div>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-sm-6" style="text-align:center;">
                <a href="Customers/index.aspx" target="_blank">
                    <img class="img-rounded" src="images/customers.jpg" alt="Generic placeholder image" width="140" height="140">
                </a>
                <h2>Customers</h2>
                <p>Find restaurants in your area and place an order before you arrive!</p>
                <p><a class="btn btn-success" href="Customers/index.aspx" role="button" target="_blank">Customers &raquo;</a></p>
            </div>
            <div class="col-sm-6" style="text-align:center;">
                <a href="Restaurants/index.aspx" target="_blank">
                    <img class="img-rounded" src="images/restaurants.jpg" alt="Generic placeholder image" width="140" height="140">
                </a>
                <h2>Restaurants</h2>
                <p>Attract customers to your restaurant and control order status from start to finish.</p>
                <p><a class="btn btn-success" href="Restaurants/index.aspx" role="button" target="_blank">Restaurants &raquo;</a></p>
            </div>
        </div>
        <hr>
        <footer>
            <%--<p>Restaurant Hub<sup>&trade;</sup> <% Response.Write(ProjectTools.NowPSTime().Year) %></p> TODO: ProjectTools is inaccessible. --%>
        </footer>
    </div> <!-- /container -->


    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="assets/js/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="assets/js/jquery.min.js"><\/script>')</script>
    <script src="assets/js/bootstrap.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="assets/js/ie10-viewport-bug-workaround.js"></script>
  </body>
</html>
