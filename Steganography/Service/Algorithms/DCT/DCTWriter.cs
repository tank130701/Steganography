
using Steganography.Service.Utils.JPEG;

namespace Steganography.Service.Algorithms;

public static class DCTWriter
{
    public static byte[] WriteStegoMessage(byte[] image)
    {
        JPEGHeader header = new();
        JpegReader reader = new(image, header);
        List<JPEGHelper.JpegMarker> supportedFrameTypes= [JPEGHelper.JpegMarker.StartOfFrame0];
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
            System.Console.WriteLine(mcu.ToString());
        }

        return image;
    }
}
