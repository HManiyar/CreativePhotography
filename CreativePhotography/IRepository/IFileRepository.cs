using PixelCreator.Models;

namespace PixelCreator.IRepository
{
    public interface IFileRepository
    {
        Task<IEnumerable<CategoryModel>> GetCategories();
        Task<IEnumerable<SubcategoryModel>> GetSubCategories(int categoryId);
    }
}
