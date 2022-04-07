using Calculation;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MyCalc.Models;
using MyCalc.Services;
using MyCalc.Views;
using OxyPlot;
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

    private IRelayCommand? _executeCommand;
    public IRelayCommand Execute
        => _executeCommand ??= CreateCommandWithTryBlock(()
            =>
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
        });

    private IRelayCommand? _addVariableCommand;
    public IRelayCommand AddVariable
        => _addVariableCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            VariableModel model = new();
            Variables.Add(model);
            SelectedVariable = model;
        });

    private IRelayCommand? _removeVariableCommand;
    public IRelayCommand RemoveVariable
        => _removeVariableCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            if(SelectedVariable is not null)
                Variables.Remove(SelectedVariable);
        });

    private IRelayCommand? _openCommand;
    public IRelayCommand Open
        => _openCommand ??= CreateCommandWithTryBlock(()
            =>
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

            From = savedState.From;
            To = savedState.To;
            Step = savedState.Step;
            AnyChanges = false;

            Execute.Execute(null);
        });

    private IRelayCommand? _saveCommand;
    public IRelayCommand Save
        => _saveCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            if(string.IsNullOrEmpty(CurrentSaveFile))
                if(_dialogService.SaveFileDialog())
                    CurrentSaveFile = _dialogService.FilePath;
                else
                    return;

            SaveCurrentState();
        });

    private IRelayCommand? _saveAsCommand;
    public IRelayCommand SaveAs
        => _saveAsCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            if(_dialogService.SaveFileDialog())
                CurrentSaveFile = _dialogService.FilePath;
            else
                return;

            SaveCurrentState();
        });

    private IRelayCommand? _exitCommand;
    public IRelayCommand Exit
        => _exitCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            if(!AnyChanges || MessageBoxResult.Yes == MessageBox.Show("Обнаружены несохранённые изменения. Всё равно закрыть", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                Application.Current.Shutdown();
        });

    private IRelayCommand? _clearHistoryCommand;
    public IRelayCommand ClearHistory
        => _clearHistoryCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            History.Clear();
            AnyChanges = true;
        });

    private IRelayCommand? _showHelpCommand;
    public IRelayCommand ShowHelp
        => _showHelpCommand ??= CreateCommandWithTryBlock(()
            =>
        {
            HelpWindow window = new();
            window.ShowDialog();
        });

    private static RelayCommand CreateCommandWithTryBlock(Action action)
    {
        return new RelayCommand(() =>
        {
            try
            {
                action();
            }
            catch
            {
                //empty
            }
        });
    }

    private void SaveCurrentState()
    {
        if(string.IsNullOrEmpty(CurrentSaveFile))
            return;

        MscSave currentState = new()
        {
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
