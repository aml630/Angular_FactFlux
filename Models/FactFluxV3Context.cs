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

        public virtual DbSet<AggregatedCounter> AggregatedCounter { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Hash> Hash { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobParameter> JobParameter { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<ParentWords> ParentWords { get; set; }
        public virtual DbSet<Rssfeeds> Rssfeeds { get; set; }
        public virtual DbSet<Schema> Schema { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Tweets> Tweets { get; set; }
        public virtual DbSet<TwitterUsers> TwitterUsers { get; set; }
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
            modelBuilder.Entity<AggregatedCounter>(entity =>
            {
                entity.ToTable("AggregatedCounter", "HangFire");

                entity.HasIndex(e => new { e.Value, e.Key })
                    .HasName("UX_HangFire_CounterAggregated_Key")
                    .IsUnique();

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);
            });

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

            modelBuilder.Entity<Counter>(entity =>
            {
                entity.ToTable("Counter", "HangFire");

                entity.HasIndex(e => new { e.Value, e.Key })
                    .HasName("IX_HangFire_Counter_Key");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Hash>(entity =>
            {
                entity.ToTable("Hash", "HangFire");

                entity.HasIndex(e => new { e.ExpireAt, e.Key })
                    .HasName("IX_HangFire_Hash_Key");

                entity.HasIndex(e => new { e.Id, e.ExpireAt })
                    .HasName("IX_HangFire_Hash_ExpireAt");

                entity.HasIndex(e => new { e.Key, e.Field })
                    .HasName("UX_HangFire_Hash_Key_Field")
                    .IsUnique();

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);
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

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "HangFire");

                entity.HasIndex(e => e.StateName)
                    .HasName("IX_HangFire_Job_StateName");

                entity.HasIndex(e => new { e.Id, e.ExpireAt })
                    .HasName("IX_HangFire_Job_ExpireAt");

                entity.Property(e => e.Arguments).IsRequired();

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.InvocationData).IsRequired();

                entity.Property(e => e.StateName).HasMaxLength(20);
            });

            modelBuilder.Entity<JobParameter>(entity =>
            {
                entity.ToTable("JobParameter", "HangFire");

                entity.HasIndex(e => new { e.JobId, e.Name })
                    .HasName("IX_HangFire_JobParameter_JobIdAndName");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobParameter)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_JobParameter_Job");
            });

            modelBuilder.Entity<JobQueue>(entity =>
            {
                entity.ToTable("JobQueue", "HangFire");

                entity.HasIndex(e => new { e.Queue, e.FetchedAt })
                    .HasName("IX_HangFire_JobQueue_QueueAndFetchedAt");

                entity.Property(e => e.FetchedAt).HasColumnType("datetime");

                entity.Property(e => e.Queue)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.ToTable("List", "HangFire");

                entity.HasIndex(e => new { e.Id, e.ExpireAt })
                    .HasName("IX_HangFire_List_ExpireAt");

                entity.HasIndex(e => new { e.ExpireAt, e.Value, e.Key })
                    .HasName("IX_HangFire_List_Key");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Value).HasColumnType("nvarchar(max)");
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

            modelBuilder.Entity<Schema>(entity =>
            {
                entity.HasKey(e => e.Version);

                entity.ToTable("Schema", "HangFire");

                entity.Property(e => e.Version).ValueGeneratedNever();
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("Server", "HangFire");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastHeartbeat).HasColumnType("datetime");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.ToTable("Set", "HangFire");

                entity.HasIndex(e => new { e.Id, e.ExpireAt })
                    .HasName("IX_HangFire_Set_ExpireAt");

                entity.HasIndex(e => new { e.Key, e.Value })
                    .HasName("UX_HangFire_Set_KeyAndValue")
                    .IsUnique();

                entity.HasIndex(e => new { e.ExpireAt, e.Value, e.Key })
                    .HasName("IX_HangFire_Set_Key");

                entity.Property(e => e.ExpireAt).HasColumnType("datetime");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State", "HangFire");

                entity.HasIndex(e => e.JobId)
                    .HasName("IX_HangFire_State_JobId");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Reason).HasMaxLength(100);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.State)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_HangFire_State_Job");
            });

            modelBuilder.Entity<Tweets>(entity =>
            {
                entity.HasKey(e => e.TweetId);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTweeted).HasColumnType("datetime");

                entity.Property(e => e.EmbedHtml).IsRequired();

                entity.HasOne(d => d.TwitterUser)
                    .WithMany(p => p.Tweets)
                    .HasForeignKey(d => d.TwitterUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tweets__TwitterU__787EE5A0");
            });

            modelBuilder.Entity<TwitterUsers>(entity =>
            {
                entity.HasKey(e => e.TwitterUserId);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.TwitterUsername)
                    .IsRequired()
                    .HasMaxLength(70);
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
