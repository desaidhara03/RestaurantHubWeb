<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="RestHubWebApp.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" type="text/css" href="../assets/css/bootstrap.css">

    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">

    <!-- Website CSS style -->
    <link rel="stylesheet" type="text/css" href="../assets/css/main.css">

    <!-- Website Font style -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">

    <!-- Google Fonts -->
    <link href='https://fonts.googleapis.com/css?family=Passion+One' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Oxygen' rel='stylesheet' type='text/css'>

    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Create a new account</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="name_tb" CssClass="col-md-2 control-label">Name</asp:Label>
            <div class="col-md-10">
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-user" aria-hidden="true"></i></span>
                    <asp:TextBox type="text" class="form-control" name="name" ID="name_tb" placeholder="Enter your Name" runat="server" />
                </div>
            </div>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="name_tb"
                CssClass="text-danger" ErrorMessage="The name field is required." />

        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="email_tb" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <%----------------Email--------------%>
                <div class="cols-sm-10">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="fa fa-envelope fa" aria-hidden="true"></i></span>
                        <asp:TextBox type="text" placeholder="Enter your Email" runat="server" ID="email_tb" CssClass="form-control" TextMode="Email" />
                    </div>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="email_tb"
                    CssClass="text-danger" ErrorMessage="The email field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="password_tb" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <div class="input-group">
                    <span class="input-group-addon"><i class="fa fa-lock fa-lg" aria-hidden="true"></i></span>
                    <asp:TextBox type="password" class="form-control" name="password" ID="password_tb" placeholder="Enter your Password" runat="server" />
                </div>
            </div>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="password_tb"
                CssClass="text-danger" ErrorMessage="The password field is required." />

        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="confirmPassword_tb" CssClass="col-md-2 control-label">Confirm password</asp:Label>
            <div class="col-md-10">
                <div class="input-group">
                    <span class="input-group-addon"><i class="fa fa-lock fa-lg" aria-hidden="true"></i></span>
                    <asp:TextBox type="password" class="form-control" name="confirm" ID="confirmPassword_tb" TextMode="Password" placeholder="Confirm your Password" runat="server" />
                </div>
            </div>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="confirmPassword_tb"
                CssClass="text-danger" ErrorMessage="The confirm password field is required." />
            <asp:CompareValidator runat="server" ControlToCompare="password_tb" ControlToValidate="confirmPassword_tb"
                CssClass="text-danger" ErrorMessage="The password and confirmation password do not match." />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="phone_tb" CssClass="col-md-2 control-label">Phone Number</asp:Label>
            <div class="col-md-10">
                <div class="cols-sm-10">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-earphone" aria-hidden="true"></i></span>
                        <asp:TextBox type="text" placeholder="Enter your Phone Number" runat="server" ID="phone_tb" CssClass="form-control" TextMode="Phone" />
                    </div>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="phone_tb"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The phone number is required." />
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="nextbtn_Click" Text="Next" class="btn btn-primary" />
            </div>
        </div>

    </div>
</asp:Content>
