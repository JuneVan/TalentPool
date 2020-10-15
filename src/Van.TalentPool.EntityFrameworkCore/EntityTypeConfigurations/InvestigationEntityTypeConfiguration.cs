using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Investigations;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class InvestigationEntityTypeConfiguration : IEntityTypeConfiguration<Investigation>
    {
        public void Configure(EntityTypeBuilder<Investigation> builder)
        {
            builder.ToTable("Investigations");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name).HasMaxLength(32);
            builder.Property(p => p.ExpectedSalary).HasMaxLength(128);
            builder.Property(p => p.ExpectedDate).HasMaxLength(128);
            builder.Property(p => p.ExpectedInterviewDate).HasMaxLength(128);
            builder.Property(p => p.ExpectedPhoneInterviewDate).HasMaxLength(128);
            builder.Property(p => p.CityOfResidence).HasMaxLength(128);
            builder.Property(p => p.CityOfDomicile).HasMaxLength(128);
            builder.Property(p => p.NotAcceptTravelReason).HasMaxLength(256);
            builder.Property(p => p.QualifiedRemark).HasMaxLength(512);

            builder.HasQueryFilter(f => !f.IsDeleted);//未删除的数据
        }
    }
}
