using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiPFE.Models
{
    public partial class UserContext : DbContext
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Folders> Folders { get; set; }
        public virtual DbSet<Groupes> Groupes { get; set; }
        public virtual DbSet<Passwords> Passwords { get; set; }
        public virtual DbSet<Userss> Userss { get; set; }
        public virtual DbSet<WebSites> WebSites { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folders>(entity =>
            {


                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

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


                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Groupes)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("UserGroupe");
            });

            modelBuilder.Entity<Passwords>(entity =>
            {
 

                entity.Property(e => e.IdWs).HasColumnName("IdWS");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFldrNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdFldr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PasswordFldr");

                entity.HasOne(d => d.IdGrpNavigation)
                    .WithMany(p => p.Passwords)
                    .HasForeignKey(d => d.IdGrp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                entity.Property(e => e.Login)
                    .HasColumnName("login")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WebSites>(entity =>
            {

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
