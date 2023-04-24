using Calculation;
using Calculation.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyCalc.Models;
using MyCalc.Services;
using MyCalc.Views;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace MyCalc.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;

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
    private PlotModel _plot = new();
    [ObservableProperty]
    private bool _plotMode = false;
    [ObservableProperty]
    private string _currentSaveFile = string.Empty;
    [ObservableProperty]
    private bool _anyChanges = false;

    public ObservableCollection<int> AvailableAccuracy { get; set; } = new(new[] { 0, 1, 2, 3, 4, 5, 6 });
    public ObservableCollection<VariableModel> Variables { get; set; } = new();
    public ObservableCollection<string> History { get; set; } = new();

    public MainViewModel()
    {

    }

    public MainViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    private void BuildPlot(SmartCalculator calc, Dictionary<string, IExpressionNode> variablesTrees)
    {
        if(To <= From)
            throw new ArgumentException("From have to be less than To.");
        if(Step <= 0)
            throw new ArgumentException("Step should be more than 0.");

        List<string> realVariableNames = new();
        foreach(var variable in Variables.Where(x => string.IsNullOrEmpty(x.Expression)))
        {
            string name = variable.Name;
            variablesTrees.Add(name, new VariableNode(name));
            realVariableNames.Add(name);
        }


        IExpressionNode head = calc.Parse(CalcExpression, RoundAccuracy);
        LineSeries series = new();
        for(double i = From; i < To; i += Step)
        {
            ValueNode node = new(i);
            foreach(string name in realVariableNames)
                variablesTrees[name] = node;
            head.Recalculate(variablesTrees);
            series.Points.Add(new(i, head.Value));
        }

        Plot.Series.Clear();
        Plot.Series.Add(series);
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
            BuildPlot(calc, variablesTrees);
        else
            result = calc.Execute(CalcExpression, RoundAccuracy, variablesTrees);

        Answer = result;

        if(!History.Contains(CalcExpression))
            History.Add(CalcExpression);
    }

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
        if(!_dialogService.OpenFileDialog())
            return;
        string path = _currentSaveFile = _dialogService.FilePath;
        using FileStream fs = File.OpenRead(path);
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
            if(_dialogService.SaveFileDialog())
                CurrentSaveFile = _dialogService.FilePath;
            else
                return;

        SaveCurrentState();
    }

    [RelayCommand]
    private void SaveAs()
    {
        if(_dialogService.SaveFileDialog())
            CurrentSaveFile = _dialogService.FilePath;
        else
            return;

        SaveCurrentState();
    }

    [RelayCommand]
    private void Exit()
    {
        if(!AnyChanges || MessageBoxResult.Yes == MessageBox.Show("Обнаружены несохранённые изменения. Всё равно закрыть", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            Application.Current.Shutdown();
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
        HelpWindow window = new();
        window.ShowDialog();
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
            History = History.ToList(),
            Variables = Variables.ToList(),
        };

        if(File.Exists(CurrentSaveFile))
            File.Delete(CurrentSaveFile);

        using FileStream fs = File.OpenWrite(CurrentSaveFile);
        JsonSerializer.Serialize(fs, currentState);

        AnyChanges = false;
    }
}
