﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Une.TalentPool.Configurations;
using Une.TalentPool.DailyStatistics;
using Une.TalentPool.Dictionaries;
using Une.TalentPool.Entities;
using Une.TalentPool.EntityFrameworkCore.EntityTypeConfigurations;
using Une.TalentPool.Evaluations;
using Une.TalentPool.Interviews;
using Une.TalentPool.Investigations;
using Une.TalentPool.Jobs;
using Une.TalentPool.Resumes;
using Une.TalentPool.Roles;
using Une.TalentPool.Users;

namespace Une.TalentPool.EntityFrameworkCore
{
    public class TalentDbContext : IdentityDbContext<User, Role, Guid>
    {

        public TalentDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
        {
            UserIdentifier = serviceProvider.GetService<IUserIdentifier>();
        }
        public TalentDbContext(IServiceProvider serviceProvider)
        {
            UserIdentifier = serviceProvider.GetService<IUserIdentifier>();
        }

        public DbSet<SettingValue> SettingValues { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<DictionaryItem> DictionaryItems { get; set; }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ResumeAuditRecord> ResumeAuditRecords { get; set; }
        public DbSet<ResumeAuditSetting> ResumeAuditSettings { get; set; }
        public DbSet<ResumeKeywordMap> ResumeKeyMaps { get; set; }
        public DbSet<ResumeCompare> ResumeCompares { get; set; }

        public DbSet<Investigation> Investigations { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EvaluationSubject> EvaluationSubjects { get; set; }
        public DbSet<EvaluationQuestion> EvaluationQuestions { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<DailyStatistic> DailyStatistics { get; set; }
        public DbSet<DailyStatisticItem> DailyStatisticItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new RoleClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserLoginEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserTokenEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserClaimEntityTypeConfiguration());
            builder.ApplyConfiguration(new DictionaryEntityTypeConfiguration());
            builder.ApplyConfiguration(new DictionaryItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResumeEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResumeKeyMapEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResumeCompareEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResumeAuditSettingEntityTypeConfiguration());
            builder.ApplyConfiguration(new ResumeAuditRecordEntityTypeConfiguration());
            builder.ApplyConfiguration(new InvestigationEntityTypeConfiguration());
            builder.ApplyConfiguration(new InterviewEntityTypeConfiguration());
            builder.ApplyConfiguration(new EvaluationEntityTypeConfiguration());
            builder.ApplyConfiguration(new EvaluationSubjectEntityTypeConfiguration());
            builder.ApplyConfiguration(new EvaluationQuestionEntityTypeConfiguration());
            builder.ApplyConfiguration(new DailyStatisticEntityTypeConfiguration());
            builder.ApplyConfiguration(new DailyStatisticItemEntityTypeConfiguration());
        }

        protected IUserIdentifier UserIdentifier { get; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        {
                            IHasCreationTime hasCreation = entry.Entity as IHasCreationTime;
                            if (hasCreation == null)
                                continue;
                            hasCreation.CreationTime = DateTime.Now;

                            ICreationAudited creationAudited = entry.Entity as ICreationAudited;
                            if (creationAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                creationAudited.CreatorUserId = UserIdentifier.UserId.Value;

                        }
                        break;
                    case EntityState.Modified:
                        {
                            IHasModificationTime hasModification = entry.Entity as IHasModificationTime;
                            if (hasModification == null)
                                continue;
                            hasModification.LastModificationTime = DateTime.Now;

                            IModificationAudited modificationAudited = entry.Entity as IModificationAudited;
                            if (modificationAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                modificationAudited.LastModifierUserId = UserIdentifier.UserId.Value;
                        }
                        break;
                    case EntityState.Deleted:
                        {

                            IHasDeletionTime hasDeletion = entry.Entity as IHasDeletionTime;
                            if (hasDeletion == null)
                                continue;
                            entry.Reload();
                            entry.State = EntityState.Modified;

                            hasDeletion.DeletionTime = DateTime.Now;
                            hasDeletion.IsDeleted = true;

                            IDeletionAudited deletionAudited = entry.Entity as IDeletionAudited;
                            if (deletionAudited == null)
                                continue;
                            if (UserIdentifier != null && UserIdentifier.UserId.HasValue)
                                deletionAudited.DeleterUserId = UserIdentifier.UserId.Value;

                        }
                        break;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }

        LoggerFactory _loggerFactory = new LoggerFactory(new[] {
            new DebugLoggerProvider()
        });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}