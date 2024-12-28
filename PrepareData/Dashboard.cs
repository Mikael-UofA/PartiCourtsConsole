using PrepareData.Data.Services;
using PrepareData.Data.Types;
using System.IO;
using System.Timers;

namespace PrepareData
{
    public partial class Dashboard : Form
    {
        private List<CurcuitCourt> ccourts = new List<CurcuitCourt>();
        private List<DistrictCourt> dcourts  = new List<DistrictCourt>();
        private List<Judge> judges = new List<Judge>();
        private string geojsonPath = "sources/boundaries.geojson";
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void DisableEnableButton(Button buttonClicked, Button? nextButton)
        {
            buttonClicked.Enabled = false;
            buttonClicked.BackColor = Color.Chartreuse;
            buttonClicked.Cursor = Cursors.No;
            if (nextButton != null)
            {
                nextButton.Enabled = true;
                nextButton.BackColor = Color.IndianRed;
                nextButton.Cursor = Cursors.Default;
            }
        }
        private async void CCourtsButton_Click(object sender, EventArgs e)
        {
            ccourts = await WikipediaScrapper.GenerateCC();
            DisableEnableButton(CCourtsButton, DCourtsButton);
        }
        private async void DCourtsButton_Click(object sender, EventArgs e)
        {
            dcourts = await WikipediaScrapper.GenerateDC();

            List<int> remove = new List<int> { 74, 61, 86 };
            for (int i = 0; i < dcourts.Count; i++)
            {
                if (!dcourts[i].SetIdFromGeoJson(geojsonPath))
                {
                    Console.WriteLine("A failure occurred when retrieving ID of some district court");
                    break;
                }
                if (remove.Contains(dcourts[i].Id))
                {
                    dcourts.RemoveAt(i);
                }
            }
            DisableEnableButton(DCourtsButton, JudgesButton);
        }

        private async void JudgesButton_Click(object sender, EventArgs e)
        {
            foreach (CurcuitCourt court in ccourts)
            {
                List<Judge> returning = await WikipediaScrapper.GetCJudges(court);
                judges.AddRange(returning);
            }
            foreach (DistrictCourt court in dcourts)
            {
                List<Judge> returning = await WikipediaScrapper.GetDJudges(court);
                judges.AddRange(returning);
            }
            DisableEnableButton(JudgesButton, StoreDBButton);
        }

        private void StoreDBButton_Click(object sender, EventArgs e)
        {
            DisableEnableButton(StoreDBButton, null);
        }

        private void TestConnButton_Click(object sender, EventArgs e)
        {
            Helper.DemoDB();
            TestConnButton.Text = "OK";
        }

        private void DemoIntButton_Click(object sender, EventArgs e)
        {
            Helper.InsertDB();
        }

        private void DemoDelButton_Click(object sender, EventArgs e)
        {
            Judge test = new Judge("Demo", true, 3, 1979, "Chief Judge", "Obama", 2014, true);
            DataAccess dataAccess = new DataAccess();
            dataAccess.DeleteJudge(test);
        }

        private async void DropTablesButton_Click(object sender, EventArgs e)
        {
            //DataAccess dataAccess = new DataAccess();
            //dataAccess.ClearTable("Judges");
            //dataAccess.ClearTable("DCourts");
            //dataAccess.ClearTable("CCourts");

            ChangeStatus(0);
            await Task.Delay(2000);
            ChangeStatus(1);

        }
        private void ChangeStatus(int statusCode)
        {
            string text = "Status: ";
            string code = string.Empty;
            switch (statusCode)
            {
                case 1:
                    code = "Success!";
                    break;
                case -1:
                    code = "Error";
                    break;
                default:
                    code = "Processing...";
                    break;
            }
            StatusBox.Text = text + code;
        }

    }
}
