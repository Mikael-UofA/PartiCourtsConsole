
namespace PrepareData.Data.Types
{
    public class CurcuitCourt: ICourt
    {
        public int Id { get; }
        public string Name { get; }
        public string SupervisingJustice { get; }
        public int? ActiveJudges { get; set; }
        public int MaxJudges { get; }
        public string? ChiefJudge { get; set; }
        public int? SeniorEligibleJudges { get; set; }
        public int? DEMJudges { get; set; }
        public int? GOPJudges { get; set; }

        public CurcuitCourt(int id, string name, string supervisingJustice, int maxJudges)
        {
            Id = id;
            Name = name;
            SupervisingJustice = supervisingJustice;
            MaxJudges = maxJudges;
        }

        public CurcuitCourt(int id, string name, string supervisingJustice, string chiefJudge, int activeJudges, int maxJudges, int seniorEligibleJudges, int dem, int gop)
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

        public int GetNumberofVacancies()
        {
            if (ActiveJudges != null)
            {
                return (int)(MaxJudges - ActiveJudges);
            }
            return MaxJudges;
        }

        public void FindPartisanshipOfCourt(List<Judge> judges)
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
