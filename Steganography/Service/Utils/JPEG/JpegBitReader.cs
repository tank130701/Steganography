using System.Collections;

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

    public int ReadBits(int bitCount)
    {
        var bits = new BitArray(_bitStream.ReadBitsAsBool().Take(bitCount).ToArray());
        // TODO might cause problems with endianess
        int[] array = new int[1];
        bits.CopyTo(array,0);
        return array[0];
    }

    private int GetIntFromBitArray(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 32 bits.");

        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];

    }

    internal byte GetNextSymbol(HuffmanTable table)
    {
        // TODO HANDLE CASE WHERE YOU COULDN'T READ A BIT, SINCE THAT WILL HAPPEN AT THE END OF THE STREAM
        uint code = 0;
        for(int i = 0; i<16; i++)
        {
            uint bit = (uint)_bitStream.ReadBits().Take(1).ElementAt(0);
            code = (code << 1) | bit;
            for(uint j = table._offsets[i];j<table._offsets[i+1];j++)
            {
                if(code == table._codes[j])return table._symbols[j];
            }
        }

        throw new Exception("Could not find matching huffman symbol");
        
    }

    // TODO implement
    internal void Align()
    {
        _bitStream.Align();
    }
}
