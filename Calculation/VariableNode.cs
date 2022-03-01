using System;
using System.Collections.Generic;

namespace Calculation;
internal class VariableNode : IExpressionNode
{
    private readonly string _name;
    public double Value { get; private set; }

    public bool IsConst => false;

    public VariableNode(string name)
    {
        _name = name;
    }

    public void Recalculate(Dictionary<string, double> keyValues)
    {
        if(!keyValues.TryGetValue(_name, out double value))
            throw new ArgumentException($"Variable {_name} not found.");
        Value = value;
    }
}
