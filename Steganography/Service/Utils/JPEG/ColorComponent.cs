namespace Steganography.Service.Utils.JPEG;

public struct ColorComponent
{
    byte _horizontalSamplingFactor = 1;
    byte _verticalSamplingFactor = 1;
    byte _quantizationTableID = 0;
    bool used = false;
    
    public ColorComponent(){}
}
