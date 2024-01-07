using Microsoft.VisualBasic;
using static Steganography.Service.Utils.JPEG.JPEGHelper;

namespace Steganography.Service.Utils.JPEG;

public class JpegReader
{
    readonly byte[] _data;
    JPEGHeader _header;

    /// <summary>
    /// Checks if the first 2 bytes are a start of image marker
    /// </summary>
    /// <returns></returns>
    public bool ContainsValidStartOfImage()
    {
        if(FindJPEGMarker(JpegMarker.StartOfImage).Item1!=0) return false;
        return true;
    }
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

    /// <summary>
    /// Tries to read a JPEG marker after a given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns>JPEG Marker enum, null if no marker is present</returns>
    JpegMarker? TryReadMarker(uint index)
    {
        if(TryPeekMarker(index)==false) return null;
        // Possible failure point
        return (JpegMarker)_data[index+1];
    }

    public (int?, JpegMarker?) FindJPEGMarker(JpegMarker marker)
    {
        byte[] markerSequence = [(byte)JpegMarker.Padding, (byte)marker];
        int? index = SequenceLocator.LocateSequence<byte>(_data, markerSequence);
        if(index!=null) return (index, marker);
        return (null, null);
    }

    /// <summary>
    /// Used to find the SOF (Start of frame) marker
    /// </summary>
    /// <returns>
    /// (index, JpegMarker) If successful, (null,null) otherwise 
    /// </returns>
    /// <exception cref="Exception"></exception>
    public (int?, JpegMarker?) FindSOFMarker()
    {
        bool foundSOFMarker = false;
        (int?, JpegMarker?) returnVal = (null,null);
        JpegMarker[] SOFMarkers = 
        {
            JpegMarker.StartOfFrame0, JpegMarker.StartOfFrame1, JpegMarker.StartOfFrame2, 
            JpegMarker.StartOfFrame3, JpegMarker.StartOfFrame5, JpegMarker.StartOfFrame6,
            JpegMarker.StartOfFrame7, JpegMarker.StartOfFrame9, JpegMarker.StartOfFrame10,
            JpegMarker.StartOfFrame11, JpegMarker.StartOfFrame13, JpegMarker.StartOfFrame14,
            JpegMarker.StartOfFrame15
        };
        // Very inefficient, searches through the entire array 13 times, but i cba :)
        foreach (var marker in SOFMarkers)
        {
            int? index = FindJPEGMarker(marker).Item1;
            if(foundSOFMarker == true && index != null) throw new Exception("Invalid JPEG: Multiple SOF Markers found");
            if(index!= null)
            {
                foundSOFMarker = true;
                returnVal = ((int)index, marker);
            }
        }
        return returnVal;
    }
}
