namespace KingMarvel.Application.ViewModels.Response
{
    [Serializable]
    public class CharacterResponseViewModel : IViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumb { get; set; }

        public bool Favorite { get; set; }
    }
}
