using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Solidaridad.Core.Entities;

namespace Solidaridad.DataAccess.Persistence.Configurations;

public class FarmerConfiguration : IEntityTypeConfiguration<Farmer>
{
    public void Configure(EntityTypeBuilder<Farmer> builder)
    {
        builder.Property(ti => ti.FirstName)
           .HasMaxLength(126)
           .IsRequired();

        builder.Property(ti => ti.OtherNames)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(ti => ti.DateOfBirth)
            .IsRequired(false);

        builder.Property(ti => ti.Mobile)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(ti => ti.AlternateContactNumber)
            .HasMaxLength(30)
            .IsRequired(false);

        builder.Property(ti => ti.Email)
            .HasMaxLength(150)
            .IsRequired(false);

        builder.Property(ti => ti.Mobile)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ti => ti.SystemId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.ParticipantId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.EnumerationDate)
            .IsRequired(false);

        //builder.Property(ti => ti.CooperativeId)
        //    .IsRequired(false);

        builder.Property(ti => ti.HasDisability)
            .IsRequired();

        builder.Property(ti => ti.AccessToMobile)
            .IsRequired();

        builder.Property(ti => ti.PaymentPhoneNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ti => ti.IsFarmerPhoneOwner)
            .IsRequired();

        builder.Property(ti => ti.PhoneOwnerName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(ti => ti.PhoneOwnerNationalId)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(ti => ti.PhoneOwnerRelationWithFarmer)
           .HasMaxLength(50)
           .IsRequired(false);

        builder.Property(ti => ti.PhoneOwnerAddress)
           .HasMaxLength(255)
           .IsRequired(false);

        builder.Property(ti => ti.Gender)
            .IsRequired();

        builder.Property(ti => ti.CountryId)
           .IsRequired();

        builder.Property(ti => ti.AdminLevel1Id)
           .IsRequired();

        builder.Property(ti => ti.AdminLevel2Id)
           .IsRequired();

        builder.Property(ti => ti.AdminLevel3Id)
           .IsRequired();

        //builder.Property(ti => ti.AdminLevel4Id)
        //    .IsRequired(false);

        builder.Property(ti => ti.Village)
           .IsRequired(false);

        builder.Property(ti => ti.ImportId)
           .IsRequired(false);

        builder.Property(ti => ti.BirthMonth)
           .IsRequired(false);

        builder.Property(ti => ti.BirthYear)
           .IsRequired(false);

        builder.Property(ti => ti.IsFarmerVerified)
            .IsRequired();

        builder.Property(ti => ti.Source)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("UI");
    }
}
