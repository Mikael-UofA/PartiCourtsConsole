
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
                            judges.Add(OrganizeRow(cells));
                        }
                        return judges;
                    }
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

            map["courtNum"] = court[0];
            map["isCircuit"] = court[1] == "CCA" ? true : false;
            map["courtName"] = court[1];
            if (court.Length == 3)
            {
                map["courtDiv"] = court[2];
            }
            map["judgeName"] = judge[1] + " " + judge[0];

            return map;

        }
    }

}
