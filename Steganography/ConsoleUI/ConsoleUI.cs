using Steganography.Models;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography.ConsoleUI
{
    public class ConsoleUi(IImageEncoder encoder, IImageDecoder decoder) : IConsoleUi
    {
        private readonly IImageEncoder _encoder = encoder;
        private readonly IImageDecoder _decoder = decoder;
        
        void DisplayOptions(ConsoleMenu menu)
        {
            Console.WriteLine(menu.Title);

            for (int i = 0; i < menu.Options.Count; i++)
            {
                string currentOption = menu.Options[i];
                string prefix;

                if(i == menu.SelectedIndex)
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

        string DisplayMenu(ConsoleMenu menu)
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions(menu);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if(keyPressed == ConsoleKey.DownArrow)
                {
                    menu.SelectedIndex++;
                    if(menu.SelectedIndex >= menu.Options.Count)
                    {
                        menu.SelectedIndex = 0;
                    }
                }else if(keyPressed == ConsoleKey.UpArrow)
                {
                    menu.SelectedIndex--;
                    if(menu.SelectedIndex < 0)
                    {
                        menu.SelectedIndex = menu.Options.Count-1;
                    }
                }
            } while(keyPressed!=ConsoleKey.Enter);

            return menu.Options[menu.SelectedIndex];
        }
        
        public void Run()
        {
            ConsoleMenu mainMenu = new ConsoleMenu("Main Menu",
                new List<string>
                {
                    MainMenuButtons.EncodeMessage, 
                    MainMenuButtons.DecodeMessage,
                    MainMenuButtons.Exit
                });
            
            ConsoleMenu encodeMenu = new ConsoleMenu("Encode Menu", new List<string>
                {
                    EncodeMenuButtons.SelectImage,
                    EncodeMenuButtons.SelectAlgorithm,
                    EncodeMenuButtons.WriteMessage,
                    EncodeMenuButtons.EncodeMessage,
                    EncodeMenuButtons.BackToMainMenu
                });
            
            ConsoleMenu decodeMenu = new ConsoleMenu("Encode Menu", new List<string>
                {
                    DecodeMenuButtons.SelectImage,
                    DecodeMenuButtons.SelectAlgorithm,
                    DecodeMenuButtons.DecodeMessage,
                    DecodeMenuButtons.BackToMainMenu
                });
            
            
            State state = State.MainMenu;
            string button = "";
            while (state != State.Exit)
            {
                switch (state)
                {
                    case State.MainMenu:
                        button = DisplayMenu(mainMenu);
                        break;
                    case State.EncodeMenu:
                        button = DisplayMenu(encodeMenu);
                        break;
                    case State.DecodeMenu:
                        button = DisplayMenu(decodeMenu);
                        break;
                }
                HandleInput(button, ref state);
            }
        }
        
        void HandleInput(string button, ref State currentState)
        {
            switch (currentState)
            {
                case State.MainMenu:
                    switch (button)
                    {
                        case MainMenuButtons.EncodeMessage:
                            currentState = State.EncodeMenu;
                            break;
                        case MainMenuButtons.DecodeMessage:
                            currentState = State.DecodeMenu;
                            break;
                        case MainMenuButtons.Exit:
                            currentState = State.Exit;
                            break;
                    }
                    break;
                case State.EncodeMenu:
                    switch (button)
                    {
                        case EncodeMenuButtons.SelectImage:
                            Console.WriteLine("Вы выбрали опцию SelectImage в EncodeMenu");
                            break;
                        case EncodeMenuButtons.SelectAlgorithm:
                            Console.WriteLine("Вы выбрали опцию SelectAlgorithm в подменю EncodeMenu");
                            break;
                        case EncodeMenuButtons.WriteMessage:
                            break;
                        case EncodeMenuButtons.EncodeMessage:
                            break;
                        case EncodeMenuButtons.BackToMainMenu:
                            currentState = State.MainMenu;
                            break;
                    }
                    break;
                case State.DecodeMenu:
                    switch (button)
                    {
                        case DecodeMenuButtons.SelectImage: 
                            Console.WriteLine("Вы выбрали опцию SelectImage в DecodeMenu");
                            break;
                        case DecodeMenuButtons.DecodeMessage:
                            Console.WriteLine("Вы выбрали опцию DecodeMessage в DecodeMenu");
                            break;
                        case DecodeMenuButtons.SelectAlgorithm:
                            Console.WriteLine("Вы выбрали опцию SelectAlgorithm в DecodeMenu");
                            break;
                        case DecodeMenuButtons.BackToMainMenu:
                            currentState = State.MainMenu;
                            break;
                    }
                    break;
            }
        }
    }
}