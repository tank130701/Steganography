namespace Steganography.ConsoleUI;

public class ConsoleMenu(string title, List<string> options)
{
    public string Title { get; } = title;
    public int SelectedIndex { get; set; } = 0;
    public List<string> Options { get; } = options;
    
}