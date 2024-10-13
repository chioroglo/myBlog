using Common.Dto.SemanticAnalysis;
using Common.Options;
using LanguageDetection;
using Microsoft.Extensions.Options;
using ProfanityFilter.Interfaces;
using Service.Abstract;

namespace Service;

public class SemanticAnalysisService : ISemanticAnalysisService
{
    private readonly LanguageDetector _languageDetector; // LangDetect NuGet
    private readonly IProfanityFilter _profanityFilter; // Profanity.Detector
    private readonly SemanticAnalysisOptions _analysisOptions;

    public SemanticAnalysisService(LanguageDetector languageDetector,
        IProfanityFilter profanityFilter,
        IOptions<SemanticAnalysisOptions> options)
    {
        _profanityFilter = profanityFilter;
        _languageDetector = languageDetector;
        _analysisOptions = options.Value;
    }

    public SemanticAnalysisResultDto Analyze(string text)
    {
        var response = new SemanticAnalysisResultDto(text)
        {
            // Language Detection
            Language = _analysisOptions.DetectLanguage ? DetectLanguage(text) : null
        };

        // Profanity Checking
        var profanities = _profanityFilter.DetectAllProfanities(text);
        if (profanities != null && profanities.Any())
        {
            response.DoPunish = true;
            response.PunishmentExplanation = $"Text content contains {profanities.Count} profanities";
            response.CensoredText = _profanityFilter.CensorString(text);
        }

        return response;
    }

    private string? DetectLanguage(string text)
    {
        var predictedLanguage = _languageDetector.Detect(text);
        return predictedLanguage;
    }
}