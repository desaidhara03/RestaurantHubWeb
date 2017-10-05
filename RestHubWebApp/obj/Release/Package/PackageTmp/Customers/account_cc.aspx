<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_cc.aspx.cs" Inherits="RestHubWebApp.Customers.AccountCC" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- #include file ="includes\html_includes.aspx" -->
    <title>Restaurant Hub: Payment Info</title>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    <div class="container">
        <h1 class="page-header" style="margin-bottom:0;">Credit Card</h1>
        <button class="btn btn-md btn-success pull-right" id="btnAddAddressSwitch" style="margin-top:-40px;" onclick="switch_editCC(this);"><% if (Session["cc_type"] == null) Response.Write("Add Card"); else Response.Write("Update Card"); %></button>
        <form method="post" action="account_cc" class="form-signin" id="frmCCInfo" style="display:none;">
            <div class="row">
                <div class="col-sm-2">
                </div>
                <div class="col-sm-8">
                    <h3>Credit Card Information</h3>
                    <label for="inputCCType">Type: </label>
                    <div class="dropdown">
                        <button class="btn btn-default dropdown-toggle" type="button" id="ddCCType" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span id="ddCCTypeLabel">
                                <% 
                                    if (Session["cc_type"]!=null)
                                        Response.Write(Session["cc_type"]);
                                    else
                                        Response.Write("Please, select one.");
                                %>
                            </span>
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="ddCCType" id="ccTypeOprions">
                            <li><a href="javascript:void(0);" onclick="updateCCType('Visa');">Visa</a></li>
                            <li><a href="javascript:void(0);" onclick="updateCCType('Master Card');">Master Card</a></li>
                            <li><a href="javascript:void(0);" onclick="updateCCType('American Express');">American Express</a></li>
                            <li><a href="javascript:void(0);" onclick="updateCCType('Discover');">Discover</a></li>
                        </ul>
                    </div>
                    <br />
                    <label for="inputCCName">Name on Card: </label>
                    <input name="cc_name" id="inputCCName" class="form-control" autocomplete="off" value="<% Response.Write(Session["ccName"]); %>" maxlength="50" required />
                    <br />
                    <label for="inputCCNumber">Number: </label>
                    <input name="cc_number" id="inputCCNumber" class="form-control" autocomplete="off" value="" maxlength="16" onblur="validateCCNumber(this.value);" required />
                    <br />
                    <div class="row">
                        <div class="col-sm-6 col-xs-9">
                            <div class="col-sm-8 col-xs-7">
                                <label for="inputExpirationMonth">Expiration Month:</label>
<% 
    Int16 expMonth = 0;
    if (Session["ccExpMonth"] != null)
    {
        expMonth = Convert.ToInt16(Session["ccExpMonth"]);
    }
%>
                                <select class="form-control" name="expiry-month" id="inputExpirationMonth">
                                    <option value="01"<% if (expMonth == 1) Response.Write(" selected"); %>>Jan (01)</option>
                                    <option value="02"<% if (expMonth == 2) Response.Write(" selected"); %>>Feb (02)</option>
                                    <option value="03"<% if (expMonth == 3) Response.Write(" selected"); %>>Mar (03)</option>
                                    <option value="04"<% if (expMonth == 4) Response.Write(" selected"); %>>Apr (04)</option>
                                    <option value="05"<% if (expMonth == 5) Response.Write(" selected"); %>>May (05)</option>
                                    <option value="06"<% if (expMonth == 6) Response.Write(" selected"); %>>June (06)</option>
                                    <option value="07"<% if (expMonth == 7) Response.Write(" selected"); %>>July (07)</option>
                                    <option value="08"<% if (expMonth == 8) Response.Write(" selected"); %>>Aug (08)</option>
                                    <option value="09"<% if (expMonth == 9) Response.Write(" selected"); %>>Sep (09)</option>
                                    <option value="10"<% if (expMonth == 10) Response.Write(" selected"); %>>Oct (10)</option>
                                    <option value="11"<% if (expMonth == 11) Response.Write(" selected"); %>>Nov (11)</option>
                                    <option value="12"<% if (expMonth == 12) Response.Write(" selected"); %>>Dec (12)</option>
                                </select>
                            </div>
                            <div class="col-sm-4 col-xs-5">
                                <label for="inputExpirationYear">Year:</label>
<%
    Int16 expYear = 0;
    if (Session["ccExpYear"] != null)
    {
        expYear = Convert.ToInt16(Session["ccExpYear"]);
    }
