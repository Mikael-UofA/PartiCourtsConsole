using PrepareData.Data.Types;

namespace PrepareData.nUnitTests
{
    public class JudgeTests
    {
        private Judge Judge { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            Judge = new Judge("Test Dummy", true, 1, 1942, "Chief Judge", "Reagan", 1982, true);
        }

        [TestCase(1990, 2015, false)]
        [TestCase(1944, 2024, true)]
        [TestCase(1942, 1982, true)]
        public void IsEligibleSeniorStatus_EqualTest(int birth, int appointYear, bool solution)
        {
            Judge.YearOfBirth = birth;
            Judge.AppointmentYear = appointYear;
            bool answer = Judge.IsEligibleSeniorStatus();

            Assert.That(answer, Is.EqualTo(solution));


        }

        [Test]
        public void MakePartisanship_EqualTest()
        {
            int solution = -1;

            Assert.That(Judge.Partisanship, Is.EqualTo(solution));
        }
    }
}
