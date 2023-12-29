using System.ComponentModel.Design.Serialization;

namespace Steganography.ConsoleUI;

public class Info(string selectedAlgorithm, string selectedFilePath, string messageToEncode, bool infoFlag)
{
    private string SelectedAlgorithm { get; set; } = selectedAlgorithm;
    private string SelectedFilePath { get; set; } = selectedFilePath;
    private string MessageToEncode { get; set; } = messageToEncode;

    // infoFlag: encode = true, decode = false 

    public void DisplayInfo()
    {
        Console.WriteLine(infoFlag ? $"Message to Encode: {MessageToEncode}" : $"Decoded Message: {MessageToEncode}");
        Console.WriteLine($"Selected Algorithm: {SelectedAlgorithm}");
        Console.WriteLine($"Selected FilePath: {SelectedFilePath}");
    }
    
    public void IsAlgorithmChanged(string newText)
    {
        SelectedAlgorithm = newText;
    }
    
    public void IsMessageChanged(string newText)
    {
        MessageToEncode = newText;
    }
    
    public void IsFileChanged(string newText)
    {
        SelectedFilePath = newText;
    }
    
}