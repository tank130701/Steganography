namespace Steganography.Service.Utils;

public class BitStream
{
    private MemoryStream wrapped;
    private int bitPos = -1;
    private int buffer;

    public BitStream(MemoryStream stream) => this.wrapped = stream;

    public IEnumerable<bool> ReadBits()
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
