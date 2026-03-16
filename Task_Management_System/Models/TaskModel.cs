namespace Task_Management_System.Models
{
    public class TaskModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<ImagesOfTask> Images { get; set; } = new List<ImagesOfTask>();
    }
}
