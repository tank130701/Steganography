namespace Steganography.ConsoleUI;

public class FilesMenu(string title, List<string> files)
{
    string Title { get; } = title;
    int SelectedIndex { get; set; } = 0;
    private List<string> Files { get; set; } = new ();

    void DisplayOptions()
        {
            Console.WriteLine(Title);

            for (int i = 0; i < Files.Count; i++)
            {
                string currentOption = Files[i];
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
                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if(keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if(SelectedIndex >= Files.Count)
                    {
                        SelectedIndex = 0;
                    }
                }else if(keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if(SelectedIndex < 0)
                    {
                        SelectedIndex = Files.Count-1;
                    }
                }
            } while(keyPressed!=ConsoleKey.Enter);

            return Files[SelectedIndex];
        }
        
        public void DirectoryListUpdated(List<string> files)
        {
            Files = files;
            if (!Files.Contains(EncodeAlgorithmsButtons.BackToMenu))
                Files.Add(EncodeAlgorithmsButtons.BackToMenu);
        }
}