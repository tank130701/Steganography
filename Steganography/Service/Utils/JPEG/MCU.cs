using System.Text;

namespace Steganography.Service.Utils.JPEG;

public struct MCU
{
    int[] _y  =     new int[64];
    int[] _cb =     new int[64];
    int[] _cr =     new int[64];

    public int[] this[int componentID]
    {
        get
        {
            switch(componentID)
            {
                case 0:
                    return _y;
                case 1:
                    return _cb;
                case 2:
                    return _cr;
                default:
                    throw new Exception($"Invalid component id: expected 0,1 or 2; got {componentID}");
            }
        }
        set
        {
            switch(componentID)
            {
                case 0:
                    _y = value;
                    break;
                case 1:
                    _cb = value;
                    break;
                case 2:
                    _cr= value;
                    break;
                default:
                    throw new Exception($"Invalid component id: expected 0,1 or 2; got {componentID}");
            }
        }
    }

    public int this[(int componentID, int index) inputTuple]
    {
        get
        {
            switch(inputTuple.componentID)
            {
                case 0:
                    return _y[inputTuple.index];
                case 1:
                    return _cb[inputTuple.index];
                case 2:
                    return _cr[inputTuple.index];
                default:
                    throw new Exception($"Invalid component id: expected 0,1 or 2; got {inputTuple.index}");
            }
        }
        set
        {
            switch(inputTuple.componentID)
            {
                case 0:
                    _y[inputTuple.index] = value;
                    break;
                case 1:
                    _cb[inputTuple.index] = value;
                    break;
                case 2:
                    _cr[inputTuple.index] = value;
                    break;
                default:
                    throw new Exception($"Invalid component id: expected 0,1 or 2; got {inputTuple.index}");
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("Y");
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j<8; j++)
            {
                sb.Append(_y[i*8+j]);
            }
            sb.AppendLine("");
        }
        sb.AppendLine("Cb");
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j<8; j++)
            {
                sb.Append(_cb[i*8+j]);
            }
            sb.AppendLine("");
        }
        sb.AppendLine("Cr");
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j<8; j++)
            {
                sb.Append(_cr[i*8+j]);
            }
            sb.AppendLine("");
        }
        return sb.ToString();
    }

    public MCU(){}

}
