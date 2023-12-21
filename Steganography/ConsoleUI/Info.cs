namespace Steganography.ConsoleUI;

public class Info(string selectedAlgorithm, string selectedFilePath, string messageToEncode)
{
    private string SelectedAlgorithm { get; set; } = selectedAlgorithm;
    private string SelectedFilePath { get; set; } = selectedFilePath;
    private string MessageToEncode { get; set; } = messageToEncode;

    public void DisplayInfo()
    {
        Console.WriteLine($"Message to Encode: {MessageToEncode}");
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