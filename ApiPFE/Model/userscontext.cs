using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiPFE.Model
{
    public partial class userscontext : DbContext
    {
        public userscontext()
        {
        }

        public userscontext(DbContextOptions<userscontext> options)
            : base(options)
        {
        }

        public virtual DbSet<Folders> Folders { get; set; }
        public virtual DbSet<Groupes> Groupes { get; set; }
        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Userss> Userss { get; set; }
        public virtual DbSet<WebSites> WebSites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-FQ3INT6;Initial Catalog=ProjetPFE;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folders>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.IdParentFolderNavigation)
                    .WithMany(p => p.InverseIdParentFolderNavigation)
                    .HasForeignKey(d => d.IdParentFolder)
                    .HasConstraintName("ParentFolder");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("UserFolder");
            });

            modelBuilder.Entity<Groupes>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Groupes)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("UserGroupe");
            });

            modelBuilder.Entity<Passwords>(entity =>
            {
                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);

                entity.HasOne(d => d.IdFldrNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdFldr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PasswordFldr");

                entity.HasOne(d => d.IdGrpNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdGrp)
                    .HasConstraintName("PasswordGrp");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PasswordUser");

                entity.HasOne(d => d.IdWsNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdWs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PasswordWS");
            });

            modelBuilder.Entity<Userss>(entity =>
            {
                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);
            });

            modelBuilder.Entity<WebSites>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_WebSites");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.WebSites)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_WebSites_ToUserss");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
