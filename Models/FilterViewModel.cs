using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NewWebShopApp.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Category> categories, int category, string name)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            categories.Insert(0, new Category { name = "Все", id = 0 });
            Categories = new SelectList(categories, "id", "name", category);
            SelectedCategory = category;
            SelectedName = name;
        }
        public SelectList Categories { get; } // список компаний
        public int SelectedCategory { get; } // выбранная компания
        public string SelectedName { get; } // введенное имя
    }
}
