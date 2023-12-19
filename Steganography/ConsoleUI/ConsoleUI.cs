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
            ConsoleMenu selectingFileToEncodeMenu = new ConsoleMenu(
                "Select File",
                filesToEncode,
                selectingFileToEncodeInfo
                );
            
            // Info selectingFileToDecodeInfo = null;
            // var filesToDecode = decoder.GetImageFilesInDirectory();
            // ConsoleMenu selectingFileToDecodeMenu = new ConsoleMenu(
            //     "Select File",
            //     filesToDecode,
            //     selectingFileToDecodeInfo
            // );
            
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
                    case MenuStates.AlgorithmsMenu:
                        button = algorithmsMenu.DisplayMenu();
                        break;
                    case MenuStates.SelectFileToEncodeMenu:
                        button = selectingFileToEncodeMenu.DisplayMenu();
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
                            currentMenuStates = MenuStates.AlgorithmsMenu;
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
                            encoder.EncodeMessage(_selectedFilePath, _selectedAlgorithm, _messageToEncode);
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
                            currentMenuStates = MenuStates.SelectFileToDecodeMenu;
                            break;
                        case DecodeMenuButtons.SelectAlgorithm:
                            currentMenuStates = MenuStates.AlgorithmsMenu;
                            break;
                        case DecodeMenuButtons.DecodeMessage:
                            var message = decoder.DecodeMessage(_selectedFilePath, _selectedAlgorithm);
                            Console.WriteLine($"Decoded Message: {message}");
                            break;
                        case DecodeMenuButtons.BackToMainMenu:
                            currentMenuStates = MenuStates.MainMenu;
                            break;
                    }
                    break;
                case MenuStates.SelectFileToDecodeMenu:
                    _selectedFilePath = button;
                    FileChanged?.Invoke(_selectedFilePath);
                    currentMenuStates = MenuStates.DecodeMenu;
                    break;
                case MenuStates.SelectFileToEncodeMenu:
                    _selectedFilePath = button;
                    FileChanged?.Invoke(_selectedFilePath);
                    currentMenuStates = MenuStates.EncodeMenu;
                    break;
                case MenuStates.AlgorithmsMenu:
                    switch (button)
                    {
                        case EncodeAlgorithmsButtons.Lsb:
                            _selectedAlgorithm = EncodeAlgorithms.Lsb;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.AlphaChannel:
                            _selectedAlgorithm = EncodeAlgorithms.AlphaChannel;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Palette:
                            _selectedAlgorithm = EncodeAlgorithms.Palette;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Metadata:
                            _selectedAlgorithm = EncodeAlgorithms.Metadata;
                             AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Dct:
                            _selectedAlgorithm = EncodeAlgorithms.Dct;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.F5:
                            _selectedAlgorithm = EncodeAlgorithms.F5;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
                            currentMenuStates = MenuStates.EncodeMenu;
                            break;
                        case EncodeAlgorithmsButtons.Eof:
                            _selectedAlgorithm = EncodeAlgorithms.Eof;
                            AlgorithmChanged?.Invoke(_selectedAlgorithm);
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