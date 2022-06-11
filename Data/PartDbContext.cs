using Microsoft.EntityFrameworkCore;
using StackApi.Models;

namespace StackApi.Data;

public class PartDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserDetails> UserDetails { get; set; }
    public virtual DbSet<Part> Part { get; set; }
    public virtual DbSet<PartImages> PartImages { get; set; }
    public virtual DbSet<SearchViewHistory> SearchViewHistory { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<Discount> Discount { get; set; }
    public virtual DbSet<CartItems> CartItems { get; set; }
    public PartDbContext(DbContextOptions<PartDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsID).HasName("PK_UsID");
            entity.Property(e => e.UsID).HasColumnType("uniqueidentifier").HasColumnName("UsID").HasDefaultValue(Guid.NewGuid());
            entity.Property(e => e.Fname).HasColumnType("nvarchar(20)").HasColumnName("Fname").IsRequired();
            entity.Property(e => e.Mname).HasColumnType("nvarchar(20)").HasColumnName("Mname").IsRequired(false);
            entity.Property(e => e.Lname).HasColumnType("nvarchar(20)").HasColumnName("Lname").IsRequired();
            entity.Property(e => e.DOB).HasColumnType("Datetime").HasColumnName("DOB").IsRequired();
            entity.Property(e => e.EmailID).HasColumnType("nvarchar(60)").HasColumnName("EmailID").IsRequired();
            entity.Property(e => e.EmailConfirmed).HasColumnType("bit").HasColumnName("EmailConfirmed").IsRequired().HasDefaultValue(0);
            entity.Property(e => e.Password).HasColumnType("nvarchar(80)").HasColumnName("Password").IsRequired();
            entity.Property(e => e.IsAdmin).HasColumnType("bit").HasColumnName("IsAdmin").IsRequired();
        });

        modelBuilder.Entity<UserDetails>()
        .HasOne<User>()
        .WithOne()
        .HasForeignKey<UserDetails>(p => p.UsId)
        .HasConstraintName("FK_UsdUsID");

        modelBuilder.Entity<UserDetails>(entity =>
        {
            entity.HasKey(e => e.UsId).HasName("PK_UsdUdID");
            entity.Property(e => e.UsId).HasColumnType("uniqueidentifier").HasColumnName("UsId").IsRequired();
            entity.Property(e => e.CompanyCategory).HasColumnType("nvarchar(100)").HasColumnName("CompanyCategory").IsRequired();
            entity.Property(e => e.CompanyName).HasColumnType("nvarchar(100)").HasColumnName("CompanyName").IsRequired();
        });

        modelBuilder.Entity<Part>()
        .HasOne(e => e.category)
        .WithMany(e => e.Parts)
        .HasForeignKey(p => p.PcId)
        .HasConstraintName("FK_PcId");

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK_Pid");
            entity.Property(e => e.Pid).HasColumnName("Pid").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()");
            entity.Property(e => e.PartName).HasColumnName("PartName").HasColumnType("nvarchar(200)").IsRequired(true);
            entity.Property(e => e.PartDesc).HasColumnName("PartDesc").HasColumnType("nvarchar(500)").IsRequired(true);
            entity.Property(e => e.PartPrice).HasColumnName("PartPrice").HasColumnType("decimal(16,2)").IsRequired(true);
            entity.Property(e => e.ModifiedOn).HasColumnName("ModifiedOn").HasColumnType("datetime").HasDefaultValueSql("getdate()");
            entity.Property(e => e.PcId).HasColumnName("PcId").HasColumnType("uniqueidentifier").IsRequired(true);
        });

        modelBuilder.Entity<PartImages>()
        .HasOne(e => e.parts)
        .WithMany(e => e.PartImages)
        .HasForeignKey(p => p.Pid)
        .HasConstraintName("FK_Pid");


        modelBuilder.Entity<PartImages>(entity =>
        {
            entity.HasKey(e => e.PiId).HasName("PK_PiID");
            entity.Property(e => e.PiId).HasColumnName("PiId").HasColumnType("uniqueidentifier").HasDefaultValueSql("newid()");
            entity.Property(e => e.Pid).HasColumnName("Pid").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.PiFilename).HasColumnName("PiFilename").HasColumnType("nvarchar(250)").IsRequired();
            entity.Property(e => e.PiIsTD).HasColumnName("PiIsTD").HasColumnType("bit").HasDefaultValueSql("0").IsRequired();
            entity.Property(e => e.ModifiedOn).HasColumnName("ModifiedOn").HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });

        modelBuilder.Entity<Stock>()
        .HasOne<Part>()
        .WithOne()
        .HasForeignKey<Stock>(stck => stck.Pid)
        .HasConstraintName("FK_Parid");

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK_Parid");
            entity.Property(e => e.Pid).HasColumnName("Pid").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.PStock).HasColumnName("PStock").HasColumnType("int").IsRequired();
            entity.Property(e => e.ModifiedOn).HasColumnName("ModifiedOn").HasColumnType("datetime").HasDefaultValueSql("getdate()");
        });
        modelBuilder.Entity<SearchViewHistory>(entity =>
        {
            entity.HasKey(e => e.SvId).HasName("PK_SvId");
            entity.Property(e => e.SvId).HasColumnType("uniqueidentifier").HasColumnName("SvId").HasDefaultValueSql("newid()");
            entity.Property(e => e.searchterm).HasColumnType("nvarchar(max)").HasColumnName("searchterm").IsRequired(false);
            entity.Property(e => e.visitedPrd).HasColumnType("nvarchar(max)").HasColumnName("visitedPrd").IsRequired(false);
            entity.Property(e => e.UsID).HasColumnType("nvarchar(70)").HasColumnName("UsID").IsRequired(false);
            entity.Property(e => e.SerachedOn).HasColumnType("datetime").HasColumnName("SerachedOn").HasDefaultValueSql("getdate()");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_CID");
            entity.Property(e => e.CId).HasColumnType("uniqueidentifier").HasColumnName("CId").HasDefaultValueSql("newid()");
            entity.Property(e => e.CName).HasColumnName("CName").HasColumnType("nvarchar(50)").IsRequired(true);
            entity.Property(e => e.CisActive).HasColumnName("CisActive").HasColumnType("bit").HasDefaultValueSql("1");
        });

        modelBuilder.Entity<Discount>()
        .HasOne(e => e.Categories)
        .WithMany(e => e.Discounts)
        .HasForeignKey(e => e.CId)
        .HasConstraintName("FK_DCID");

        modelBuilder.Entity<Discount>()
        .HasOne(e => e.Parts)
        .WithMany(e => e.Discounts)
        .HasForeignKey(e => e.PrdId)
        .HasConstraintName("FK_DPRDID");

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK_DID");
            entity.HasAlternateKey(e => e.CouponCode).HasName("Uk_CouponCode");

            entity.Property(e => e.Did).HasColumnName("Did").HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();
            entity.Property(e => e.CouponCode).HasColumnName("CouponCode").HasColumnType("varchar(20)").IsRequired();
            entity.Property(e => e.CouponName).HasColumnName("CouponName").HasColumnType("varchar(40)").IsRequired();
            entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("Decimal(16,2)").IsRequired();
            entity.Property(e => e.DType).HasColumnName("DType").HasColumnType("smallint").IsRequired();
            entity.Property(e => e.CId).HasColumnName("CId").HasColumnType("uniqueidentifier");
            entity.Property(e => e.PrdId).HasColumnName("PrdId").HasColumnType("uniqueidentifier");
            entity.Property(e => e.StartDate).HasColumnName("StartDate").HasColumnType("datetime").IsRequired();
            entity.Property(e => e.EndDate).HasColumnName("EndDate").HasColumnType("datetime").IsRequired();
        });

        modelBuilder.Entity<CartItems>()
        .HasOne(e => e.Part)
        .WithMany(p => p.cartItems)
        .HasForeignKey(k => k.CIPrid)
        .HasConstraintName("FK_CIPrid");

        modelBuilder.Entity<CartItems>()
        .HasOne(e => e.User)
        .WithMany(p => p.cartItems)
        .HasForeignKey(k => k.CIUsid)
        .HasConstraintName("FK_CIUsid");

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.HasKey("CITId").HasName("PK_CITId");
            entity.Property(e => e.CITId).HasColumnName("CITId").HasColumnType("uniqueidentifier").ValueGeneratedOnAdd();
            entity.Property(e => e.CIQty).HasColumnName("CIQty").HasColumnType("int").IsRequired();
            entity.Property(e => e.CIPrid).HasColumnName("CIPrid").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.CIUsid).HasColumnName("CIUsid").HasColumnType("uniqueidentifier").IsRequired();
            entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn").HasColumnType("datetime").ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn").HasColumnType("datetime").ValueGeneratedOnUpdate();
        });
    }
}