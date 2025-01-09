using System.Data;
using PrepareData.Data.Types;
using Dapper;

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
        public List<DistrictCourt> GetDistrictCourts()
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
        public List<CircuitCourt> GetCircuitCourts()
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
        public List<Judge> GetDistrictJudges(int districtId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCircuitJudge = False AND Court = '{districtId}'").ToList();
            }
        }

        /// <summary>
        /// Retrieves all Circuit Judges belonging to a specific Circuit Court.
        /// </summary>
        /// <param name="curcuitId">The ID of the Circuit Court.</param>
        /// <returns>A list of <see cref="Judge"/> objects representing Circuit Judges.</returns>
        public List<Judge> GetCircuitJudges(int curcuitId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCircuitJudge = True AND Court = '{curcuitId}'").ToList();
            }
        }

        /// <summary>
        /// Inserts a list of Circuit Courts into the database.
        /// </summary>
        /// <param name="curcuitCourts">The list of <see cref="CircuitCourt"/> objects to insert.</param>
        public void InsertCircuitCourts(List<CircuitCourt> curcuitCourts)
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
        public void InsertDistrictCourts(List<DistrictCourt> districtCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                INSERT INTO DCourts 
                (Id, Name, Abbreviation, CourtOfAppeal, ActiveJudges, MaxJudges, ChiefJudge, SeniorEligibleJudges, DEMJudges, GOPJudges) 
                VALUES 
                (@Id, @Name, @Abbreviation, @CourtOfAppeal, @ActiveJudges, @MaxJudges, @ChiefJudge, @SeniorEligibleJudges, @DEMJudges, @GOPJudges)";

                connection.Execute(sqlQuery, districtCourts);
            }
        }

        /// <summary>
        /// Inserts a list of Judges into the database.
        /// </summary>
        /// <param name="judges">The list of <see cref="Judge"/> objects to insert.</param>
        public void InsertJudges(List<Judge> judges)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal(cnnVal)))
            {
                string sqlQuery = @"
                INSERT INTO Judges 
                (Name, IsCircuitJudge, Court, Title, AppointedBy, YearOfBirth, AppointmentYear, IsChief, Partisanship) 
                VALUES 
                (@Name, @IsCircuitJudge, @Court, @Title, @AppointedBy, @YearOfBirth, @AppointmentYear, @IsChief, @Partisanship)";

                connection.Execute(sqlQuery, judges);
            }
        }

        /// <summary>
        /// Deletes a specific Judge from the database.
        /// </summary>
        /// <param name="judge">The <see cref="Judge"/> object to delete.</param>
        public void DeleteJudge(Judge judge)
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
        public void ClearTable(string tableName)
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
        public void UpdateDistrictCourts(List<DistrictCourt> districtCourts)
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
        public void UpdateCircuitCourts(List<CircuitCourt> curcuitCourts)
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
