<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CreditCard.aspx.cs" Inherits="RestHubWebApp.Account.CreditCard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <meta charset="utf-8">
    <!-- This file has been downloaded from Bootsnipp.com. Enjoy! -->
    <title>RestHubdroid-creditcard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="http://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet">
    <style type="text/css">
        .submit-button {
            margin-top: 10px;
        }
    </style>
    <script src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="http://netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>

    <div class="container">
        <div class='row'>
            <div class='col-md-4'></div>
            <div class='col-md-4'>
                <script src='https://js.stripe.com/v2/' type='text/javascript'></script>
                <%--<form accept-charset="UTF-8" action="/" class="require-validation" data-cc-on-file="false" data-stripe-publishable-key="pk_bQQaTxnaZlzv4FnnuZ28LFHccVSaj" id="payment-form" method="post">--%>
                <div style="margin: 0; padding: 0; display: inline">
                    <input name="utf8" type="hidden" value="&#x2713;" /><input name="_method" type="hidden" value="PUT" /><input name="authenticity_token" type="hidden" value="qLZ9cScer7ZxqulsUWazw4x3cSEzv899SP/7ThPCOV8=" />
                </div>
                <div class='form-row'>
                    <div class='col-xs-12 form-group required'>
                        <label class='control-label'>Name on Card</label>
                        <asp:TextBox type="text" placeholder="Enter Name on Card" runat="server" ID="nameOnCard_tb" CssClass="form-control" size='4' />
                    </div>
                </div>
                <div class='form-row'>
                    <div class='col-xs-12 form-group card required'>
                        <label class='control-label'>Card Number</label>
                        <asp:TextBox type="text" autocomplete='off' placeholder="Enter Card Number" runat="server" ID="cardNumber_tb" CssClass="form-control card-number" size='20' />
                    </div>
                </div>
                <div class='form-row'>
                    <div class='col-xs-4 form-group cvc required'>
                        <label class='control-label'>CVC</label>
                        <asp:TextBox type="text" autocomplete='off' placeholder="ex. 311" runat="server" ID="cvv_tb" CssClass="form-control card-cvc" size='4' />
                    </div>
                    <div class='col-xs-4 form-group expiration required'>
                        <label class='control-label'>Expiration</label>
                        <asp:TextBox type="text" placeholder="MM" runat="server" ID="expirationMonth_tb" CssClass="form-control card-expiry-month" size='2' />
                    </div>
                    <div class='col-xs-4 form-group expiration required'>
                        <label class='control-label'>&nbsp;</label>
                        <asp:TextBox type="text" placeholder="YYYY" runat="server" ID="ExpirationYear_tb" CssClass="form-control card-expiry-year" size='4' />
                    </div>
                </div>



                <!-- Form Name -->
                <legend>Billing Address Details</legend>

                <div class='form-row'>
                    <div class='col-xs-12 form-group required'>
                        <label class="control-label">Line 1</label>
                    <asp:TextBox type="text" placeholder="Address Line 1" runat="server" ID="AddrLine1_tb" CssClass="form-control" size='4' />
                    </div>
                </div>
                
                <div class='form-row'>
                    <div class='col-xs-12 form-group required'>
                        <label class="control-label">Line 2</label>
                    <asp:TextBox type="text" placeholder="Address Line 2" runat="server" ID="AddrLine2_tb" CssClass="form-control" size='4' />
                    </div>
                </div>
                

                <div class='form-row'>
                    <div class='col-xs-12 form-group required'>
                        <label class="control-label">City</label>
                    <asp:TextBox type="text" placeholder="City" runat="server" ID="City_tb" CssClass="form-control" size='4' />
                    </div>
                </div>

                <div class='form-row'>
                    <div class='col-xs-6 form-group required'>
                        <label class='control-label'>State</label>
                        <asp:TextBox type="text" placeholder="State" runat="server" ID="State_tb" CssClass="form-control" size='4' />
                    </div>
                    <div class='col-xs-6 form-group required'>
                        <label class='control-label'>Postcode</label>
                        <asp:TextBox type="text" placeholder="Post Code" runat="server" ID="Postcode_tb" CssClass="form-control" size='4' />
                    </div>
                </div>
                
                <div class='form-row'>
                    <div class='col-xs-12 form-group required'>
                        <label class="control-label">Country</label>
                    <asp:TextBox type="text" placeholder="Country" runat="server" ID="Country_tb" CssClass="form-control" size='4' />
                    </div>
                </div>
               
                <%------ Buttons --------%>
                <div class='form-row'>
                    <div class='col-md-12'>
                        <asp:Button runat="server" ID="FinishRegistration_btn" Text="Register" class="btn btn-primary" OnClick="FinishRegistration_btn_Click" />
                        <asp:Button runat="server" ID="Button1" Text="skip" class="btn btn-primary" OnClick="Skip_btn_Click" />

                    </div>
                </div>
                <div class='form-row'>
                    <div class='col-md-12 error form-group hide'>
                        <div class='alert-danger alert' runat="server">
                            <asp:Label ID="ErrorMessage" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <%--</form>--%>
            </div>
            <div class='col-md-4'></div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('form.require-validation').bind('submit', function (e) {
                var $form = $(e.target).closest('form'),
                    inputSelector = ['input[type=email]', 'input[type=password]',
                                     'input[type=text]', 'input[type=file]',
                                     'textarea'].join(', '),
                    $inputs = $form.find('.required').find(inputSelector),
                    $errorMessage = $form.find('div.error'),
                    valid = true;

                $errorMessage.addClass('hide');
                $('.has-error').removeClass('has-error');
                $inputs.each(function (i, el) {
                    var $input = $(el);
                    if ($input.val() === '') {
                        $input.parent().addClass('has-error');
                        $errorMessage.removeClass('hide');
                        e.preventDefault(); // cancel on first error
                    }
                });
            });
        });

        $(function () {
            var $form = $("#payment-form");

            $form.on('submit', function (e) {
                if (!$form.data('cc-on-file')) {
                    e.preventDefault();
                    Stripe.setPublishableKey($form.data('stripe-publishable-key'));
                    Stripe.createToken({
                        number: $('.card-number').val(),
                        cvc: $('.card-cvc').val(),
                        exp_month: $('.card-expiry-month').val(),
                        exp_year: $('.card-expiry-year').val()
                    }, stripeResponseHandler);
                }
            });

            function stripeResponseHandler(status, response) {
                if (response.error) {
                    $('.error')
                      .removeClass('hide')
                      .find('.alert')
                      .text(response.error.message);
                } else {
                    // token contains id, last4, and card type
                    var token = response['id'];
                    // insert the token into the form so it gets submitted to the server
                    $form.find('input[type=text]').empty();
                    $form.append("<input type='hidden' name='reservation[stripe_token]' value='" + token + "'/>");
                    $form.get(0).submit();
                }
            }
        })
    </script>
</asp:Content>
