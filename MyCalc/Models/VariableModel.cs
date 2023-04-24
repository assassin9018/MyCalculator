using CommunityToolkit.Mvvm.ComponentModel;

namespace MyCalc.Models;

public partial class VariableModel : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;
    [ObservableProperty]
    private string _expression = string.Empty;
}
