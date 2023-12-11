using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.ConsoleUI
{
    internal static class MainMenuButtons
    {
        public const string TextMainMenuEncodeMessage = "Закодировать сообщение";
        public const string TextMainMenuDecodeMessage = "Декодировать сообщение";
        public const string TextMainMenuExit = "Выход";
    }

    internal static class EncodeMenuButtons
    {
        public const string TextEncodeMenuSelectImage = "Выбрать изображение";
        public const string TextEncodeMenuSelectAlogrithm = "Выбрать алгоритм";
        public const string TextEncodeMenuWriteMessage = "Написать сообщение";
        public const string TextEncodeMenuEncodeMessage = "Закодировать сообщение";

    }

    internal static class DecodeMenuButtons
    {
        public const string TextDecodeMenuSelectImage = "Выбрать изображение";
        public const string TextDecodeMenuSelectAlogrithm = "Выбрать алгоритм";
        public const string TextDecodeMenuDecodeMessage = "Декодировать сообщение";

    }


}
