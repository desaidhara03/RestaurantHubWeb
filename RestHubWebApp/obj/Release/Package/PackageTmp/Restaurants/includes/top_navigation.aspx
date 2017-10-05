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
          <% if( Session["restaurant_branch_id"] != null && Session["restaurant_branch_id"] != ""){ %>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="orders.aspx">Orders Manager</a></li>
            <li><a href="menu.aspx">Menu</a></li>
            <li><a href="account.aspx">My Account</a></li>
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