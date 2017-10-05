<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_item.aspx.cs" Inherits="RestHubWebApp.Restaurants.RestaurantMenuItem" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <!-- #include file ="includes\html_includes.aspx" -->
        <title>Restaurant Hub: Menu Item Details</title>
    </head>
    <body>
        <!-- #include file ="includes\top_navigation.aspx" -->
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 main">
                    <h1 class="page-header">Menu Item Details</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 main">
                    <%
                        //load values from server if any and dismiss session values
                        string itemName = "";
                        string itemDescription = "";
                        string itemPrice = "";
                        string itemPhoto = "";
                        string itemStatus = "";

                        if (Session["menu_item_name"] != null && Session["menu_item_name"].ToString() != "")
                        {
                            itemName        = Session["menu_item_name"].ToString();
                            itemDescription = Session["menu_item_description"].ToString();
                            itemPrice       = Session["menu_item_price"].ToString();
                            itemPhoto       = Session["menu_item_photo"].ToString();
                            itemStatus      = Session["admin_approval_status"].ToString();

                            //dismiss session values
                            Session["menu_item_name"] = "";
                            Session["menu_item_description"] = "";
                            Session["menu_item_price"] = "";
                            Session["menu_item_photo"] = "";
                            Session["admin_approval_status"] = "";
                        }
                    %>
                    <div class="col-sm-3">
                        <div class="thumbnail">
                        <% if (itemPhoto != "") { %>
                            <img src="../images/menu_items/<% Response.Write(itemPhoto); %>" alt="<% Response.Write(itemName); %>" />
                            <p><% Response.Write(itemPhoto); %></p>
                        <% } else { %>
                            <img src="../images/menu_items/nophoto.png" alt="Photo not available" />
                        <% } %>
                        </div>
                    </div>
                    <div class="col-sm-8">
                        <form name="frmMenu" action="menu_item" method="post" enctype="multipart/form-data">
                            <input type="hidden" name="menu_item_id" id="menu_item_id" value="<% Response.Write(Request.QueryString["id"]); %>" />
                            <label for="restaurantName">Restaurant Name: </label>
                            <span id="restaurantName"><% Response.Write(Session["restaurant_name"]); %></span>
                            <br />

                            <label for="inputMenuItemName">Name: </label>
                            <input type="text" name="menu_item_name" id="inputMenuItemName" class="form-control" placeholder="Menu Item Name" autocomplete="off" value="<% Response.Write(itemName); %>" required autofocus />
                            <br />

                            <label for="inputMenuItemDescription">Description: </label>
                            <input type="text" name="menu_item_description" id="inputMenuItemDescription" class="form-control" placeholder="Menu Item Description" autocomplete="off" value="<% Response.Write(itemDescription); %>" />
                            <br />

                            <label for="inputMenuItemPrice">Price: </label>
                            <input type="text" name="menu_item_price" id="inputMenuItemPrice" class="form-control" placeholder="5.00" autocomplete="off" value="<% Response.Write(itemPrice); %>" required />
                            <br />

                            <label for="inputMenuItemPhoto">Photo: </label>
                            <input type="file" name="menu_item_photo" id="inputMenuItemPhoto" class="form-control" />
                            <br />
                            <label for="status">Status: </label>
                            <% if (itemStatus == "True") Response.Write("Approved"); else if (itemStatus == "False") Response.Write("Pending");  %>
						    <br />
                        
                            <% if(Request.QueryString["id"]!=null) { %>
                            <input type="submit" class="btn btn-lg btn-success pull-right" name="btnSend" value="Update" />
                            <a href="menu.aspx?delID=<% Response.Write(Request.QueryString["id"]); %>" class="btn btn-lg btn-danger" onclick ="return confirm('Are you sure you want to delete this item?');"><span class="glyphicon glyphicon-remove"></span> Delete</a>
                            <% }else { %>
                            <input type="submit" name="btnSend" value="Save" class="btn btn-lg btn-warning pull-right" />
                            <% } %>
                            <a href="menu.aspx" class="btn btn-lg btn-default pull-right" style="margin:0 10px;" /><span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span> Return</a>
                        </form>
                    </div>
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
