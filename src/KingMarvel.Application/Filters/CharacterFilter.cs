using KingMarvel.Domain.Interfaces;

namespace KingMarvel.Application.Filters
{
    public class CharacterFilter : IFilter
    {
        #region properties

        public string? Name { get; set; }
        public bool? Favorite { get; set; }

        #endregion

        #region constructors

        public CharacterFilter() : base()
        {
            Name = null;
            Favorite = null;
        }

        public CharacterFilter(string? name = "", bool? favorite = null) : base()
        {
            Name = name;
            Favorite = favorite;
        }

        public void Validate()
        {
            throw new NotImplementedException();
        }

        public int GetPageNumber()
        {
            throw new NotImplementedException();
        }

        public int GetPageSize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
