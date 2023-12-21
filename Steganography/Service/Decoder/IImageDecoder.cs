using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Service
{
    internal interface IImageDecoder
    {
        void DecodeText(string imagePath);
    }
}