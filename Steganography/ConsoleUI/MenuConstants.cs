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


}
