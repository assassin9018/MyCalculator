using System.Windows;
using System.Windows.Controls;

namespace MyCalc;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void tbExpresion_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
