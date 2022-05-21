namespace Calculation.Nodes;

internal class ArithmeticOperationNode : BaseNode
{
    private readonly OperationType _operation;

    public ArithmeticOperationNode(IExpressionNode left, IExpressionNode right, OperationType operation)
        : base(left, right)
    {
        _operation = operation;
        if(left.IsConst & right.IsConst)
            Execute();
    }

    protected override void Execute()
    {
        Value = _operation switch
        {
            OperationType.Add => _first.Value + _second.Value,
            OperationType.Sub => _first.Value - _second.Value,
            OperationType.Mul => _first.Value * _second.Value,
            OperationType.Div => _first.Value / _second.Value,
            OperationType.Exp => Math.Pow(_first.Value, _second.Value),
            _ => throw new InvalidOperationException("Не поддерживаемый тип операции")
        };
    }
}