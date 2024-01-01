using System.Text;
using Steganography.Service.Utils;
using Steganography.Service.Utils.Enums;

namespace Steganography.Service.Algorithms.EOF;

internal static class EOFReader
{
    /// <summary>
    /// Reads message bytes after the JPEG's end of file marker.
    /// </summary>
    /// <remarks> 
    /// Message must be encoded in ASCII
    /// </remarks>
    /// <param name="byteArray"></param>
    /// <param name="message"></param>
    /// <returns>Message string past the EOF marker</returns>
    internal static string? ReadPastEOFMarker(byte[] byteArray)
    {
        int? EOFIndex = SequenceLocator.LocateSequence(byteArray, new byte[] {(byte)JpegMarker.Padding, (byte)JpegMarker.EndOfImage}) ?? throw new Exception("Broken JPEG: Couldn't find EOF marker");
        
        int messageLengthInBytes = byteArray.Length - (int)EOFIndex - 2;

        byte[] messageBytes = new byte[messageLengthInBytes];

        // TODO check for failure points
        int j = 0;
        for (int i = (int)EOFIndex+2; i < byteArray.Length; i++)        
        {
            messageBytes[j] = byteArray[i];
            j++;
        }
        
        //TODO check for failure points
        string message = Encoding.ASCII.GetString(messageBytes);

        return message;
    }
}
