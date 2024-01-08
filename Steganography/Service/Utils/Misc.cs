namespace Steganography.Service.Utils;

public static class Misc
{
    /// <summary>
    /// Extension method to get a specific bit from a byte
    /// </summary>
    /// <param name="b"></param>
    /// <param name="bitNumber"></param>
    /// <returns></returns>
    public static bool GetBit(this byte b, int bitNumber)
    {
        return (b & (1 << bitNumber)) != 0;
    }

    /// <summary>
    /// Splits byte into 2 bytes, with values representing the 4 bit nibbles
    /// </summary>
    /// <param name="b"></param>
    /// <returns>Tuple of upper and lower nibbles</returns>
    public static (byte upperNibble, byte lowerNibble) SplitIntoNibbles(this byte b)
    {
        byte upperNibble = (byte)   ((b & 0xF0) >> 4);
        byte lowerNibble = (byte)   (b & 0x0F);
        return (upperNibble, lowerNibble);
    }
}
