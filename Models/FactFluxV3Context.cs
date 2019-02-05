using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FactFluxV3.Models
{
    public partial class FactFluxV3Context : DbContext
    {
        public FactFluxV3Context()
        {
        }

        public FactFluxV3Context(DbContextOptions<FactFluxV3Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<ParentWords> ParentWords { get; set; }
        public virtual DbSet<Rssfeeds> Rssfeeds { get; set; }
        public virtual DbSet<WordLogs> WordLogs { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=FactFluxV3;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.ArticleTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ArticleUrl)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DatePublished).HasColumnType("datetime");

                entity.HasOne(d => d.Feed)
                    .WithMany(p => p.Article)
                    .HasForeignKey(d => d.FeedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Article__FeedId__25869641");
            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ImageLocation).HasMaxLength(150);
            });

            modelBuilder.Entity<ParentWords>(entity =>
            {
                entity.HasKey(e => e.WordJoinId);

                entity.HasOne(d => d.ChildWord)
                    .WithMany(p => p.ParentWordsChildWord)
                    .HasForeignKey(d => d.ChildWordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ParentWor__Child__35BCFE0A");

                entity.HasOne(d => d.ParentWord)
                    .WithMany(p => p.ParentWordsParentWord)
                    .HasForeignKey(d => d.ParentWordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ParentWor__Paren__36B12243");
            });

            modelBuilder.Entity<Rssfeeds>(entity =>
            {
                entity.HasKey(e => e.FeedId);

                entity.ToTable("RSSFeeds");

                entity.Property(e => e.FeedImage).IsUnicode(false);

                entity.Property(e => e.FeedLink)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FeedTitle).HasMaxLength(250);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.VideoLink)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WordLogs>(entity =>
            {
                entity.HasKey(e => e.WordLogId);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.WordLogs)
                    .HasForeignKey(d => d.ArticleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WordLogs__Articl__2B3F6F97");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.WordLogs)
                    .HasForeignKey(d => d.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WordLogs__WordId__2C3393D0");
            });

            modelBuilder.Entity<Words>(entity =>
            {
                entity.HasKey(e => e.WordId);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateIncremented).HasColumnType("datetime");

                entity.Property(e => e.Word)
                    .IsRequired()
                    .HasMaxLength(500);
            });
        }
    }
}
