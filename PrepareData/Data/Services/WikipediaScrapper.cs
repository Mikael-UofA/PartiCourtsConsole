using Newtonsoft.Json.Linq;
using PrepareData.Data.Types;

namespace PrepareData.Data.Services
{
    /// <summary>
    /// The WikipediaScrapper class provides methods to fetch and parse data from Wikipedia pages 
    /// related to the United States Courts of Appeals and District Courts, as well as their judges.
    /// </summary>
    public class WikipediaScrapper
    {
        // A set containing the names of U.S. territorial courts.
        private static readonly HashSet<string> TerritorialCourts =
        [
        "District of Guam",
        "District of the Northern Mariana Islands",
        "District of the Virgin Islands"
        ];

        /// <summary>
        /// Validates if a judge's information meets specific criteria for inclusion.
        /// </summary>
        /// <param name="title">The title of the judge.</param>
        /// <param name="name">The name of the judge.</param>
        /// <param name="endDate">The end date of the judge's service.</param>
        /// <returns>True if the judge is valid; otherwise, false.</returns>
        private static bool IsValidJudge(string title, string name, string endDate)
        {
            return title != "Senior Judge" && title != "Senior Circuit Judge" && name != "vacant" && !endDate.StartsWith("beg");
        }

        /// <summary>
        /// Fetches the raw HTML content of a Wikipedia page using the Wikipedia API.
        /// </summary>
        /// <param name="pageName">The name of the Wikipedia page to fetch.</param>
        /// <returns>The raw HTML content of the page, or null if the fetch fails.</returns>
        private static async Task<string?> FetchPageContentAsync(string pageName)
        {
            string baseUrl = "https://en.wikipedia.org/w/api.php"; // Wikipedia API URL
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(baseUrl + query);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(jsonResponse);
                        return json["parse"]?["text"]?["*"]?.ToString();
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception occurred: {ex.Message}");
                    return null;
                }
            }
        }

        /// <summary>
        /// Generates a list of Circuit Courts by scraping and parsing the relevant Wikipedia page.
        /// </summary>
        /// <returns>A list of CircuitCourt objects.</returns>
        public static async Task<List<CircuitCourt>> GenerateCC()
        {
            List<CircuitCourt> temp = new List<CircuitCourt>();
            string? htmlContent = await FetchPageContentAsync("United_States_courts_of_appeals");
            if (!string.IsNullOrEmpty(htmlContent))
            {
                return ParseCCTableFromHtml(htmlContent);
            }
            else
            {
                MessageBox.Show("No content found for the page. #1");
            }
            return temp;
        }

        /// <summary>
        /// Generates a list of District Courts by scraping and parsing the relevant Wikipedia page.
        /// </summary>
        /// <returns>A list of DistrictCourt objects.</returns>
        public static async Task<List<DistrictCourt>> GenerateDC()
        {
            List<DistrictCourt> temp = new List<DistrictCourt>();
            string? htmlContent = await FetchPageContentAsync("List_of_United_States_district_and_territorial_courts");
            if (!string.IsNullOrEmpty(htmlContent))
            {
                return ParseDCTableFromHtml(htmlContent);
            }
            else
            {
                MessageBox.Show("No content found for the page. #2");
            }
            return temp;
        }

        /// <summary>
        /// Fetches and parses a list of district judges for a given district court.
        /// </summary>
        /// <param name="court">The district court for which to fetch judges.</param>
        /// <returns>A list of Judge objects for the district court.</returns>
        public static async Task<List<Judge>> GetDJudges(DistrictCourt court)
        {
            string pageName = "United_States_District_Court_for_the_" + court.GetNoWhiteSpace();
            if (court.Name == "District of the District of Columbia")
            {
                pageName = "United_States_District_Court_for_the_District_of_Columbia";
            }
            List<Judge> temp = new List<Judge>();
            string? htmlContent = await FetchPageContentAsync(pageName);
            if (!string.IsNullOrEmpty(htmlContent))
            {
                return ParseDJTableFromHtml(htmlContent, court);
            }
            else
            {
                MessageBox.Show("No content found for the page. #3 " + court.Name);
            }
            return temp;
        }

        /// <summary>
        /// Fetches and parses a list of circuit judges for a given circuit court.
        /// </summary>
        /// <param name="court">The circuit court for which to fetch judges.</param>
        /// <returns>A list of Judge objects for the circuit court.</returns>
        public static async Task<List<Judge>> GetCJudges(CircuitCourt court)
        {
            string pageName = "United_States_Court_of_Appeals_for_the_" + court.GetNoWhiteSpace();
            List<Judge> temp = new List<Judge>();
            string? htmlContent = await FetchPageContentAsync(pageName);
            if (!string.IsNullOrEmpty(htmlContent))
            {
                return ParseCJTableFromHtml(htmlContent, court.Id);
            }
            else
            {
                MessageBox.Show("No content found for the page. #4");
            }
            return temp;
        }

        /// <summary>
        /// Parses the HTML content to extract a list of Circuit Courts.
        /// </summary>
        /// <param name="htmlContent">The HTML content containing circuit court data.</param>
        /// <returns>A list of CircuitCourt objects.</returns>
        private static List<CircuitCourt> ParseCCTableFromHtml(string htmlContent)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'sortable')]");
            List<CircuitCourt> courts = new List<CircuitCourt>();

            if (table != null)
            {
                var rows = table.SelectNodes("./tbody/tr");
                rows.Remove(rows.Count - 1);
                rows.Remove(rows.Count - 1);
                rows.Remove(0);
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count; i++)
                    {
                        var cells = rows[i].SelectNodes("./td");
                        if (cells != null)
                        {
                            string name = cells[0].InnerText.Trim();
                            string supervisingJustice = cells[1].InnerText.Trim();
                            int maxJudges = int.Parse(cells[2].InnerText.Trim());
                            CircuitCourt court = new(i, name, supervisingJustice, maxJudges);
                            courts.Add(court);
                        }
                    }
                }
                return courts;
            }
            else
            {
                MessageBox.Show("No tables found in the HTML.");
                return courts;
            }
        }

        /// <summary>
        /// Parses the HTML content to extract a list of District Courts.
        /// </summary>
        /// <param name="htmlContent">The HTML content containing district court data.</param>
        /// <returns>A list of DistrictCourt objects.</returns>
        private static List<DistrictCourt> ParseDCTableFromHtml(string htmlContent)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<DistrictCourt> courts = new List<DistrictCourt>();
            if (tables != null)
            {
                var table = tables[1];
                var rows = table.SelectNodes("./tbody/tr");
                rows.Remove(0);
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("./td");
                    if (cells != null)
                    {
                        string name = cells[0].InnerText.Trim();
                        if (!TerritorialCourts.Contains(name))
                        {
                            string abbreviation = cells[1].InnerText.Trim();
                            string courtOfAppeal = cells[2].InnerText.Trim();
                            string chiefJudge = cells[6].InnerText.Trim();
                            int maxJudges = int.Parse(cells[4].InnerText.Trim());
                            DistrictCourt court = new(name, abbreviation, courtOfAppeal, maxJudges, chiefJudge);
                            courts.Add(court);
                        }
                    }
                }
                return courts;
            }
            else
            {
                MessageBox.Show("No tables found in the HTML.");
                return courts;
            }
        }

        /// <summary>
        /// Parses the HTML content to extract a list of District Judges.
        /// </summary>
        /// <param name="htmlContent">The HTML content containing judge data.</param>
        /// <returns>A list of Judge objects.</returns>
        private static List<Judge> ParseDJTableFromHtml(string htmlContent, DistrictCourt court)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'sortable')]");
            List<Judge> judges = [];

            if (table != null)
            {
                var rows = table.SelectNodes("./tbody/tr");
                rows.Remove(0);
                rows.Remove(0);
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("./td");
                    if (cells != null)
                    {
                        string title = cells[1].InnerText.Trim();
                        string name = cells[2].InnerText.Trim();
                        string endDate = cells[5].InnerText.Trim();
                        if (IsValidJudge(title, name, endDate))
                        {
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[8].InnerText.Trim();

                            _ = int.TryParse(cells[4].InnerText.Trim(), out int birth);
                            string temp1 = cells[5].InnerText.Trim();
                            string temp2 = temp1[..4];
                            _ = int.TryParse(temp2, out int appointmentDate);

                            Judge judge = new(name, false, court.Id, birth, title, appointedBy, appointmentDate, isChief, false);
                            judges.Add(judge);
                        }
                    }
                }
                return judges;
            }
            else
            {
                MessageBox.Show(court.Name);
                MessageBox.Show("No tables found in the HTML.");
                return judges;
            }
        }

        /// <summary>
        /// Parses the HTML content to extract a list of Circuit Judges.
        /// </summary>
        /// <param name="htmlContent">The HTML content containing judge data.</param>
        /// <returns>A list of Judge objects.</returns>
        private static List<Judge> ParseCJTableFromHtml(string htmlContent, int courtId)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<Judge> judges = [];

            if (tables != null)
            {
                var table = tables[0];
                var rows = table.SelectNodes(".//tbody/tr");
                rows.Remove(0);
                rows.Remove(0);
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes(".//td");
                    if (cells != null)
                    {
                        string title = cells[1].InnerText.Trim();
                        string name = cells[2].InnerText.Trim();
                        string endDate = cells[5].InnerText.Trim();
                        if (IsValidJudge(title, name, endDate))
                        {
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[8].InnerText.Trim();

                            _ = int.TryParse(cells[4].InnerText.Trim(), out int birth);
                            string temp1 = cells[5].InnerText.Trim();
                            string temp2 = temp1[..4];
                            _ = int.TryParse(temp2, out int appointmentDate);

                            Judge judge = new(name, true, courtId, birth, title, appointedBy, appointmentDate, isChief, false);
                            judges.Add(judge);
                        }
                    }
                }
                return judges;
            }
            else
            {
                MessageBox.Show("No tables found in the HTML.");
                return judges;
            }
        }
    }
}
