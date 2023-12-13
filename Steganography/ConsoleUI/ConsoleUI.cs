using Steganography.Models;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography.ConsoleUI
{
    public class ConsoleUi(IImageEncoder encoder, IImageDecoder decoder) : IConsoleUi
    {
        private readonly IImageEncoder _encoder = encoder;
        private readonly IImageDecoder _decoder = decoder;
        
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
                        button = mainMenu.DisplayMenu();
                        break;
                    case State.EncodeMenu:
                        button = encodeMenu.DisplayMenu();
                        break;
                    case State.DecodeMenu:
                        button = decodeMenu.DisplayMenu();
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