%>
                                <select class="form-control" name="expiry-year" id="inputExpirationYear">
                                    <option<% if (expYear == 2016) Response.Write(" selected"); %>>2016</option>
                                    <option<% if (expYear == 2017) Response.Write(" selected"); %>>2017</option>
                                    <option<% if (expYear == 2018) Response.Write(" selected"); %>>2018</option>
                                    <option<% if (expYear == 2019) Response.Write(" selected"); %>>2019</option>
                                    <option<% if (expYear == 2020) Response.Write(" selected"); %>>2020</option>
                                    <option<% if (expYear == 2021) Response.Write(" selected"); %>>2021</option>
                                    <option<% if (expYear == 2022) Response.Write(" selected"); %>>2022</option>
                                    <option<% if (expYear == 2023) Response.Write(" selected"); %>>2023</option>
                                    <option<% if (expYear == 2024) Response.Write(" selected"); %>>2024</option>
                                    <option<% if (expYear == 2025) Response.Write(" selected"); %>>2025</option>
                                    <option<% if (expYear == 2026) Response.Write(" selected"); %>>2026</option>
                                    <option<% if (expYear == 2027) Response.Write(" selected"); %>>2027</option>
                                    <option<% if (expYear == 2028) Response.Write(" selected"); %>>2028</option>
                                    <option<% if (expYear == 2029) Response.Write(" selected"); %>>2029</option>
                                    <option<% if (expYear == 2030) Response.Write(" selected"); %>>2030</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-sm-6 col-xs-3">
                            <label for="inputName">CVV: </label>
                            <input name="cvv_number" id="inputCVV" class="form-control" autocomplete="off" maxlength="3" value="" onblur="validateCVV(this.value);" required />
                        </div>
                    </div>
                    <br />
                    <label>Billing Address</label><br />
                    <div class="dropdown">
                        <button class="btn btn-default dropdown-toggle" type="button" id="ddAddress" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span id="ddAddressLabel">
                            <%
                                if (Session["billing_address"]!=null)
                                    Response.Write(Session["billing_address"]);
                                else
                                    Response.Write("Please, select one");
                            %>
                            </span>
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" aria-labelledby="ddAddress" id="billAddressOptions" runat="server"></ul>
                    </div>

                    <input type="hidden" name="cc_type" id="ccType" value="<% Response.Write(Session["cc_type"]); %>" />
                    <input type="hidden" name="billing_address_id" id="billing_address_id" value="<% Response.Write(Session["billing_address_id"]); %>" />
                    <br />
                    <button class="btn btn-lg btn-primary btn-block" type="submit">Submit</button>
                </div>
                <div class="col-sm-2">
                </div>
            </div>
        </form>

        <br clear="all" />
        <div class="row">
            <div class="col-sm-3"></div>
            <div class="col-sm-6">
                <div id="ccOnFile" runat="server"></div>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <div class="row">
            <div class="col-sm-3"></div>
            <div class="col-sm-6">
                <div id="feedbackDiv" runat="server"></div>
                <a href="find_restaurant" class="btn btn-lg btn-success pull-right" style="margin:0 10px;">Restaurants <span class="glyphicon glyphicon-map-marker"></span></a>
                <a href="account_address" class="btn btn-lg btn-default pull-right" style="margin:0 10px;">Previous <span class="glyphicon glyphicon-arrow-left"></span></a>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <!-- #include file ="includes\footer.aspx" -->
    </div>
    <script>
        function switch_editCC(e) {
            var el = document.getElementById(e.id);
            
            if (el.innerHTML == "Add Card" | el.innerHTML == "Update Card") {
                $("#frmCCInfo").css("display", "block");
                $("#ccOnFile").css("display", "none");
                el.innerHTML = "Hide Form";
            } else {
                $("#frmCCInfo").css("display", "none");
                $("#ccOnFile").css("display", "block");
                el.innerHTML = "Update Card";
            }
        }

        function updateAddress(aid, addr) {
            document.getElementById("billing_address_id").value = aid;
            document.getElementById("ddAddressLabel").innerText = addr;
        }

        function updateCCType(newType) {
            document.getElementById("ccType").value = newType;
            document.getElementById("ddCCTypeLabel").innerText = newType;
        }

        function validateCCNumber(str) {
            if (str == "")
                return;
            //remove everything that is not a number
            var pattern = /[^0-9]/g; //not a number pattern
            var result = str.replace(pattern,"");
            if ( result.length < 15 ) {
                //number is too short
                $("#inputCCNumber").val("");
                alert("The Credit Card Number you entered is too short. A valid credit card number must have at least 15 characters. Please, try again.");
            } else {
                $("#inputCCNumber").val(result);
            }

        }

        function validateCVV(str) {
            if (str == "")
                return;
            //remove everything that is not a number
            var pattern = /[^0-9]/g; //not a number pattern
            var result = str.replace(pattern, "");
            if (result.length != 3) {
                $("#inputCVV").val("");
            }
        }
    </script>
</body>
</html>
