using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Xml;
using System.Net;
using System;
using System.IO;
using System.Data.SqlClient;

namespace RestHubWebApp
{
    public static class ProjectTools
    {
        /* PUBLIC VARIABLES */
        //EMAIL SETTINGS
        public static string mailFrom = "";         // Email address that identifies the sender of the messages
        public static string mailHost = "";         // IP or domain of the e-mail server
        public static string mailPort = "";         // Port number for the e-mail server
        public static string mailUser = "";         // User ID for the e-mail account
        public static string mailPassword = "";     // Password for the e-mail account
        /* DATABASE SETTINGS */
        public static string connectionString = ""; // Database connection string
        /* GOOGLE MAPS API SETTINGS */
        public static string GoogleMapsKey = "";    // Developer's key for Google Maps API
        public static string initRangeLimitMiles = ""; // Initial distance between the user and surrounding restaurants in that area
        public static string maxDistMiles = "";     // Maximum distance the app will search for a restaurant
        /* AUTHORIZE NET API */
        public static string authorizeNetApiLoginID = "";
        public static string authorizeNetApiTransactionKey = "";

        /* METHODS */
        public static string CalculateMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }

        public static string RemQuot(string content)
        { // REMOVE SINGLE AND DOUBLE QUOTES FROM A STRING
            if (content != null)
            {
                content = content.Replace("\"", "") ?? ""; //Remove Double Quotes if not null, empty string if null
                content = content.Replace("\'", ""); //Remove Single Quotes
                content = content.Trim();
            }
            return content;
        }

        public static double[] GetLatLng(string addr)
        { /* ** Sends an address to Google and returns an array with latitude and longitude ** */
            double[] coord = new double[2];
            string address = string.Format("https://maps.google.com/maps/api/geocode/json?address={0}&key=AIzaSyBAoi3Pa0Zu3zIruo6gfurAZ4j2gHc2fa4&sensor=false", addr.Replace(" ", "+"));
            string APIResponse = new System.Net.WebClient().DownloadString(address);
            GeoResponse GoogleResult = JsonConvert.DeserializeObject<GeoResponse>(APIResponse);

            if (GoogleResult.Status == "OK")
            {
                coord[0] = GoogleResult.Results[0].geometry.location.lat;
                coord[1] = GoogleResult.Results[0].geometry.location.lng;
            }

            return coord;
        }

