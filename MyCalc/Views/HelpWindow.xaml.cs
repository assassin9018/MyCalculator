using System.IO;
using System.Windows;

namespace MyCalc.Views;
/// <summary>
/// Interaction logic for HelpWindow.xaml
/// </summary>
public partial class HelpWindow : Window
{
    public HelpWindow()
    {
        InitializeComponent();
        using FileStream fs = File.OpenRead("Assets\\help_ru.rtf");
        Rtb.Selection.Load(fs, DataFormats.Rtf);
    }
}
