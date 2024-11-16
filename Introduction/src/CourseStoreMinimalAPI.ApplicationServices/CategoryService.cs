using CourseStoreMinimalAPI.Entities;

namespace CourseStoreMinimalAPI.ApplicationServices;

public class CategoryService
{
    public List<Category> GetCategories()
    {

        return
        [
             new Category {Id = 1,Name="Software Engineering"},
             new Category {Id = 2,Name=".NET Development"},
             new Category {Id = 3,Name="AI"},
             new Category {Id = 4,Name="Databse"},
             new Category {Id = 5,Name="softskils"}
        ];
    }
}
