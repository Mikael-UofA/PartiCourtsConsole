using System.Configuration;
using PrepareData.Data.Types;


namespace PrepareData.Data.Services
{
    /// <summary>
    /// A helper class for database operations, providing methods to test, insert, and delete data from the database.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// A demo Judge instance used for testing database operations.
        /// </summary>
        private static readonly Judge test = new Judge("Demo", true, 3, 1979, "Chief Judge", "Obama", 2014, true, false);


        /// <summary>
        /// Retrieves the connection string for the specified database.
        /// </summary>
        /// <param name="name">The name of the connection string in the configuration file.</param>
        /// <returns>The connection string.</returns>
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Demonstrates a successful connection to the database by opening a connection.
        /// </summary>
        public static void DemoDB()
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(DataAccess.cnnVal)))
            {
                connection.Open();
                MessageBox.Show("Connection successful!");
            }
        }

        /// <summary>
        /// Inserts a demo Judge record into the database.
        /// </summary>
        public static void InsertDB()
        {
            List<Judge> list = [test];
            DataAccess.InsertJudges(list);
        }

        /// <summary>
        /// Deletes the demo Judge record from the database.
        /// </summary>
        public static void DeleteDB()
        {
            DataAccess.DeleteJudge(test);
        }
    }
}
