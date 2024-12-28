using Newtonsoft.Json.Linq;
using PrepareData.Data.Types;

namespace PrepareData.Data.Services
{
    public class WikipediaScrapper
    {
        public static async Task<List<CircuitCourt>> GenerateCC()
        {
            // Wikipedia API URL
            string baseUrl = "https://en.wikipedia.org/w/api.php";
            string pageName = "United_States_courts_of_appeals";
            string query = $"?action=parse&page={Uri.EscapeDataString(pageName)}&format=json&prop=text";
            List<CircuitCourt> temp = new List<CircuitCourt>();
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
                        MessageBox.Show("No content found for the page.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
            }
            return temp;
        }
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
                            int maxJudges = Int32.Parse(cells[2].InnerText.Trim());
                            CircuitCourt court = new CircuitCourt(i, name, supervisingJustice, maxJudges);
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
                        MessageBox.Show("No content found for the page.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
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
            List<string> territorialCourts = new List<string> { "District of Guam", 
                "District of the Northern Mariana Islands", "District of the Virgin Islands" };
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
                        if (! territorialCourts.Contains(name))
                        {
                            string abbreviation = cells[1].InnerText.Trim();
                            string courtOfAppeal = cells[2].InnerText.Trim();
                            string chiefJudge = cells[6].InnerText.Trim();
                            int maxJudges = Int32.Parse(cells[4].InnerText.Trim());
                            DistrictCourt court = new DistrictCourt(name, abbreviation, courtOfAppeal, maxJudges, chiefJudge);
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
                        return ParseDJTableFromHtml(htmlContent, court);
                    }
                    else
                    {
                        MessageBox.Show("No content found for the page.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
            }
            return temp;
        }
        private static List<Judge> ParseDJTableFromHtml(string htmlContent, DistrictCourt court)
        {
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlContent);
            var table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'sortable')]");
            List<Judge> judges = new List<Judge>();

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
                        if (title != "Senior Judge" && name != "vacant" && cells[5].InnerText.Trim()[..3] != "beg")
                        {
                            int birth = 0;
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[8].InnerText.Trim();

                            Int32.TryParse(cells[4].InnerText.Trim(), out birth);
                            string temp1 = cells[5].InnerText.Trim();
                            string temp2 = temp1[..4];
                            int appointmentDate = 0;
                            Int32.TryParse(temp2, out appointmentDate);

                            Judge judge = new Judge(name, false, court.Id, birth, title, appointedBy, appointmentDate, isChief);
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

        public static async Task<List<Judge>> GetCJudges(CircuitCourt court)
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
                        MessageBox.Show("No content found for the page.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
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
                        if (title != "Senior Circuit Judge" && name != "vacant" && cells[5].InnerText.Trim()[..3] != "beg")
                        {
                            int birth = 0;
                            bool isChief = (title == "Chief Judge");
                            string appointedBy = cells[8].InnerText.Trim();

                            Int32.TryParse(cells[4].InnerText.Trim(), out birth);
                            string temp1 = cells[5].InnerText.Trim();
                            int appointmentDate = 0;
                            string temp2 = temp1[..4];
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
                MessageBox.Show("No tables found in the HTML.");
                return judges;
            }
        }
    }
}
