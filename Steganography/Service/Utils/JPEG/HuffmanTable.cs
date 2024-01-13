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
    public uint[] _offsets = new uint[17];
    public byte[] _symbols = new byte[162];
    public uint[] _codes = new uint[162];
    bool _set = false;
    public bool Set{get;}

    public void GenerateCodeList()
    {
        uint code = 0;
        for (uint i = 0; i<16; i++)
        {
            for (uint j = _offsets[i]; j<_offsets[i+1]; j++)
            {
                _codes[j] = code;
                code++;
            }
            code <<= 1;
        }
    }

}
