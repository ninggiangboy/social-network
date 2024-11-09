using Application.Abstracts;
using Syncfusion.PMML;

namespace Application.Services;

public class PmmlModelService
{
    private readonly PMMLEvaluator _evaluator;

    public PmmlModelService(string pmmlFilePath)
    {
        using var fileStream = new FileStream(pmmlFilePath, FileMode.Open, FileAccess.Read);
        var pmmlDocument = new PMMLDocument(fileStream);
        _evaluator = new PMMLEvaluatorFactory().GetPMMLEvaluatorInstance(pmmlDocument);
    }

    public Dictionary<string, object> Predict(Dictionary<string, object> inputData)
    {
        var result = _evaluator.GetResult(inputData, null);

        return (Dictionary<string, object>)result.PredictedValue;
    }
}