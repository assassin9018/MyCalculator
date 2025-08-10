namespace Calculation.Smart.Nodes;

public class VariableNode(string name) : IExpressionNode
{
    public double Value { get; private set; }

    public bool IsConst => false;

    public void Recalculate(Dictionary<string, IExpressionNode> keyValues)
    {
        if(!keyValues.TryGetValue(name, out var node))
            throw new ArgumentException($"Variable {name} not found.");

        if(!node.IsConst)
            node.Recalculate(keyValues);

        Value = node.Value;
    }
}
