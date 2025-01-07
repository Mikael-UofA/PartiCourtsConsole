using Newtonsoft.Json;
using PrepareData.Data.Types;
using PrepareGeojson.Formats;

namespace PrepareGeojson
{
    public static class GeojsonConfig
    {
       public static string geoDCInputPath = "../../../../PrepareData/sources/dc_boundaries.geojson";
       public static string geoDCOutputPath = "../../../../DisplayMaps/sources/dc_usable.geojson";

       public static string geoCCInputPath = "../../../../PrepareData/sources/cc_boundaries.geojson";
       public static string geoCCOutputPath = "../../../../DisplayMaps/sources/cc_usable.geojson";
        public static void CreateUsableGeojsonDC(List<DistrictCourt> courts)
       {
            string geo1Content = File.ReadAllText(geoDCInputPath);
            var geo1 = JsonConvert.DeserializeObject<GeoJson>(geo1Content);
            if (geo1 != null) {
                GeoJson geo2 = new GeoJson(geo1.Type, "dcourts", geo1.Crs, new List<Feature>());

                foreach (Feature feature in geo1.Features)
                {
                    Int64 fid = (Int64)feature.Properties["FID"];
                    DistrictCourt? currentCourt = courts.Find(court => court.Id == fid);
                    if (currentCourt != null)
                    {
                        Dictionary<string, object> newProperties = new Dictionary<string, object>();
                        newProperties.Add("FID", fid);
                        newProperties.Add("NAME", (string)feature.Properties["NAME"]);
                        newProperties.Add("CHIEF_JUDGE", currentCourt.ChiefJudge);
                        newProperties.Add("ACTIVE_JUDGES", currentCourt.ActiveJudges);
                        newProperties.Add("SENIOR_ELIGIBLE_JUDGES", currentCourt.SeniorEligibleJudges);
                        newProperties.Add("VACANCIES", currentCourt.GetNumberOfVacancies());
                        newProperties.Add("DEMJUDGES", currentCourt.DEMJudges);
                        newProperties.Add("GOPJUDGES", currentCourt.GOPJudges);
                        newProperties.Add("PARTISANSHIP", currentCourt.FindPartisanshipOfCourt());
                        Feature newFeature = new Feature(feature.Type, newProperties, feature.Geometry);
                        geo2.Features.Add(newFeature);
                    }
                }
                string newGeoContent = JsonConvert.SerializeObject(geo2, Formatting.Indented);
                File.WriteAllText(geoDCOutputPath, newGeoContent);
                
            }
        }

        public static void CreateUsableGeojsonCC(List<CircuitCourt> courts)
        {
            string geo1Content = File.ReadAllText(geoCCInputPath);
            var geo1 = JsonConvert.DeserializeObject<GeoJson>(geo1Content);
            if (geo1 != null)
            {
                GeoJson geo2 = new GeoJson(geo1.Type, "ccourts", geo1.Crs, new List<Feature>());

                foreach (Feature feature in geo1.Features)
                {
                    string name = (string)feature.Properties["NAME"];
                    CircuitCourt? currentCourt = courts.Find(court => string.Compare(court.GetCircuitCourtName(), name, StringComparison.OrdinalIgnoreCase) == 0);
                    if (currentCourt != null)
                    {
                        Dictionary<string, object> newProperties = new Dictionary<string, object>();
                        newProperties.Add("NAME", name);
                        newProperties.Add("SUPERVISING_JUSTICE", currentCourt.SupervisingJustice);
                        newProperties.Add("CHIEF_JUDGE", currentCourt.ChiefJudge);
                        newProperties.Add("ACTIVE_JUDGES", currentCourt.ActiveJudges);
                        newProperties.Add("SENIOR_ELIGIBLE_JUDGES", currentCourt.SeniorEligibleJudges);
                        newProperties.Add("VACANCIES", currentCourt.GetNumberOfVacancies());
                        newProperties.Add("DEMJUDGES", currentCourt.DEMJudges);
                        newProperties.Add("GOPJUDGES", currentCourt.GOPJudges);
                        newProperties.Add("PARTISANSHIP", currentCourt.FindPartisanshipOfCourt());
                        Feature newFeature = new Feature(feature.Type, newProperties, feature.Geometry);
                        geo2.Features.Add(newFeature);
                    }
                }
                string newGeoContent = JsonConvert.SerializeObject(geo2, Formatting.Indented);
                File.WriteAllText(geoCCOutputPath, newGeoContent);

            }
        }
    }
}
