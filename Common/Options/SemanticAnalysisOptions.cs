namespace Common.Options;

public class SemanticAnalysisOptions : BaseApplicationOptions
{
    public new static string Config => "SemanticAnalysisOptions";
    public bool CheckProfanity { get; set; }
    public bool DetectLanguage { get; set; }
}