
namespace PrepareData.Data.Services
{
    public class USStateLookup
    {
        private static readonly Dictionary<string, string> StateAbbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"AL", "Alabama"}, {"AK", "Alaska"}, {"AZ", "Arizona"}, {"AR", "Arkansas"},
            {"CA", "California"}, {"CO", "Colorado"}, {"CT", "Connecticut"}, {"DE", "Delaware"},
            {"FL", "Florida"}, {"GA", "Georgia"}, {"HI", "Hawaii"}, {"ID", "Idaho"},
            {"IL", "Illinois"}, {"IN", "Indiana"}, {"IA", "Iowa"}, {"KS", "Kansas"},
            {"KY", "Kentucky"}, {"LA", "Louisiana"}, {"ME", "Maine"}, {"MD", "Maryland"},
            {"MA", "Massachusetts"}, {"MI", "Michigan"}, {"MN", "Minnesota"}, {"MS", "Mississippi"},
            {"MO", "Missouri"}, {"MT", "Montana"}, {"NE", "Nebraska"}, {"NV", "Nevada"},
            {"NH", "New Hampshire"}, {"NJ", "New Jersey"}, {"NM", "New Mexico"}, {"NY", "New York"},
            {"NC", "North Carolina"}, {"ND", "North Dakota"}, {"OH", "Ohio"}, {"OK", "Oklahoma"},
            {"OR", "Oregon"}, {"PA", "Pennsylvania"}, {"RI", "Rhode Island"}, {"SC", "South Carolina"},
            {"SD", "South Dakota"}, {"TN", "Tennessee"}, {"TX", "Texas"}, {"UT", "Utah"},
            {"VT", "Vermont"}, {"VA", "Virginia"}, {"WA", "Washington"}, {"WV", "West Virginia"},
            {"WI", "Wisconsin"}, {"WY", "Wyoming"}
        };
        private static readonly Dictionary<string, string> Divisions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"E", "Eastern"}, {"W", "Western"}, {"N", "Northern"}, {"S", "Southern"}
        };

        public static string GetStateName(string abbreviation)
        {
            if (StateAbbreviations.TryGetValue(abbreviation, out string stateName))
            {
                return stateName;
            }
            return "Invalid abbreviation";
        }

        public static string GetDivision(string abbreviation)
        {
            if (Divisions.TryGetValue(abbreviation, out string division))
            {
                return division;
            }
            return "";
        }
    }
}
