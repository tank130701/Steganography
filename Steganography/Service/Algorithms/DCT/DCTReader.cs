using Steganography.Service.Utils.JPEG;

namespace Steganography.Service.Algorithms.DCT;

public static class DCTReader
{
    internal static string ReadMessageFromImage(byte[] image)
    {
        int messageIndex = 0;
        List<byte> messageBytes = new List<byte>();
        JPEGHeader header = new();
        JpegReader reader = new(image, header);
        List<JPEGHelper.JpegMarker> supportedFrameTypes =  new [JPEGHelper.JpegMarker.StartOfFrame0];
        if(!reader.ContainsValidStartOfImage()) throw new Exception("File does not contain a valid SOI marker");
        reader.ReadSOFData(supportedFrameTypes);
        reader.ReadHuffmanTables();
        reader.ReadRestartInterval();
        var huffmanStream = reader.ReadStartOfScan();
        var huffmanStreamNoMarkers = reader.RemoveMarkersFromHuffmanCodedData(huffmanStream);
        huffmanStream = null;
        GC.Collect();
        var mcuArray = reader.DecodeHuffmanData(huffmanStreamNoMarkers);
        foreach (var mcu in mcuArray)
        {
            for(int i = 0; i<3; i++)
            {
                messageBytes.Add((byte)mcu[i][62]);
                messageIndex++;
            }
        }

        return reader.GetMessageFromMCUs();
    }
}
