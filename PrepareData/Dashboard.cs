using PrepareData.Data.Services;
using PrepareData.Data.Types;

namespace PrepareData
{
    public partial class Dashboard : Form
    {
        private List<CurcuitCourt> ccourts = new List<CurcuitCourt>();
        private List<DistrictCourt> dcourts = new List<DistrictCourt>();
        private List<Judge> judges = new List<Judge>();
        private string geojsonPath = "sources/boundaries.geojson";
        private DataAccess dataAccess1 = new DataAccess();

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
                Console.WriteLine(ex.Message);
            }
        }

        private async void DCourtsButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
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
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                Console.WriteLine(ex.Message);
            }
        }

        private async void JudgesButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                foreach (CurcuitCourt court in ccourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetCJudges(court);
                    court.ActiveJudges = returning.Count;
                    court.FindPartisanshipOfCourt(returning);
                    court.FindChiefJudge(returning);
                    court.FindNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }
                foreach (DistrictCourt court in dcourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetDJudges(court);
                    court.ActiveJudges = returning.Count;
                    court.FindPartisanshipOfCourt(returning);
                    court.FindNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }
                DisableEnableButton(JudgesButton, StoreDBButton);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                Console.WriteLine(ex.Message);
            }
            
        }

        private void StoreDBButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                dataAccess1.InsertCurcuitCourts(ccourts);
                dataAccess1.InsertDistrictCourts(dcourts);
                dataAccess1.InsertJudges(judges);
                DisableEnableButton(StoreDBButton, null);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
            }
        }

        private void DemoDelButton_Click(object sender, EventArgs e)
        {
            try
            {
                Judge test = new Judge("Demo", true, 3, 1979, "Chief Judge", "Obama", 2014, true);
                DataAccess dataAccess = new DataAccess();
                dataAccess.DeleteJudge(test);
                ChangeStatus(1);
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                Console.WriteLine(ex.Message);
            }
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

        private async void UpdateTabsButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            ccourts = dataAccess1.GetCurcuitCourts();
            dcourts = dataAccess1.GetDistrictCourts();
            judges = new List<Judge>();
            try 
            {
                foreach (CurcuitCourt curcuit in ccourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetCJudges(curcuit);
                    curcuit.ActiveJudges = returning.Count;
                    curcuit.FindPartisanshipOfCourt(returning);
                    curcuit.FindChiefJudge(returning);
                    curcuit.FindNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);

                }
                foreach (DistrictCourt district in dcourts)
                {
                    List<Judge> returning = await WikipediaScrapper.GetDJudges(district);
                    district.ActiveJudges = returning.Count;
                    district.FindPartisanshipOfCourt(returning);
                    district.FindChiefJudge(returning);
                    district.FindNumOfSeniorEligibles(returning);
                    judges.AddRange(returning);
                }

                dataAccess1.ClearTable("Judges");
                dataAccess1.InsertJudges(judges);
                dataAccess1.UpdateCurcuitCourts(ccourts);
                dataAccess1.UpdateDistrictCourts(dcourts);
                ChangeStatus(1);
            }
            catch
            {
                ChangeStatus(-1);
            }
                
                

        }
    }
}
