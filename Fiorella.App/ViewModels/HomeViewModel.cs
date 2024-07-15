using Fiorella.App.Models;

namespace Fiorella.App.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
        public ICollection<Employee>? Employees { get; set; }

    }
}
