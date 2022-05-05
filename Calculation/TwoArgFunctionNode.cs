using System;

namespace Calculation;

internal class TwoArgFunctionNode : BaseNode
{
    private const string DecimalsCountIsFloat = "Decimals count value can not be floating point value.";
    private readonly TwoArgFunctionType _function;

    public TwoArgFunctionNode(IExpressionNode first, IExpressionNode second, TwoArgFunctionType function)
        : base(first, second)
    {
        _function = function;
        if(first.IsConst & second.IsConst)
            Execute();
    }

    protected override void Execute()
    {
        Value = _function switch
        {
            TwoArgFunctionType.min => Math.Min(_first.Value, _second.Value),
            TwoArgFunctionType.max => Math.Max(_first.Value, _second.Value),
            TwoArgFunctionType.pow => Math.Pow(_first.Value, _second.Value),
            TwoArgFunctionType.logx => Math.Log(_first.Value, _second.Value),
            TwoArgFunctionType.rndx => Math.Round(_first.Value, (int)_second.Value == _second.Value ? (int)_second.Value : throw new ArgumentException(DecimalsCountIsFloat)),
            _ => throw new InvalidOperationException("Не поддерживаемая функция")
        };
    }
}