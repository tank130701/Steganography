namespace Steganography.Service.Utils.JPEG;

public class HuffmanTable
{
    public HuffmanTable(byte tableID)
    {
        _tableID = tableID;
        _set = true;
    }
    byte _tableID;
    public byte TableID{get;}
    public int[] _offsets = new int[17];
    public byte[] _symbols = new byte[162];
    //uint[] codes = [0];
    bool _set = false;

}
