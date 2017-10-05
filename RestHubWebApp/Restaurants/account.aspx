<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="RestHubWebApp.Restaurants.RestaurantAccount" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub Application: My Account</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container">
        <h1 class="page-header"><% if (Session["restaurant_branch_id"] != null) Response.Write("My Account"); else Response.Write("Create an Account"); %></h1>
        <div class="row">
            <div class="col-sm-2">
            </div>
            <div class="col-sm-8">
                <form method="post" action="account" class="form-signin" enctype = "multipart/form-data">
                    <input type="hidden" name="restaurant_id" value="<% if (Session["restaurant_branch_id"] != null) Response.Write( Session["restaurant_branch_id"] ); %>" />
                    <label for="inputRestaurant">Restaurant Name: </label>
                    <input name="restaurant" id="inputRestaurant" class="form-control" placeholder="Restaurant Name" autocomplete="off" value="<% if(Session["restaurant_name"]!=null) Response.Write(Session["restaurant_name"]); %>" required autofocus />
                    <br />
                    <label for="inputAddress">Address: </label>
                    <input name="address" id="inputAddress" class="form-control" placeholder="Street Address" autocomplete="off" value="<% if(Session["street_address"]!=null) Response.Write(Session["street_address"]); %>" required />
                    <br />
                    <label for="inputCity">City: </label>
                    <input name="city" id="inputCity" class="form-control" placeholder="City" autocomplete="off" value="<% if(Session["city"]!=null) Response.Write(Session["city"]); %>" required />
                    <br />
                    <label for="inputState">State</label>
                    <input name="state" id="inputState" class="form-control" placeholder="State" autocomplete="off" value="<% if(Session["state"]!=null) Response.Write(Session["state"]); %>" required />
                    <br />
                    <label for="inputZipCode">Zip Code: </label>
                    <input name="zipcode" id="inputZipCode" class="form-control" placeholder="Zip Code" autocomplete="off" value="<% if(Session["zip_code"]!=null) Response.Write(Session["zip_code"]); %>" required />
                    <br />
                    <label for="inputPhone1">Phone: </label>
                    <input name="phone1" id="inputPhone1" class="form-control" placeholder="(000)000-0000" autocomplete="off" value="<% if(Session["phone1"]!=null) Response.Write(Session["phone1"]); %>" required />
                    <br />
                    <label for="inputPhone2">Phone: </label>
                    <input name="phone2" id="inputPhone2" class="form-control" placeholder="(000)000-0000" autocomplete="off" value="<% if(Session["phone2"]!=null) Response.Write(Session["phone2"]); %>" />
                    <br />
                    <label for="inputPassword">Sales Tax Rate: </label>
                    <input name="sales_tax_rate" type="text" id="taxRate" class="form-control" placeholder="8.75%" autocomplete="off" value="<% if (Session["sales_tax_rate"] != null) Response.Write( (Convert.ToDouble(Session["sales_tax_rate"])*100.0) + "%" ); %>" required />
                    <br />
                    <label for="inputName">Contact Name: </label>
                    <input name="contact_name" id="inputName" class="form-control" placeholder="Full Name" autocomplete="off" value="<% if(Session["branch_manager_name"]!=null) Response.Write(Session["branch_manager_name"]); %>" required />
                    <br />
                    <label for="inputEmail">Email</label>
                    <input name="branch_manager_email" type="email" id="inputEmail" class="form-control" placeholder="Email address" autocomplete="off" value="<% if(Session["branch_manager_email"]!=null) Response.Write(Session["branch_manager_email"]); %>" required />
                    <br />
                    <label for="inputPassword">Password</label>
                    <input name="password" type="password" id="inputPassword" class="form-control" placeholder="Password" autocomplete="off" value="" <% if (Session["branch_manager_email"] == null) Response.Write("required"); %> />
                    <br />
                    <label for="inputCapacity">Max Orders Queue</label>
                    <input name="max_orders_queue" type="text" id="inputCapacity" class="form-control" placeholder="50" autocomplete="off" value="<% if(Session["max_orders_queue"]!=null) Response.Write(Session["max_orders_queue"]); else Response.Write("50"); %>" required />
                    <br />
                    <label for="inputPhoto">Restaurant Photo:</label>
                    <input name="photo" type="file" id="inputPhoto" class="form-control" />
                    <br />
<%  if (Session["restaurant_photo"] != null)
    {
        if (Session["restaurant_photo"].ToString() != "")
        { %>
                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-10 thumbnail">
                            <div class="caption">
                                <h4>Current Photo:</h4>
                                <p><% Response.Write(Session["restaurant_photo"]); %></p>
                            </div>
                            <img src="../images/restaurants/<% Response.Write(Session["restaurant_photo"]); %>" />
                        </div>
                        <div class="col-md-1"></div>
                    </div>
<% 
        }
        else
        {
%>
                    <div class="row">
                        <div class="col-md-3"></div>
                        <div class="col-xs-12 col-md-6 thumbnail">
                            <img src="../images/restaurants/nophoto.png" class="thumbnail" />
                        </div>
                        <div class="col-md-3"></div>
                    </div>
<% 
        }
    }
%>
                    <button class="btn btn-lg btn-primary btn-block" type="submit">Submit</button>
                </form>
                <div id="feedbackDiv" runat="server"></div>
                <% if (Session["restaurant_branch_id"] == null || Convert.ToString(Session["restaurant_branch_id"]) == "")
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
        <!-- #include file ="includes\copyright.aspx" -->
    </div>
    
    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="../assets/js/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="../assets/js/jquery.min.js"><\/script>')</script>
    <script src="../assets/js/bootstrap.min.js"></script>
</body>
</html>
