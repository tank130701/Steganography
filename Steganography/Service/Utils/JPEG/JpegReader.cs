namespace Steganography.Service.Utils.JPEG;

public class JpegReader
{
    readonly byte[] _data;
    JPEGHeader _header;

    /// <summary>
    /// Checks if a marker might be present after given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns>True if marker might be preset
    /// false if padding 0x00 is encountered
    /// </returns>
    bool TryPeekMarker(uint index)
    {
        if(_data[index+1]==(byte)0x00) return false;
        return true;
    }
}
