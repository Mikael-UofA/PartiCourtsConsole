
namespace PrepareData.Data.Services
{
    public class DistrictCourtLookup
    {
        private static readonly Dictionary<string, int> DistrictCourtIDs = new(StringComparer.OrdinalIgnoreCase)
    {
        {"Western District of Kentucky", 1}, {"Eastern District of Kentucky", 2}, {"Southern District of Indiana", 3},
        {"Middle District of Alabama", 4}, {"Southern District of Alabama", 5}, {"Western District of Arkansas", 6},
        {"Eastern District of Arkansas", 7}, {"Northern District of California", 8}, {"Eastern District of California", 9},
        {"Central District of California", 10}, {"District of Colorado", 11}, {"District of District of Columbia", 12},
        {"Middle District of Florida", 13}, {"Northern District of Florida", 14}, {"Middle District of Georgia", 15},
        {"Southern District of Georgia", 16}, {"Northern District of Georgia", 17}, {"District of Idaho", 18},
        {"District of Kansas", 19}, {"Northern District of Texas", 20}, {"Southern District of Texas", 21},
        {"Western District of Texas", 22}, {"Western District of Missouri", 23}, {"Eastern District of Missouri", 24},
        {"District of Montana", 25}, {"District of Nebraska", 26}, {"Eastern District of Washington", 27},
        {"Western District of Louisiana", 28}, {"Eastern District of Louisiana", 29}, {"District of Maine", 30},
        {"District of Maryland", 31}, {"Southern District of Iowa", 32}, {"District of New Jersey", 33},
        {"District of New Mexico", 34}, {"Western District of New York", 35}, {"Northern District of New York", 36},
        {"Eastern District of New York", 37}, {"Western District of North Carolina", 38}, {"Eastern District of North Carolina", 39},
        {"Middle District of North Carolina", 40}, {"District of North Dakota", 41}, {"Northern District of Ohio", 42},
        {"Western District of Oklahoma", 43}, {"Eastern District of Texas", 44}, {"District of Utah", 45},
        {"Eastern District of Oklahoma", 46}, {"District of Oregon", 47}, {"Western District of Pennsylvania", 48},
        {"Eastern District of Pennsylvania", 49}, {"Middle District of Pennsylvania", 50}, {"District of South Carolina", 51},
        {"District of South Dakota", 52}, {"Eastern District of Tennessee", 53}, {"Western District of Tennessee", 54},
        {"Middle District of Tennessee", 55}, {"District of Massachusetts", 56}, {"District of Hawaii", 57},
        {"Southern District of Illinois", 58}, {"Western District of Virginia", 59}, {"Western District of Washington", 60},
        {"District of US Virgin Islands", 61}, {"District of Minnesota", 62}, {"Northern District of Mississippi", 63},
        {"Southern District of Mississippi", 64}, {"Northern District of Indiana", 65}, {"District of Nevada", 66},
        {"District of New Hampshire", 67}, {"Southern District of California", 68}, {"District of Arizona", 69},
        {"Northern District of West Virginia", 70}, {"Southern District of West Virginia", 71}, {"Western District of Wisconsin", 72},
        {"District of Wyoming", 73}, {"District of Northern Marianas Islands", 74}, {"Southern District of Florida", 75},
        {"Northern District of Oklahoma", 76}, {"District of Vermont", 77}, {"District of Delaware", 78},
        {"Eastern District of Wisconsin", 79}, {"Southern District of Ohio", 80}, {"Western District of Michigan", 81},
        {"Southern District of New York", 82}, {"District of Rhode Island", 83}, {"District of Alaska", 84},
        {"Northern District of Illinois", 85}, {"District of Guam", 86}, {"District of Puerto Rico", 87},
        {"Eastern District of Michigan", 88}, {"Northern District of Iowa", 89}, {"Central District of Illinois", 90},
        {"District of Connecticut", 91}, {"Northern District of Alabama", 92}, {"Middle District of Louisiana", 93},
        {"Eastern District of Virginia", 94}
    };

        public static int GetDistrictID(string districtName)
        {
            if (DistrictCourtIDs.TryGetValue(districtName, out int districtID))
            {
                return districtID;
            }
            return -1;
        }
    }
}
