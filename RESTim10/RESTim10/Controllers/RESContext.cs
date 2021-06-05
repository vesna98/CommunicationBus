using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RESTim10.Controllers
{
    public partial class RESContext : DbContext
    {
        public RESContext()
        {
        }

        public RESContext(DbContextOptions<RESContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Resurs> Resurs { get; set; }
        public virtual DbSet<Tip> Tip { get; set; }
        public virtual DbSet<TipVeze> TipVeze { get; set; }
        public virtual DbSet<Veza> Veza { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MARIJA;Database=RES;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resurs>(entity =>
            {
                entity.HasKey(e => e.IdResurs)
                    .HasName("PK__Resurs__12EFB3DD473CD942");

                entity.Property(e => e.IdResurs)
                    .HasColumnName("id_resurs")
                    .ValueGeneratedNever();

                entity.Property(e => e.NazivR)
                    .IsRequired()
                    .HasColumnName("naziv_r")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Opis)
                    .HasColumnName("opis")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TipId).HasColumnName("tip_id");
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.HasKey(e => e.IdTip)
                    .HasName("PK__Tip__6A28C2C835ED5973");

                entity.Property(e => e.IdTip)
                    .HasColumnName("id_tip")
                    .ValueGeneratedNever();

                entity.Property(e => e.NazivTip)
                    .IsRequired()
                    .HasColumnName("naziv_tip")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipVeze>(entity =>
            {
                entity.ToTable("Tip_veze");

                entity.Property(e => e.TipVezeId)
                    .HasColumnName("tip_veze_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.NazivVeze)
                    .IsRequired()
                    .HasColumnName("naziv_veze")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Veza>(entity =>
            {
                entity.HasKey(e => e.IdVeze)
                    .HasName("PK__Veza__532DD25288A6FD52");

                entity.Property(e => e.IdVeze)
                    .HasColumnName("id_veze")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdDrugog).HasColumnName("id_drugog");

                entity.Property(e => e.IdPrvog).HasColumnName("id_prvog");

                entity.Property(e => e.TipVezeId).HasColumnName("tip_veze_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
