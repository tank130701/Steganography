namespace Steganography.Service.Utils;

public struct HuffmanTable
{
    public HuffmanTable(){}
    byte[] offsets = [0];
    byte[] symbols = [0];
    uint[] codes = [0];
    bool set = false;
}
