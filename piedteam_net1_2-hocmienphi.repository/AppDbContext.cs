using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository.Entity;
namespace piedteam_net1_2_hocmienphi.repository;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {}
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Mentor> Mentors { get; set; }
    public DbSet<MentorCategory> MentorCategories { get; set; }
    public DbSet<ApplyRequest> ApplyRequests { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<MentorFreeTime> MentorFreeTimes { get; set; }
    public DbSet<ApplyRequestCategory> ApplyRequestCategories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

            builder
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasOne(u => u.Mentor)
                .WithOne(m => m.User)
                .HasForeignKey<Mentor>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(u => u.ApplyRequests)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Mentor>(builder =>
        {
            builder
                .HasMany(m => m.MentorCategories)
                .WithOne(mc => mc.Mentor)
                .HasForeignKey(mc => mc.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(m => m.Bookings)
                .WithOne(b => b.Mentor)
                .HasForeignKey(b => b.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(m => m.MentorFreeTimes)
                .WithOne(ft => ft.Mentor)
                .HasForeignKey(ft => ft.MentorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder
                .HasMany(c => c.MentorCategories)
                .WithOne(mc => mc.Category)
                .HasForeignKey(mc => mc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasMany(u => u.ApplyRequestsCategories)
                .WithOne(n => n.Category)
                .HasForeignKey(u => u.CategoryId) 
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<ApplyRequest>(builder =>
        {
            builder
                .HasMany(c => c.ApplyRequestsCategories)
                .WithOne(mc => mc.ApplyRequest)
                .HasForeignKey(mc => mc.ApplyRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
/*
 // với 1 user thì có nhiều meeting, với khóa ngoại là userId
            
// khi mà mình chỉ định việc khai báo các mqh ở đây
// thì các mqh sẽ kh bao g bị tạo thiếu
            
// mình có thể config các behavior (hành động)
// khi có sự thay đổi dữ liệu giữa các mqh
            
// vi dụ: 1 User thì có nhiều Meetings, vậy thì
// khi User đó bị xóa đi
// thì các Meetings sẽ xử lí như thế nào, sống chết ra sao
// OnDelete: sẽ giải quyết
// DeleteBehavior.Cascade: khi 1 user bị xóa thì meetings liên quan cũng bị xóa theo
// DeleteBehavior.Restrict: ngăn chặn việc xóa 1 user nếu có meetings liên quan tồn tại
// tham chiếu tới PK tồn tại
//1 meeting còn thuộc về 1 user thì kh xóa đc
// DeleteBehavior.NoAction: kh thực hiện hành động gì đặc biệt khi user bị xóa. 
// gần giống Restrict, xử lí ở DB
// DeleteBehavior.SetNull: khi 1 user bị xóa, thì trường userId trong bảng meeting sẽ
// đc đặt thành NULL
// chỉ áp dụng khi trường FK cho phép NULL
// seed data

//tech lead keu la tao. moi table di
// mo hinh 3 lop (trong Program.cs) -> di vao repo
// em tim ngay folder Entity: tao. moi class Category
// khai bao cac field: Id, Name, 
// khai bao cac field lquan toi mqh: ParentId, Parent, Children
// di vao' dbContext khai bao
// chay. migration de tao file migration(cac phien ban cua db)
// upd db
 */