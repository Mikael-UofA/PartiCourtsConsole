using System.Configuration;
using PrepareData.Data.Types;


namespace PrepareData.Data.Services
{
    internal class Helper
    {
        public static string CnnVal(string name)
        {
            Console.WriteLine(ConfigurationManager.ConnectionStrings[name].ConnectionString);
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static void DemoDB()
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public static void InsertDB()
        {
            using (var connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                try
                {
                    Judge test = new Judge("Demo", true, 3, 1979, "Chief Judge", "Obama", 2014, true);
                    List<Judge> list = new List<Judge>();
                    list.Add(test);
                    DataAccess dataAccess = new DataAccess();
                    dataAccess.InsertJudges(list);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
