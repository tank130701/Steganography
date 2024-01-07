using System.Buffers.Binary;
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

    public void ReadHuffmanTables()
    {

    }

    public void ProcessSOFData(List<JpegMarker> supportedFrameTypes)
    {
        var SOF = FindSOFMarker();
        if(SOF.Item1==null) throw new Exception("Could not find Start Of Frame marker");
        if(!supportedFrameTypes.Contains((JpegMarker)SOF.Item2)) throw new Exception($"Unsupported frame type: {SOF.Item2}");
        // Get length and everything else
        int currentIndex = (int)SOF.Item1+4;
        int sofDataLength = GetSectionLength((int)SOF.Item1);
        // Note everything in jpeg is big endian, apparently
        // TODO TODO TODO ACTUALLY ACCOUNT FOR MACHINE ENDIANESS LMAO, THIS WORKS ON LITTLE ENDIAN MACHINES
        // Checked with BitConverter.IsLittleEndian
        if(!(_data[currentIndex]== 8)) throw new Exception("Invalid JPEG: Precision must be 8");

        _header.height = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data, currentIndex+1));
        _header.width = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data, currentIndex+3));
        _header.componentCount = _data[currentIndex+5];

        currentIndex+=6;

        for(int i = 0; i<_header.componentCount; i++)
        {
            // TODO Create color component object
        }

    }
    /// <summary>
    /// Gets section length, excluding the length bytes.
    /// Pass index of the marker you found.
    /// </summary>
    /// <param name="index"> Marker index </param>
    /// <returns></returns>
    int GetSectionLength(int index)
    {
        // Length stored in big endian
        return BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data, index+2))-2;
    }
    /// <summary>
    /// Finds the index of given JPEG marker
    /// </summary>
    /// <param name="marker"></param>
    /// <returns>
    /// Starting index of jpeg marker (i.e. index of 0xFF byte preceding the marker)
    /// </returns>
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
