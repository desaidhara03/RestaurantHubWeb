<%@ Page Title="Landing Page" Language="C#" AutoEventWireup="true" CodeBehind="locationPage.aspx.cs" Inherits="RestHubWebApp.Customers.locationPage" %>

<html>
<head>
    <!-- #include file ="includes\html_includes.aspx" -->
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/yqlgeo.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC-tban20Rjd7R7HXLZtt5f0AGVVrwnjVc"></script>
    <script>

        jQuery(window).ready(function () {
            initiate_geolocation();
        })

        function initiate_geolocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(handle_geolocation_query1_map, handle_errors, {
                    enableHighAccuracy: true // This is needed.
                });
            }
            else {
                yqlgeo.get('visitor', normalize_yql_response);
            }
            return false;
        }

        function handle_errors(error) {
            switch (error.code) {
                case error.PERMISSION_DENIED: alert("user did not share geolocation data");
                    //setCurrentGPSStatus("user did not share geolocation data");
                    break;

                case error.POSITION_UNAVAILABLE: alert("could not detect current position");
                    //setCurrentGPSStatus("could not detect current position");
                    break;

                case error.TIMEOUT: alert("retrieving position timedout");
                    //setCurrentGPSStatus("retrieving position timedout");
                    break;

                default: alert("unknown error");
                    //setCurrentGPSStatus("unknown error");
                    break;
            }
        }

        function normalize_yql_response(response) {
            //setCurrentGPSStatus("inside normalize_yql_response");
            if (response.error) {
                var error = { code: 0 };
                handle_error(error);
                return;
            }

            var position = {
                coords:
                {
                    latitude: response.place.centroid.latitude,
                    longitude: response.place.centroid.longitude
                },
                address:
                {
                    city: response.place.locality2.content,
                    region: response.place.admin1.content,
                    country: response.place.country.content
                }
            };

            handle_geolocation_query1_map(position); // This is not executing.
        }

        function setCurrentGPSStatus(status) {
            document.getElementById("location_tb").innerText = document.getElementById("location_tb").innerText + " " + status;
        }

        function handle_geolocation_query(position) {
            setCurrentGPSStatus('\nLat: ' + position.coords.latitude + ' \n' +
              'Lon: ' + position.coords.longitude);

            // Setting javascript Long Lat values in hiddenfields so that we can look up in code behind file.
            var Longitude_hf = document.getElementById('<%= Longitude_hf.ClientID %>');
            Longitude_hf.value = position.coords.longitude;

            var Latitude_hf = document.getElementById('<%= Latitude_hf.ClientID %>');
            Latitude_hf.value = position.coords.latitude;


        }

        function handle_geolocation_query_alert(position) {
            alert('Lat: ' + position.coords.latitude + ' ' +
                  'Lon: ' + position.coords.longitude);
        }

        function handle_geolocation_query1_map(position) {
            var mapProp = {
                center: new google.maps.LatLng(position.coords.latitude, position.coords.longitude),
                zoom: 14,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

            // Adding Pin to current location
            var im = 'http://www.robotwoods.com/dev/misc/bluecircle.png';
            var myLatLng = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            var userMarker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                icon: im
            });
            handle_geolocation_query(position);

        }

    </script>
</head>
<body>
    <!-- #include file ="includes\top_navigation.aspx" -->
    
    <form runat="server">
        <div>
            <%--<button id="btnInitLocation" type="button" class="btn btn-primary">Find my location</button>--%>
            <br /> <br /> <br />
            <asp:Button type="button" ID="findRest_btn" class="btn btn-primary" runat="server" Text="Find nearby Restaurants" OnClick="FindNearByRest_Click" Visible="true" />
            <%--<button id="btnInitLocation">Find my location</button>--%>
        </div>
        <label hidden="hidden">My Current Location:</label>
        <label id="location_tb" hidden="hidden"></label>
        <br /> <br /> 
        <div id="map-container" class="col-md-6"></div>

        <div id="googleMap" class="col-md-6" style="width: 100%; height: 350px;"></div>

        <%--This is to pass LongLat values from javascript to codebehind file.--%>
        <asp:HiddenField ID="Longitude_hf" runat="server" />
        <asp:HiddenField ID="Latitude_hf" runat="server" />
    </form>
    <!-- #include file ="includes\footer.aspx" -->
</html>
