using System.Collections.Generic;

namespace Calculation;

internal interface IExpressionNode
{
    double Value { get; }
    bool IsConst { get; }
    void Recalculate(Dictionary<string, double> keyValues);
}
