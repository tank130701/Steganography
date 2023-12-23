using Steganography.Models;
using Steganography.Service.Algorithms;
using Steganography.Service.Decoder;
using Steganography.Service.Encoder;

namespace Steganography.ConsoleUI
{
    public class ConsoleUi(IImageEncoder encoder, IImageDecoder decoder) : IConsoleUi
    {
        private string _messageToEncode = "The message is empty. Write a message.";
        private string _selectedAlgorithm = "The encoding algorithm is not selected. Select an algorithm.";
        private string _selectedFilePath = "The file is not selected. Select a file.";
        public delegate void ChangeAlgorithm(string newText);
        public delegate void ChangeMessage(string newText);
        public delegate void ChangeFile(string newText);
        public event ChangeAlgorithm? AlgorithmChanged;
        public event ChangeMessage? MessageChanged;
        public event ChangeFile? FileChanged;

        private MenuStates ChangeEncodeAlgorithm(string algorithm)
        {
            _selectedAlgorithm = algorithm;
            AlgorithmChanged?.Invoke(_selectedAlgorithm);
            return MenuStates.EncodeMenu;
        }
        
        private MenuStates ChangeDecodeAlgorithm(string algorithm)
        {
            _selectedAlgorithm = algorithm;
            AlgorithmChanged?.Invoke(_selectedAlgorithm);
            return MenuStates.DecodeMenu;
        }
        
        public void Run()
        {
            Info mainMenuInfo = null;
            ConsoleMenu mainMenu = new ConsoleMenu("Main Menu",
                new List<string>
                {
                    MainMenuButtons.EncodeMessage, 
                    MainMenuButtons.DecodeMessage,
                    MainMenuButtons.Exit
                }, mainMenuInfo);

            var encodeInfo = new Info(
                _selectedAlgorithm,
                _selectedFilePath,
                _messageToEncode
                );
            ConsoleMenu encodeMenu = new ConsoleMenu( $"Select Option",
                new List<string>
                {
                    EncodeMenuButtons.SelectImage,
                    EncodeMenuButtons.SelectAlgorithm,
                    EncodeMenuButtons.WriteMessage,
                    EncodeMenuButtons.EncodeMessage,
                    EncodeMenuButtons.BackToMainMenu
                }, encodeInfo);

            var decodeInfo = new Info(
                _selectedAlgorithm,
                _selectedFilePath,
                _messageToEncode
                );
            ConsoleMenu decodeMenu = new ConsoleMenu("Select Option",
                new List<string>
                {
                    DecodeMenuButtons.SelectImage,
                    DecodeMenuButtons.SelectAlgorithm,
                    DecodeMenuButtons.DecodeMessage,
                    DecodeMenuButtons.BackToMainMenu
                }, decodeInfo);
            
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
                    EncodeAlgorithmsButtons.Eof,
                    EncodeAlgorithmsButtons.BackToMenu
                },algorithmsMenuInfo);

            Info selectingFileToEncodeInfo = null;
            var filesToEncode = encoder.GetImageFilesInDirectory();
            filesToEncode.Add(EncodeAlgorithmsButtons.BackToMenu);
            ConsoleMenu selectingFileToEncodeMenu = new ConsoleMenu(
                "Select File",
                filesToEncode,
                selectingFileToEncodeInfo
                );
            
            Info selectingFileToDecodeInfo = null;
            var filesToDecode = decoder.GetImageFilesInDirectory();
            filesToDecode.Add(EncodeAlgorithmsButtons.BackToMenu);
            ConsoleMenu selectingFileToDecodeMenu = new ConsoleMenu(
                "Select File",
                filesToDecode,
                selectingFileToDecodeInfo
            );
            
