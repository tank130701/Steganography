namespace Steganography.Service.Utils.JPEG;

public struct ColorComponent
{
    byte _horizontalSamplingFactor = 1;
    byte _verticalSamplingFactor = 1;
    byte _quantizationTableID = 0;
    int _colorComponentID;
    bool used = false;
    
    public ColorComponent(){}

    public ColorComponent(int colorComponentID, byte quantizationTableID)
    {
        _colorComponentID = colorComponentID;
        _quantizationTableID = quantizationTableID;
        used=true;
    }
}
