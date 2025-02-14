
namespace PrepareData.Data.Types
{
    /// <summary>
    /// Represents a circuit court and its properties, including the number of judges, partisanship, and senior eligible judges.
    /// Implements the <see cref="ICourt"/> interface.
    /// </summary>
    public class CircuitCourt : ICourt
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
        public int DEMRetiring { get; set; }
        public int GOPRetiring { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitCourt"/> class with default values.
        /// </summary>
        public CircuitCourt() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitCourt"/> class with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier for the court.</param>
        /// <param name="name">The name of the court.</param>
        /// <param name="supervisingJustice">The supervising justice for the court.</param>
        /// <param name="maxJudges">The maximum number of judges in the court.</param>
        public CircuitCourt(int id, string name, string supervisingJustice, int maxJudges)
        {
            Id = id;
            Name = name;
            ChiefJudge = "";
            ActiveJudges = 0;
            DEMJudges = 0;
            GOPJudges = 0;
            SeniorEligibleJudges = 0;
            SupervisingJustice = supervisingJustice;
            MaxJudges = maxJudges;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircuitCourt"/> class with all details about the court.
        /// </summary>
        /// <param name="id">The unique identifier for the court.</param>
        /// <param name="name">The name of the court.</param>
        /// <param name="supervisingJustice">The supervising justice for the court.</param>
        /// <param name="chiefJudge">The name of the chief judge.</param>
        /// <param name="activeJudges">The number of active judges in the court.</param>
        /// <param name="maxJudges">The maximum number of judges in the court.</param>
        /// <param name="seniorEligibleJudges">The number of senior-eligible judges.</param>
        /// <param name="dem">The number of Democratic judges.</param>
        /// <param name="gop">The number of Republican judges.</param>
        public CircuitCourt(int id, string name, string supervisingJustice, string chiefJudge, int activeJudges, int maxJudges, int seniorEligibleJudges, int dem, int gop, int demRetiring, int gopRetiring)
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
            DEMRetiring = demRetiring;
            GOPRetiring = gopRetiring;
        }

        public int GetNumberOfVacancies() => MaxJudges - ActiveJudges;

        public void SetPartisanshipOfCourt(List<Judge> judges)
        {
            DEMJudges = judges.Count(judge => judge.Partisanship == 1);
            GOPJudges = ActiveJudges - DEMJudges; // The remaining judges are assumed to be from the GOP
        }

        public int FindPartisanshipOfCourt()
        {
            if (DEMJudges > GOPJudges)
            {
                return 1; // Court leans Democratic
            }
            else if (DEMJudges < GOPJudges)
            {
                return -1; // Court leans Republican
            }
            return 0; // Court is evenly split or nonpartisan
        }

        public void SetChiefJudge(List<Judge> judges)
        {
            ChiefJudge = judges.FirstOrDefault(judge => judge.IsChief)?.Name;
        }

        public void SetNumOfSeniorEligibles(List<Judge> judges)
        {
            SeniorEligibleJudges = judges.Count(judge => judge.IsEligibleSeniorStatus());
        }

        public string GetNoWhiteSpace()
        {
            return GetCircuitCourtName().Replace(" ", "_");
        }

        public void AddToRetiring(Judge judge)
        {
            if (judge.Partisanship == 1)
            {
                DEMRetiring += 1;
            }
            else
            {
                GOPRetiring += 1;
            }
        }

        /// <summary>
        /// Gets the name of the circuit court based on its ID.
        /// </summary>
        /// <returns>The name of the circuit court corresponding to the ID.</returns>
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