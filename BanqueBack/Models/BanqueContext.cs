using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BanqueBack.Models;

namespace BanqueBack.Models
{
    public partial class BanqueContext : DbContext
    {
        public BanqueContext()
        {
        }

        public BanqueContext(DbContextOptions<BanqueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Agence> Agences { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Directeur> Directeurs{ get; set; } = null!;
        public DbSet<BanqueBack.Models.Commercial> Commercials { get; set; }
        public virtual DbSet<Manager> Managers{ get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Banque;User Id=postgres;Password=root");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasIndex(e => e.Numaccount, "ak1_order_ordernumber")
                    .IsUnique();

                entity.HasIndex(e => e.Agenceid, "fk_144");

                entity.HasIndex(e => e.Userid, "fk_order_customerid_customer");

                entity.Property(e => e.Accountid)
                    .HasColumnName("accountid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Agenceid).HasColumnName("agenceid");

                entity.Property(e => e.Datecloture).HasColumnName("datecloture");

                entity.Property(e => e.Datecreation).HasColumnName("datecreation");

                entity.Property(e => e.Numaccount)
                    .HasMaxLength(50)
                    .HasColumnName("numaccount");

                entity.Property(e => e.Solde)
                    .HasPrecision(12, 2)
                    .HasColumnName("solde");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Agence)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Agenceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_142");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_customerid_customer");
            });

            modelBuilder.Entity<Agence>(entity =>
            {
                entity.ToTable("Agence");

                entity.HasIndex(e => e.Nomagence, "ak1_supplier_companyname")
                    .IsUnique();

                entity.Property(e => e.Agenceid)
                    .HasColumnName("agenceid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Nomagence)
                    .HasMaxLength(40)
                    .HasColumnName("nomagence");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.Property(e => e.Ville)
                    .HasMaxLength(50)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.HasIndex(e => e.Email, "AK1_Login_Login")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Motdepasse)
                    .HasMaxLength(50)
                    .HasColumnName("motdepasse");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasIndex(e => e.Accountid, "fk_orderitem_orderid_order");

                entity.Property(e => e.Transactionid)
                    .HasColumnName("transactionid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Accountid).HasColumnName("accountid");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Montant)
                    .HasPrecision(18, 2)
                    .HasColumnName("montant");

                entity.Property(e => e.Operation)
                    .HasMaxLength(50)
                    .HasColumnName("operation");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Accountid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_orderitem_orderid_order");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "email")
                    .IsUnique();

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .HasColumnName("adresse");

                entity.Property(e => e.Cp)
                    .HasMaxLength(50)
                    .HasColumnName("cp");

                entity.Property(e => e.Datenaissance).HasColumnName("datenaissance");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Motdepasse)
                    .HasMaxLength(50)
                    .HasColumnName("motdepasse");

                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");

                entity.Property(e => e.Ville)
                    .HasMaxLength(50)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<Commercial>(entity =>
            {
                entity.ToTable("Commercial");

                entity.HasIndex(e => e.Nom, "ak1_commercial_commercialname")
                    .IsUnique();

                entity.Property(e => e.commercialid)
                    .HasColumnName("commercialid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .HasColumnName("adresse");

                entity.Property(e => e.Cp)
                    .HasMaxLength(50)
                    .HasColumnName("cp");

                entity.Property(e => e.Datenaissance)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("datenaissance");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");


                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");

                entity.Property(e => e.Ville)
                    .HasMaxLength(50)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<Directeur>(entity =>
            {
                entity.ToTable("Directeur");

                entity.HasIndex(e => e.Nom, "ak1_directeur_directeurname")
                    .IsUnique();

                entity.Property(e => e.directeurid)
                    .HasColumnName("directeurid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .HasColumnName("adresse");

                entity.Property(e => e.Cp)
                    .HasMaxLength(50)
                    .HasColumnName("cp");

                entity.Property(e => e.Datenaissance)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("datenaissance");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");


                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");

                entity.Property(e => e.Ville)
                    .HasMaxLength(50)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.ToTable("Manager");

                entity.HasIndex(e => e.Nom, "ak1_manager_managername")
                    .IsUnique();

                entity.Property(e => e.managerid)
                    .HasColumnName("managerid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Adresse)
                    .HasMaxLength(50)
                    .HasColumnName("adresse");

                entity.Property(e => e.Cp)
                    .HasMaxLength(50)
                    .HasColumnName("cp");

                entity.Property(e => e.Datenaissance)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("datenaissance");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");


                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");

                entity.Property(e => e.Ville)
                    .HasMaxLength(50)
                    .HasColumnName("ville");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        public DbSet<BanqueBack.Models.Directeur> Directeur { get; set; }
    }
}
