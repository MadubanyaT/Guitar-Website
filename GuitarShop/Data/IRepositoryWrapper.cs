namespace GuitarShop.Data
{
    public interface IRepositoryWrapper
    {
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }

        void Save();
    }
}
