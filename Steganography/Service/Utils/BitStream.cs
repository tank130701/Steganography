namespace Steganography.Service.Utils;

public class BitStream
{
    private MemoryStream wrapped;
    private int bitPos = -1;
    private int buffer;

    public BitStream(MemoryStream stream) => this.wrapped = stream;

    public IEnumerable<int> ReadBits()
    {
        do
        {
            while (bitPos >= 0)
            {
                yield return (buffer & (1 << bitPos--)) > 0 ? 1 : 0;
            }
            buffer = wrapped.ReadByte();
            bitPos = 7;  
        } while (buffer > -1);
    }

    public IEnumerable<bool> ReadBitsAsBool()
    {
        do
        {
            while (bitPos >= 0)
            {
                yield return (buffer & (1 << bitPos--)) > 0;
            }
            buffer = wrapped.ReadByte();
            bitPos = 7;  
        } while (buffer > -1);
    }
}
