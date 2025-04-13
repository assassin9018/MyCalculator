using CalcAppShared.Services;
using System.Diagnostics.CodeAnalysis;

namespace MauiCalc.Services;

internal class DialogService : IDialogService
{
    private const string SmartCalcExtension = ".scm";
    private readonly static FilePickerFileType _calcFileType = new(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                    { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { SmartCalcExtension } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
                    { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
                });

    
    private readonly PickOptions options = new()
    {
        PickerTitle = "Please select a comic file",
        FileTypes = _calcFileType,
    };

    //public async Task<Stream?> OpenFileDialog()
    //{
    //    try
    //    {
    //        var result = await FilePicker.Default.PickAsync(options);
    //        if(IsSmartCalcFile(result.FileName))
    //            return await result.OpenReadAsync();
    //    }
    //    catch(Exception ex)
    //    {
    //        // The user canceled or something went wrong
    //    }

    //    return null;
    //}

    public bool SaveFileDialog([NotNullWhen(true)] out string path)
    {
        throw new NotImplementedException();
    }

    public bool ShowAgreementForm(string message, string tittle)
    {
        throw new NotImplementedException();
    }

    public void ShowError(string message)
    {
        throw new NotImplementedException();
    }

    public void ShowMessage(string message)
    {
        throw new NotImplementedException();
    }

    private static bool IsSmartCalcFile(string? filePath)
        => string.Equals(Path.GetExtension(filePath), SmartCalcExtension);

    public bool OpenFileDialog([NotNullWhen(true)] out string? path)
    {
        throw new NotImplementedException();
    }
}
