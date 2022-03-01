using System;

namespace Calculation;

internal class OneArgFunctionNode : BaseNode
{
    private readonly OneArgFunctionType _function;

    public OneArgFunctionNode(IExpressionNode arg, OneArgFunctionType function)
        : base(arg)
    {
        _function = function;
        if(arg.IsConst)
            Execute();
    }

    protected override void Execute()
    {
        const double DegriesPerRad = 57.2958;
        Value = _function switch
        {
            OneArgFunctionType.Abs => Math.Abs(_first.Value),
            OneArgFunctionType.Sin => Math.Sin(_first.Value / DegriesPerRad),
            OneArgFunctionType.Cos => Math.Cos(_first.Value / DegriesPerRad),
            OneArgFunctionType.Tan => Math.Tan(_first.Value / DegriesPerRad),
            OneArgFunctionType.Int => Math.Truncate(_first.Value),
            OneArgFunctionType.Rnd => Math.Round(_first.Value),
            OneArgFunctionType.Exp => Math.Exp(_first.Value),
            OneArgFunctionType.Ln => Math.Log10(_first.Value),
            OneArgFunctionType.Sqr => Math.Pow(_first.Value, 2),
            OneArgFunctionType.Sqrt => Math.Sqrt(_first.Value),
            _ => throw new InvalidOperationException("Не поддерживаемая функция")
        };
    }
}
