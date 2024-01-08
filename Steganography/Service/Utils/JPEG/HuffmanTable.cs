namespace Steganography.Service.Utils.JPEG;

public struct HuffmanTable
{
    public HuffmanTable(byte tableID)
    {
        _tableID = tableID;
        _set = true;
    }
    byte _tableID;
    public int[] _offsets = new int[17];
    public byte[] _symbols = new byte[162];
    //uint[] codes = [0];
    bool _set = false;

}