            AlgorithmChanged += encodeInfo.IsAlgorithmChanged;
            AlgorithmChanged += decodeInfo.IsAlgorithmChanged;
            MessageChanged += encodeInfo.IsMessageChanged;
            FileChanged += encodeInfo.IsFileChanged;
            
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
                    case MenuStates.AlgorithmsEncodeMenu:
                        button = algorithmsMenu.DisplayMenu();
                        break;
                    case MenuStates.AlgorithmsDecodeMenu:
                        button = algorithmsMenu.DisplayMenu();
                        break;
                    case MenuStates.SelectFileToEncodeMenu:
                        button = selectingFileToEncodeMenu.DisplayMenu();
                        break;
                    case MenuStates.SelectFileToDecodeMenu:
                        button = selectingFileToDecodeMenu.DisplayMenu();
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
                            currentMenuStates = MenuStates.SelectFileToEncodeMenu;
                            break;
                        case EncodeMenuButtons.SelectAlgorithm:
                            currentMenuStates = MenuStates.AlgorithmsEncodeMenu;
                            break;
                        case EncodeMenuButtons.WriteMessage:
                            Console.Write("\nEnter a message: ");
                            var newMessage = Console.ReadLine();
                            if (newMessage != null)
                            {
                                _messageToEncode = newMessage;
                                MessageChanged?.Invoke(_messageToEncode);
                            } 
                            break;
                        case EncodeMenuButtons.EncodeMessage:
                            try
                            {
                                encoder.EncodeMessage(_selectedFilePath, _messageToEncode, _selectedAlgorithm);
                                Console.WriteLine("The message was successfully encoded");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.ReadKey();
                                break;
                            }
                            
                        case EncodeMenuButtons.BackToMainMenu:
                            currentMenuStates = MenuStates.MainMenu;
                            break;
                    }
                    break;
                case MenuStates.DecodeMenu:
                    switch (button)
                    {
                        case DecodeMenuButtons.SelectImage: 
                            currentMenuStates = MenuStates.SelectFileToDecodeMenu;
                            break;
                        case DecodeMenuButtons.SelectAlgorithm:
                            currentMenuStates = MenuStates.AlgorithmsDecodeMenu;
                            break;
                        case DecodeMenuButtons.DecodeMessage:
                            try
                            {
                                var message = decoder.DecodeMessage(_selectedFilePath, _selectedAlgorithm);
                                Console.WriteLine($"Decoded Message: {message}");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.ReadKey();
                                break;
                            }
                            
                        case DecodeMenuButtons.BackToMainMenu:
                            currentMenuStates = MenuStates.MainMenu;
                            break;
                    }
                    break;
                case MenuStates.SelectFileToDecodeMenu:
                    if (button == EncodeAlgorithmsButtons.BackToMenu) currentMenuStates = MenuStates.DecodeMenu;
                    _selectedFilePath = button;
                    FileChanged?.Invoke(_selectedFilePath);
                    currentMenuStates = MenuStates.DecodeMenu;
                    break;
                case MenuStates.SelectFileToEncodeMenu:
                    if (button == EncodeAlgorithmsButtons.BackToMenu) currentMenuStates = MenuStates.EncodeMenu;
                    _selectedFilePath = button;
                    FileChanged?.Invoke(_selectedFilePath);
                    currentMenuStates = MenuStates.EncodeMenu;
                    break;
                case MenuStates.AlgorithmsEncodeMenu:
                    switch (button)
                    {
                        case EncodeAlgorithmsButtons.Lsb: 
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.Lsb);
                            break;
                        case EncodeAlgorithmsButtons.AlphaChannel: 
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.AlphaChannel);
                            break;
                        case EncodeAlgorithmsButtons.Palette:
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.Palette);
                            break;
                        case EncodeAlgorithmsButtons.Metadata: 
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.Metadata);
                            break;
                        case EncodeAlgorithmsButtons.Dct:
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.Dct);
                            break;
                        case EncodeAlgorithmsButtons.F5:
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.F5);
                            break;
                        case EncodeAlgorithmsButtons.Eof:
                            currentMenuStates = ChangeEncodeAlgorithm(EncodeAlgorithms.Eof);
                            break;
                        case EncodeAlgorithmsButtons.BackToMenu:
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                    }
                    break;
                case MenuStates.AlgorithmsDecodeMenu:
                    switch (button)
                    {
                        case EncodeAlgorithmsButtons.Lsb: 
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.Lsb);
                            break;
                        case EncodeAlgorithmsButtons.AlphaChannel: 
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.AlphaChannel);
                            break;
                        case EncodeAlgorithmsButtons.Palette:
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.Palette);
                            break;
                        case EncodeAlgorithmsButtons.Metadata: 
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.Metadata);
                            break;
                        case EncodeAlgorithmsButtons.Dct:
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.Dct);
                            break;
                        case EncodeAlgorithmsButtons.F5:
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.F5);
                            break;
                        case EncodeAlgorithmsButtons.Eof:
                            currentMenuStates = ChangeDecodeAlgorithm(EncodeAlgorithms.Eof);
                            break;
                        case EncodeAlgorithmsButtons.BackToMenu:
                            currentMenuStates = MenuStates.DecodeMenu;
                            break;
                    }
                    break;
            }
        }
    }
}