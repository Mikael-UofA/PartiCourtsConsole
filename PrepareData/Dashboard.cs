using PrepareData.Data.Services;
using PrepareData.Data.Types;

namespace PrepareData
{
    public partial class Dashboard : Form
    {
        private List<CircuitCourt> ccourts = new List<CircuitCourt>();
        private List<DistrictCourt> dcourts = new List<DistrictCourt>();
        private List<Judge> judges = new List<Judge>();
        private string geojsonDCPath = "../../../sources/dc_boundaries.geojson";

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
            ChangeStatus(0);
            try
            {
                ccourts = await WikipediaScrapper.GenerateCC();
                DisableEnableButton(CCourtsButton, DCourtsButton);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private async void DCourtsButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                dcourts = await WikipediaScrapper.GenerateDC();
                List<DistrictCourt> remove = new();
                foreach (DistrictCourt dcourt in dcourts)
                {
                    dcourt.SetIdFromGeoJson(geojsonDCPath);
                }
                DisableEnableButton(DCourtsButton, JudgesButton);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private async void JudgesButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                foreach (CircuitCourt court in ccourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetCJudges(court);
                    court.ActiveJudges = returning.Count;
                    court.SetPartisanshipOfCourt(returning);
                    court.SetChiefJudge(returning);
                    court.SetNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }
                foreach (DistrictCourt court in dcourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetDJudges(court);
                    court.ActiveJudges = returning.Count;
                    court.SetPartisanshipOfCourt(returning);
                    court.SetNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }
                DisableEnableButton(JudgesButton, StoreDBButton);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }

        }
        private async void RetirementsButton_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeStatus(0);
                List<Dictionary<string, Object>> retiringJudges = await GovScrapper.FetchRetiringJudges();
                DataAccess.UpdateRetiringJudges(retiringJudges);
                ccourts = DataAccess.GetCircuitCourts();
                dcourts = DataAccess.GetDistrictCourts();

                foreach (CircuitCourt court in ccourts)
                {
                    List<Judge> returning = DataAccess.GetCircuitJudges(court.Id);
                    court.AddToRetiring(returning);
                    DataAccess.UpdateCourtRetirements(court);
                }
                foreach (DistrictCourt court in dcourts)
                {
                    List<Judge> returning = DataAccess.GetDistrictJudges(court.Id);
                    court.AddToRetiring(returning);
                    DataAccess.UpdateCourtRetirements(court);
                }
                ChangeStatus(1);

            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private void StoreDBButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                DataAccess.InsertCircuitCourts(ccourts);
                DataAccess.InsertDistrictCourts(dcourts);
                DataAccess.InsertJudges(judges);
                DisableEnableButton(StoreDBButton, null);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private void TestConnButton_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.DemoDB();
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }

        }

        private void DemoIntButton_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.InsertDB();
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private void DemoDelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.DeleteDB();
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }

        private void DropTablesButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            DataAccess.ClearTable("Judges");
            DataAccess.ClearTable("DCourts");
            DataAccess.ClearTable("CCourts");
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

        private async void UpdateTabsButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            ccourts = DataAccess.GetCircuitCourts();
            dcourts = DataAccess.GetDistrictCourts();
            judges = new List<Judge>();
            try
            {
                foreach (CircuitCourt curcuit in ccourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetCJudges(curcuit);
                    curcuit.ActiveJudges = returning.Count;
                    curcuit.SetPartisanshipOfCourt(returning);
                    curcuit.SetChiefJudge(returning);
                    curcuit.SetNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);

                }
                foreach (DistrictCourt district in dcourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetDJudges(district);
                    district.ActiveJudges = returning.Count;
                    district.SetPartisanshipOfCourt(returning);
                    district.SetChiefJudge(returning);
                    district.SetNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }

                DataAccess.ClearTable("Judges");
                DataAccess.InsertJudges(judges);
                DataAccess.UpdateCircuitCourts(ccourts);
                DataAccess.UpdateDistrictCourts(dcourts);
                ChangeStatus(1);
            }
            catch
            {
                ChangeStatus(-1);
            }



        }

    }
}
