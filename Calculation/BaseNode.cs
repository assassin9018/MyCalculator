using System.Collections.Generic;

namespace Calculation;

internal abstract class BaseNode : IExpressionNode
{
    private static readonly IExpressionNode _zeroNode = new ValueNode(0);
    protected readonly IExpressionNode _first;
    protected readonly IExpressionNode _second;

    public double Value { get; protected set; }
    public bool IsConst { get; }

    protected BaseNode(IExpressionNode left) : this(left, _zeroNode)
    { }

    protected BaseNode(IExpressionNode left, IExpressionNode right)
    {
        IsConst = left.IsConst && right.IsConst;
        _first = left;
        _second = right;
    }

    protected abstract void Execute();

    public void Recalculate(Dictionary<string, IExpressionNode> keyValues)
    {
        if(_first.IsConst & _second.IsConst)
            return;

        if(!_first.IsConst)
            _first.Recalculate(keyValues);
        if(!_second.IsConst)
            _second.Recalculate(keyValues);

        Execute();
    }
}
