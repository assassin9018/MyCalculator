using System.Diagnostics.CodeAnalysis;

namespace CalcAppShared.Services;

public interface IDialogService
{
    void ShowMessage(string message);
    void ShowError(string message);
    bool ShowAgreementForm(string message, string tittle);
    bool OpenFileDialog([NotNullWhen(true)] out string? path);
    bool SaveFileDialog([NotNullWhen(true)] out string? path);
}