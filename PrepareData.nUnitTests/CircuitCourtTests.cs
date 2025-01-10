using PrepareData.Data.Types;

namespace PrepareData.nUnitTests
{
    public class CircuitCourtTests
    {
        private CircuitCourt circuitCourt { get; set; } = null!;
        private Judge circuitJudge1 { get; set; } = null!;
        private Judge circuitJudge2 { get; set; } = null!;
        private List<Judge> judges { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            circuitCourt = new CircuitCourt(2, "Test Court", "Arne Slot", 7);
            circuitJudge1 = new Judge("Pedro Gonzales", true, 2, 1976, "Circuit Judge", "Obama", 2014, false);
            circuitJudge2 = new Judge("Pedro Martinez", true, 2, 1966, "Chief Judge", "Trump", 2017, true);
            judges = [circuitJudge1, circuitJudge2];
        }

        [Test]
        public void GetNumberOfVacancies_EqualTest()
        {
            int solution = circuitCourt.MaxJudges;

            int answer = circuitCourt.GetNumberOfVacancies();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetPartisanshipOfCourt_EqualTest()
        {
            int solution = 0;

            circuitCourt.ActiveJudges = 2;
            circuitCourt.SetPartisanshipOfCourt(judges);
            int answer = circuitCourt.FindPartisanshipOfCourt();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetChiefJudge_EqualTest()
        {
            string solution = "Pedro Martinez";

            circuitCourt.SetChiefJudge(judges);

            string answer = circuitCourt.ChiefJudge;

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void SetNumOfSeniorEligibles_EqualTest()
        {
            int solution = 0;

            circuitCourt.SetNumOfSeniorEligibles(judges);

            int answer = circuitCourt.SeniorEligibleJudges;

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void GetNoWhiteSpace_EqualTest()
        {
            string solution = "Second_Circuit";

            string answer = circuitCourt.GetNoWhiteSpace();

            Assert.That(answer, Is.EqualTo(solution));
        }

        [Test]
        public void GetCircuitCourtName_EqualTest()
        {
            string solution = "Second Circuit";

            string answer = circuitCourt.GetCircuitCourtName();

            Assert.That(answer, Is.EqualTo(solution));
        }
    }
}