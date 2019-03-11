<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="TaxApp.Welcome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome - Tax App</title>
    <!-- Theam & Bootstrap -->
      <!-- Favicon -->
      <link href="Theam/assets/img/brand/favicon.png" rel="icon" type="image/png">
      <!-- Fonts -->
      <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" rel="stylesheet">
      <!-- Icons -->
      <link href="Theam/assets/vendor/nucleo/css/nucleo.css" rel="stylesheet">
      <link href="Theam/assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" rel="stylesheet">
      <!-- Argon CSS -->
      <link type="text/css" href="Theam/assets/css/argon.css?v=1.0.0" rel="stylesheet">
    <!-- End Theam & Bootstrap -->
</head>
<body>
    <form id="form1" runat="server">
        <!-- Navbar -->
    <nav class="navbar navbar-top navbar-horizontal navbar-expand-md navbar-dark">
      <div class="container px-4">
        <a class="navbar-brand" href="../index.html">
          <img src="../assets/img/brand/white.png" />
        </a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbar-collapse-main" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbar-collapse-main">
          <!-- Collapse header -->
          <div class="navbar-collapse-header d-md-none">
            <div class="row">
              <div class="col-6 collapse-brand">
                <a href="../index.html">
                  <img src="../assets/img/brand/blue.png">
                </a>
              </div>
              <div class="col-6 collapse-close">
                <button type="button" class="navbar-toggler" data-toggle="collapse" data-target="#navbar-collapse-main" aria-controls="sidenav-main" aria-expanded="false" aria-label="Toggle sidenav">
                  <span></span>
                  <span></span>
                </button>
              </div>
            </div>
          </div>
          <!-- Navbar items -->
          <ul class="navbar-nav ml-auto">
            <li class="nav-item">
              <a class="nav-link nav-link-icon" href="../examples/register.html">
                <i class="ni ni-circle-08"></i>
                <span class="nav-link-inner--text">Register</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link nav-link-icon" href="../examples/login.html">
                <i class="ni ni-key-25"></i>
                <span class="nav-link-inner--text">Login</span>
              </a>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Footer -->
  <footer class="py-5">
    <div class="container">
      <div class="row align-items-center justify-content-xl-between">
        <div class="col-xl-6">
          <div class="copyright text-center text-xl-left text-muted">
            &copy; 2019 <a href="" class="font-weight-bold ml-1" target="_blank">Tax App</a>
          </div>
        </div>
        <div class="col-xl-6">
          <ul class="nav nav-footer justify-content-center justify-content-xl-end">
            <li class="nav-item">
              <a href="" class="nav-link" target="_blank">menue</a>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </footer>

    <!-- Theam & Bootstrap -->
      <!-- Argon Scripts -->
      <!-- Core -->
      <script src="Theam/assets/vendor/jquery/dist/jquery.min.js"></script>
      <script src="Theam/assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
      <!-- Argon JS -->
      <script src="Theam/assets/js/argon.js?v=1.0.0"></script>
    <!-- End Theam & Bootstrap -->

        
    </form>
</body>
</html>
