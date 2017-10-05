using System;
using System.Data;
using System.Data.SqlClient;

namespace RestHubWebApp
{
    class DBObject
    {
        //INSTANCE VARIABLES
        SqlConnection sqlCon;
        SqlCommand cmd;
        SqlDataReader reader;

        /* CLASS TO HANDLE DATABASE OPERATIONS */
        public SqlDataReader ProcessData(string sql)
        {
            /* RETRIEVE TABLE DATA OR EXECUTE A SQL BASED ON COMMAND */
            /** VARIABLE DECLARATION / INITIALIZATION, DB CONNECTION **/
            //string connStr = "Server=tcp:restauranthub.database.windows.net,1433;Database=RestaurantHub;Uid=manager@restauranthub;Pwd=CSC686/7;"; //Azure SQL DB
            ProjectTools.initVars(); //ensure variables are initialized
            string connStr = ProjectTools.connectionString; //Load connection string (based on configuration.xml)

            sqlCon = new SqlConnection(Convert.ToString(connStr));
            cmd = new SqlCommand();

            /** EXECUTE QUERY **/
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlCon;
            sqlCon.Open();
            reader = cmd.ExecuteReader();

            return reader; //return results
        }

        public void DestroyConnection()
        {
            sqlCon.Dispose();
            cmd.Dispose();
            reader.Close();
            SqlConnection.ClearAllPools();
        }
    } //end class
}