using System.Data;
using PrepareData.Data.Types;
using Dapper;
using System.Windows.Forms.VisualStyles;
using Microsoft.Data.SqlClient;

namespace PrepareData.Data.Services
{
    /// <summary>
    /// A data access class for interacting with the Courts database.
    /// Provides methods for retrieving, inserting, updating, and deleting data from the database.
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// The connection string name for the Courts database.
        /// </summary>
        public static readonly string cnnVal = "CourtsDB";

        /// <summary>
        /// Retrieves all District Courts from the database.
        /// </summary>
        /// <returns>A list of <see cref="DistrictCourt"/> objects representing the District Courts.</returns>
        public static List<DistrictCourt> GetDistrictCourts()
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<DistrictCourt>($"SELECT * FROM DCourts").ToList();
            }
        }

        /// <summary>
        /// Retrieves all Circuit Courts from the database.
        /// </summary>
        /// <returns>A list of <see cref="CircuitCourt"/> objects representing the Circuit Courts.</returns>
        public static List<CircuitCourt> GetCircuitCourts()
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<CircuitCourt>($"SELECT * FROM CCourts").ToList();
            }
        }

        /// <summary>
        /// Retrieves all District Judges belonging to a specific District Court.
        /// </summary>
        /// <param name="districtId">The ID of the District Court.</param>
        /// <returns>A list of <see cref="Judge"/> objects representing District Judges.</returns>
        public static List<Judge> GetDistrictJudges(int districtId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCircuitJudge = 0 AND Court = '{districtId}'").ToList();
            }
        }

        /// <summary>
        /// Retrieves all Circuit Judges belonging to a specific Circuit Court.
        /// </summary>
        /// <param name="curcuitId">The ID of the Circuit Court.</param>
        /// <returns>A list of <see cref="Judge"/> objects representing Circuit Judges.</returns>
        public static List<Judge> GetCircuitJudges(int curcuitId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCircuitJudge = 1 AND Court = '{curcuitId}'").ToList();
            }
        }

        /// <summary>
        /// Inserts a list of Circuit Courts into the database.
        /// </summary>
        /// <param name="curcuitCourts">The list of <see cref="CircuitCourt"/> objects to insert.</param>
        public static void InsertCircuitCourts(List<CircuitCourt> curcuitCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                INSERT INTO CCourts 
                (Id, Name, SupervisingJustice, ActiveJudges, MaxJudges, ChiefJudge, SeniorEligibleJudges, DEMJudges, GOPJudges) 
                VALUES 
                (@Id, @Name, @SupervisingJustice, @ActiveJudges, @MaxJudges, @ChiefJudge, @SeniorEligibleJudges, @DEMJudges, @GOPJudges)";

                connection.Execute(sqlQuery, curcuitCourts);
            }
        }

        /// <summary>
        /// Inserts a list of District Courts into the database.
        /// </summary>
        /// <param name="districtCourts">The list of <see cref="DistrictCourt"/> objects to insert.</param>
        public static void InsertDistrictCourts(List<DistrictCourt> districtCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                INSERT INTO DCourts 
                (Id, Name, Abbreviation, CourtOfAppeal, ActiveJudges, MaxJudges, ChiefJudge, SeniorEligibleJudges, DEMJudges, GOPJudges, DEMRetiring, GOPRetiring) 
                VALUES 
                (@Id, @Name, @Abbreviation, @CourtOfAppeal, @ActiveJudges, @MaxJudges, @ChiefJudge, @SeniorEligibleJudges, @DEMJudges, @GOPJudges, @DEMRetiring, @GOPRetiring)";

                connection.Execute(sqlQuery, districtCourts);
            }
        }

        /// <summary>
        /// Updates the 'IsRetiring' status of a judge in the database based on the provided details.
        /// </summary>
        /// <param name="judgeObjects">A dictionary containing judge details with keys "judgeName", "isCircuit", and "CourtNum".</param>
        /// <exception cref="ArgumentException">Thrown if required keys are missing from judgeObjects.</exception>
        public static void UpdateRetiringJudges(List<Dictionary<string, Object>> judgeObjects)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                connection.Open();
                string query = @"UPDATE Judges 
                         SET IsRetiring = 1 
                         WHERE Name = @Name AND IsCircuitJudge = @IsCircuitJudge AND Court = @Court";

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    foreach (var judgeObject in judgeObjects)
                    {
                        if (!judgeObject.ContainsKey("judgeName") || !judgeObject.ContainsKey("isCircuit") || !judgeObject.ContainsKey("courtNum"))
                        {
                            throw new ArgumentException("Missing required keys in judgeObjects.");
                        }

                        command.Parameters.Clear(); // Clear parameters before reusing the command

                        var param1 = command.CreateParameter();
                        param1.ParameterName = "@Name";
                        param1.Value = judgeObject["judgeName"].ToString();
                        command.Parameters.Add(param1);

                        var param2 = command.CreateParameter();
                        param2.ParameterName = "@IsCircuitJudge";
                        param2.Value = Convert.ToBoolean(judgeObject["isCircuit"]);
                        command.Parameters.Add(param2);

                        var param3 = command.CreateParameter();
                        param3.ParameterName = "@Court";
                        param3.Value = Convert.ToInt32(judgeObject["courtNum"]);
                        command.Parameters.Add(param3);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void UpdateCourtRetirements(ICourt court)
        {
            string table = "DCourts";
            if (court is CircuitCourt)
            {
                table = "CCourts";
            }
            string query = $@"
            UPDATE {table}
            SET DEMRetiring = @DEMRetiring, 
                GOPRetiring = @GOPRetiring
            WHERE Id = @Id;";

            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    var param1 = command.CreateParameter();
                    param1.ParameterName = "@DEMRetiring";
                    param1.Value = court.DEMRetiring;
                    command.Parameters.Add(param1);

                    var param2 = command.CreateParameter();
                    param2.ParameterName = "@GOPRetiring";
                    param2.Value = court.GOPRetiring;
                    command.Parameters.Add(param2);

                    var param3 = command.CreateParameter();
                    param3.ParameterName = "@Id";
                    param3.Value = court.Id;
                    command.Parameters.Add(param3);

                    command.ExecuteNonQuery();
                }
            }


        }

        /// <summary>
        /// Inserts a list of Judges into the database.
        /// </summary>
        /// <param name="judges">The list of <see cref="Judge"/> objects to insert.</param>
        public static void InsertJudges(List<Judge> judges)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                INSERT INTO Judges 
                (Name, IsCircuitJudge, Court, Title, AppointedBy, YearOfBirth, AppointmentYear, IsChief, Partisanship, IsRetiring) 
                VALUES 
                (@Name, @IsCircuitJudge, @Court, @Title, @AppointedBy, @YearOfBirth, @AppointmentYear, @IsChief, @Partisanship, @IsRetiring)";

                connection.Execute(sqlQuery, judges);
            }
        }

        /// <summary>
        /// Deletes a specific Judge from the database.
        /// </summary>
        /// <param name="judge">The <see cref="Judge"/> object to delete.</param>
        public static void DeleteJudge(Judge judge)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                DELETE FROM Judges 
                WHERE Name = @Name 
                AND Court = @Court 
                AND AppointedBy = @AppointedBy";

                connection.Execute(sqlQuery, new
                {
                    judge.Name,
                    judge.Court,
                    judge.AppointedBy
                });
            }
        }

        /// <summary>
        /// Clears all data from a specified database table.
        /// </summary>
        /// <param name="tableName">The name of the table to clear.</param>
        public static void ClearTable(string tableName)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                try
                {
                    string sqlQuery = $"TRUNCATE TABLE {tableName}";
                    connection.Execute(sqlQuery);
                }
                catch
                {
                    MessageBox.Show("Table not found");
                }
            }
        }

        /// <summary>
        /// Updates District Courts with the provided data.
        /// </summary>
        /// <param name="districtCourts">The list of <see cref="DistrictCourt"/> objects to update.</param>
        public static void UpdateDistrictCourts(List<DistrictCourt> districtCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                foreach (DistrictCourt districtCourt in districtCourts)
                {
                    string sqlQuery = @"
                    UPDATE DCourts 
                    SET ActiveJudges = @ActiveJudges, ChiefJudge = @ChiefJudge,
                        SeniorEligibleJudges = @SeniorEligibleJudges, DEMJudges = @DEMJudges,
                        GOPJudges = @GOPJudges
                    WHERE Id = @Id";

                    connection.Execute(sqlQuery, new
                    {
                        districtCourt.ActiveJudges,
                        districtCourt.ChiefJudge,
                        districtCourt.SeniorEligibleJudges,
                        districtCourt.DEMJudges,
                        districtCourt.GOPJudges,
                        districtCourt.Id
                    });
                }
            }
        }

        /// <summary>
        /// Updates Circuit Courts with the provided data.
        /// </summary>
        /// <param name="curcuitCourts">The list of <see cref="CircuitCourt"/> objects to update.</param>
        public static void UpdateCircuitCourts(List<CircuitCourt> curcuitCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                foreach (CircuitCourt curcuitCourt in curcuitCourts)
                {
                    string sqlQuery = @"
                    UPDATE CCourts 
                    SET ActiveJudges = @ActiveJudges, ChiefJudge = @ChiefJudge,
                        SeniorEligibleJudges = @SeniorEligibleJudges, DEMJudges = @DEMJudges,
                        GOPJudges = @GOPJudges
                    WHERE Id = @Id";

                    connection.Execute(sqlQuery, new
                    {
                        curcuitCourt.ActiveJudges,
                        curcuitCourt.ChiefJudge,
                        curcuitCourt.SeniorEligibleJudges,
                        curcuitCourt.DEMJudges,
                        curcuitCourt.GOPJudges,
                        curcuitCourt.Id
                    });
                }
            }
        }

    }

}
