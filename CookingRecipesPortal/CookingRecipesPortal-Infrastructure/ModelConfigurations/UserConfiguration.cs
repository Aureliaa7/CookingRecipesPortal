using CookingRecipesPortal_DAL.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookingRecipesPortal_Infrastructure.ModelConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            builder.Ignore(x => x.Password); // do not store password as a plain text
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        }
    }
}
