using System.Text;
using Steganography.Service.Utils;
using static Steganography.Service.Utils.JPEG.JPEGHelper;

namespace Steganography.Service.Algorithms.EOF;

public static class EOFWriter
{
    /// <summary>
    /// Writes the message bytes after the JPEG's end of file marker.
    /// </summary>
    /// <remarks> 
    /// Message must be encoded in ASCII
    /// </remarks>
    /// <param name="byteArray"></param>
    /// <param name="message"></param>
    /// <returns>Modified byte array representing the modified image </returns>
    public static byte[] WritePastEOFMarker(byte[] byteArray, string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);

        // Do NOT trust the IDE0300 message
        int? EOFIndex = SequenceLocator.LocateSequence(byteArray, new byte[] {(byte)JpegMarker.Padding, (byte)JpegMarker.EndOfImage}) ?? throw new Exception("Broken JPEG: Couldn't find EOF marker");

        Array.Resize(ref byteArray, byteArray.Length+messageBytes.Length);

        // Appends message bytes past EOF marker.
        // TODO check for failure points, fix if needed.
        int j = 0;
        for (int i = (int)(EOFIndex + 2); i < byteArray.Length; i++)
        {
            byteArray[i] = messageBytes[j];
            j++;
        }

        return byteArray;  
    }
}