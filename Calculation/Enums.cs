using NetEscapades.EnumGenerators;

namespace Calculation;

[EnumExtensions]
internal enum OperationType
{
    None,
    Exp,
    Mul,
    Div,
    Add,
    Sub,
}

[EnumExtensions]
internal enum OneArgFunctionType
{
    abs,
    sin,
    cos,
    tan,
    intg,
    rnd,
    exp,
    ln,
    log,
    sqr,
    sqrt,
}

[EnumExtensions]
internal enum TwoArgFunctionType
{
    min,
    max,
    pow,
    rndx,
    logx,
}