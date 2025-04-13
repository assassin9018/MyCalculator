namespace Calculation.Smart.Nodes;

public class VariableNode : IExpressionNode
{
    private readonly string _name;
    public double Value { get; private set; }

    public bool IsConst => false;

    public VariableNode(string name)
    {
        _name = name;
    }

    public void Recalculate(Dictionary<string, IExpressionNode> keyValues)
    {
        if(!keyValues.TryGetValue(_name, out var node))
            throw new ArgumentException($"Variable {_name} not found.");

        if(!node.IsConst)
            node.Recalculate(keyValues);

        Value = node.Value;
    }
}
