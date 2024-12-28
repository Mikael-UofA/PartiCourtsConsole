using Newtonsoft.Json.Linq;
using PrepareData.Data.Types;

namespace PrepareData.Data.Services
{
    public class WikipediaScrapper
    {
        public static async Task<List<CurcuitCourt>> GenerateCC()
        {
            // Wikipedia API URL
            string baseUrl = "https://en.wikipedia.org/w/api.php";
            string pageName = "United_States_courts_of_appeals";
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            List<CurcuitCourt> temp = new List<CurcuitCourt>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl + query);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResponse);
                    string? htmlContent = json["parse"]?["text"]?["*"]?.ToString();

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        return ParseCCTableFromHtml(htmlContent);
                    }
                    else
                    {
                        Console.WriteLine("No content found for the page.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            return temp;
        }
        private static List<CurcuitCourt> ParseCCTableFromHtml(string htmlContent)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<CurcuitCourt> courts = new List<CurcuitCourt>();

            if (tables != null)
            {
                foreach (var table in tables)
                {
                    int i = 0;
                    var rows = table.SelectNodes(".//tr");
                    foreach (var row in rows)
                    {
                        // Skips header and the last two rows which are the federal court and total row
                        if (i == rows.Count - 2) { continue; }
                        if (i++ == 0) { continue; }

                        var cells = row.SelectNodes(".//th|.//td");
                        if (cells != null)
                        {
                            string name = cells[0].InnerText.Trim();
                            string supervisingJustice = cells[1].InnerText.Trim();
                            int maxJudges = 0;
                            int ccourt_id = i - 2;
                            if (Int32.TryParse(cells[2].InnerText.Trim(), out maxJudges))
                            {
                                CurcuitCourt court = new CurcuitCourt(ccourt_id, name, supervisingJustice, maxJudges);
                                courts.Add(court);
                            }
                        }
                    }
                }
                return courts;
            }
            else
            {
                Console.WriteLine("No tables found in the HTML.");
                return courts;
            }
        }


        public static async Task<List<DistrictCourt>> GenerateDC()
        {
            // Wikipedia API URL
            string baseUrl = "https://en.wikipedia.org/w/api.php";
            string pageName = "List_of_United_States_district_and_territorial_courts";
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            List<DistrictCourt> temp = new List<DistrictCourt>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl + query);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResponse);
                    string? htmlContent = json["parse"]?["text"]?["*"]?.ToString();

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        return ParseDCTableFromHtml(htmlContent);
                    }
                    else
                    {
                        Console.WriteLine("No content found for the page.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            return temp;

        }
        private static List<DistrictCourt> ParseDCTableFromHtml(string htmlContent)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<DistrictCourt> courts = new List<DistrictCourt>();

            if (tables != null)
            {
                var table = tables[1];
                int i = 0;
                var rows = table.SelectNodes(".//tr");
                foreach (var row in rows)
                {
                    // Skips header
                    if (i++ == 0) { continue; }

                    var cells = row.SelectNodes(".//th|.//td");
                    if (cells != null)
                    {
                        string name = cells[0].InnerText.Trim();
                        string abbreviation = cells[1].InnerText.Trim();
                        string courtOfAppeal = cells[2].InnerText.Trim();
                        string chiefJudge = cells[6].InnerText.Trim();
                        int maxJudges = 0;
                        if (Int32.TryParse(cells[4].InnerText.Trim(), out maxJudges))
                        {
                            DistrictCourt court = new DistrictCourt(name, abbreviation, courtOfAppeal, maxJudges, chiefJudge);
                            courts.Add(court);
                        }
                    }
                }
                return courts;
            }
            else
            {
                Console.WriteLine("No tables found in the HTML.");
                return courts;
            }
        }


        public static async Task<List<Judge>> GetDJudges(DistrictCourt court)
        {
            // Wikipedia API URL
            string baseUrl = "https://en.wikipedia.org/w/api.php";
            string pageName = "United_States_District_Court_for_the_" + court.GetNoWhiteSpace();
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            List<Judge> temp = new List<Judge>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl + query);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResponse);
                    string? htmlContent = json["parse"]?["text"]?["*"]?.ToString();

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        return ParseDJTableFromHtml(htmlContent, court.Id);
                    }
                    else
                    {
                        Console.WriteLine("No content found for the page.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            return temp;
        }
        private static List<Judge> ParseDJTableFromHtml(string htmlContent, int courtId)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<Judge> judges = new List<Judge>();

            if (tables != null)
            {
                var table = tables[0];
                int i = 0;
                var rows = table.SelectNodes(".//tr");
                foreach (var row in rows)
                {
                    // Skips header
                    if (i++ < 2) { continue; }

                    var cells = row.SelectNodes(".//th|.//td");
                    if (cells != null)
                    {
                        string title = cells[1].InnerText.Trim();
                        string name = cells[2].InnerText.Trim();
                        if (title != "Senior Judge" && name != "vacant")
                        {
                            int birth = 0;
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[8].InnerText.Trim();

                            Int32.TryParse(cells[4].InnerText.Trim(), out birth);
                            string temp1 = cells[5].InnerText.Trim();
                            string temp2 = temp1.Substring(0, 4);
                            int appointmentDate = 0;
                            Int32.TryParse(temp2, out appointmentDate);

                            Judge judge = new Judge(name, false, courtId, birth, title, appointedBy, appointmentDate, isChief);
                            judges.Add(judge);
                        }
                    }
                }
                return judges;
            }
            else
            {
                Console.WriteLine(courtId);
                Console.WriteLine("No tables found in the HTML.");
                return judges;
            }
        }

        public static async Task<List<Judge>> GetCJudges(CurcuitCourt court)
        {
            // Wikipedia API URL
            string baseUrl = "https://en.wikipedia.org/w/api.php";
            string pageName = "United_States_Court_of_Appeals_for_the_" + court.GetNoWhiteSpace();
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            List<Judge> temp = new List<Judge>();
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl + query);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonResponse);
                    string? htmlContent = json["parse"]?["text"]?["*"]?.ToString();

                    if (!string.IsNullOrEmpty(htmlContent))
                    {
                        return ParseCJTableFromHtml(htmlContent, court.Id);
                    }
                    else
                    {
                        Console.WriteLine("No content found for the page.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            return temp;
        }
        private static List<Judge> ParseCJTableFromHtml(string htmlContent, int courtId)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var tables = doc.DocumentNode.SelectNodes("//table[contains(@class, 'sortable')]");
            List<Judge> judges = new List<Judge>();

            if (tables != null)
            {
                var table = tables[0];
                int i = 0;
                var rows = table.SelectNodes(".//tr");
                foreach (var row in rows)
                {
                    // Skips header
                    if (i++ == 0) { continue; }

                    var cells = row.SelectNodes(".//th|.//td");
                    if (cells != null)
                    {
                        string title = cells[0].InnerText.Trim();
                        if (title != "Senior Curcuit Judge")
                        {
                            int birth = 0;
                            string name = cells[1].InnerText.Trim();
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[7].InnerText.Trim();

                            Int32.TryParse(cells[3].InnerText.Trim(), out birth);
                            string temp1 = cells[4].InnerText.Trim();
                            string temp2 = temp1.Substring(0, 4);
                            int appointmentDate = 0;
                            Int32.TryParse(temp2, out appointmentDate);

                            Judge judge = new Judge(name, true, courtId, birth, title, appointedBy, appointmentDate, isChief);
                            judges.Add(judge);
                        }
                    }
                }
                return judges;
            }
            else
            {
                Console.WriteLine("No tables found in the HTML.");
                return judges;
            }
        }
    }
}
