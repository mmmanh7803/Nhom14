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

      public DbSet<EventMember> EventMembers { get; set; } 

      

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Account>()
        .HasOne(a => a.member)
        .WithOne(m => m.account)
        .HasForeignKey<Account>(a => a.MemberID); // Account có khóa ngoại là MemberID

    modelBuilder.Entity<Event>()
    .HasOne(e => e.Account)
    .WithMany(a => a.Events)
    .HasForeignKey(e => e.AccountID);

  modelBuilder.Entity<EventMember>()
                .HasKey(em => em.EventAccountID); // Đặt khóa chính

            modelBuilder.Entity<EventMember>()
                .HasOne(em => em.Event)
                .WithMany(e => e.EventMembers)
                .HasForeignKey(em => em.EventID);

            modelBuilder.Entity<EventMember>()
                .HasOne(em => em.Account)
                .WithMany(a => a.EventMembers)
                .HasForeignKey(em => em.AccountID);
}

    }
}
