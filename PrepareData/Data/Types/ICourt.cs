
namespace PrepareData.Data.Types
{
    internal interface ICourt
    {
        string Name { get; }
        int? ActiveJudges { get; set; }
        int MaxJudges { get; }
        string? ChiefJudge { get; }
    }
}
