using PixelCreator.Models;

namespace PixelCreator.IService
{
    public interface IFileService
    {
        Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType, string categoryName, string subCategoryName);
        Task<IEnumerable<CategoryModel>> GetCategories();
        Task<IEnumerable<SubcategoryModel>> GetSubCategories(int categoryId);
        (List<string> originalImagePaths, List<string> compressedImagePaths) GetRequestedImages(string categoryName, string subCategoryName);
        string DeleteImage(string imagePath, IWebHostEnvironment _hostingEnvironment);
    }
}
