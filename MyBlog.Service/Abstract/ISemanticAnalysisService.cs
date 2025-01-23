using MyBlog.Common.Dto.SemanticAnalysis;

namespace MyBlog.Service.Abstract;

public interface ISemanticAnalysisService
{
    SemanticAnalysisResultDto Analyze(string text);
}