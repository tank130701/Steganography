namespace Steganography.ConsoleUI
{
    internal static class MainMenuButtons
    {
        public const string EncodeMessage = "Encode message";
        public const string DecodeMessage = "Decode message";
        public const string Exit = "Exit";
    }

    internal static class EncodeMenuButtons
    {
        public const string SelectImage = "Select Image";
        public const string SelectAlgorithm = "Select Algorithm";
        public const string WriteMessage = "Write a message";
        public const string EncodeMessage = "Encode message";
        public const string BackToMainMenu = "Back to Main Menu";

    }

    internal static class DecodeMenuButtons
    {
        public const string SelectImage = "Select Image";
        public const string SelectAlgorithm = "Select Algorithm";
        public const string DecodeMessage = "Decode message";
        public const string BackToMainMenu = "Back to Main Menu";

    }

    internal static class EncodeAlgorithmsButtons
    {
        public const string Lsb = "Least Significant Bit (LSB) (PNG only)";
        public const string AlphaChannel = "Embedding into the alpha channel (Alhpa Channel) (JPEG only)";
        public const string Metadata = "Embedding in metadata (Metadata)";
        public const string Palette = "Using color palettes (Palette)";
        public const string Dct = "Encoding in DCT coefficients (DCT)";
        public const string F5 = "F5 Steganography (F5)";
        public const string Eof = "EOF";
        public const string BackToMenu = "Back to Menu";
    }

}
