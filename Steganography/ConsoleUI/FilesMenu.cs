namespace Steganography.ConsoleUI;

public class FilesMenu
{
    public FilesMenu(string title, List<string> files)
    {
        Title = title;
        Files = files;
        Buttons.Add(EncodeAlgorithmsButtons.BackToMenu);
    }

    string Title { get; }
    int SelectedIndex { get; set; } = 0;
    private List<string> Files { get; set; }
    List<string> Buttons { get; } = new();
    
    void DisplayOptions()
        {
            Console.WriteLine(Title);

            for (int i = 0; i < Buttons.Count; i++)
            {
                string currentOption = Buttons[i];
                string prefix;

                if(i == SelectedIndex)
                {
                    prefix = "*";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                } else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix} -- {currentOption} --");
            }

            Console.ResetColor();
        }

        public string DisplayMenu()
        {
            ConsoleKey keyPressed;
            foreach (var item in Files)
            {
                if (!Buttons.Contains(item))
                {
                    Buttons.Add(item);
                }
            }
            do
            {
                Console.Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if(keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if(SelectedIndex >= Buttons.Count)
                    {
                        SelectedIndex = 0;
                    }
                }else if(keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if(SelectedIndex < 0)
                    {
                        SelectedIndex = Buttons.Count-1;
                    }
                }
            } while(keyPressed!=ConsoleKey.Enter);

            return Buttons[SelectedIndex];
        }
        
        public void DirectoryListUpdated(List<string> files)
        {
            Files = files;
        }
}