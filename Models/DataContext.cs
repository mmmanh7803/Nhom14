using Microsoft.EntityFrameworkCore;
using QLcaulacbosinhvien.Areas.Admin.Models; // Add this line
using QLcaulacbosinhvien.Models;


namespace QLcaulacbosinhvien.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                                
        }

      public DbSet<Menu> Menus { get; set; }
       public DbSet<AdminMenu> AdminMenus { get; set; }

       public DbSet<Account> Accounts { get; set; }

       public DbSet<Event> Events { get; set; }

       public DbSet<Member> Members { get; set; }

       public DbSet<Article> Articles { get; set; } 

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Account>()
        .HasOne(a => a.member)
        .WithOne(m => m.account)
        .HasForeignKey<Account>(a => a.MemberID); // Account có khóa ngoại là MemberID
}

    }
}
