using System.Data;
using PrepareData.Data.Types;
using Dapper;
using System.Windows.Forms.VisualStyles;

namespace PrepareData.Data.Services
{
    public class DataAccess
    {
        public List<DistrictCourt> GetDistrictCourts()
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                return connection.Query<DistrictCourt>($"SELECT * FROM DCourts").ToList();
            }
        }
        public List<CurcuitCourt> GetCurcuitCourts()
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                return connection.Query<CurcuitCourt>($"SELECT * FROM CCourts").ToList();
            }
        }
        public List<Judge> GetDistrictJudges(int districtId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCurcuitJudge = False AND Court = '{districtId}'").ToList();
            }
        }
        public List<Judge> GetCurcuitJudges(int curcuitId)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                return connection.Query<Judge>($"SELECT * FROM Judges WHERE isCurcuitJudge = True AND Court = '{curcuitId}'").ToList();
            }
        }
        public void InsertCurcuitCourts(List<CurcuitCourt> curcuitCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                string sqlQuery = @"INSERT INTO CCourts 
            (Id, Name, SupervisingJustice, ActiveJudges, MaxJudges, ChiefJudge, SeniorEligibleJudges, Population, DEMJudges, GOPJudges) 
            VALUES 
            (@Id, @Name, @SupervisingJustice, @ActiveJudges, @MaxJudges, @ChiefJudge, @SeniorEligibleJudges, @Population, @DEMJudges, @GOPJudges)";

                connection.Execute(sqlQuery, curcuitCourts);
            }

        }
        public void InsertDistrictCourts(List<DistrictCourt> districtCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                string sqlQuery = @"INSERT INTO Courts 
            (Id, Name, Abbreviation, CourtOfAppeal, ActiveJudges, MaxJudges, ChiefJudge, SeniorEligibleJudges, DEMJudges, GOPJudges) 
            VALUES 
            (@Id, @Name, @Abbreviation, @CourtOfAppeal, @ActiveJudges, @MaxJudges, @ChiefJudge, @SeniorEligibleJudges, @DEMJudges, @GOPJudges)";

                connection.Execute(sqlQuery, districtCourts);
            }

        }
        public void InsertJudges(List<Judge> judges)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                string sqlQuery = @"
            INSERT INTO Judges 
            (Name, IsCurcuitJudge, Court, Title, AppointedBy, YearOfBirth, AppointmentYear, IsChief, Partisanship) 
            VALUES 
            (@Name, @IsCurcuitJudge, @Court, @Title, @AppointedBy, @YearOfBirth, @AppointmentYear, @IsChief, @Partisanship)";
                connection.Execute(sqlQuery, judges);
            }
        }
        public void DeleteJudge(Judge judge)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
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
        public void ClearTable(string tableName)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                try
                {
                    string sqlQuery = $"TRUNCATE TABLE {tableName}";
                    connection.Execute(sqlQuery);
                }
                catch
                {
                    Console.WriteLine("Table not found");
                }

            }
        }
        public void UpdateDistrictCourts(List<DistrictCourt> districtCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
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
        public void UpdateCurcuitCourts(List<CurcuitCourt> curcuitCourts)
        {
            using (IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(Helper.CnnVal("CourtsDB")))
            {
                foreach (CurcuitCourt curcuitCourt in curcuitCourts)
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
