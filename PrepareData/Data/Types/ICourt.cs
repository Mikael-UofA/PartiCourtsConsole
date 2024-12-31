
namespace PrepareData.Data.Types
{
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

        int GetNumberOfVacancies();
        void SetPartisanshipOfCourt(List<Judge> judges);
        string GetNoWhiteSpace();

    }
}
