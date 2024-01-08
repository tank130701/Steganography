namespace Steganography.Service.Utils.JPEG;

public struct JPEGHeader
{
    public HuffmanTable[] huffmanDCTables = new HuffmanTable[4];
    public HuffmanTable[] huffmanACTables = new HuffmanTable[4];
    public ColorComponent[] colorComponents = new ColorComponent[3];
    public int restartInterval = 0;
    public byte frameType;
    public int width;
    public int height;
    public byte componentCount;
    bool valid = false;
    public JPEGHeader()
    {

    }
}
