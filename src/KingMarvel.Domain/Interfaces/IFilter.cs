namespace KingMarvel.Domain.Interfaces
{
    public interface IFilter
    {
        void Validate();
        int GetPageNumber();
        int GetPageSize();
    }
}
