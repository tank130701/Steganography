namespace Steganography.Service.Utils.JPEG;

public class JpegBitReader
{
    BitStream _bitStream;
    JPEGHeader _header;

    public JpegBitReader(byte[] huffmanDataArray, JPEGHeader header)
    {
        MemoryStream memoryStream = new(huffmanDataArray);
        _bitStream = new(memoryStream);
        _header = header;
    }

    

}
