namespace Steganography.Service.Utils.JPEG;

public class ColorComponent
{
    byte _horizontalSamplingFactor = 1;
    byte _verticalSamplingFactor = 1;
    byte _quantizationTableID = 0;
    int _colorComponentID;
    public int ColorComponentID{get;}
    HuffmanTable _huffmanDCTable;
    public HuffmanTable HuffmanDCTable{get;}
    HuffmanTable _huffmanACTable;
    public HuffmanTable HuffmanACTable{get;}
    bool used = false;
    
    public ColorComponent(){}

    public ColorComponent(int colorComponentID, byte quantizationTableID)
    {
        _colorComponentID = colorComponentID;
        _quantizationTableID = quantizationTableID;
        used=true;
    }
    public void SetHuffmanTable(int tableType, HuffmanTable table)
    {
        if(tableType == 1) _huffmanACTable = table;
        else if(tableType==0) _huffmanDCTable = table;
        else throw new Exception($"Unknown huffman table type: Expected 0 or 1, got {tableType}");
    }
}
