using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace RestHubWebApp.Customers
{
    public partial class find_restaurant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Validate customer's Authentication
            Ini init = new Ini();
            init.isAuthUser();

            //VARIABLES DECLARATION
            ProjectTools.initVars();        //Initialize configuration values that will be available for this page
            string googleMapsUrl = "";      //Dynamicaly generated URL to request data from Google Maps API
            double distanceMeters = 0;      //distance value returned by Google Maps API
            double distanceMiles = 0;       //distance after conversion from meters to miles
            int elCounter = 0;              //Count the position of the element in the loop through Google's response
            int restaurantPos = 0;          //The position (index) of the restaurant in the list
            const int maxResults = 25;      //Maximum number of restaurants to list at one time (due to Google's free API restriction)
            int maxDistMiles = Convert.ToInt16(ProjectTools.maxDistMiles); //Maximum number of miles from customer to search for a restaurant (to prevent exhausting Google's allowance of hits/day)
            bool restaurantsListed = false; //Loop through the database more than once if necessary to find restaurants within range
            List<string[]> restaurants = new List<string[]>(); //Holds the list of restaurants with details
            string output = "";             //Will hold the output content

            string longitude_str = "0";
            string latitude_str = "0";
            double longitude = 0.00;
            double latitude = 0.00;

            if (Application["longitude"] != null && Application["latitude"] != null)
            { //LAT/LNG FROM THE MOBILE DEVICE
                longitude_str = Application["longitude"].ToString();
                latitude_str = Application["latitude"].ToString();
                longitude = Convert.ToDouble(longitude_str);
                latitude = Convert.ToDouble(latitude_str);
            }

            //latitude = 32.829602;     //At National University
            //longitude = -117.127454;  //At National University

            //SESSION LATITUDE / LONGITUDE: DIRECTIONS BUTTON
            Session["latitude"] = latitude;
            Session["longitude"] = longitude;

            int rangeLimitInMiles = Convert.ToInt16(ProjectTools.initRangeLimitMiles); //Define the initial distance between the user and surrounding restaurants in that area


            while (!restaurantsListed)
            {
                //Calculate lat/lng extreme values
                double[] extremePositions = GetExtremeLatLng(latitude, longitude, rangeLimitInMiles);
                double maxLat = extremePositions[0];
                double minLat = extremePositions[1];
                double maxLng = extremePositions[2];
                double minLng = extremePositions[3];

                //Query DB: load a list of restaurants withing that range
                DBObject db = new DBObject();
                SqlDataReader rec;
                string destinations = "";

                string sql;
                //CHECK IF THERE ARE ANY ITEMS IN THE CART. IF SO, LIMIT TO ONE RESTAURANT
                List<string[]> cart = new List<string[]>();
                if (Session["cart"] == null)
                    Session["cart"] = cart;
                else
                    cart = (List<string[]>)Session["cart"];

                if (Session["cart"] != null && cart != null && cart.Count > 0)
                {
                    //CART CLEAR: LIMIT DISPLAY TO THE SAME RESTAURANT
                    sql = "SELECT * FROM dbo.restaurant_branch WHERE restaurant_branch_id = '" + cart[0][6] + "'";
                }
                else
                {
                    //CART CLEAR: FIND RESTAURANT
                    sql = "SELECT TOP " + maxResults + " * FROM dbo.restaurant_branch " +
                        "WHERE latitude < '" + maxLat + "' " +
                        "AND latitude >= '" + minLat + "' " +
                        "AND longitude > '" + maxLng + "' " +
                        "AND longitude <= '" + minLng + "' " +
                        "AND admin_approval_status = 1";
                }

                rec = db.ProcessData(sql);

                if (rec.HasRows)
                {
                    //Start loop: load a restaurant for processing
                    while (rec.Read())
                    {
                        if ( RestaurantIsOnline( rec["restaurant_branch_id"].ToString() ) )
                        { //ONLY SHOW RESTAURANT IF IT IS ONLINE
                            //destinations += rec["latitude"] + "," + rec["longitude"] + "|";
                            destinations += HttpUtility.UrlEncode(rec["street_address"] + "," +
                                                                rec["city"] + "," +
                                                                rec["state"] + "+" +
                                                                rec["zip_code"]) + "|";

                            // if distance from user to restaurant <= range limit, add restaurant to the list
                            string[] restaurant = new string[11];
                            restaurant[0] = rec["restaurant_branch_id"].ToString();
                            restaurant[1] = rec["restaurant_name"].ToString();
                            restaurant[2] = rec["street_address"].ToString();
                            restaurant[3] = rec["city"].ToString();
                            restaurant[4] = rec["state"].ToString();
                            restaurant[5] = rec["zip_code"].ToString();
                            restaurant[6] = rec["restaurant_photo"].ToString();
                            restaurant[7] = rec["latitude"].ToString();
                            restaurant[8] = rec["longitude"].ToString();
                            restaurant[9] = "";     // distance (in miles) not calculated yet
                            restaurant[10] = "";    // travel time (in minutes) not calculated yet
                            restaurants.Add(restaurant);
                        } //end if
                    } //end while

                    //Calculate the [actual] distance between the user and the restaurant
                    if (destinations != "")
                    {
                        destinations = destinations.TrimEnd('|'); //remove the last pipe symbol from the string
                        googleMapsUrl = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + latitude + "," + longitude + "&destinations=" + destinations + "&key=" + ProjectTools.GoogleMapsKey);
                        string APIResponse = new System.Net.WebClient().DownloadString(googleMapsUrl);
                        DistanceMatrixData GoogleResult = JsonConvert.DeserializeObject<DistanceMatrixData>(APIResponse);
                        if (GoogleResult.status == "OK")
                        { //Distances provided by Google
                            List<Row> row = GoogleResult.rows;
                            List<Element> element = row[0].elements;
                            List<string[]> googleResponseList = new List<string[]>();

                            elCounter = 0;
                            foreach (Element el in element)
                            { //load a "table" with important returning values
                                distanceMeters = Convert.ToDouble(el.distance.value); //get distance in meters from Google
                                distanceMiles = distanceMeters / 1609.34; //Convert distance from meters to Miles
                                string[] googleResponse = new string[3];
                                googleResponse[0] = GoogleResult.destination_addresses[elCounter]; //Load the location's address
                                if (distanceMiles > 1)
                                {
                                    googleResponse[1] = distanceMiles.ToString("N2");
                                }
                                else
                                {
                                    googleResponse[1] = "1";
                                }
                                googleResponse[2] = el.duration.text;
                                googleResponseList.Add(googleResponse); //add array to the list
                                elCounter++;
                            } //End Foreach element loop

                            elCounter = 0;
                            foreach (string[] GoogleResponseItem in googleResponseList)
                            {
                                if (Convert.ToDouble(GoogleResponseItem[1]) > rangeLimitInMiles)
                                { //If distance to restaurant is greater than the current limit (in miles)
                                    //find restaurant in the main list and remove it
                                    restaurantPos = 0;
                                    foreach (string[] restaurant in restaurants.ToArray())
                                    { //find the restaurant in the restaurants list
                                        if (GoogleResponseItem[0].Substring(0, 10) == restaurant[2].Substring(0, 10))
                                        { //if the first 10 characters of street address match
                                            restaurants.RemoveAt(restaurantPos);
                                        }
                                        restaurantPos = restaurantPos + 1;
                                    }
                                }
                                else
                                { //restaurant is withing the acceptable range limit in miles
                                    restaurantPos = 0;
                                    foreach (string[] restaurant in restaurants.ToArray())
                                    { //find the restaurant in the restaurants list
                                        if (GoogleResponseItem[0].Substring(0, 10) == restaurant[2].Substring(0, 10))
                                        { //if the first 10 characters of street address match
                                            restaurants[restaurantPos][9] = GoogleResponseItem[1]; //Distance (in Miles)
                                            restaurants[restaurantPos][10] = GoogleResponseItem[2]; //Travel Time (in Minutes)
                                        }
                                        restaurantPos = restaurantPos + 1;
                                    }
                                }
                                elCounter = elCounter + 1;
                            }
                        } //End if Google Results Status is Ok
                    } //End if destinations are not empty

                    //LOAD SESSION WITH RESTAURANT LIST
                    Session["restaurant_list"] = restaurants;

                } //End if rec has rows

                //If there are no restaurants in the list and distance limit has not yet been reached...
                if (restaurants.Count < 1 && rangeLimitInMiles <= maxDistMiles)
                {
                    rangeLimitInMiles = 2 * rangeLimitInMiles; //Double distance range limit and start over
                }
                else
                { //either there are restaurants in the list or the range limit exceeds the maximum distance in miles
                    restaurantsListed = true;
                }
            } //End of main loop

            if (restaurants.Count > 0)
            { //There are restaurants in the list
                foreach (string[] restaurant in restaurants.ToArray())
                {
                    if (restaurant[9] != "")
                        distanceMiles = Math.Round(Convert.ToDouble(restaurant[9]));
                    output += "<div class='media' data-href='restaurant_menu.aspx?id=" + restaurant[0] + "' style='cursor:pointer;'>\n";
                    if (restaurant[6] != "")
                    {
                        output += "<div class='media-left'>\n" +
                                        "<a href='restaurant_menu.aspx?id=" + restaurant[0] + "' class='thumbnail'>\n" +
                                            "<img class='media-object' src='../images/restaurants/" + restaurant[6] + "' alt='" + restaurant[1] + "' style='max-width:100px; max-height:100px;'>\n" +
                                        "</a>\n" +
                                    "</div>\n";  // TODO - Make sure the restaurant image is working.
                    }
                    output += "<div class='media-body'>\n" +
                                "<h4 class='media-heading'>" + restaurant[1] + "</h4>\n" +
                                "<p>" + restaurant[2] + ", " + restaurant[3] + ", " + restaurant[4] + " " + restaurant[5] + "<br />\n" +
                                "Distance: " + restaurant[9] + " miles.<br />\n" +
                                "Travel Time: " + restaurant[10] + ".</p>\n" +
                            "</div>\n" +
                        "</div><hr />\n";
                }
            }
            else
            {
                output = "There are no restaurants listed in your area. We do apologize for the inconvenience.";
            }
            restaurantList.InnerHtml = output;
        }

        /* SUPPORTING FUNCTIONS */
        protected double distanceMiles(double latCustomer, double lngCustomer, double latRestaurant, double lngRestaurant)
        { //Calculate the distance between two latitude, longitude pairs in miles
            const double EarthRadius = 6371;                       // Radius of the earth in km

            double latDifference = deg2rad(latRestaurant - latCustomer);    // Difference in latitude between the customer and the restaurant (in Radians)
            double lngDifference = deg2rad(lngRestaurant - lngCustomer);    // Difference in longitude between the customer and the restaurant (in Radians)
            double a =
                Math.Pow(Math.Sin(latDifference / 2), 2) +
                Math.Cos(deg2rad(latCustomer)) * Math.Cos(deg2rad(latRestaurant)) *
                Math.Pow(Math.Sin(lngDifference / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceKm = EarthRadius * c;                            // Distance in km
            return distanceKm * 0.621371;                                   //Return the distance in miles
        }

        public double deg2rad(double deg)
        { /* Convert degrees into radians */
            return deg * Math.PI / 180;
        }

        public double[] GetExtremeLatLng(double lat, double lng, int rangeInMiles)
        {   /* Provided: latitude, longitude and range in miles. Return: an array with   *
             * Maximum Latitude, Minimum Latitude, Maximum Longitude, Minimum Longitude. */

            //Instantiate variables
            double maxLat;
            double maxLng;
            double minLat;
            double minLng;
            double[] extremes;
            double milesRange = Convert.ToDouble(rangeInMiles);
            double latRadius;
            double lngVariation;

            //Instantiate constants
            const double earthRadius = 3960.0; //in miles
            const double degreesToRadians = Math.PI / 180.0;
            const double radiansToDegrees = 180.0 / Math.PI;

            //Assigning values
            //Based on: www.johndcook.com/blog/2009/04/27/converting-miles-to-degrees-longitude-or-latitude/
            //Calculating Extreme Latitudes
            if (lat > 0)
            {
                maxLat = lat + milesRange / earthRadius * radiansToDegrees; //69 miles increases the latitude by 1 degree
                minLat = lat - milesRange / earthRadius * radiansToDegrees; //69 miles decreases the latitude by 1 degree
            }
            else
            {
                maxLat = lat - milesRange / 69.172;
                minLat = lat + milesRange / 69.172;
            }

            //Calculating Extreme Longitudes
            latRadius = earthRadius * Math.Cos(lat * degreesToRadians); //radius of earth at a specific latitude
            lngVariation = (milesRange / latRadius) * radiansToDegrees; //variation in longitude for a given distance in miles
            if (lng > 0)
            {
                maxLng = lng + lngVariation;
                minLng = lng - lngVariation;
            }
            else
            {
                maxLng = lng - lngVariation;
                minLng = lng + lngVariation;
            }

            //Load return array
            extremes = new double[4] { maxLat, minLat, maxLng, minLng };

            return extremes;
        }

        public bool RestaurantIsOnline(string restaurantID)
        {
            bool isOnline = false;
            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;
            DateTime now;
            DateTime lastSeen;
            TimeSpan ts;
            int restaurantCapacity;

            //CHECK IF RESTAURANT IS ONLINE
            sql = "SELECT public_visibility_status FROM restaurant_branch WHERE restaurant_branch_id='" + restaurantID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                if ( Convert.ToBoolean(rec["public_visibility_status"]) == true )
                {
                    isOnline = true; //UPDATE STATUS TO ONLINE
                }
            }


            if (isOnline)
            {
                //CHECK IF RESTAURANT STATUS IS OUTDATED
                rec.Read();
                //LOAD LAST SEEN
                sql = "SELECT last_seen FROM restaurant_branch WHERE restaurant_branch_id='" + restaurantID + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    lastSeen = Convert.ToDateTime(rec["last_seen"]);

                    //COMPARE THE TWO DATE/TIME VALUES
                    DateTime dtNow = ProjectTools.NowPSTime();
                    ts = dtNow.Subtract(lastSeen);
                    if ( Math.Abs( ts.TotalMinutes ) > 1 )
                    { //THE RESTAURANT WAS LAST SEEN OVER A MINUTE AGO
                        //UPDATE RESTAURANT STATUS TO OFFLINE
                        sql = "UPDATE restaurant_branch SET public_visibility_status = 0 WHERE restaurant_branch_id='" + restaurantID + "'";
                        db.ProcessData(sql);

                        isOnline = false; //UPDATE STATUS BACK TO OFFLINE
                    }
                }
            }

            if (isOnline)
            {
                //CHECK IF RESTAURANT IS AT CAPACITY
                restaurantCapacity = 50;
                //FIND ACTUAL CAPACITY
                sql = "SELECT max_orders_queue FROM restaurant_branch WHERE restaurant_branch_id='" + restaurantID + "'";
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    restaurantCapacity = Convert.ToInt16(rec["max_orders_queue"]);
                }

                //FIND NUMBER OF ORDERS PROCESSING
                sql = "SELECT COUNT(*) AS ordersCount " +
                        "FROM restaurant_orders " +
                        "WHERE restaurant_branch_id='" + restaurantID + "' " +
                        "AND (order_status = 'new' OR order_status = 'preparing')"; //NOT ready or delivered
                rec = db.ProcessData(sql);
                if (rec.HasRows)
                {
                    rec.Read();
                    if (Convert.ToInt16(rec["ordersCount"]) >= restaurantCapacity)
                    {
                        isOnline = false;
                    }
                }
            }

            return isOnline;
        }
    }
}