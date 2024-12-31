
namespace PrepareData.Data.Types
{
    public class CircuitCourt: ICourt
    {
        public int Id { get; set; }
        public string Name { get; }
        public string SupervisingJustice { get; }
        public int ActiveJudges { get; set; }
        public int MaxJudges { get; }
        public string ChiefJudge { get; set; }
        public int SeniorEligibleJudges { get; set; }
        public int DEMJudges { get; set; }
        public int GOPJudges { get; set; }

        public CircuitCourt(int id, string name, string supervisingJustice, int maxJudges)
        {
            Id = id;
            Name = name;
            ChiefJudge = "";
            ActiveJudges = 0;
            SupervisingJustice = supervisingJustice;
            MaxJudges = maxJudges;
        }

        public CircuitCourt(int id, string name, string supervisingJustice, string chiefJudge, int activeJudges, int maxJudges, int seniorEligibleJudges, int dem, int gop)
        {
            Id = id;
            Name = name;
            SupervisingJustice = supervisingJustice;
            ActiveJudges = activeJudges;
            MaxJudges = maxJudges;
            ChiefJudge = chiefJudge;
            SeniorEligibleJudges = seniorEligibleJudges;
            DEMJudges = dem;
            GOPJudges = gop;
        }

        public int GetNumberOfVacancies()
        {
            if (ActiveJudges != 0)
            {
                return (int)(MaxJudges - ActiveJudges);
            }
            return MaxJudges;
        }

        public void SetPartisanshipOfCourt(List<Judge> judges)
        {
            DEMJudges = 0;
            foreach (Judge judge in judges)
            {
                if (judge.Partisanship == 1)
                {
                    DEMJudges++;
                }
            }
            GOPJudges = ActiveJudges - DEMJudges;
        }

        public void FindChiefJudge(List<Judge> judges)
        {
            foreach (Judge judge in judges)
            {
                if (judge.IsChief)
                {
                    ChiefJudge = judge.Name;
                    break;
                }
            }
        }

        public void FindNumOfSeniorEligibles(List<Judge> judges)
        {
            SeniorEligibleJudges = 0;
            foreach(Judge judge in judges)
            {
                if (judge.IsEligibleSeniorStatus())
                {
                    SeniorEligibleJudges++;
                }
            }
        }

        public string GetNoWhiteSpace()
        {
            return GetCircuitCourtName().Replace(" ", "_");
        }

        public string GetCircuitCourtName()
        {
            string[] courtNames =
            {
                "District of Columbia Circuit",
                "First Circuit",
                "Second Circuit",
                "Third Circuit",
                "Fourth Circuit",
                "Fifth Circuit",
                "Sixth Circuit",
                "Seventh Circuit",
                "Eighth Circuit",
                "Ninth Circuit",
                "Tenth Circuit",
                "Eleventh Circuit"
            };

            return courtNames[Id];

        }
    }
}
