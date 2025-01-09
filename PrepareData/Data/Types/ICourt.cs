
namespace PrepareData.Data.Types
{
    /// <summary>
    /// Interface representing the structure of a court, defining required properties and methods for implementation.
    /// </summary>
    internal interface ICourt
    {
        int Id { get; set; }
        string Name { get; }
        int ActiveJudges { get; set; }
        int MaxJudges { get; }
        string ChiefJudge { get; set; }
        int SeniorEligibleJudges { get; set; }
        int DEMJudges { get; set; }
        int GOPJudges { get; set; }

        /// <summary>
        /// Calculates and returns the number of vacant judge positions in the court.
        /// </summary>
        /// <returns>The number of vacancies in the court.</returns>
        int GetNumberOfVacancies();

        /// <summary>
        /// Sets the partisanship (Democratic and Republican) of the court based on the given list of judges.
        /// </summary>
        /// <param name="judges">A list of judges to determine partisanship.</param>
        void SetPartisanshipOfCourt(List<Judge> judges);

        /// <summary>
        /// Determines the overall partisanship of the court based on the number of Democratic and Republican judges.
        /// </summary>
        /// <returns>
        /// 1 if the court is Democratic, -1 if Republican, or 0 if evenly split.
        /// </returns>
        int FindPartisanshipOfCourt();

        /// <summary>
        /// Finds and sets the chief judge from a list of judges.
        /// </summary>
        /// <param name="judges">A list of judges from which to identify the chief judge.</param>
        void FindChiefJudge(List<Judge> judges);

        /// <summary>
        /// Finds and sets the number of senior-eligible judges from the given list of judges.
        /// </summary>
        /// <param name="judges">A list of judges to check for senior eligibility.</param>
        void FindNumOfSeniorEligibles(List<Judge> judges);

        /// <summary>
        /// Returns the name of the court with all spaces replaced by underscores.
        /// </summary>
        /// <returns>The name of the court with no white spaces.</returns>
        string GetNoWhiteSpace();
    }
}
