using PrepareData.Data.Types;

namespace PrepareData.nUnitTests
{
    public class DistrictCourtTests
    {
        private DistrictCourt DistrictCourt { get; set; } = null!;
        private Judge DistrictJudge1 { get; set; } = null!;
        private Judge DistrictJudge2 { get; set; } = null!;
        private List<Judge> Judges { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            DistrictCourt = new DistrictCourt("Northern District of Alabama", "N.D. Ala.", "11th", 8, "R. David Proctor");
            DistrictJudge1 = new Judge("Pedro Gonzales", false, 2, 1976, "District Judge", "Obama", 2014, false);
            DistrictJudge2 = new Judge("Pedro Martinez", false, 2, 1966, "Chief Judge", "Trump", 2017, true);
            Judges = [DistrictJudge1, DistrictJudge2];
        }

        [Test]
        public void GetNumberOfVacancies_EqualTest()
        {
            int solution = DistrictCourt.MaxJudges;

            int answer = DistrictCourt.GetNumberOfVacancies();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetPartisanshipOfCourt_EqualTest()
        {
            int solution = 0;

            DistrictCourt.ActiveJudges = 2;
            DistrictCourt.SetPartisanshipOfCourt(Judges);
            int answer = DistrictCourt.FindPartisanshipOfCourt();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetChiefJudge_EqualTest()
        {
            string solution = "Pedro Martinez";

            DistrictCourt.SetChiefJudge(Judges);

            string answer = DistrictCourt.ChiefJudge;

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetNumOfSeniorEligibles_EqualTest()
        {
            int solution = 0;

            DistrictCourt.SetNumOfSeniorEligibles(Judges);

            int answer = DistrictCourt.SeniorEligibleJudges;

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void GetNoWhiteSpace_EqualTest()
        {
            string solution = "Northern_District_of_Alabama";

            string answer = DistrictCourt.GetNoWhiteSpace();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [TestCase("3rd", 3)]
        [TestCase("10th", 10)]
        [TestCase("1st", 1)]
        [TestCase("D.C.", 0)]
        public void MakeAppeal_EqualTest(string appeal, int solution)
        {
            DistrictCourt.MakeAppeal(appeal);
            int answer = DistrictCourt.CourtOfAppeal;

            Assert.That(answer, Is.EqualTo(solution));
        }
    }
}
