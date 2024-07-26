using Fiorella.App.Dtos.Blog;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Employee;
using Fiorella.App.Dtos.Product;

namespace Fiorella.App.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<CategoryGetDto> Categories { get; set; } = [];
        public ICollection<BlogGetDto> Blogs { get; set; } = [];
        public ICollection<EmployeeGetDto> Employees { get; set; } = [];
        public ICollection<ProductGetDto> Products { get; set; } = [];
    }
}
