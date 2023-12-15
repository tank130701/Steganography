namespace Steganography.ConsoleUI;

public class Info(ref string selectedAlgorithm, ref string selectedFilePath)
{
    private string _selectedAlgorithm { get; set; } = selectedAlgorithm;
    private string _selectedFilePath { get; set; } = selectedFilePath;

    public void DisplayInfo()
    {
        Console.WriteLine($"Selected Algorithm: {_selectedAlgorithm}");
        Console.WriteLine($"Selected FilePath: {_selectedFilePath}");
    }
}