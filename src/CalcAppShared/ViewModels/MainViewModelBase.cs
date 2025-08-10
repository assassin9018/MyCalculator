using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CalcAppShared.Models;
using CalcAppShared.Services;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Drawing;
using Calculation.Smart.Nodes;
using Calculation.Smart;

namespace CalcAppShared.ViewModels;

public abstract partial class MainViewModelBase(IDialogService dialogService) : ObservableObject
{
    [ObservableProperty]
    private string _calcExpression = string.Empty;
    [ObservableProperty]
    private double _answer;
    [ObservableProperty]
    private int _roundAccuracy = 2;
    [ObservableProperty]
    private VariableModel? _selectedVariable;
    [ObservableProperty]
    private double _from;
    [ObservableProperty]
    private double _to;
    [ObservableProperty]
    private double _step;
    [ObservableProperty]
    private bool _plotMode = false;
    [ObservableProperty]
    private string _currentSaveFile = string.Empty;
    [ObservableProperty]
    private bool _anyChanges = false;

    public ObservableCollection<int> AvailableAccuracy { get; set; } = new([0, 1, 2, 3, 4, 5, 6]);
    public ObservableCollection<VariableModel> Variables { get; set; } = [];
    public ObservableCollection<string> History { get; set; } = [];

    private IEnumerable<PointF> BuildPlot(SmartCalculator calc, Dictionary<string, IExpressionNode> variablesTrees)
    {
        if(To <= From)
            throw new ArgumentException("From have to be less than To.");
        if(Step <= 0)
            throw new ArgumentException("Step should be more than 0.");

        List<string> realVariableNames = [];
        foreach(var variable in Variables.Where(x => !string.IsNullOrWhiteSpace(x.Name)).Where(x => string.IsNullOrEmpty(x.Expression)))
        {
            string name = variable.Name;
            variablesTrees.Add(name, new VariableNode(name));
            realVariableNames.Add(name);
        }


        IExpressionNode head = calc.Parse(CalcExpression, RoundAccuracy);
        for(double i = From; i < To; i += Step)
        {
            ValueNode node = new(i);
            foreach(string name in realVariableNames)
                variablesTrees[name] = node;
            head.Recalculate(variablesTrees);
            yield return new PointF((float)i, (float)head.Value);
        }
    }

    [RelayCommand]
    private void Execute()
    {
        double result = 0;

        SmartCalculator calc = new(Variables.Select(x => x.Name));

        var variablesTrees = Variables
            .Where(x => !string.IsNullOrEmpty(x.Expression))
            .ToDictionary(key => key.Name, value => calc.Parse(value.Expression, RoundAccuracy));

        if(PlotMode)
        {
            var series = BuildPlot(calc, variablesTrees);
            Redraw(series);
        }
        else
            result = calc.Execute(CalcExpression, RoundAccuracy, variablesTrees);

        Answer = result;

        if(!History.Contains(CalcExpression))
            History.Add(CalcExpression);
    }

    protected abstract void Redraw(IEnumerable<PointF> series);

    [RelayCommand]
    private void AddVariable()
    {
        VariableModel model = new();
        Variables.Add(model);
        SelectedVariable = model;
    }

    [RelayCommand]
    private void RemoveVariable()
    {
        if(SelectedVariable is not null)
            Variables.Remove(SelectedVariable);
    }

    [RelayCommand]
    private void Open()
    {
        if(!dialogService.OpenFileDialog(out var filePath))
            return;
        _currentSaveFile = filePath;
        using FileStream fs = File.OpenRead(filePath);
        MscSave savedState = JsonSerializer.Deserialize<MscSave>(fs) ?? throw new ArgumentNullException(nameof(savedState));

        History.Clear();

        foreach(string exp in savedState.History)
            History.Add(exp);

        Variables.Clear();
        foreach(VariableModel vm in savedState.Variables)
            Variables.Add(vm);

        CalcExpression = History.LastOrDefault(string.Empty);

        PlotMode = savedState.PlotMode;
        From = savedState.From;
        To = savedState.To;
        Step = savedState.Step;
        AnyChanges = false;

        Execute();
    }

    [RelayCommand]
    private void Save()
    {
        if(string.IsNullOrEmpty(CurrentSaveFile))
            if(dialogService.SaveFileDialog(out var filePath))
                CurrentSaveFile = filePath;
            else
                return;

        SaveCurrentState();
    }

    [RelayCommand]
    private void SaveAs()
    {
        if(dialogService.SaveFileDialog(out var filePath))
            CurrentSaveFile = filePath;
        else
            return;

        SaveCurrentState();
    }

    [RelayCommand]
    private void Exit()
    {
        if(!AnyChanges || dialogService.ShowAgreementForm("Обнаружены не сохранённые изменения. Всё равно закрыть?", "Внимание"))
            ShutDown();
    }

    [RelayCommand]
    private void ClearHistory()
    {
        History.Clear();
        AnyChanges = true;
    }

    [RelayCommand]
    private void ShowHelp()
    {
    }

    private void SaveCurrentState()
    {
        if(string.IsNullOrEmpty(CurrentSaveFile))
            return;

        MscSave currentState = new()
        {
            PlotMode = PlotMode,
            From = From,
            To = To,
            Step = Step,
            History = [.. History],
            Variables = [.. Variables],
        };

        if(File.Exists(CurrentSaveFile))
            File.Delete(CurrentSaveFile);

        using FileStream fs = File.OpenWrite(CurrentSaveFile);
        JsonSerializer.Serialize(fs, currentState);

        AnyChanges = false;
    }

    protected abstract void ShutDown();
    protected abstract void ShowReadMe();
}
