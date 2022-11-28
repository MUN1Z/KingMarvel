using KingMarvel.Database.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using KingMarvel.Domain.Entities;

namespace KingMarvel.Database.Mappings
{
    public class CharacterMapping : EntityTypeConfiguration<Character>
    {
        public override void Map(EntityTypeBuilder<Character> builder)
        {
            builder.HasKey(c => c.Guid);

            builder.Property(e => e.Id)
               .HasColumnType("int")
               .IsRequired();

            builder.Property(e => e.Name)
               .HasColumnType("varchar(255)")
               .IsRequired();

            builder.Property(e => e.Description)
               .HasColumnType("text")
               .IsRequired();

            builder.Property(e => e.Favorite)
               .HasColumnType("bit")
               .IsRequired();

            builder.ToTable("Character");
        }
    }
}
