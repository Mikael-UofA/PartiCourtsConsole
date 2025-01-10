using Newtonsoft.Json.Linq;

namespace PrepareData.Data.Types
{
    /// <summary>
    /// Represents a district court and its properties, including the number of judges, partisanship, and senior eligible judges.
    /// Implements the <see cref="ICourt"/> interface.
    /// </summary>
    public class DistrictCourt : ICourt
    {
       
        public int Id { get; set; }
        public string Name { get; }
        public string Abbreviation { get; }
        public int CourtOfAppeal { get; private set; }
        public int ActiveJudges { get; set; }
        public int MaxJudges { get; }
        public string ChiefJudge { get; set; }
        public int SeniorEligibleJudges { get; set; }
        public int DEMJudges { get; set; }
        public int GOPJudges { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictCourt"/> class.
        /// </summary>
        public DistrictCourt() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictCourt"/> class with specified parameters.
        /// </summary>
        /// <param name="name">The name of the district court.</param>
        /// <param name="abbreviation">The abbreviation of the district court.</param>
        /// <param name="courtOfAppeal">The court of appeal the district court is part of.</param>
        /// <param name="maxJudges">The maximum number of judges allowed in the district court.</param>
        /// <param name="chiefJudge">The name of the chief judge of the district court.</param>
        public DistrictCourt(string name, string abbreviation, string courtOfAppeal, int maxJudges, string chiefJudge)
        {
            Name = name;
            Abbreviation = abbreviation;
            MaxJudges = maxJudges;
            ChiefJudge = chiefJudge;
            ActiveJudges = 0;
            MakeAppeal(courtOfAppeal);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistrictCourt"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier for the district court.</param>
        /// <param name="name">The name of the district court.</param>
        /// <param name="abbreviation">The abbreviation of the district court.</param>
        /// <param name="courtOfAppeal">The court of appeal the district court is part of.</param>
        /// <param name="activeJudges">The number of active judges in the district court.</param>
        /// <param name="maxJudges">The maximum number of judges allowed in the district court.</param>
        /// <param name="chiefJudge">The name of the chief judge of the district court.</param>
        /// <param name="seniorEligibleJudges">The number of senior eligible judges in the district court.</param>
        /// <param name="dem">The number of Democratic judges in the district court.</param>
        /// <param name="gop">The number of Republican judges in the district court.</param>
        public DistrictCourt(Int32 id, String name, String abbreviation, Int32 courtOfAppeal, Int32 activeJudges,
            Int32 maxJudges, String chiefJudge, Int32 seniorEligibleJudges, Int32 dem, Int32 gop)
        {
            Id = id;
            Name = name;
            Abbreviation = abbreviation;
            CourtOfAppeal = courtOfAppeal;
            ActiveJudges = activeJudges;
            MaxJudges = maxJudges;
            ChiefJudge = chiefJudge;
            SeniorEligibleJudges = seniorEligibleJudges;
            DEMJudges = dem;
            GOPJudges = gop;
        }

        /// <summary>
        /// Sets the court of appeal based on the given appeal value.
        /// </summary>
        /// <param name="appeal">The court of appeal information as a string.</param>
        public void MakeAppeal(string appeal)
        {
            if (appeal == "D.C.")
            {
                CourtOfAppeal = 0;
                return;
            }
            string digits = appeal.Substring(0, appeal.Length - 2); // 1st, 3rd, 10th, ...
            CourtOfAppeal = int.Parse(digits);
        }

        public int GetNumberOfVacancies() => MaxJudges - ActiveJudges;

        public void SetPartisanshipOfCourt(List<Judge> judges)
        {
            DEMJudges = judges.Count(judge => judge.Partisanship == 1);
            GOPJudges = ActiveJudges - DEMJudges; // The remaining judges are assumed to be from the GOP
        }

        public int FindPartisanshipOfCourt()
        {
            if (DEMJudges > GOPJudges)
            {
                return 1; // Court leans Democratic
            }
            else if (DEMJudges < GOPJudges)
            {
                return -1; // Court leans Republican
            }
            return 0; // Court is evenly split or nonpartisan
        }

        public void SetChiefJudge(List<Judge> judges)
        {
            ChiefJudge = judges.FirstOrDefault(judge => judge.IsChief)?.Name;
        }

        public void SetNumOfSeniorEligibles(List<Judge> judges)
        {
            SeniorEligibleJudges = judges.Count(judge => judge.IsEligibleSeniorStatus());
        }

        public string GetNoWhiteSpace()
        {
            return Name.Replace(" ", "_");
        }

        /// <summary>
        /// Sets the ID of the district court from a GeoJSON file.
        /// </summary>
        /// <param name="path">The file path of the GeoJSON data.</param>
        /// <exception cref="Exception">Thrown if the ID cannot be generated from the GeoJSON data.</exception>
        public void SetIdFromGeoJson(string path)
        {
            string geoJsonContent = File.ReadAllText(path);
            JObject geoJson = JObject.Parse(geoJsonContent);
            if (geoJson["type"]?.ToString() == "FeatureCollection")
            {
                if (geoJson["features"] != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    foreach (var feature in geoJson["features"])
                    {
                        var properties = feature["properties"];

                        if (properties != null && properties["NAME"].ToString() == Name)
                        {
                            if (properties["FID"].ToString() != null)
                            {
                                Id = Int32.Parse(properties["FID"].ToString());
                                return;
                            }
                        }
                    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
            throw new Exception($"Couldn't generate ID of {Name}");
        }
    }
}
