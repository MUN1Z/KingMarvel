namespace KingMarvel.Domain.Entities
{
    public class Character : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumb { get; set; }
        public bool Favorite { get; set; }
    }
}
