namespace CalcAppShared.Models;

internal class MscSave
{
    public List<string> History { get; set; } = new();
    public bool PlotMode { get; set; }
    public double From { get; set; }
    public double To { get; set; }
    public double Step { get; set; }
    public List<VariableModel> Variables { get; set; } = new();
}
