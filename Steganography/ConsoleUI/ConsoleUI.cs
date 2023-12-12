using Steganography.Models;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography.ConsoleUI
{
    public class ConsoleUi(IImageEncoder encoder, IImageDecoder decoder) : IConsoleUi
    {
        private readonly IImageEncoder _encoder = encoder;
        private readonly IImageDecoder _decoder = decoder;
        
        private readonly Stack<ConsoleMenu> _menuStack = new();

        private void DisplayMenu(ConsoleMenu menu)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(menu.Title);
                Console.WriteLine(new string('=', menu.Title.Length));

                for (int i = 0; i < menu.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {menu.Options[i]}");
                }

                Console.WriteLine("0. Go back");

                char key = GetKeyPress();
                if (key == '0')
                {
                    if (_menuStack.Count > 0)
                    {
                        menu = _menuStack.Pop();
                    }
                    else
                    {
                        Console.WriteLine("Cannot go back. This is the root menu.");
                        Console.ReadKey();
                        return;
                    }
                }
                else if (char.IsDigit(key))
                {
                    int choice = int.Parse(key.ToString());

                    if (choice > 0 && choice <= menu.Options.Count)
                    {
                        object selectedOption = menu.Options[choice - 1];

                        if (selectedOption is ConsoleMenu submenu)
                        {
                            _menuStack.Push(menu);
                            menu = submenu;
                        }
                        else if (selectedOption is Action action)
                        {
                            action.Invoke();
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
        
        public void Run()
        {
            ConsoleMenu subMenu = new ConsoleMenu("Encode Menu", new List<string>
            {
                EncodeMenuButtons.EncodeMenuSelectImage,
                EncodeMenuButtons.EncodeMenuSelectAlgorithm,
                EncodeMenuButtons.EncodeMenuWriteMessage,
                EncodeMenuButtons.EncodeMenuEncodeMessage
            });
            ConsoleMenu mainMenu = new ConsoleMenu("Main Menu",
                new List<string>
                {
                    MainMenuButtons.MainMenuEncodeMessage, 
                    MainMenuButtons.MainMenuDecodeMessage,
                    MainMenuButtons.MainMenuExit
                });
            DisplayMenu(mainMenu);
        }
        
        private char GetKeyPress()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            return keyInfo.KeyChar;
        }
    }
}