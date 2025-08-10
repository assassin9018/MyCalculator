namespace Calculation.Smart.Nodes;

public class ValueNode(double value) : IExpressionNode
{
    public double Value { get; } = value;

    public bool IsConst => true;

    public void Recalculate(Dictionary<string, IExpressionNode> keyValues)
    {
        //nothing to do
    }
}
