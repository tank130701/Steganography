using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Service
{
    internal interface IImageEncoder
    {
        void EncodeText(string imagePath, string outputImagePath, string text);
    }
}