        public static void initVars()
        {
            /* THIS METHOD WILL GRAB A PROPRIETARY XML FILE AND PARSE ITS VALUES MAKING THOSE VALUES AVAILABLE TO THE APPLICATION */
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + "configuration.xml");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    /* LOAD EMAIL PARAMETERS */
                    case "mailFrom":
                        mailFrom = node.InnerText;
                        break;
                    case "mailHost":
                        mailHost = node.InnerText;
                        break;
                    case "mailPort":
                        mailPort = node.InnerText;
                        break;
                    case "mailUser":
                        mailUser = node.InnerText;
                        break;
                    case "mailPassword":
                        mailPassword = node.InnerText;
                        break;
                    /* CONNECTION STRING */
                    case "DBServer":              //127.0.0.1\\DEVREDHORSEMSSQL
                        if (node.InnerText != "")
                            connectionString += "Server=" + node.InnerText + "; ";
                        break;
                    case "Database":            //Database name.
                        if (node.InnerText != "")
                        {
                            connectionString += "Database=" + node.InnerText + "; ";
                        }
                        break;
                    case "DBUserID":              //e.g. sa
                        if (node.InnerText != "")
                            connectionString += "User Id=" + node.InnerText + "; ";
                        break;
                    case "DBPassword":            //e.g. pwd24get
                        if (node.InnerText != "")
                            connectionString += "Password=" + node.InnerText + "; ";
                        break;
                    case "DBProvider":            //e.g. SQLOLEDB.1
                        if (node.InnerText != "")
                            connectionString += "Provider=" + node.InnerText + "; ";
                        break;
                    case "DBIntegratedSecurity":  //e.g. SSPI
                        if (node.InnerText != "")
                            connectionString += "Integrated Security = " + node.InnerText + "; ";
                        break;
                    case "DBPersistSecurityInfo": //e.g. False
                        if (node.InnerText != "")
                            connectionString += "Persist Security Info = " + node.InnerText + "; ";
                        break;
                    case "DBInitialCatalog":      //Database name.
                        if (node.InnerText != "")
                        {
                            connectionString += "Initial Catalog = " + node.InnerText + "; ";
                        }
                        break;
                    case "DBDataSource":          //e.g. 127.0.0.1\\DEVREDHORSEMSSQL
                        if (node.InnerText != "")
                            connectionString += "Data Source = " + node.InnerText + ";";
                        break;
                    case "MaxPoolSize":
                        if (node.InnerText != "")
                            connectionString += ";Max Pool Size="+ node.InnerText + ";";
                        break;
                    /* GOOGLE MAPS API */
                    case "GoogleMapsKey":
                        GoogleMapsKey = node.InnerText;
                        break;
                    case "initRangeLimitMiles":
                        initRangeLimitMiles = node.InnerText;
                        break;
                    case "maxDistMiles":
                        maxDistMiles = node.InnerText;
                        break;
                    /* AUTHORIZE NET API */
                    case "authorizeNetApiLoginID":
                        authorizeNetApiLoginID = node.InnerText;
                        break;
                    case "authorizeNetApiTransactionKey":
                        authorizeNetApiTransactionKey = node.InnerText;
                        break;
                } //end switch
            } //end foreach
        } //end of method initVars()

        public static void SendEmail(string mailTo, string subject,  string message)
        {
            if (ProjectTools.mailFrom == "")
                ProjectTools.initVars(); //Make sure that the configuration values (from configuration.xml) are loaded

            if (mailTo != "" && message != "")
            {
                MailMessage mail = new MailMessage(ProjectTools.mailFrom, mailTo);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = ProjectTools.mailHost;
                client.Port = Convert.ToInt16(ProjectTools.mailPort);
                mail.Subject = subject;
                mail.Body = message;
                client.Credentials = new NetworkCredential(ProjectTools.mailUser, ProjectTools.mailPassword);
                client.EnableSsl = true;
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
        }

        public static double GetRestaurantSalesTax(int restaurant_id)
        {
            /* RETURNS THE RESTAURANT'S SALES TAX GIVEN A RESTAURANT ID OR ZERO IF SALES TAX IS NOT AVAILABLE */
            double sales_tax = 0.00;
            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;
            sql = "SELECT sales_tax_rate FROM dbo.restaurant_branch WHERE restaurant_branch_id='" + restaurant_id + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                sales_tax = Convert.ToDouble(rec["sales_tax_rate"]);
            }
            return sales_tax;
        }

        /* ENCRYPT / DECRYPT CREDIT CARD */
        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

        public static string EncryptCC(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        public static string DecryptCC(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        public static string GetEncryptionKey()
        {
            Random rand = new Random();
            string encKey = rand.Next(1000000, 9999999).ToString();
            return encKey;
        }

        public static string GetRestaurantNameFromID(string rID)
        {
            string restaurantName = "";
            DBObject db = new DBObject();
            SqlDataReader rec;
            string sql;
            sql = "SELECT restaurant_name FROM dbo.restaurant_branch WHERE restaurant_branch_id='" + rID + "'";
            rec = db.ProcessData(sql);
            if (rec.HasRows)
            {
                rec.Read();
                restaurantName = rec["restaurant_name"].ToString();
            }
            return restaurantName;
        }

        public static DateTime NowPSTime()
        { //GET THE CURRENT DATE/TIME OFFSET TO PACIFIC STANDARD TIME
            var pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset dtNow = TimeZoneInfo.ConvertTime(localServerTime, pacificTimeZone);
            return dtNow.DateTime;
        }
    }
}