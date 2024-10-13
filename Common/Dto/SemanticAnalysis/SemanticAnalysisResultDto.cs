namespace Common.Dto.SemanticAnalysis;

public record SemanticAnalysisResultDto()
{
    public SemanticAnalysisResultDto(string initialText) : this()
    {
        InitialText = initialText;
    }

    public string? InitialText { get; init; }
    public string? CensoredText { get; set; }
    public bool DoPunish { get; set; }
    public string? PunishmentExplanation { get; set; }
    public string? Language { get; set; }
}