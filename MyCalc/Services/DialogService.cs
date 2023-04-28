using CalcAppShared.Services;
using Microsoft.Win32;
using System.Windows;

namespace MyCalc.Services;

internal class DialogService : IDialogService
{
    private const MessageBoxButton AgreementButtons = MessageBoxButton.YesNo;
    private const MessageBoxImage AgreementIcon = MessageBoxImage.Warning;

    public bool OpenFileDialog(out string? path)
    {
        path = null;
        OpenFileDialog openFileDialog = new();
        if(openFileDialog.ShowDialog() == true)
        {
            path = openFileDialog.FileName;
            return true;
        }
        return false;
    }

    public bool SaveFileDialog(out string? path)
    {
        path = null;
        SaveFileDialog saveFileDialog = new();
        if(saveFileDialog.ShowDialog() == true)
        {
            path = saveFileDialog.FileName;
            return true;
        }
        return false;
    }

    public bool ShowAgreementForm(string message, string tittle)
    {
        return MessageBoxResult.Yes == MessageBox.Show(message, tittle, AgreementButtons, AgreementIcon);
    }

    public void ShowMessage(string message)
        => MessageBox.Show(message, "Message", MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowError(string message)
        => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
}