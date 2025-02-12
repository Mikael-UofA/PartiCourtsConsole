
namespace PrepareData.Data.Services
{
    public class GovScrapper
    {
        public static async Task<List<Dictionary<string, Object>>?> FetchRetiringJudges()
        {
            string url = "https://www.uscourts.gov/data-news/judicial-vacancies/current-judicial-vacancies";
            using HttpClient client = new();

            try
            {
                string html = await client.GetStringAsync(url);

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var tbody = doc.DocumentNode.SelectSingleNode("//table/tbody");

                if (tbody != null)
                {
                    List<Dictionary<string, Object>> judges = [];
                    var rows = tbody.SelectNodes(".//tr");

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            var map = OrganizeRow(cells);
                            MessageBox.Show(map["judgeName"] + ", " + map["courtDiv"] + " District of " + map["courtName"]);
                            judges.Add(map);
                        }
                    }
                    return judges;
                }
                else
                {
                    MessageBox.Show("Table body (tbody) not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return null;
            
        }
        private static Dictionary<string, Object> OrganizeRow(HtmlAgilityPack.HtmlNodeCollection cells)
        {
            Dictionary<string, Object> map = new();
            string[] court = cells[0].InnerText.Trim().Split("-");
            string[] judge = cells[1].InnerText.Trim().Split(",");

            map["courtNum"] = court[0].Trim();
            map["courtName"] = "N/A";
            map["courtDiv"] = "";
            if (court[1].Trim() == "CCA")
            {
                map["isCircuit"] = true;
            } else
            {
                map["isCircuit"] = false;
                map["courtName"] = USStateLookup.GetStateName(court[1].Trim());
            }
        
            if (court.Length == 3)
            {
                map["courtDiv"] = USStateLookup.GetDivision(court[2].Trim());
            }
            map["judgeName"] = judge[1].Trim() + " " + judge[0].Trim();

            return map;

        }
    }

}
