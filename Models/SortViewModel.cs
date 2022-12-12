namespace NewWebShopApp.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; } // значение для сортировки по имени
        public SortState PriceSort { get; }    // значение для сортировки по цене
        public SortState CategorySort { get; }   // значение для сортировки по категории
        public SortState Current { get; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            CategorySort = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            Current = sortOrder;
        }
    }
}
