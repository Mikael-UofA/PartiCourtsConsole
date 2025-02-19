using Newtonsoft.Json;
using PrepareData.Data.Types;
using PrepareGeojson.Formats;

namespace PrepareGeojson
{
    /// <summary>
    /// Static class for managing GeoJSON configuration and processing.
    /// </summary>
    public static class GeojsonConfig
    {
        /// <summary>
        /// Input path for district court GeoJSON file.
        /// </summary>
        public static string geoDCInputPath = "../../../../PrepareData/sources/dc_boundaries.geojson";

        /// <summary>
        /// Output path for processed district court GeoJSON file.
        /// </summary>
        public static string geoDCOutputPath = "../../../../DisplayMaps/sources/dc_usable.geojson";

        /// <summary>
        /// Input path for circuit court GeoJSON file.
        /// </summary>
        public static string geoCCInputPath = "../../../../PrepareData/sources/cc_boundaries.geojson";

        /// <summary>
        /// Output path for processed circuit court GeoJSON file.
        /// </summary>
        public static string geoCCOutputPath = "../../../../DisplayMaps/sources/cc_usable.geojson";

        /// <summary>
        /// Processes the district court GeoJSON file, enriching it with data from a list of district courts.
        /// </summary>
        /// <param name="courts">List of district courts containing additional data to enrich the GeoJSON file.</param>
        public static void CreateUsableGeojsonDC(List<DistrictCourt> courts)
        {
            string geo1Content = File.ReadAllText(geoDCInputPath);
            var geo1 = JsonConvert.DeserializeObject<GeoJson>(geo1Content);
            if (geo1 != null)
            {
                // Create a new GeoJSON object to store enriched features
                GeoJson geo2 = new GeoJson(geo1.Type, "dcourts", geo1.Crs, new List<Feature>());

                foreach (Feature feature in geo1.Features)
                {
                    Int64 fid = (Int64)feature.Properties["FID"];
                    DistrictCourt? currentCourt = courts.Find(court => court.Id == fid);
                    if (currentCourt != null)
                    {
                        // Enrich feature properties with court data
                        Dictionary<string, object> newProperties = new Dictionary<string, object>
                    {
                        { "FID", fid },
                        { "NAME", (string)feature.Properties["NAME"] },
                        { "CHIEF_JUDGE", currentCourt.ChiefJudge },
                        { "ACTIVE_JUDGES", currentCourt.ActiveJudges },
                        { "SENIOR_ELIGIBLE_JUDGES", currentCourt.SeniorEligibleJudges },
                        { "VACANCIES", currentCourt.GetNumberOfVacancies() },
                        { "DEMJUDGES", currentCourt.DEMJudges },
                        { "GOPJUDGES", currentCourt.GOPJudges },
                        { "PARTISANSHIP", currentCourt.FindPartisanshipOfCourt() },
                        { "DEMRETIRING", currentCourt.DEMRetiring},
                        { "GOPRETIRING", currentCourt.GOPRetiring}
                    };
                        Feature newFeature = new Feature(feature.Type, newProperties, feature.Geometry);
                        geo2.Features.Add(newFeature);
                    }
                }
                // Write the enriched GeoJSON data to the output file
                string newGeoContent = JsonConvert.SerializeObject(geo2, Formatting.Indented);
                File.WriteAllText(geoDCOutputPath, newGeoContent);
            }
        }

        /// <summary>
        /// Processes the circuit court GeoJSON file, enriching it with data from a list of circuit courts.
        /// </summary>
        /// <param name="courts">List of circuit courts containing additional data to enrich the GeoJSON file.</param>
        public static void CreateUsableGeojsonCC(List<CircuitCourt> courts)
        {
            string geo1Content = File.ReadAllText(geoCCInputPath);
            var geo1 = JsonConvert.DeserializeObject<GeoJson>(geo1Content);
            if (geo1 != null)
            {
                // Create a new GeoJSON object to store enriched features
                GeoJson geo2 = new GeoJson(geo1.Type, "ccourts", geo1.Crs, new List<Feature>());

                foreach (Feature feature in geo1.Features)
                {
                    string name = (string)feature.Properties["NAME"];
                    CircuitCourt? currentCourt = courts.Find(court => string.Compare(court.GetCircuitCourtName(), name, StringComparison.OrdinalIgnoreCase) == 0);
                    if (currentCourt != null)
                    {
                        // Enrich feature properties with court data
                        Dictionary<string, object> newProperties = new Dictionary<string, object>
                    {
                        { "NAME", name },
                        { "SUPERVISING_JUSTICE", currentCourt.SupervisingJustice },
                        { "CHIEF_JUDGE", currentCourt.ChiefJudge },
                        { "ACTIVE_JUDGES", currentCourt.ActiveJudges },
                        { "SENIOR_ELIGIBLE_JUDGES", currentCourt.SeniorEligibleJudges },
                        { "VACANCIES", currentCourt.GetNumberOfVacancies() },
                        { "DEMJUDGES", currentCourt.DEMJudges },
                        { "GOPJUDGES", currentCourt.GOPJudges },
                        { "PARTISANSHIP", currentCourt.FindPartisanshipOfCourt() },
                        { "DEMRETIRING", currentCourt.DEMRetiring},
                        { "GOPRETIRING", currentCourt.GOPRetiring}
                    };
                        Feature newFeature = new Feature(feature.Type, newProperties, feature.Geometry);
                        geo2.Features.Add(newFeature);
                    }
                }
                // Write the enriched GeoJSON data to the output file
                string newGeoContent = JsonConvert.SerializeObject(geo2, Formatting.Indented);
                File.WriteAllText(geoCCOutputPath, newGeoContent);
            }
        }
    }
}
