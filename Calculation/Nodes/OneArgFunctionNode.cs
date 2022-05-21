namespace Calculation.Nodes;

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
            OneArgFunctionType.abs => Math.Abs(_first.Value),
            OneArgFunctionType.sin => Math.Sin(_first.Value / DegriesPerRad),
            OneArgFunctionType.cos => Math.Cos(_first.Value / DegriesPerRad),
            OneArgFunctionType.tan => Math.Tan(_first.Value / DegriesPerRad),
            OneArgFunctionType.intg => Math.Truncate(_first.Value),
            OneArgFunctionType.rnd => Math.Round(_first.Value),
            OneArgFunctionType.exp => Math.Exp(_first.Value),
            OneArgFunctionType.ln => Math.Log2(_first.Value),
            OneArgFunctionType.log => Math.Log10(_first.Value),
            OneArgFunctionType.sqr => Math.Pow(_first.Value, 2),
            OneArgFunctionType.sqrt => Math.Sqrt(_first.Value),
            _ => throw new InvalidOperationException("Не поддерживаемая функция")
        };
    }
}
