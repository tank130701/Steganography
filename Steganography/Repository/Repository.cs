namespace Steganography.Repository;

public class Repository : IRepository
{
    public void SaveImage()
    {
        throw new NotImplementedException();
    }

    public void LoadImage()
    {
        throw new NotImplementedException();
    }
    
    public List<string> GetImageFilesInDirectory(string directoryPath)
    {
        List<string> imageFileList = new List<string>();
        const string root = "./";
        try
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            foreach (string extension in imageExtensions)
            {
                string[] files = Directory.GetFiles(root + directoryPath, "*" + extension);
                imageFileList.AddRange(files);
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"Error when getting the list of images: {ex.Message}", ex);
        }

        return imageFileList;
    }
}