namespace Steganography.Service.Utils.JPEG;

public struct HuffmanTable
{
    public HuffmanTable(){}
    byte tableID;
    byte[] offsets = [0];
    byte[] symbols = [0];
    uint[] codes = [0];
    bool set = false;
}
