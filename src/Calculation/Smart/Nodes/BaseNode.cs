namespace Calculation.Smart.Nodes;

internal abstract class BaseNode(IExpressionNode left, IExpressionNode right) : IExpressionNode
{
    private static readonly IExpressionNode _zeroNode = new ValueNode(0);
    protected readonly IExpressionNode _first = left;
    protected readonly IExpressionNode _second = right;

    public double Value { get; protected set; }
    public bool IsConst { get; } = left.IsConst && right.IsConst;

    protected BaseNode(IExpressionNode left) : this(left, _zeroNode)
    { }

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
