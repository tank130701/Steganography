using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steganography.Repository
{
    public interface IRepository
    {
        void SaveImage();
        void LoadImage();
        List<string> GetImageFilesInDirectory(string directoryPath);
    }
}