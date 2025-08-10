namespace CalcAppShared.Models;

internal class MscSave
{
    public required List<string> History { get; set; } = [];
    public required bool PlotMode { get; set; }
    public required double From { get; set; }
    public required double To { get; set; }
    public required double Step { get; set; }
    public required List<VariableModel> Variables { get; set; } = [];
}
