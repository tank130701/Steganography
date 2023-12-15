using Steganography.Models;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography.ConsoleUI
{
    public class ConsoleUi(IImageEncoder encoder, IImageDecoder decoder) : IConsoleUi
    {
        private readonly IImageEncoder _encoder = encoder;
        private readonly IImageDecoder _decoder = decoder;
        private string _selectedAlgorithm = "The encoding algorithm is not selected. Choose an algorithm.";
        private string _selectedFilePath = "The file is not selected";
        public void Run()
        {
            Info mainMenuInfo = null;
            ConsoleMenu mainMenu = new ConsoleMenu("Main Menu",
                new List<string>
                {
                    MainMenuButtons.EncodeMessage, 
                    MainMenuButtons.DecodeMessage,
                    MainMenuButtons.Exit
                }, ref mainMenuInfo);

            var encodeInfo = new Info(ref _selectedAlgorithm, ref _selectedFilePath);
            ConsoleMenu encodeMenu = new ConsoleMenu( $"Select Option",
                new List<string>
                {
                    EncodeMenuButtons.SelectImage,
                    EncodeMenuButtons.SelectAlgorithm,
                    EncodeMenuButtons.WriteMessage,
                    EncodeMenuButtons.EncodeMessage,
                    EncodeMenuButtons.BackToMainMenu
                }, ref encodeInfo);

            var decodeInfo = new Info(ref _selectedAlgorithm, ref _selectedFilePath);
            ConsoleMenu decodeMenu = new ConsoleMenu("Select Option",
                new List<string>
                {
                    DecodeMenuButtons.SelectImage,
                    DecodeMenuButtons.SelectAlgorithm,
                    DecodeMenuButtons.DecodeMessage,
                    DecodeMenuButtons.BackToMainMenu
                }, ref decodeInfo);
            
            Info algorithmsMenuInfo = null;
            ConsoleMenu algorithmsMenu = new ConsoleMenu("Select Algorithm",
                new List<string>
                {
                    EncodeAlgorithmsButtons.Lsb,
                    EncodeAlgorithmsButtons.AlphaChannel,
                    EncodeAlgorithmsButtons.Metadata,
                    EncodeAlgorithmsButtons.Palette,
                    EncodeAlgorithmsButtons.Dct,
                    EncodeAlgorithmsButtons.F5,
                    EncodeAlgorithmsButtons.BackToMenu
                },ref algorithmsMenuInfo);
            
            
            MenuStates menuStates = MenuStates.MainMenu;
            string button = "";
            while (menuStates != MenuStates.Exit)
            {
                switch (menuStates)
                {
                    case MenuStates.MainMenu:
                        button = mainMenu.DisplayMenu();
                        break;
                    case MenuStates.EncodeMenu:
                        button = encodeMenu.DisplayMenu();
                        break;
                    case MenuStates.DecodeMenu:
                        button = decodeMenu.DisplayMenu();
                        break;
                    case MenuStates.AlgorithmsMenu:
                        button = algorithmsMenu.DisplayMenu();
                        break;
                }
                HandleInput(button, ref menuStates);
            }
        }
        
        void HandleInput(string button, ref MenuStates currentMenuStates)
        {
            switch (currentMenuStates)
            {
                case MenuStates.MainMenu:
                    switch (button)
                    {
                        case MainMenuButtons.EncodeMessage:
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case MainMenuButtons.DecodeMessage:
                            currentMenuStates = MenuStates.DecodeMenu;
                            break;
                        case MainMenuButtons.Exit:
                            currentMenuStates = MenuStates.Exit;
                            break;
                    }
                    break;
                case MenuStates.EncodeMenu:
                    switch (button)
                    {
                        case EncodeMenuButtons.SelectImage:
                            Console.WriteLine("Вы выбрали опцию SelectImage в EncodeMenu");
                            break;
                        case EncodeMenuButtons.SelectAlgorithm:
                            currentMenuStates = MenuStates.AlgorithmsMenu;
                            break;
                        case EncodeMenuButtons.WriteMessage:
                            break;
                        case EncodeMenuButtons.EncodeMessage:
                            break;
                        case EncodeMenuButtons.BackToMainMenu:
                            currentMenuStates = MenuStates.MainMenu;
                            break;
                    }
                    break;
                case MenuStates.DecodeMenu:
                    switch (button)
                    {
                        case DecodeMenuButtons.SelectImage: 
                            Console.WriteLine("Вы выбрали опцию SelectImage в DecodeMenu");
                            break;
                        case DecodeMenuButtons.SelectAlgorithm:
                            currentMenuStates = MenuStates.AlgorithmsMenu;
                            break;
                        case DecodeMenuButtons.DecodeMessage:
                            Console.WriteLine("Вы выбрали опцию DecodeMessage в DecodeMenu");
                            break;
                        case DecodeMenuButtons.BackToMainMenu:
                            currentMenuStates = MenuStates.MainMenu;
                            break;
                    }
                    break;
                case MenuStates.AlgorithmsMenu:
                    switch (button)
                    {
                        case EncodeAlgorithmsButtons.Lsb:
                            _selectedAlgorithm = "LSB";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.AlphaChannel:
                            _selectedAlgorithm = "AlphaChannel";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Palette:
                            _selectedAlgorithm = "Palette";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Metadata:
                            _selectedAlgorithm = "Metadata";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Dct:
                            _selectedAlgorithm = "DCT";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.F5:
                            _selectedAlgorithm = "F5";
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.BackToMenu:
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                    }
                    break;
            }
        }
    }
}