namespace Steganography.ConsoleUI;

public class ConsoleMenu(string title, List<string> options, Info info)
{
    string Title { get; } = title;
    int SelectedIndex { get; set; } = 0;
    List<string> Options { get; } = options;
    private Info Info { get; } = info;
    void DisplayOptions()
        {
            Console.WriteLine(Title);

            for (int i = 0; i < Options.Count; i++)
            {
                string currentOption = Options[i];
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
            do
            {
                Console.Clear();
                if (Info != null) Info.DisplayInfo();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if(keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if(SelectedIndex >= Options.Count)
                    {
                        SelectedIndex = 0;
                    }
                }else if(keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if(SelectedIndex < 0)
                    {
                        SelectedIndex = Options.Count-1;
                    }
                }
            } while(keyPressed!=ConsoleKey.Enter);

            return Options[SelectedIndex];
        }
}