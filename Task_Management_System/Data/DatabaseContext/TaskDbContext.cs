namespace Task_Management_System.Data.DatabaseContext
{
    public class TaskDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<TaskModel> _tasks { get; set; }
        public DbSet<ImagesOfTask> imagesOfTasks { get; set; }
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {

        }
    }
}
