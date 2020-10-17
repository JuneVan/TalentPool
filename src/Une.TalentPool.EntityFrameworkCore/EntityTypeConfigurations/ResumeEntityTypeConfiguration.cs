﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Une.TalentPool.Resumes;

namespace Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations
{
    public class ResumeEntityTypeConfiguration : IEntityTypeConfiguration<Resume>
    {
        public void Configure(EntityTypeBuilder<Resume> builder)
        {

            builder.ToTable("Resumes");
            builder.Property(p => p.ConcurrencyStamp).IsConcurrencyToken();
            builder.HasKey(k => k.Id); 
            builder.Property(p => p.Name).HasMaxLength(32);
            builder.Property(p => p.PhoneNumber).HasMaxLength(16);
            builder.Property(p => p.City).HasMaxLength(32);
            builder.Property(p => p.Email).HasMaxLength(128);
            builder.Property(p => p.PlatformName).HasMaxLength(128);
            builder.Property(p => p.PlatformId).HasMaxLength(256);
            builder.Property(p => p.EnableReason).HasMaxLength(256);

            builder.HasQueryFilter(f => !f.IsDeleted);//未删除的数据

            builder.HasMany(m => m.KeyMaps).WithOne().HasForeignKey(k => k.ResumeId);
            builder.HasMany(m => m.ResumeCompares).WithOne().HasForeignKey(k => k.ResumeId);
            builder.HasMany(m => m.AuditRecords).WithOne().HasForeignKey(k => k.ResumeId);

        }
    }
}
