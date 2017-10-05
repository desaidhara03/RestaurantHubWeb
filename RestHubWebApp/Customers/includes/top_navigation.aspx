    <nav class="navbar navbar-default navbar-fixed-top">
      <div class="container-fluid">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="index.aspx"><img src="../images/logo.png" alt="Restaurant Hub" /></a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
          <% if (Session["customer_id"] != null && Session["auth"]!=null && Session["auth"] != ""){ %>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="find_restaurant.aspx">Restaurants</a></li>
            <li><a href="recommendations.aspx">Recomendations</a></li>
            <li><a href="account.aspx">Account</a></li>
            <% 
                if (Session["cart"] != null){
            %>
            <li><a href="cart.aspx">Cart</a></li>
            <% } %>
            <li><a href="logout.aspx">Log Out</a></li>
          </ul>
          <% }else{ %>
          <ul class="nav navbar-nav navbar-right">
            <li>
              <a href="login.aspx">Login</a>
            </li>
            <li>
              <a href="account.aspx">Create an Account</a>
            </li>
          </ul>
          <% } %>
        </div>
      </div>
    </nav>
    <br clear="all" />