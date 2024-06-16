using CreativePhotography.Context;
using CreativePhotography.IRepository;
using CreativePhotography.Models;
using Microsoft.EntityFrameworkCore;

namespace CreativePhotography.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryModel>> GetCategories()
        {
            return await _context.category.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<SubcategoryModel>> GetSubCategories(int categoryId)
        {
            return await _context.subcategory.AsNoTracking().Where(s => s.CategoryId == categoryId).ToListAsync();
        }
    }
}
