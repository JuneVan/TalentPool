using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Van.TalentPool.Evaluations;

namespace Van.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    class EvaluationSubjectEntityTypeConfiguration : IEntityTypeConfiguration<EvaluationSubject>
    {
        public void Configure(EntityTypeBuilder<EvaluationSubject> builder)
        {
           builder.ToTable("EvaluationSubjects");
           builder.HasKey(k => k.Id);
           builder.Property(p => p.Keyword).HasMaxLength(128);
           builder.Property(p => p.Description).HasMaxLength(1024);  
        }
    }
}
