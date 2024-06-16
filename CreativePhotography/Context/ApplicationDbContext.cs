using CreativePhotography.Models;
using Microsoft.EntityFrameworkCore;

namespace CreativePhotography.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CategoryModel> category { get; set; }
        public DbSet<SubcategoryModel> subcategory { get; set; }
    }
}
