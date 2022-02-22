using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyCalc;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        roundDigits.SelectedIndex = 2;
    }


    private void button1_Click(object sender, EventArgs e)
    {
        try
        {
            RoundDigit = int.Parse(roundDigits.SelectedItem.ToString());
            answerBox.Text = calculate(getCalcStr());
            if(History.Items.Count == 0 || textBox1.Text != History.Items[History.Items.Count - 1].ToString())
                History.Items.Add(textBox1.Text);
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
    {
        if(e.KeyChar == (char)Keys.Enter)
            button1_Click(sender, new EventArgs());
    }

    private void History_SelectedIndexChanged(object sender, EventArgs e)
    {
        textBox1.Text = History.SelectedItem.ToString();
    }
}
