using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN_Project.Models
{
    public partial class prn_projectContext : DbContext
    {
        public prn_projectContext()
        {
        }

        public prn_projectContext(DbContextOptions<prn_projectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<QuizSet> QuizSets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerDetail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Correct).HasColumnName("correct");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK__Answer__QuizId__29572725");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("Quiz");

                entity.Property(e => e.QuizDetail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QuizType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Quizset)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.QuizsetId)
                    .HasConstraintName("FK__Quiz__QuizsetId__267ABA7A");
            });

            modelBuilder.Entity<QuizSet>(entity =>
            {
                entity.ToTable("QuizSet");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QuizsetTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Score).HasColumnType("decimal(18, 0)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
