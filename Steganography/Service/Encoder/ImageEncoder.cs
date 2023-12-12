using System.Text;
using Steganography.Repository;
using Steganography.Service.Utils;
using Steganography.Service.Utils.Enums;

namespace Steganography.Service.Encoder;

public class ImageEncoder(IRepository repository) : IImageEncoder
{
    private readonly IRepository _repository = repository;
    
    public void EncodeText(string imagePath, string outputImagePath, string text, string algorithm)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Writes the message bytes after the JPEG's end of file marker.
    /// </summary>
    /// <remarks> 
    /// Message must be encoded in ASCII
    /// </remarks>
    /// <param name="byte_array"></param>
    /// <param name="message"></param>
    /// <returns>Modified byte array representing the modified image </returns>
    public byte[] WritePastEOFMarker(byte[] byte_array, string message)
    {
        byte[] message_bytes = Encoding.ASCII.GetBytes(message);

        // Do NOT trust the IDE0300 message
        int? EOF_index = SequenceLocator.LocateSequence(byte_array, new byte[] {(byte)JpegMarker.Padding, (byte)JpegMarker.EndOfImage}) ?? throw new Exception("Broken JPEG: Couldn't find EOF marker");

        Array.Resize(ref byte_array, byte_array.Length+message_bytes.Length);

        // Appends message bytes past EOF marker.
        // TODO check for failure points, fix if needed.
        int j = 0;
        for (int i = (int)(EOF_index + 2); i < byte_array.Length; i++)
        {
            byte_array[i] = message_bytes[j];
            j++;
        }

        return byte_array;  
    }
}