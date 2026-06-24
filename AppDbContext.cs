using Microsoft.EntityFrameworkCore;

namespace CrmLite.Data;

/// <summary>
/// 数据库上下文 — Code First 核心
/// 管理 Customer / Contact / Contract 三个实体集
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Customer> Customers => Set<Models.Customer>();
    public DbSet<Models.Contact> Contacts => Set<Models.Contact>();
    public DbSet<Models.Contract> Contracts => Set<Models.Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ============ Customer 配置 ============
        modelBuilder.Entity<Models.Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(c => c.CustomerId);

            entity.HasIndex(c => c.Code).IsUnique();

            entity.Property(c => c.Code)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(c => c.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(c => c.Industry).HasMaxLength(50);
            entity.Property(c => c.Scale).HasMaxLength(20);
            entity.Property(c => c.Region).HasMaxLength(50);
            entity.Property(c => c.Address).HasMaxLength(200);
            entity.Property(c => c.PostalCode).HasMaxLength(10);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Website).HasMaxLength(100);
            entity.Property(c => c.Remark).HasMaxLength(500);

            entity.Property(c => c.CreatedAt)
                  .HasDefaultValueSql("GETDATE()");
        });

        // ============ Contact 配置 ============
        modelBuilder.Entity<Models.Contact>(entity =>
        {
            entity.ToTable("Contacts");

            entity.HasKey(c => c.ContactId);

            entity.Property(c => c.Name)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(c => c.Position).HasMaxLength(50);
            entity.Property(c => c.Department).HasMaxLength(50);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Email).HasMaxLength(100);

            entity.Property(c => c.IsPrimary)
                  .HasDefaultValue(false);

            // Contact -> Customer (CASCADE: 删除客户时级联删除联系人)
            entity.HasOne(c => c.Customer)
                  .WithMany(cu => cu.Contacts)
                  .HasForeignKey(c => c.CustomerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ============ Contract 配置 ============
        modelBuilder.Entity<Models.Contract>(entity =>
        {
            entity.ToTable("Contracts");

            entity.HasKey(c => c.ContractId);

            entity.HasIndex(c => c.ContractCode).IsUnique();

            entity.Property(c => c.ContractCode)
                  .HasMaxLength(20)
                  .IsRequired();

            entity.Property(c => c.Name)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(c => c.Amount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(c => c.Remark).HasMaxLength(500);

            // Contract -> Customer (RESTRICT: 有关联合同时禁止删除客户)
            entity.HasOne(c => c.Customer)
                  .WithMany(cu => cu.Contracts)
                  .HasForeignKey(c => c.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Contract -> Contact (SET NULL: 删除联系人时合同引用置空)
            entity.HasOne(c => c.Contact)
                  .WithMany(ct => ct.Contracts)
                  .HasForeignKey(c => c.ContactId)
                  .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
