using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.ConsoleUI
{
    internal static class MainMenuButtons
    {
        public const string EncodeMessage = "Закодировать сообщение";
        public const string DecodeMessage = "Декодировать сообщение";
        public const string Exit = "Выход";
    }

    internal static class EncodeMenuButtons
    {
        public const string SelectImage = "Выбрать изображение";
        public const string SelectAlgorithm = "Выбрать алгоритм";
        public const string WriteMessage = "Написать сообщение";
        public const string EncodeMessage = "Закодировать сообщение";
        public const string BackToMainMenu = "Вернуться в главное меню";

    }

    internal static class DecodeMenuButtons
    {
        public const string SelectImage = "Выбрать изображение";
        public const string SelectAlgorithm = "Выбрать алгоритм";
        public const string DecodeMessage = "Декодировать сообщение";
        public const string BackToMainMenu = "Вернуться в главное меню";

    }

    internal static class EncodeAlgorithmsButtons
    {
        public const string Lsb = "Least Significant Bit (LSB)";
        public const string AlphaChannel = "Embedding into the alpha channel (Alhpa Channel)";
        public const string Metadata = "Embedding in metadata (Metadata)";
        public const string Palette = "Using color palettes (Palette)";
        public const string Dct = "Encoding in DCT coefficients (DCT)";
        public const string F5 = "F5 Steganography (F5)";
        public const string Eof = "EOF";
        public const string BackToMenu = "Вернуться в главное меню";
    }

}
