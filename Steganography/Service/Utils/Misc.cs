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
}
