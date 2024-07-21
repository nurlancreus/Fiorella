using Fiorella.App.Dtos.Blog;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Employee;

namespace Fiorella.App.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Categories = new HashSet<CategoryGetDto>();
            Blogs = new HashSet<BlogGetDto>();
            Employees = new HashSet<EmployeeGetDto>();
        }
        public ICollection<CategoryGetDto> Categories { get; set; }
        public ICollection<BlogGetDto> Blogs { get; set; }
        public ICollection<EmployeeGetDto> Employees { get; set; }

    }
}
