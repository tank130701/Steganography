﻿namespace Steganography.Service.Utils.JPEG;

public struct JPEGHeader
{
    public HuffmanTable[]? huffmanDCTable;
    public HuffmanTable[]? huffmanACTable;
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
