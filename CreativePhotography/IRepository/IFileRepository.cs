using CreativePhotography.Models;

namespace CreativePhotography.IRepository
{
    public interface IFileRepository
    {
        Task<IEnumerable<CategoryModel>> GetCategories();
        Task<IEnumerable<SubcategoryModel>> GetSubCategories(int categoryId);
    }
}
