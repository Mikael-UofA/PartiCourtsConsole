using Newtonsoft.Json.Linq;

namespace PrepareData.Data.Types
{
    public class DistrictCourt: ICourt
    {
        public int Id { get; set; }
        public string Name { get; }
        public string Abbreviation { get; }
        public int CourtOfAppeal { get; private set; }
        public int? ActiveJudges { get; set; }
        public int MaxJudges { get; }
        public string ChiefJudge { get; set; }
        public int? SeniorEligibleJudges { get; set; }
        public int? DEMJudges { get; set; }
        public int? GOPJudges { get; set; }


        public DistrictCourt(string name, string abbreviation, string courtOfAppeal, int maxJudges, string chiefJudge)
        {
            Name = name;
            ActiveJudges = null;
            Abbreviation = abbreviation;
            MaxJudges = maxJudges;
            ChiefJudge = chiefJudge;
            SeniorEligibleJudges = null;
            MakeAppeal(courtOfAppeal);
        }
        public DistrictCourt(int id, string name, string abbreviation, int courtOfAppeal, int activeJudges, int maxJudges, string chiefJudge, int seniorEligibleJudges, int dem, int gop)
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

        public int GetNumberOfVacancies()
        {
            if (ActiveJudges != null)
            {
                return (int)(MaxJudges - ActiveJudges);
            }
            return MaxJudges;
        }

        private void MakeAppeal(string appeal)
        {
            if (appeal == "D.C.")
            {
                CourtOfAppeal = 0;
                return;
            }
            string digits = appeal.Substring(0, appeal.Length - 2);
            CourtOfAppeal = int.Parse(digits);

        }
        public void FindPartisanshipOfCourt(List<Judge> judges)
        {
            DEMJudges = 0;
            foreach (Judge judge in judges)
            {
                if (judge.Partisanship == 1)
                {
                    DEMJudges++;
                }
            }
            GOPJudges = ActiveJudges - DEMJudges;
        }

        public string GetNoWhiteSpace()
        {
            return Name.Replace(" ", "_");
        }


        public bool SetIdFromGeoJson(string path)
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

                        if (properties != null && properties["NAME"]?.ToString() == Name)
                        {
                            int temp = 0;
                            bool temp2 = int.TryParse(properties["FID"]?.ToString(), out temp);

                            if (temp2)
                            {
                                Id = temp;
                            }
                            return temp2;
                        }
                    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }
            return false;

        }
    }
}
