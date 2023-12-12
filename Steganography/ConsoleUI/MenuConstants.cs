using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.ConsoleUI
{
    internal static class MainMenuButtons
    {
        public const string MainMenuEncodeMessage = "Закодировать сообщение";
        public const string MainMenuDecodeMessage = "Декодировать сообщение";
        public const string MainMenuExit = "Выход";
    }

    internal static class EncodeMenuButtons
    {
        public const string EncodeMenuSelectImage = "Выбрать изображение";
        public const string EncodeMenuSelectAlgorithm = "Выбрать алгоритм";
        public const string EncodeMenuWriteMessage = "Написать сообщение";
        public const string EncodeMenuEncodeMessage = "Закодировать сообщение";

    }

    internal static class DecodeMenuButtons
    {
        public const string DecodeMenuSelectImage = "Выбрать изображение";
        public const string DecodeMenuSelectAlgorithm = "Выбрать алгоритм";
        public const string DecodeMenuDecodeMessage = "Декодировать сообщение";

    }


}
