
using System.Text;
using Steganography.Service.Utils.JPEG;

namespace Steganography.Service.Algorithms;

public static class DCTWriter
{
    public static byte[] WriteStegoMessage(byte[] image, string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        int messageIndex = 0;
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
            if(messageIndex>=messageBytes.Length) break;
            for(int i = 0; i<3; i++)
            {
                mcu[i][62] = messageBytes[messageIndex];
                messageIndex++;
            }
        }

        return reader.EncodeHuffmanData(mcuArray,messageBytes.Length, image);
    }
}
