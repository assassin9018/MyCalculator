namespace Calculation.Nodes;

public interface IExpressionNode
{
    double Value { get; }
    bool IsConst { get; }
    void Recalculate(Dictionary<string, IExpressionNode> keyValues);
}
