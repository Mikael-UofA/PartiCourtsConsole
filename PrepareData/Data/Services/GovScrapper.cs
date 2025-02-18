
namespace PrepareData.Data.Services
{
    public class GovScrapper
    {
        public static async Task<List<Dictionary<string, Object>>> FetchRetiringJudges()
        {
            string url = "https://www.uscourts.gov/data-news/judicial-vacancies/current-judicial-vacancies";
            using HttpClient client = new();
            List<Dictionary<string, Object>> judges = [];
            try
            {
                string html = await client.GetStringAsync(url);

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var tbody = doc.DocumentNode.SelectSingleNode("//table/tbody");

                if (tbody != null)
                {
                    var rows = tbody.SelectNodes(".//tr");

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            var map = OrganizeRow(cells);
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
            return judges;
            
        }
        private static Dictionary<string, Object> OrganizeRow(HtmlAgilityPack.HtmlNodeCollection cells)
        {
            Dictionary<string, Object> map = new();
            string[] court = cells[0].InnerText.Trim().Split("-");
            string[] judge = cells[1].InnerText.Trim().Split(",");


            string temp = "";
            map["courtNum"] = Int32.Parse(court[0].Trim());
            map["courtName"] = "N/A";
            map["courtDiv"] = "";
            if (court[1].Trim() == "CCA")
            {
                map["isCircuit"] = true;
            } else
            {
                if (court.Length == 3)
                {
                    temp = USStateLookup.GetDivision(court[2].Trim()) + " "; 
                }
                map["isCircuit"] = false;
                map["courtName"] = temp + "District of " + USStateLookup.GetStateName(court[1].Trim());
                map["courtNum"] = DistrictCourtLookup.GetDistrictID((string) map["courtName"]);
            }
            map["judgeName"] = judge[1].Trim() + " " + judge[0].Trim();

            return map;

        }
    }

}
