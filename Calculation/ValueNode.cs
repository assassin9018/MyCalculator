using System.Collections.Generic;

namespace Calculation;

internal class ValueNode : IExpressionNode
{
    public double Value { get;}

    public bool IsConst => true;

    public ValueNode(double value)
    {
        Value = value;
    }

    public void Recalculate(Dictionary<string, double> keyValues)
    {
        //nothing to do
    }
}
