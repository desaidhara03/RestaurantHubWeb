<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_address.aspx.cs" Inherits="RestHubWebApp.Customers.AccountAddress" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: Address Book</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container">
        <div class="row">
            <div class="col-sm-2"></div>
            <div class="col-sm-8">
                <h1 class="page-header" style="margin-bottom:0;"><% if (Session["customer_id"] != null) Response.Write("My Account"); else Response.Write("Create an Account"); %></h1>
                <button class="btn btn-md btn-success pull-right" id="btnAddAddressSwitch" style="margin-top:-40px;" onclick="switch_addAddress(this);">Add Address</button>
            </div>
            <div class="col-sm-2"></div>
        </div>
        <div class="row">
            <div class="col-sm-2">
            </div>
            <div class="col-sm-8">
                <form name="frmAddAddress" id="frmAddress" method="post" action="account_address" class="form-signin" style="display:none;">
                    <h3>Address Information</h3>
                    <label for="inputStreetAddress">Street Address: </label>
                    <input name="street_address" id="inputStreetAddress" class="form-control" placeholder="Street Address" autocomplete="off" value="<% if(Session["street_address"]!=null) Response.Write(Session["street_address"]); %>" required />
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
                    <button class="btn btn-lg btn-primary pull-right" type="submit">Add Address</button>
                    <br clear="all" />
                </form>
                <div id="feedbackDiv" runat="server"></div>
                <div id="addressList" runat="server"></div>
                
                <a href="account_cc" class="btn btn-lg btn-default pull-right" style="margin-left:10px;">Next <span class="glyphicon glyphicon-arrow-right"></span></a>
                <a href="account" class="btn btn-lg btn-default pull-right" style="margin-left:10px;">Previous <span class="glyphicon glyphicon-arrow-left"></span></a>
            </div>
            <div class="col-sm-2">
            </div>
        </div>
        <!-- #include file ="includes\footer.aspx" -->
    </div>
    <script>
        function switch_addAddress(e) {
            var el = document.getElementById(e.id);
            if (el.innerHTML == "Add Address") {
                el.innerHTML = "Hide Form";
                $("#frmAddress").css("display","block");
            } else {
                el.innerHTML = "Add Address";
                $("#frmAddress").css("display", "none");
            }
        }
    </script>
</body>
</html>
