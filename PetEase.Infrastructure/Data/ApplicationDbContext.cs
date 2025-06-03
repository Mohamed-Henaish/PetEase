using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetEase.Core.Models;

namespace PetEase.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual DbSet<Product> TbProduct { get; set; }
        public virtual DbSet<Shop> TbShop { get; set; }
        public virtual DbSet<Doctor> TbDoctor { get; set; }
        public virtual DbSet<Address> TbAddress { get; set; }
        public virtual DbSet<OrderHeader> TbOrderHeader { get; set; }
        public virtual DbSet<OrderDetails> TbOrderDetails { get; set; }
        public virtual DbSet<TimeSlot> TbTimeSlot { get; set; }
        public virtual DbSet<TrainingVideo> TbTrainingVideo { get; set; }
        public virtual DbSet<DoctorRequest> TbDoctorRequest { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customize table names
            modelBuilder.Entity<ApplicationUser>().ToTable("TbUser");
            modelBuilder.Entity<IdentityRole>().ToTable("TbRoles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("TbUserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("TbUserClaim");
            // Ignore
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
