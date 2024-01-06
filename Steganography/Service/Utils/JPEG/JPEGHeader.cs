namespace Steganography.Service.Utils;

public struct JPEGHeader
{
    public HuffmanTable[]? DCTable;
    public HuffmanTable[]? ACTable;
    public ColorComponent[]? colorComponents;
    public uint restartInterval = 0;
    public byte frameType;
    public int width;
    public int height;
    public byte componentCount;
    bool valid = false;

    public JPEGHeader(){}
}
