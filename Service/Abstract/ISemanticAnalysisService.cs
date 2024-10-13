using Common.Dto.SemanticAnalysis;

namespace Service.Abstract;

public interface ISemanticAnalysisService
{
    SemanticAnalysisResultDto Analyze(string text);
}