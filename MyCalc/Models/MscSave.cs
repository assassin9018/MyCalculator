using System.Collections.Generic;

namespace MyCalc.Models;

internal class MscSave
{
    public List<string> History { get; set; } = new();
    public double From { get; set; }
    public double To { get; set; }
    public double Step { get; set; }
    public List<VariableModel> Variables { get; set; } = new();
}
