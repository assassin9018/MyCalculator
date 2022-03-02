using System.Collections.Generic;

namespace Calculation;

public class ValueNode : IExpressionNode
{
    public double Value { get;}

    public bool IsConst => true;

    public ValueNode(double value)
    {
        Value = value;
    }

    public void Recalculate(Dictionary<string, IExpressionNode> keyValues)
    {
        //nothing to do
    }
}
