namespace Steganography.Service.Utils.JPEG;

public struct JPEGHeader
{
    public HuffmanTable[] _huffmanDCTables = new HuffmanTable[4];
    public HuffmanTable[] _huffmanACTables = new HuffmanTable[4];
    public ColorComponent[] _colorComponents = new ColorComponent[3];
    public int _restartInterval = 0;
    public byte _frameType;
    public int _width;
    public int _height;
    public byte _componentCount;
    bool _valid = false;
    public JPEGHeader()
    {

    }

    // TODO i might be retarded but this should be fine since color component is a ref type now
    public ColorComponent GetColorComponentByID(int componentID)
    {
        // TODO Use first or default to actually check if its valid, component might not exist
        var component = _colorComponents.Where(colorComponent => colorComponent.ColorComponentID == componentID).First();
        return component;
    }
    public HuffmanTable GetHuffmanTableByID(int tableType, int huffmanTableID)
    {
        if(tableType == 1) return GetHuffmanACTableByID(huffmanTableID);
        if(tableType==0) return GetHuffmanDCTableByID(huffmanTableID);
        throw new Exception($"Failed to get huffman table of type {tableType} and id {huffmanTableID}");
    }
    HuffmanTable GetHuffmanACTableByID(int huffmanTableID)
    {
        var table = _huffmanACTables.Where(hTable => hTable.TableID == huffmanTableID).First();
        return table;
    }
    HuffmanTable GetHuffmanDCTableByID(int huffmanTableID)
    {
        var table = _huffmanDCTables.Where(hTable => hTable.TableID == huffmanTableID).First();
        return table;
    }
}
