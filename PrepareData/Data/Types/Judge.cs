

namespace PrepareData.Data.Types
{
    public class Judge
    {
        private const int SENIOR_INT = 80; // See Rule of 80
        private const int RETIREMENT_AGE = 65;
        private List<string> GOP_LIST = new List<string> { "Reagan", "G.H.W. Bush", "G.W. Bush", "Trump" };
        private List<string> DEM_LIST = new List<string> { "Clinton", "Obama", "Biden" };
        public string Name { get; set; }
        public bool IsCurcuitJudge { get; set; }
        public int Court { get; set; }
        public string Title { get; set; }
        public string AppointedBy { get; set; }
        public int YearOfBirth { get; set; }
        public int AppointmentYear { get; set; }
        public bool IsChief { get; set; }
        public int Partisanship { get; set; }
        public Judge(string name, bool isCurcuit, int court, int yearOfBirth, string title, string appointedBy, int appointmentYear, bool isChief)
        {
            Name = name;
            IsCurcuitJudge = isCurcuit;
            Court = court;
            Title = title;
            AppointedBy = appointedBy;
            YearOfBirth = yearOfBirth;
            AppointmentYear = appointmentYear;
            IsChief = isChief;
            MakePartisanship();
        }

        public int getAge()
        {
            return DateTime.Now.Year - YearOfBirth;
        }
        public int getYearsOfService()
        {
            return DateTime.Now.Year - AppointmentYear;
        }
        public bool eligibleSeniorStatus()
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
        public int whenEligibleForSeniorStatus()
        {
            int ruleOf80 = getAge() + getYearsOfService() >= SENIOR_INT ? 0 : SENIOR_INT - (getAge() + getYearsOfService());
            int addedYears = getAge() >= RETIREMENT_AGE ? 0 : RETIREMENT_AGE - getAge();

            return ruleOf80 + addedYears;

        }
        public void MakePartisanship()
        {
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
                Console.WriteLine(AppointedBy);
                Console.WriteLine(Name);
                Console.WriteLine(Court);
                throw new Exception("Judge was appointed by a president not accounted for");
            }
        }
    }
}
