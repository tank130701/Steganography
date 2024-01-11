using System.Buffers.Binary;
using System.Net.Sockets;
using static Steganography.Service.Utils.JPEG.JPEGHelper;

namespace Steganography.Service.Utils.JPEG;

public class JpegReader
{
    readonly byte[] _data;
    JPEGHeader _header;
    List<int> _supportedComponentCount = [1,3];
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
    
    /// <summary>
    /// Tries reading the start of scan section and returns a byte array of the huffman coded stream
    /// </summary>
    /// <returns> Byte[] containing the huffman coded data</returns>
    /// <exception cref="Exception"></exception>
    public List<byte> ReadStartOfScan()
    {
        var SOS = FindJPEGMarker(JpegMarker.StartOfScan);
        if(SOS.Item1 == null) throw new Exception("Invalid JPEG: Start of scan marker not found");
        int scanSectionLength = GetSectionLength((int)SOS.Item1);
        int currentIndex = (int)SOS.Item1+4;
        // Post ++ to avoid currentIndex+=1 after.
        int numberOfComponents = _data[currentIndex++];

        // TODO use this, maybe make a hash map or something
        // Relation between colour components and Huffman table IDs used for each component with 
        // The ID you read
        // DONE, NO HASH MAPS
        for(int i = 0; i<numberOfComponents; i++)
        {
            int componentID = _data[currentIndex++];
            var (tableType, tableID) = _data[currentIndex++].SplitIntoNibbles();
            var colorComponent = _header.GetColorComponentByID(componentID);
            colorComponent.SetHuffmanTable(tableType,_header.GetHuffmanTableByID(tableType,tableID));
        }
        int startOfSelection = _data[currentIndex++];
        int endOfSelection = _data[currentIndex++];

        if(endOfSelection-startOfSelection!=63) throw new Exception($"Unsupported MCU size - 8x8 block expected, got selection with starting index {startOfSelection}; ending index {endOfSelection}");

        // Not used in baseline JPEG but might as well store for later
        // Is actually split into nibbles but idc as long as its 0
        int successiveApproximation = _data[currentIndex++];
        if(successiveApproximation != 0) throw new Exception("Successive approximation byte is not equal to 0");

        // TODO Ideally write out read data for troubleshooting purposes
        if(currentIndex-scanSectionLength != (int)SOS.Item1+4) throw new Exception("Start of scan length mismatch");

        // Finished reading Start of Scan section, begin reading huffman coded bitstream located past this

       

        byte[] huffmanCodedDataArray = new byte[_data.Length-currentIndex];

        Array.Copy(_data,currentIndex,huffmanCodedDataArray,0, _data.Length-currentIndex);

        List<byte> huffmanCodedData = new(huffmanCodedDataArray);

        // TODO note if things go wrong.
        // Now that i think of it, i should just read here and remove markers by appending
        // to the list if a marker is encountered so i don't read the data a total of 3 times
        huffmanCodedDataArray = null;
        GC.Collect();

        return huffmanCodedData;

    }

    public List<byte>RemoveMarkersFromHuffmanCodedData(List<byte>huffmanCodedData)
    {
        return null;
    }

    /// <summary>
    /// Tries to read huffman tables and populate related JPEG header fields
    /// TODO METHOD TO DISPLAY TABLES FOR TESTING
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void ReadHuffmanTables()
    {
        var DHT = FindJPEGMarker(JpegMarker.DefineHuffmanTable);
        if(DHT.Item1 == null || DHT.Item2 == null) throw new Exception("Could not find DHT marker");
        int currentIndex = (int)DHT.Item1+4;
        int DHTDataLength = GetSectionLength((int)DHT.Item1);
        while(DHTDataLength>0)
        {
            var (upperNibble, lowerNibble) = _data[currentIndex].SplitIntoNibbles();
            bool isACTable = upperNibble == 1? true : false;
            var table = new HuffmanTable(lowerNibble);

            ++currentIndex;

            // TODO Read and set data
            
            int symbolCount = 0;
            table._offsets[0] = symbolCount;
            
            // Populate offsets array. Offset array is used to calculate the amount of huffman codes of given length
            // Where index of offset array is the huffman code length, and the value at that index
            // Is the total count of huffman codes before (and including? Double check, im bad with data structures)
            // TODO MAKE SURE YOU ACCOUNT FOR THE LAST ELEMENT IN THE OFFSET ARRAY, IT HAS TO BE A DUMMY ONE SO WE CAN GET
            // THE CORRECT COUNT FOR 16 BIT LONG HUFFMAN CODES
            // I fucking smell an error here with the indexes
            for(int i = 1; i<=16; i++)
            {
                symbolCount += _data[currentIndex+i];
                table._offsets[i] = symbolCount;
                currentIndex++;
            }

            if(symbolCount>162) throw new Exception($"Too many symbols in Huffman table with id {lowerNibble}");

            // Read actual huffman symbols and populate huffman table's symbol array
            for (int i = 0; i < symbolCount; i++)
            {
                table._symbols[i] = _data[currentIndex+i];
                currentIndex++;
            }

            if(isACTable)
            {
                _header._huffmanACTables[lowerNibble] = table;
            }else
            {
                _header._huffmanDCTables[lowerNibble] = table;
            }
            // TODO Decrease by actual number of bytes read, not by 1
            // This should be correct
            DHTDataLength -= 17+symbolCount;
        }
    }

    /// <summary>
    /// Tries to read DRI marker and fill header data accordingly
    /// </summary>
    /// <exception cref="Exception"></exception>
    public void ReadRestartInterval()
    {
        var (index, marker) = FindJPEGMarker(JpegMarker.DefineRestartInterval);
        if(index==null) throw new Exception("Restart interval not defined");
        int length = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data,(int)index+2));
        if(length!=4) throw new Exception($"Invalid DRI section length, expected: 4, found: {length}");
        index+=2;
        int restartInterval = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data,(int)index));
        // TODO error checking, in case there is a max restart interval.
        _header._restartInterval = restartInterval;
    }
    /// <summary>
    /// Finds and reads start of frame marker, and tries to read related data from its section
    /// </summary>
    /// <param name="supportedFrameTypes"></param>
    /// <exception cref="Exception"></exception>
    public void ReadSOFData(List<JpegMarker> supportedFrameTypes)
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

        _header._height = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data, currentIndex+1));
        _header._width = BinaryPrimitives.ReverseEndianness(BitConverter.ToUInt16(_data, currentIndex+3));
        _header._componentCount = _data[currentIndex+5];

        if(!_supportedComponentCount.Contains(_header._componentCount)) throw new Exception("Unsupported amount of colour channels");

        currentIndex+=6;

        // Initialise color component objects
        for(int i = 0; i<_header._componentCount; i++)
        {
            int componentID = _data[currentIndex];
            // Color components can sometimes be zero based, but i won't allow that
            if(componentID<=0 || componentID>3) throw new Exception($"Invalid component ID: {componentID}");
            currentIndex+=1;
            var (upperNibble, lowerNibble) = _data[currentIndex].SplitIntoNibbles();
            byte horizontalSamplingFactor = upperNibble;
            byte verticalSamplingFactor = lowerNibble;
            if(horizontalSamplingFactor != 1 || verticalSamplingFactor!=1) throw new Exception("Unsupported sampling factor, must be 1");
            currentIndex+=1;
            byte quantizationTableID = _data[currentIndex];
            _header._colorComponents[i] = new(i+1,quantizationTableID);
            if(sofDataLength - 5 - (3*_header._componentCount)!=0) throw new Exception("SOF Section length did not match with actual section length");
            currentIndex+=1;
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
