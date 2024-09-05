using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCCustomIdentitySample.Models;

namespace MVCCustomIdentitySample.Data
{


    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int,
        UserClaim<AppUser>, UserRole<AppUser, AppRole>, UserLogin<AppUser>,
        RoleClaim<AppRole>, UserToken<AppUser>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            

            builder.Entity<AppUser>(b =>
            {
                // Primary key
                b.ToTable(name: "Users");

                b.HasKey(u => u.Id);

                // Indexes for "normalized" username and email, to allow efficient lookups
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                // Limit the size of columns to use efficient database types
                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                // The relationships between User and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each User can have many UserClaims
                b.HasMany<UserClaim<AppUser>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();

                // Each User can have many UserLogins
                b.HasMany<UserLogin<AppUser>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();

                // Each User can have many UserTokens
                b.HasMany<UserToken<AppUser>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany<UserRole<AppUser,AppRole>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<UserClaim<AppUser>>(b =>
            {
                // Primary key
                b.HasKey(uc => uc.Id);

                // Maps to the AspNetUserClaims table
                b.ToTable(name: "UserClaims");

            });

            builder.Entity<UserLogin<AppUser>>(b =>
            {
                // Composite primary key consisting of the LoginProvider and the key to use
                // with that provider
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                // Limit the size of the composite key columns due to common DB restrictions
                b.Property(l => l.LoginProvider).HasMaxLength(128);
                b.Property(l => l.ProviderKey).HasMaxLength(128);

                // Maps to the AspNetUserLogins table
                b.ToTable("UserLogins");
            });

            builder.Entity<UserToken<AppUser>>(b =>
            {
                // Composite primary key consisting of the UserId, LoginProvider and Name
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                // Limit the size of the composite key columns due to common DB restrictions
                //b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
               // b.Property(t => t.Name).HasMaxLength(maxKeyLength);

                // Maps to the AspNetUserTokens table
                b.ToTable("UserTokens");
            });

            builder.Entity<AppRole>(b =>
            {
                // Primary key
                b.HasKey(r => r.Id);

                // Index for "normalized" role name to allow efficient lookups
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

                // Maps to the AspNetRoles table
                b.ToTable("AppRoles");

                // A concurrency token for use with the optimistic concurrency checking
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                // Limit the size of columns to use efficient database types
                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                // The relationships between Role and other entity types
                // Note that these relationships are configured with no navigation properties

                // Each Role can have many entries in the UserRole join table
                b.HasMany<UserRole<AppUser,AppRole>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany<RoleClaim<AppRole>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<RoleClaim<AppRole>>(b =>
            {
                // Primary key
                b.HasKey(rc => rc.Id);

                // Maps to the AspNetRoleClaims table
                b.ToTable("RoleClaims");
            });

            builder.Entity<UserRole<AppUser, AppRole>>(b =>
            {
                // Primary key
                b.HasKey(r => new { r.UserId, r.RoleId });

                // Maps to the AspNetUserRoles table
                b.ToTable("UserRoles");
            });

        }

        // Additional DbSets
    }


}
