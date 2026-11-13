using day3_TrinhVanChinh.Models;

namespace day3_TrinhVanChinh.Data
{
    public static class CategoryData
    {
        public static List<Category> Categories { get; } = new()
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" },
            new Category { Id = 3, Name = "Category 3" }
        };
    }
}
