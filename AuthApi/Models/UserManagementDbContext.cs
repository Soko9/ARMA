using Microsoft.EntityFrameworkCore;

namespace AuthApi.Models;

public partial class UserManagementDbContext : DbContext
{
    public UserManagementDbContext()
    {
    }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<IntegratedSystem> IntegratedSystems { get; set; }

    public virtual DbSet<IpwhiteList> IpwhiteLists { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IntegratedSystem>(entity =>
        {
            entity.HasKey(e => e.SystemId);

            entity.Property(e => e.SystemId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("SystemID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastActionUserId).HasColumnName("LastActionUserID");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.LastActionUser).WithMany(p => p.IntegratedSystems)
                .HasForeignKey(d => d.LastActionUserId)
                .HasConstraintName("FK_IntegratedSystems_Users");
        });

        modelBuilder.Entity<IpwhiteList>(entity =>
        {
            entity.HasKey(e => e.Ipid);

            entity.ToTable("IPWhiteList");

            entity.Property(e => e.Ipid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("IPID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.LastActionUserId).HasColumnName("LastActionUserID");
            entity.Property(e => e.SystemId).HasColumnName("SystemID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.LastActionUser).WithMany(p => p.IpwhiteLists)
                .HasForeignKey(d => d.LastActionUserId)
                .HasConstraintName("FK_IPWhiteList_Users");

            entity.HasOne(d => d.System).WithMany(p => p.IpwhiteLists)
                .HasForeignKey(d => d.SystemId)
                .HasConstraintName("FK_IPWhiteList_IntegratedSystems");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.Property(e => e.LogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("LogID");
            entity.Property(e => e.ActionAt).HasColumnType("datetime");
            entity.Property(e => e.ActionRowId).HasColumnName("ActionRowID");
            entity.Property(e => e.ActionTableName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ActionType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ActionUserId).HasColumnName("ActionUserID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.ActionUser).WithMany(p => p.Logs)
                .HasForeignKey(d => d.ActionUserId)
                .HasConstraintName("FK_Logs_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastActionUserId).HasColumnName("LastActionUserID");
            entity.Property(e => e.LastSuccessfulLoginAt).HasColumnType("datetime");
            entity.Property(e => e.Passcode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.LastActionUser).WithMany(p => p.InverseLastActionUser)
                .HasForeignKey(d => d.LastActionUserId)
                .HasConstraintName("FK_Users_LastActionUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
