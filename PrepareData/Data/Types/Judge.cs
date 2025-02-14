

namespace PrepareData.Data.Types
{
    /// <summary>
    /// Represents a judge and their details, including their partisanship, eligibility for senior status, and years of service.
    /// </summary>
    public class Judge
    {
        private const int SENIOR_INT = 80; // See Rule of 80
        private const int RETIREMENT_AGE = 65;
        private List<string> GOP_LIST = new List<string> { "Reagan", "G.H.W. Bush", "G.W. Bush", "Trump" };
        private List<string> DEM_LIST = new List<string> { "Clinton", "Obama", "Biden" };

        public string Name { get; set; }
        public bool IsCircuitJudge { get; set; } // Determines whether or not this is a circuit judge
        public int Court { get; set; }
        public string Title { get; set; }
        public string AppointedBy { get; set; }
        public int YearOfBirth { get; set; }
        public int AppointmentYear { get; set; }
        public bool IsChief { get; set; }
        public int Partisanship { get; set; }
        public bool IsRetiring { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Judge"/> class with default values.
        /// </summary>
        public Judge() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Judge"/> class with specified properties.
        /// </summary>
        /// <param name="name">The name of the judge.</param>
        /// <param name="isCircuit">Indicates whether the judge is a circuit judge.</param>
        /// <param name="court">The court the judge belongs to.</param>
        /// <param name="yearOfBirth">The birth year of the judge.</param>
        /// <param name="title">The title of the judge.</param>
        /// <param name="appointedBy">The president who appointed the judge.</param>
        /// <param name="appointmentYear">The year the judge was appointed.</param>
        /// <param name="isChief">Indicates whether the judge is the chief judge.</param>
        public Judge(string name, bool isCircuit, int court, int yearOfBirth, string title, string appointedBy, int appointmentYear, bool isChief, bool isRetiring)
        {
            Name = name;
            IsCircuitJudge = isCircuit;
            Court = court;
            Title = title;
            AppointedBy = appointedBy;
            YearOfBirth = yearOfBirth;
            AppointmentYear = appointmentYear;
            IsChief = isChief;
            MakePartisanship();
            IsRetiring = isRetiring;
        }

        /// <summary>
        /// Gets the current age of the judge.
        /// </summary>
        /// <returns>The judge's current age.</returns>
        public int getAge()
        {
            return DateTime.Now.Year - YearOfBirth;
        }

        /// <summary>
        /// Gets the number of years the judge has served since their appointment.
        /// </summary>
        /// <returns>The number of years the judge has served.</returns>
        public int getYearsOfService()
        {
            return DateTime.Now.Year - AppointmentYear;
        }

        /// <summary>
        /// Determines whether the judge is eligible for senior status based on the Rule of 80 and the retirement age.
        /// </summary>
        /// <returns>True if the judge is eligible for senior status; otherwise, false.</returns>
        public bool IsEligibleSeniorStatus()
        {
            if (getAge() >= RETIREMENT_AGE)
            {
                if (getAge() + getYearsOfService() >= SENIOR_INT)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Calculates when the judge will be eligible for senior status based on their age and years of service.
        /// </summary>
        /// <returns>The number of years until the judge is eligible for senior status.</returns>
        public int whenEligibleForSeniorStatus()
        {
            int ruleOf80 = getAge() + getYearsOfService() >= SENIOR_INT ? 0 : SENIOR_INT - (getAge() + getYearsOfService());
            int addedYears = getAge() >= RETIREMENT_AGE ? 0 : RETIREMENT_AGE - getAge();

            return ruleOf80 + addedYears;
        }

        /// <summary>
        /// Determines the judge's partisanship based on the president who appointed them.
        /// </summary>
        /// <exception cref="Exception">Thrown when the president who appointed the judge is not recognized.</exception>
        public void MakePartisanship()
        {
            // Handles the case where two different presidents appointed the judge
            if (AppointedBy.Contains('/'))
            {
                AppointedBy = AppointedBy.Split('/')[0].Trim();
            }

            if (DEM_LIST.Contains(AppointedBy))
            {
                Partisanship = 1;
            }
            else if (GOP_LIST.Contains(AppointedBy))
            {
                Partisanship = -1;
            }
            else
            {
                MessageBox.Show(AppointedBy);
                MessageBox.Show(Name);
                MessageBox.Show(Court.ToString());
                throw new Exception("Judge was appointed by a president not accounted for");
            }
        }
    }
}
