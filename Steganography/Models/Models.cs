namespace Steganography.Models;

public class ConsoleMenu(string title, List<string> options)
{
    public string Title { get; } = title;
    public List<string> Options { get; } = options;
}