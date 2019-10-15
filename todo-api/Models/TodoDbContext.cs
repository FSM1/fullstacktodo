using Microsoft.EntityFrameworkCore;

#nullable disable
namespace todo_api.Models
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        { }

        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskGroup> TaskGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    FirstName = "Krystian",
                    LastName = "Nowak"
                },
                new User()
                {
                    Id = 2,
                    FirstName = "Maciej",
                    LastName = "Kowalski"
                },                
                new User()
                {
                    Id = 3,
                    FirstName = "Zbigniew",
                    LastName = "Czajka"
                }
            );
        }
    }
}