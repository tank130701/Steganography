namespace Steganography.Models;

public class ConsoleMenu(string title, List<object> options)
{
    public string Title { get; } = title;
    public List<object> Options { get; } = options;
}