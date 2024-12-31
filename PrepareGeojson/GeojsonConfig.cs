using Newtonsoft.Json;
using PrepareData.Data.Types;
using PrepareGeojson.Formats;

namespace PrepareGeojson
{
    public static class GeojsonConfig
    {
       public static string geoInputPath = "../../../../PrepareData/sources/boundaries.geojson";
       public static string geoOutputPath = "../../../../PrepareData/sources/usable.geojson";
       public static void CreateUsableGeojson(List<DistrictCourt> courts)
       {
            string geo1Content = File.ReadAllText(geoInputPath);
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
                        newProperties.Add("PARTISANSHIP", currentCourt.FindPartisanshipOfCourt());
                        Feature newFeature = new Feature(feature.Type, newProperties, feature.Geometry);
                        geo2.Features.Add(newFeature);
                    }
                }
                string newGeoContent = JsonConvert.SerializeObject(geo2, Formatting.Indented);
                File.WriteAllText(geoOutputPath, newGeoContent);
                
            }
        }
    }
}
