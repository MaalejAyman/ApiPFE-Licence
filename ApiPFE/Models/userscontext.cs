using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiPFE.Models
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
        public virtual DbSet<GroupesPasswords> GroupesPasswords { get; set; }
        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Userss> Userss { get; set; }
        public virtual DbSet<UserssGroupes> UserssGroupes { get; set; }
        public virtual DbSet<WebSites> WebSites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-FQ3INT6;Initial Catalog=ProjetPFE;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folders>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Parent).IsUnicode(false);

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
            });

            modelBuilder.Entity<GroupesPasswords>(entity =>
            {
                entity.HasKey(e => new { e.IdPass, e.IdGrp })
                    .HasName("PK__GroupesP__AC4B51117CBB7A23");

                entity.Property(e => e.PasswordCrypPub).IsUnicode(false);

                entity.HasOne(d => d.IdGrpNavigation)
                    .WithMany(p => p.GroupesPasswords)
                    .HasForeignKey(d => d.IdGrp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupesPasswords_ToGroupes");

                entity.HasOne(d => d.IdPassNavigation)
                    .WithMany(p => p.GroupesPasswords)
                    .HasForeignKey(d => d.IdPass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GroupesPasswords_ToPasswords");
            });

            modelBuilder.Entity<Passwords>(entity =>
            {
                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Value).IsUnicode(false);

                entity.HasOne(d => d.IdFldrNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdFldr)
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
                entity.Property(e => e.IsAdmin).HasDefaultValueSql("((0))");

                entity.Property(e => e.Login).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);
            });

            modelBuilder.Entity<UserssGroupes>(entity =>
            {
                entity.HasKey(e => new { e.IdUsr, e.IdGrp })
                    .HasName("PK__UserssGr__7C56696B839E2A9F");

                entity.HasOne(d => d.IdGrpNavigation)
                    .WithMany(p => p.UserssGroupes)
                    .HasForeignKey(d => d.IdGrp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserssGroupes_ToGroupes");

                entity.HasOne(d => d.IdUsrNavigation)
                    .WithMany(p => p.UserssGroupes)
                    .HasForeignKey(d => d.IdUsr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserssGroupes_ToUsers");
            });

            modelBuilder.Entity<WebSites>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_WebSites");

                entity.Property(e => e.Link).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
