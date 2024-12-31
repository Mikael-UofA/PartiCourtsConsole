using PrepareData.Data.Services;
using PrepareData.Data.Types;

namespace PrepareGeojson
{
    public partial class Dashboard2 : Form
    {
        private List<DistrictCourt> dcourts = new List<DistrictCourt>();
        private DataAccess dataAccess2 = new DataAccess();

        public Dashboard2()
        {
            InitializeComponent();
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

        private void DCourtsButton_Click(object sender, EventArgs e)
        {
            ChangeStatus(0);
            try
            {
                dcourts = dataAccess2.GetDistrictCourts();
                ChangeStatus(1);

                DCourtsButton.Enabled = false;
                DCourtsButton.BackColor = Color.Chartreuse;
                DCourtsButton.Cursor = Cursors.No;
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

        private void InputGeoButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(GeojsonConfig.geoInputPath))
            {
                MessageBox.Show($"Path is valid");
                ChangeStatus(1);
            }
            else
            {
                MessageBox.Show($"Path is invalid");
                ChangeStatus(-1);
            }
        }

        private void OutputGeoButton_Click(object sender, EventArgs e)
        {
            if (File.Exists(GeojsonConfig.geoOutputPath))
            {
                MessageBox.Show($"Path is valid");
                ChangeStatus(1);
            }
            else
            {
                MessageBox.Show($"Path is invalid");
                ChangeStatus(-1);
            }
        }

        private void CrtGeoButton_Click(object sender, EventArgs e)
        {
            try
            {
                GeojsonConfig.CreateUsableGeojson(dcourts);
                ChangeStatus(1);

                DCourtsButton.Enabled = false;
                DCourtsButton.BackColor = Color.Chartreuse;
                DCourtsButton.Cursor = Cursors.No;
            }
            catch (Exception ex)
            {
                ChangeStatus(-1);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
