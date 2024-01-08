namespace Steganography.Service.Utils.JPEG;

public struct HuffmanTable
{
    public HuffmanTable(byte tableID)
    {
        _tableID = tableID;
        _set = true;
    }
    byte _tableID;
    byte[] _offsets = new byte[17];
    byte[] _symbols = new byte[162];
    //uint[] codes = [0];
    bool _set = false;

}
